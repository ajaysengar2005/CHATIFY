using Firebase.Auth;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


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
        private void pictureBoxHide_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(pictureBoxHide, "Hide Password");
        }
        private void pictureBoxShow_MouseHover_1(object sender, EventArgs e)
        {
            toolTip.SetToolTip(pictureBoxShow, "Show Password");
        }
        private void pictureBoxShow_Click_1(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = false;
            pictureBoxShow.Hide();
            pictureBoxHide.Show();
        }
        private void pictureBoxHide_Click_1(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = true;
            pictureBoxShow.Show();
            pictureBoxHide.Hide();
        }
        public FormRegister()
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

        private void buttonBack_Click(object sender, EventArgs e)
        {
            FormLogin formLogin = new FormLogin();
            formLogin.ShowDialog();
            this.Hide();
        }

        private void FormRegister_Load(object sender, EventArgs e)
        {

        }
        private bool IsValidUsername(string username)
        {
            // Example validation: Username should be at least 4 characters and contain only letters & numbers
            return username.Length >= 4 && username.All(char.IsLetterOrDigit);
        }
        private void textBoxUsername_Enter_1(object sender, EventArgs e)
        {
            if (textBoxUsername.Text.Trim() == "Enter Username")
            {
                textBoxUsername.Clear();
                textBoxUsername.ForeColor = Color.Black;
            }
            if (!IsValidUsername(textBoxUsername.Text.Trim()) || textBoxUsername.Text.Trim() == "Enter Username")
            {
                pictureBoxError.Show();
            }
            else
            {
                pictureBoxError.Hide();
            }
        }
        private void textBoxUsername_Leave_1(object sender, EventArgs e)
        {
            if (textBoxUsername.Text.Trim() == string.Empty)
            {
                textBoxUsername.Text = "Enter Username";
                textBoxUsername.ForeColor = Color.DarkGray;
            }
            if (!IsValidUsername(textBoxUsername.Text.Trim()) || textBoxUsername.Text.Trim() == "Enter Username")
            {
                pictureBoxError.Show();
            }
            else
            {
                pictureBoxError.Hide();
            }
        }
   
        private bool IsValidEmail(string email)
        {
            // Regular expression for a valid email pattern
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        private void textBoxEmail_Enter(object sender, EventArgs e)
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

        private void textBoxEmail_Leave_1(object sender, EventArgs e)
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

        private void textBoxPassword_Enter_1(object sender, EventArgs e)
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

        private void textBoxPassword_Leave_1(object sender, EventArgs e)
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
    }
    public class User
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string UID { get; set; } 
    }
}
