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
using System.Text.RegularExpressions;

namespace AirplaneApplication
{
    public partial class ViewFlights : Form
    {
        public ViewFlights()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=DESKTOP-4A2Q0M8;Initial Catalog=AirlineDB;Integrated Security=True");
        private void populate()
        {
            Con.Open();
            string query = "select * from FlightTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            FlightDGV.DataSource = ds.Tables[0];
            Con.Close();
        }

        private void ViewFlights_Load(object sender, EventArgs e)
        {
            populate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FlightTbl Addfl = new FlightTbl();
            Addfl.Show();
            this.Hide();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FcodeTb.Text = "";
            Fsrc.Text = "";
            FDest.Text = "";
            FDate.Value = DateTimePicker.MinimumDateTime;
            SeatNum.Text = "";

        }

        private void FlightDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            FcodeTb.Text = FlightDGV.Rows[e.RowIndex].Cells[0].Value.ToString();
            Fsrc.Text = FlightDGV.Rows[e.RowIndex].Cells[1].Value?.ToString();
            FDest.Text = FlightDGV.Rows[e.RowIndex].Cells[2].Value?.ToString();
            FDate.Text = FlightDGV.Rows[e.RowIndex].Cells[3].Value?.ToString();
            SeatNum.Text = FlightDGV.Rows[e.RowIndex].Cells[0].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (FcodeTb.Text == "" || Fsrc.Text == "" || FDest.Text == "" || SeatNum.Text == "")
            {
                MessageBox.Show("Missing information");
            }
            else
            {
                try
                {
                    Con.Open();
                    // Corrected and parameterized SQL Update Query
                    string query = "UPDATE FlightTbl SET Fsrc=@Fsrc, FDest=@FDest, FCap=@FCap WHERE Fcode=@Fcode";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    // Adding parameters to avoid SQL Injection
                    cmd.Parameters.AddWithValue("@Fcode", int.Parse(FcodeTb.Text)); // Assuming Fcode is an int
                    cmd.Parameters.AddWithValue("@Fsrc", Fsrc.Text); // Assuming Fsrc is a TextBox
                    cmd.Parameters.AddWithValue("@FDest", FDest.Text); // Assuming FDest is a TextBox
                    cmd.Parameters.AddWithValue("@FCap", SeatNum.Text); // Assuming FCap corresponds to SeatNum

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Flight updated successfully");
                    Con.Close();
                    populate();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (FcodeTb.Text == "")
            {
                MessageBox.Show("Enter the flight to delete");
            }
            else
            {
                try
                {
                    Con.Open();
                    string query = "delete from FlightTbl where Fcode=" + FcodeTb.Text + ";";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Flight deleted successfully");
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