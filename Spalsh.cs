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
    public partial class Spalsh : Form
    {
        public Spalsh()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Myprogress_Click(object sender, EventArgs e)
        {

        }
        int startingpoint = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            startingpoint += 1;
            Myprogress.Value = startingpoint;
            if(Myprogress.Value == 100)
            {
                Myprogress.Value = 0;
                timer1.Stop();
                Login login = new Login();
                login.Show();
                this.Hide();
            }

        }

        private void Spalsh_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
