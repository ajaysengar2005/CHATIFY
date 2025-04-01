using Firebase.Auth;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace CHATIFY
{
    public partial class FormLogin : Form
    {
        FireSharp.Config.FirebaseConfig databaseConfig = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "YOUR_API_KEY",
            BasePath = "YOUR_BASE_PATH"
        };

        Firebase.Auth.FirebaseConfig authConfig = new Firebase.Auth.FirebaseConfig("YOUR_API_KEY");

        IFirebaseClient client;
        FirebaseAuthProvider authProvider;


        public FormLogin()
        {
            InitializeComponent();
            client = new FireSharp.FirebaseClient(databaseConfig);
            authProvider = new FirebaseAuthProvider(authConfig);
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            FormRegister formRegister = new FormRegister();
            formRegister.ShowDialog();
            this.Hide();
        }

        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            string email = textBoxEmail.Text.Trim();
            string password = textBoxPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both Email and Password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                FirebaseAuthLink auth = await authProvider.SignInWithEmailAndPasswordAsync(email, password);

                if (auth != null)
                {
                    string userUID = auth.User.LocalId;

                    MessageBox.Show("Login Successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    FormChatWindow chatWindow = new FormChatWindow(userUID);
                    chatWindow.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login Failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
