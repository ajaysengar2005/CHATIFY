using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Firebase.Auth;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHATIFY
{
    public partial class FormRegister : Form
    {
        FireSharp.Config.FirebaseConfig databaseConfig = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "YOUR_API_KEY",
            BasePath = "YOUR_BASE_PATH"
        };

        Firebase.Auth.FirebaseConfig authConfig = new Firebase.Auth.FirebaseConfig("YOUR_API_KEY");

        IFirebaseClient client;
        FirebaseAuthProvider authProvider;

        public FormRegister()
        {
            InitializeComponent();
            client = new FireSharp.FirebaseClient(databaseConfig);
            authProvider = new FirebaseAuthProvider(authConfig);
        }

        private async void buttonRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxUsername.Text) ||
                string.IsNullOrWhiteSpace(textBoxEmail.Text) ||
                string.IsNullOrWhiteSpace(textBoxPassword.Text))
            {
                MessageBox.Show("Please fill all fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                var authResult = await authProvider.CreateUserWithEmailAndPasswordAsync(
                    textBoxEmail.Text, textBoxPassword.Text);

                string uid = authResult.User.LocalId;  

                if (client != null)
                {
                    var user = new User
                    {
                        Username = textBoxUsername.Text,
                        Email = textBoxEmail.Text,
                        UID = uid  
                    };

                    FirebaseResponse response = await client.SetAsync("Users/" + uid, user);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        MessageBox.Show("User Registered Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to save user details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (FirebaseAuthException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            FormLogin formLogin = new FormLogin();
            formLogin.ShowDialog();
            this.Hide();
        }
    }
    public class User
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string UID { get; set; } 
    }
}
