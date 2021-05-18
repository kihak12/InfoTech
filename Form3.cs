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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 addPersonnel = new Form2();
            addPersonnel.Show();
        }

        // Controle que tous les champs du formulaire sonts rempli
        private bool validateInput()
        {
            if (inputNom.Text == "" || inputPrenom.Text == "" || inputMail.Text == "" || inputTel.Text == "")
                return false;
            if (listService.SelectedIndex < 0)
                return false;
            
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Controle que tous les champs du formulaire sonts rempli
            if (!validateInput())
            {
                MessageBox.Show("Veuillez remplir tous les champs");
                return;
            }

            string service;

            if (listService.SelectedIndex == 0)
                service = "1";
            else if (listService.SelectedIndex == 1)
                service = "2";
            else
                service = "3";

            // Connexion à la base de donnée MySql afin d'ajouter le personnel créer avec les informations entrer par l'utilisateur
            MySqlConnection con = new MySqlConnection(Properties.Resources.connectionString);
            string req = "INSERT INTO personnel (IDSERVICE, NOM, PRENOM, TEL, MAIL) VALUES ('" + service + "','" + inputNom.Text + "','" + inputPrenom.Text + "','" + inputTel.Text + "','" + inputMail.Text + "')";


            MySqlCommand command = new MySqlCommand(req, con);

            con.Open();


            // Confirmation de création du compte
            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Compte Créer");
            }

            con.Close();
            // Retour a la page d'acceuil de gestion du personnel
            this.Hide();
            Form2 addPersonnel = new Form2();
            addPersonnel.Show();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
