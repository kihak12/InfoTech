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
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }
        public void affect(int para)
        {
            // Récupération de l'ID du personnel
            label_id.Text = para.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Ferme la fenêtre et actualise la page précédante
            this.Hide();
            Form4 absence = new Form4();
            absence.affect(int.Parse(label_id.Text));
            absence.Show();
        }

        // Controle que tous les champs du formulaire sonts rempli
        private bool validateInput()
        {
            if (comboBox1.SelectedIndex == -1)
                return false;
            if (dateTimePicker1.Value.ToString() == "01/01/2000 00:00:00" || dateTimePicker2.Value.ToString() == "01/01/2000 00:00:00")
                return false;

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Controle que tous les champs du formulaire sonts rempli
            if (!validateInput())
            {
                MessageBox.Show("Veuillez compléter tous les formulaire");
            }
            else if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                MessageBox.Show("Erreur, la date de début ne peut pas être suppérieur à la date de fin");
            }
            else
            {
                string motif;
                string id = label_id.Text; 

                if (comboBox1.SelectedIndex == 0)
                    motif = "1";
                else if (comboBox1.SelectedIndex == 1)
                    motif = "2";
                else if (comboBox1.SelectedIndex == 2)
                    motif = "3";
                else
                    motif = "4";

                DateTime date_debut = dateTimePicker1.Value;
                DateTime date_fin = dateTimePicker2.Value;


                // Connexion à la base de donnée MySql afin d'ajouter les détails de l'absences selon l'ID du personnel dans la base de donnée
                MySqlConnection con = new MySqlConnection(Properties.Resources.connectionString);
                string req = "INSERT INTO absence (IDPERSONNEL, DATEDEBUT, IDMOTIF, DATEFIN) VALUES ('" + id + "','" + date_debut.ToString("yyyy-MM-dd HH:mm:ss") + "','" + motif + "','" + date_fin.ToString("yyyy-MM-dd HH:mm:ss") + "')";


                MySqlCommand command = new MySqlCommand(req, con);

                con.Open();

                // Confirmation de l'ajout de l'absence
                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Absence ajouter avec succès");
                }

                con.Close();

                // Ferme la fenêtre et actualise la page précédante
                this.Hide();
                Form4 absence = new Form4();
                absence.affect(int.Parse(label_id.Text));
                absence.Show();
            }
        }
    }
}
