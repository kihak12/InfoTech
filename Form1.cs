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

        // Action lors du clic sur le bouton "Connexion" de la page d'identification
        private void button1_Click(object sender, EventArgs e)
        {
            //Connexion à la base de donnée MySql et récupération des identifiants de connexions
            MySqlConnection connexion = new MySqlConnection(Properties.Resources.connectionString);

            connexion.Open();

            string req = "select * from responsable";
            MySqlCommand command = new MySqlCommand(req, connexion);
            MySqlDataReader reader = command.ExecuteReader();

            reader.Read();
            string log = (string)reader["login"];
            string pass = (string)reader["pwd"];

            // Hashage du mot de passe entré par l'utilisateur
            string enter_pass = utils.hashPassword(password_user.Text);

            // Test du mot de pass entré Hasher, avec celui de la base de donnée 
            if (login_user.Text == log && enter_pass == pass)
            {
                // Si mot de passe valide affichage de l'interface utilisateur
                Form2 f = new Form2();
                f.Show();
                this.Hide();
            }
            else
                // Sinon affichage du texte "Identifiants Incorrects"
                label4.Visible = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
