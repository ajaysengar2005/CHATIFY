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
    public partial class FormChatWindow : Form
    {
        private string username;
        public FormChatWindow(string username)
        {
            InitializeComponent();
            this.username = username;
            //lblWelcome.Text = $"Welcome, {username}!";
        }
    }
}
