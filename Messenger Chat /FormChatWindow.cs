using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirebaseConfig = Firebase.Auth.FirebaseConfig;

namespace CHATIFY
{
    public partial class FormChatWindow : Form
    {
        private string currentUserUID;
        private string userUID;
        private string selectedUserUID;
        private string selectedUsername;

        FireSharp.Config.FirebaseConfig databaseConfig = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "YOUR_API_KEY",
            BasePath = "YOUR_BASE_PATH"
        };

        IFirebaseClient firebaseClient;

        public FormChatWindow(string userUID)
        {
            InitializeComponent();
            currentUserUID = userUID;
            this.userUID = userUID;
            this.Text = $"Chat - {currentUserUID}";

            firebaseClient = new FireSharp.FirebaseClient(databaseConfig);

            if (firebaseClient == null)
            {
                MessageBox.Show("Connection Error! Check your Firebase credentials.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoadUserContacts();
        }

        private async void LoadUserContacts()
        {
            listBoxContacts.Items.Clear();

            FirebaseResponse response = await firebaseClient.GetAsync("Users");
            Dictionary<string, UserItem> users = response.ResultAs<Dictionary<string, UserItem>>();

            if (users != null)
            {
                foreach (var user in users)
                {
                    if (user.Key != userUID) 
                    {
                        listBoxContacts.Items.Add(new UserItem { UID = user.Key, Username = user.Value.Username });
                    }
                }
            }
        }

        private void listBoxContacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxContacts.SelectedItem != null)
            {
                UserItem selectedUser = (UserItem)listBoxContacts.SelectedItem;
                selectedUserUID = selectedUser.UID;
                selectedUsername = selectedUser.Username;
                labelChatWith.Text = $"Chat with: {selectedUsername}";

                LoadChatHistory();
                ListenForNewMessages();
            }
        }

        private async void LoadChatHistory()
        {
            richTextBoxReceive.Clear();

            string chatPath = $"Chats/{currentUserUID}_{selectedUserUID}";

            FirebaseResponse response = await firebaseClient.GetAsync(chatPath);
            Dictionary<string, ChatMessage> messages = response.ResultAs<Dictionary<string, ChatMessage>>();

            if (messages != null)
            {
                foreach (var message in messages.Values)
                {
                    richTextBoxReceive.AppendText(message.SenderUID == currentUserUID
                        ? $"You: {message.MessageText}\n"
                        : $"{selectedUsername}: {message.MessageText}\n");
                }
            }
        }

   
        private void ListenForNewMessages()
        {
            string chatPath = $"Chats/{selectedUserUID}_{currentUserUID}";
            firebaseClient.OnAsync(chatPath, (sender, args, context) =>
            {
                if (!string.IsNullOrEmpty(args.Data) && args.Data != "null")
                {
                    try
                    {
                        ChatMessage newMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<ChatMessage>(args.Data);

                        if (newMessage != null && newMessage.SenderUID == selectedUserUID)
                        {
                            richTextBoxReceive.Invoke((MethodInvoker)(() =>
                            {
                                richTextBoxReceive.AppendText($"{selectedUsername}: {newMessage.MessageText}\n");
                            }));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error processing new message: " + ex.Message);
                    }
                }
            });
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBoxMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void pictureBoxClose_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBoxMinimize_Click_1(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private async void buttonSendMessage_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(richTextBoxEnter.Text) && !string.IsNullOrEmpty(selectedUserUID))
            {
                string messageText = richTextBoxEnter.Text;

                ChatMessage chatMessage = new ChatMessage
                {
                    SenderUID = currentUserUID,
                    ReceiverUID = selectedUserUID,
                    MessageText = messageText,
                    Timestamp = DateTime.UtcNow.ToString("o") 
                };

                string senderChatPath = $"Chats/{currentUserUID}_{selectedUserUID}";
                string receiverChatPath = $"Chats/{selectedUserUID}_{currentUserUID}";

                await firebaseClient.PushAsync(senderChatPath, chatMessage);
                await firebaseClient.PushAsync(receiverChatPath, chatMessage);

                richTextBoxReceive.AppendText($"You: {messageText}\n");
                richTextBoxEnter.Clear();
            }
            else
            {
                MessageBox.Show("Select a user before sending a message!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

    public class UserItem
    {
        public string UID { get; set; }
        public string Username { get; set; }

        public override string ToString()
        {
            return Username;  
        }
    }

    public class ChatMessage
    {
        public string SenderUID { get; set; }
        public string ReceiverUID { get; set; }
        public string MessageText { get; set; }
        public string Timestamp { get; set; }
    }
}
