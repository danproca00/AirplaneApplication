using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AirplaneApplication
{
    public partial class ViewPassenger : Form
    {
        public ViewPassenger()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-4A2Q0M8;Initial Catalog=AirlineDB;Integrated Security=True");

        private void populate()
        {
            Con.Open();
            string query = "select * from PassengerTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            PassengerDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void ViewPassenger_Load(object sender, EventArgs e)
        {
            populate();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddPassenger addpas = new AddPassenger();
            addpas.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(PidTb.Text == "")
            {
                MessageBox.Show("Enter the passenger to delete");
            } else
            {
                try
                {
                    Con.Open();
                    string query = "delete from Passengertbl where PassId=" + PidTb.Text + ";";
                    SqlCommand cmd = new SqlCommand(query,Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Passenger deleted successfully");
                    Con.Close();
                    populate();
                } catch(Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void PassengerDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the click is on a valid row (to exclude header row clicks)
            if (e.RowIndex >= 0)
            {
                // Access the cells directly from the clicked row using e.RowIndex
                PidTb.Text = PassengerDGV.Rows[e.RowIndex].Cells[0].Value.ToString();
                PnameTb.Text = PassengerDGV.Rows[e.RowIndex].Cells[1].Value.ToString();
                PpassTb.Text = PassengerDGV.Rows[e.RowIndex].Cells[2].Value.ToString();
                PaddTb.Text = PassengerDGV.Rows[e.RowIndex].Cells[3].Value.ToString();
                natcb.SelectedItem = PassengerDGV.Rows[e.RowIndex].Cells[4].Value?.ToString();
                GendCb.SelectedItem = PassengerDGV.Rows[e.RowIndex].Cells[5].Value?.ToString();
                PphoneTb.Text = PassengerDGV.Rows[e.RowIndex].Cells[6].Value?.ToString();
            }
            else
            {
                {
                    MessageBox.Show("Please select a row first.");
                }
            }

        }
        private void button4_Click(object sender, EventArgs e)
        {
            PidTb.Text = "";
            PnameTb.Text = "";
            PpassTb.Text = "";
            PaddTb.Text = "";
            natcb.SelectedItem = "";
            GendCb.SelectedItem = "";
            PphoneTb.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (PidTb.Text == "" || PnameTb.Text == "" || PpassTb.Text == "" || PaddTb.Text == "")
            {
                MessageBox.Show("Missing information");
            }
            else
            {
                try
                {
                    Con.Open();
                    // Corrected and parameterized SQL Update Query
                    string query = "UPDATE PassengerTbl SET PassName=@PassName, Passport=@Passport, PassAd=@PassAd, PassNat=@PassNat, PassGend=@PassGend, PassPhone=@PassPhone WHERE PassId=@PassId";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    // Adding parameters to avoid SQL Injection
                    cmd.Parameters.AddWithValue("@PassName", PnameTb.Text);
                    cmd.Parameters.AddWithValue("@Passport", PpassTb.Text);
                    cmd.Parameters.AddWithValue("@PassAd", PaddTb.Text);
                    cmd.Parameters.AddWithValue("@PassNat", natcb.SelectedItem?.ToString()); // Handling possible null selections
                    cmd.Parameters.AddWithValue("@PassGend", GendCb.SelectedItem?.ToString()); // Handling possible null selections
                    cmd.Parameters.AddWithValue("@PassPhone", PphoneTb.Text);
                    cmd.Parameters.AddWithValue("@PassId", PidTb.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Passenger updated successfully");
                    Con.Close();
                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

    }
}
