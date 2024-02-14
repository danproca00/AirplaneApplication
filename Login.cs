using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AirplaneApplication
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(UidTb.Text == "" || PassTb.Text == "")
            {
                MessageBox.Show("Enter Username and password :)");
            } else if(UidTb.Text == "Admin" && PassTb.Text == "Admin")
            {
                Home home = new Home();
                home.Show();
                this.Hide();
            } else
            {
                MessageBox.Show("Wrong, try again");
            }
        }


        private void label8_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            UidTb.Text = "";
            PassTb.Text = "";
        }
    }
}
