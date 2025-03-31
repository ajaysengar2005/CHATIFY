using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace New_Messenger_App
{
    public partial class FormRegister : Form
    {
        private FirestoreDb firestoreDb;

        public FormRegister()
        {
            InitializeComponent();
            InitializeFirebase();
        }
        private void InitializeFirebase()
        {
            string path = @"D:\Zoom";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            firestoreDb = FirestoreDb.Create("chatapp-24374");
        }

        private async void buttonRegister_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string email = textBoxEmail.Text;
            string password = textBoxPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            try
            {
                // Store user details in Firestore
                DocumentReference docRef = firestoreDb.Collection("Users").Document(email);
                await docRef.SetAsync(new User { Username = username, Email = email, Password = password });

                MessageBox.Show("Registration Successful! You can now log in.");

                // Open Login Form
                this.Hide();
                FormLogin loginForm = new FormLogin();
                loginForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Registration Failed: {ex.Message}");
            }
        }

        public class User
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; } // Store password securely in real apps
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            FormLogin loginForm = new FormLogin();
            loginForm.Show();
            this.Close(); // Close the registration form
        }
    }
}
