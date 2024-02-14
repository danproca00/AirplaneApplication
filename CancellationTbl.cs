using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AirplaneApplication
{
    public partial class CancellationTbl : Form
    {
        // Define the connection string once to use throughout the class
        private readonly string connectionString = @"Data Source=DESKTOP-4A2Q0M8;Initial Catalog=AirlineDB;Integrated Security=True";

        public CancellationTbl()
        {
            InitializeComponent();
        }

        private void CancellationTbl_Load(object sender, EventArgs e)
        {
            fillTicket();
            populate();
            // Removed fetchfcode() from here to prevent it from being called before a ticket is selected
        }

        private void fillTicket()
        {
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                var cmd = new SqlCommand("SELECT TId FROM TicketTbl", con);
                var rdr = cmd.ExecuteReader();
                var dt = new DataTable();
                dt.Columns.Add("TId", typeof(int));
                dt.Load(rdr);
                TidCb.DisplayMember = "TId"; // Ensure this is set if you want to display TId in the ComboBox
                TidCb.ValueMember = "TId";
                TidCb.DataSource = dt;
            } // Using block ensures connection is closed
        }

        private void TidCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Call fetchfcode when a new ticket ID is selected
            fetchfcode();
        }

        private void fetchfcode()
        {
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT * FROM TicketTbl WHERE TId = @TId";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@TId", TidCb.SelectedValue);
                    var dt = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    foreach (DataRow dr in dt.Rows)
                    {
                        FcodeTb.Text = dr["Fcode"].ToString();
                    }
                }
            }
        }

        private void populate()
        {
            using (var con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT * FROM CancelTbl";
                SqlDataAdapter sda = new SqlDataAdapter(query, con);
                var ds = new DataSet();
                sda.Fill(ds);
                CancelDGV.DataSource = ds.Tables[0];
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Hide();
        }

        private void deleteTicket()
        {
            try
            {
                using (var con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string query = "DELETE FROM TicketTbl WHERE Tid = @Tid";
                    using (var cmd = new SqlCommand(query, con))
                    {
                        // Ensure the parameter value is what you expect
                        var ticketId = TidCb.SelectedValue;
                        Console.WriteLine("Attempting to delete ticket with ID: " + ticketId); // Use logging or debugging to check this value

                        cmd.Parameters.AddWithValue("@Tid", ticketId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Flight deleted successfully");
                        }
                        else
                        {
                            MessageBox.Show("No flight was deleted. Check if the ticket ID exists.");
                        }
                    }
                }
                populate(); // Refresh data
            }
            catch (Exception Ex)
            {
                MessageBox.Show("An error occurred: " + Ex.Message);
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CanId.Text) || string.IsNullOrWhiteSpace(FcodeTb.Text))
            {
                MessageBox.Show("Missing Information");
            }
            else
            {
                try
                {
                    using (var con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        string query = "INSERT INTO CancelTbl (CancId, TicId, Flcode, CancDate) VALUES (@CancId, @TicId, @Flcode, @CancDate)";
                        using (var cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@CancId", CanId.Text);
                            cmd.Parameters.AddWithValue("@TicId", TidCb.SelectedValue);
                            cmd.Parameters.AddWithValue("@Flcode", FcodeTb.Text);
                            cmd.Parameters.AddWithValue("@CancDate", CancDate.Value.Date);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Ticket Canceled Successfully");
                        }
                    }
                    populate(); 
                    deleteTicket();
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CanId.Text = "";
            FcodeTb.Text = "";
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Add event handler hookups in the form designer for events like Load, Click, SelectedIndexChanged, etc.
    }
}
