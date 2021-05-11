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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlConnection connexion = new MySqlConnection(Properties.Resources.connectionString);

            connexion.Open();

            string req = "select * from responsable";
            MySqlCommand command = new MySqlCommand(req, connexion);
            MySqlDataReader reader = command.ExecuteReader();

            reader.Read();
            string log = (string)reader["login"];
            string pass = (string)reader["pwd"];

            string enter_pass = utils.hashPassword(password_user.Text);

            if (login_user.Text == log && enter_pass == pass)
            {
                Form2 f = new Form2();
                f.Show();
                this.Hide();
            }
            else
                label4.Visible = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
