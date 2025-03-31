using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using New_Messenger_App; // Replace with the correct namespace where User class is defined
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
    public partial class FormLogin : Form
    {
        private static string apiKey = "AIzaSyDsi3VYY8hdA8z8jDsQxa6Ls4UxF615sH4";
        private static string projectId = "chatapp-24374";
        private FirestoreDb firestoreDb;

        //private FirebaseAuthProvider authProvider;
        //private FirebaseClient firebaseClient;
        public FormLogin()
        {
            InitializeComponent();
            // Initialize Firestore
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("D:\\Zoom")
            });

            firestoreDb = FirestoreDb.Create(projectId);
        }

        private async void buttonLogin_Click(object sender, EventArgs e)
        {
            string email = textBoxEmail.Text;
            string password = textBoxPassword.Text;

            try
            {
                // Authenticate user using FirebaseAuth
                var authUser = await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(email);

                if (authUser != null)
                {
                    // Fetch user details from Firestore
                    DocumentReference docRef = firestoreDb.Collection("Users").Document(authUser.Uid);
                    DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                    if (snapshot.Exists)
                    {
                        string username = snapshot.GetValue<string>("Username");
                        MessageBox.Show($"Login Successful! Welcome, {username}");

                        // Open Chat Window
                        this.Hide();
                        FormChatWindow chatWindow = new FormChatWindow(username);
                        chatWindow.Show();
                    }
                    else
                    {
                        MessageBox.Show("User data not found in Firestore!");
                    }
                }
                else
                {
                    MessageBox.Show("Invalid email or password!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login Failed: {ex.Message}");
            }
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            FormRegister registerForm = new FormRegister();
            registerForm.Show();
            this.Hide(); // Hide the login form
        }
    }
}
