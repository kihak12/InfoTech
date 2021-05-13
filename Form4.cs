using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace InfoTech
{
    public partial class Form4 : Form
    {


        public Form4()
        {
            InitializeComponent();
        }

        public void affect(int para)
        {
            initList(para);
        }

        public void initList(int id)
        {
            MySqlConnection con = new MySqlConnection(Properties.Resources.connectionString);
            MySqlCommand command = con.CreateCommand();
            con.Open();
            command.CommandText = "SELECT PRENOM, NOM, TEL, MAIL, IDPERSONNEL FROM personnel WHERE (IDPERSONNEL LIKE @query)";
            command.Parameters.AddWithValue("@query", id + "%");

            MySqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                label8.Text = reader.GetString(0);
                label9.Text = reader.GetString(1);
                label10.Text = reader.GetString(2);
                label11.Text = reader.GetString(3); 
                label_id.Text = reader.GetString(4);
                listAbsence(id);
            }
            con.Close();
        }

        public void listAbsence(int id)
        {
            MySqlConnection con = new MySqlConnection(Properties.Resources.connectionString);
            MySqlCommand command = con.CreateCommand();
            con.Open();
            command.CommandText = "SELECT IDMOTIF, DATEDEBUT, DATEFIN FROM absence WHERE (IDPERSONNEL LIKE @query) ORDER BY DATEDEBUT";
            command.Parameters.AddWithValue("@query", id + "%");

            MySqlDataReader reader = command.ExecuteReader();

            listBox1.Items.Clear();
            while (reader.Read())
            {
                string motif = utils.getAbsence(reader.GetInt32(0));
                listBox1.Items.Add(reader.GetMySqlDateTime(1));
            }

            con.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                string id;
                string date_debut;
                try
                {
                    id = label_id.Text;
                    date_debut = listBox1.SelectedItem.ToString();
                }
                catch (Exception)
                {
                    MessageBox.Show("erreur");
                    return;
                }

                DateTime dt = DateTime.ParseExact(date_debut, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                Console.WriteLine(dt.ToString("yyyy-MM-dd HH:mm:ss"));


                MySqlConnection con2 = new MySqlConnection(Properties.Resources.connectionString);
                MySqlCommand command2 = con2.CreateCommand();
                con2.Open();
                command2.CommandText = "SELECT * FROM absence WHERE (IDPERSONNEL LIKE @personnel) AND (DATEDEBUT LIKE '" + dt.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                command2.Parameters.AddWithValue("@personnel", id  + "%");

                MySqlDataReader reader2 = command2.ExecuteReader();

                if (reader2.Read())
                {
                    label14.Text = reader2.GetString(1);
                    label15.Text = reader2.GetString(3);
                    label16.Text = utils.getAbsence(reader2.GetInt32(2));

                }

                con2.Close();
            }
        }
    }
}
