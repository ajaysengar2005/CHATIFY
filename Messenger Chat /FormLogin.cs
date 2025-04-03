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
            textBoxPassword.UseSystemPasswordChar = true;
            pictureBoxHide.Hide();
            pictureBoxError.Hide();
            labelError.Hide();
            pictureBoxError.Hide();
        }

        private void pictureBoxHide_MouseHover_1(object sender, EventArgs e)
        {
            toolTip.SetToolTip(pictureBoxHide, "Hide Password");
        }
        private void pictureBoxShow_MouseHover_1(object sender, EventArgs e)
        {
            toolTip.SetToolTip(pictureBoxShow, "Show Password");
        }
        private void pictureBoxShow_Click(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = false;
            pictureBoxShow.Hide();
            pictureBoxHide.Show();
        }
        private void pictureBoxHide_Click(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = true;
            pictureBoxShow.Show();
            pictureBoxHide.Hide();
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
        private bool IsValidEmail(string email)
        {
            // Regular expression for a valid email pattern
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        private void textBoxEmail_Enter_1(object sender, EventArgs e)
        {
            if (textBoxEmail.Text.Trim() == "Enter Email")
            {
                textBoxEmail.Clear();
                textBoxEmail.ForeColor = Color.Black;
            }
            if (!IsValidEmail(textBoxEmail.Text.Trim()) || textBoxEmail.Text.Trim() == "Enter Email")
            {
                pictureBoxError.Show();
            }
            else
            {
                pictureBoxError.Hide();
            }
        }

        private void textBoxEmail_Leave(object sender, EventArgs e)
        {
            if (textBoxEmail.Text.Trim() == string.Empty)
            {
                textBoxEmail.Text = "Enter Email";
                textBoxEmail.ForeColor = Color.DarkGray;
            }
            if (!IsValidEmail(textBoxEmail.Text.Trim()) || textBoxEmail.Text.Trim() == "Enter Email")
            {
                pictureBoxError.Show();
            }
            else
            {
                pictureBoxError.Hide();
            }
        }
        private bool IsValidPassword(string password)
        {
            // At least 8 characters, 1 uppercase, 1 lowercase, 1 number, 1 special character
            string pattern = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            return Regex.IsMatch(password, pattern);
        }

        private void textBoxPassword_Enter(object sender, EventArgs e)
        {
            if (textBoxPassword.Text.Trim() == "Enter Password")
            {
                textBoxPassword.Clear();
                textBoxPassword.ForeColor = Color.Black;
            }
            if (!IsValidPassword(textBoxPassword.Text.Trim()) || textBoxPassword.Text.Trim() == "Enter Password")
            {
                pictureBoxError.Show();
            }
            else
            {
                pictureBoxError.Hide();
            }
        }

        private void textBoxPassword_Leave(object sender, EventArgs e)
        {
            if (textBoxPassword.Text.Trim() == string.Empty)
            {
                textBoxPassword.Text = "Enter Password";
                textBoxPassword.ForeColor = Color.DarkGray;
            }
            if (!IsValidPassword(textBoxPassword.Text.Trim()) || textBoxPassword.Text.Trim() == "Enter Password")
            {
                pictureBoxError.Show();
            }
            else
            {
                pictureBoxError.Hide();
            }
        }

        private void labelSignUp_Click(object sender, EventArgs e)
        {
            FormRegister formRegister = new FormRegister();
            formRegister.ShowDialog();
            this.Hide();
        }
    }
}
