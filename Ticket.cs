using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace AirplaneApplication
{
    public partial class Ticket : Form
    {
        public Ticket()
        {
            InitializeComponent();
        }

        SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-4A2Q0M8;Initial Catalog=AirlineDB;Integrated Security=True");

        private void populate()
        {
            Con.Open();
            string query = "select * from TicketTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            TicketDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void fillPassenger()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("select PassId from PassengerTbl", Con);
            SqlDataReader rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("PassId", typeof(int));
            dt.Load(rdr);
            Pid.ValueMember = "PassId";
            Pid.DataSource = dt;
            Con.Close();
        }
        

        private void fillFlightCode()
        {
            Con.Open();
            SqlCommand cmd = new SqlCommand("select Fcode from FlightTbl", Con);
            SqlDataReader rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("Fcode", typeof(int));
            dt.Load(rdr);
            Fcode.ValueMember = "Fcode";
            Fcode.DataSource = dt;
            Con.Close();
        }

        string pname, ppass, pnat;

        private void fetchpassenger()
        {
            Con.Open();
            string query = "select * from PassengerTbl where PassId=" + Pid.SelectedValue.ToString() + "";
            SqlCommand cmd = new SqlCommand(@query, Con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach(DataRow dr in dt.Rows)
            {
                pname = dr["PassName"].ToString();
                ppass = dr["Passport"].ToString();
                pnat = dr["PassNat"].ToString();
                PNameTb.Text = pname;
                PPassTb.Text = ppass;
                PNatTb.Text = pnat;
            }

            Con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PNameTb.Text = "";
            PPassTb.Text = "";
            PNatTb.Text = "";
            PGen.Text = "";
            PAmtTb.Text = "";
            Tid.Text = "";
        }

        private void label10_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Tid.Text == "" || PNameTb.Text == "" )
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-4A2Q0M8;Initial Catalog=AirlineDB;Integrated Security=True"))
                    {
                        con.Open();
                        string query = "INSERT INTO TicketTbl (Tid, Fcode, PId, PName, PGen, PPass, PNation, Amt) VALUES (@Tid, @Fcode, @PId, @PName, @PGen, @PPass, @PNation, @Amt)";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@Tid", Tid.Text);
                            cmd.Parameters.AddWithValue("@Fcode", Fcode.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@PId", Pid.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@PName", PNameTb.Text);
                            cmd.Parameters.AddWithValue("@PGen", PGen.Text);
                            cmd.Parameters.AddWithValue("@PPass", PPassTb.Text);
                            cmd.Parameters.AddWithValue("@PNation", PNatTb.Text);
                            cmd.Parameters.AddWithValue("@Amt", PAmtTb.Text);
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("Ticket Booked Successfully");
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void TicketDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Hide();
        }

        private void Ticket_Load(object sender, EventArgs e)
        {
            fillPassenger();
            fillFlightCode();
            fetchpassenger();
            populate();


        }

        private void Pid_SelectionChangeCommitted(object sender, EventArgs e)
        {
            
        }
    }
}
