using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace InfoTech
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void updateList(string query)
        {
            MySqlConnection con = new MySqlConnection(Properties.Resources.connectionString);
            MySqlCommand command = con.CreateCommand();
            con.Open();
            command.CommandText = "SELECT * FROM personnel WHERE (PRENOM LIKE @query OR NOM LIKE @query) ORDER BY IDPERSONNEL";
            command.Parameters.AddWithValue("@query", query + "%");

            MySqlDataReader reader = command.ExecuteReader();

            listBox1.Items.Clear();
            while (reader.Read())
                listBox1.Items.Add(new personnel(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5)));

            con.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            updateList("");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            updateList(textBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 addPersonnel = new Form3();
            addPersonnel.Show();
            this.Hide();
        }
    }
}
