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
            // Connexion à la base de donnée MySql afin de récuperer tout les personnel présent dans la base de donnée
            MySqlConnection con = new MySqlConnection(Properties.Resources.connectionString);
            MySqlCommand command = con.CreateCommand();
            con.Open();
            command.CommandText = "SELECT * FROM personnel WHERE (PRENOM LIKE @query OR NOM LIKE @query) ORDER BY IDPERSONNEL";
            command.Parameters.AddWithValue("@query", query + "%");

            MySqlDataReader reader = command.ExecuteReader();

            // Ajout des personnels dans la listBox1
            listBox1.Items.Clear();
            while (reader.Read())
                listBox1.Items.Add(new personnel(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), reader.GetString(5)));

            con.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Initialisation de la liste des personnels lors de l'ouverture de la page
            updateList("");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Modification des résultat de la liste des personnel lorsque un utilisateur est rechercher depuis le textBox1
            updateList(textBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Affichage de la fenetre d'ajout d'un personnel et fermeture de cele précédante.
            Form3 addPersonnel = new Form3();
            addPersonnel.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Suppression du personnel sélectionner, affichage d'une erreur si aucun personnel n'est sélectionner.
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Aucun personnel sélectionner");
                return;
            }
            else
            {
                var result = MessageBox.Show("Voulez vous supprimer ce personnel ?\n\rCette action éffacera également les absences de ce personnel.", "Supprimer un personnel", MessageBoxButtons.YesNo);
                // Demande de confirmation de suppression de l'utilisateur
                if (result == DialogResult.Yes)
                {
                    int personnel_id = 0;
                    // Récupération de l'ID du personnel sélectionner 
                    try
                    {
                        personnel_id = ((personnel)listBox1.SelectedItem).getID();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("erreur");
                    }

                    //Connexion à la base de donnée MySql
                    MySqlConnection con2 = new MySqlConnection(Properties.Resources.connectionString);
                    MySqlCommand command2 = con2.CreateCommand();
                    con2.Open();
                    command2.CommandText = "DELETE FROM absence WHERE IDPERSONNEL = @id_personnel";
                    command2.Parameters.AddWithValue("@id_personnel", personnel_id);

                    // Suppression des absences du personnel sélectionner
                    command2.ExecuteNonQuery();

                    MySqlConnection con = new MySqlConnection(Properties.Resources.connectionString);
                    MySqlCommand command = con.CreateCommand();
                    con.Open();
                    command.CommandText = "DELETE FROM personnel WHERE IDPERSONNEL = @id_personnel";
                    command.Parameters.AddWithValue("@id_personnel", personnel_id);

                    // Suppression de l'utilisateur sélectionner
                    if (command.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Compte Supprimer");
                        this.Hide();
                        Form2 addPersonnel = new Form2();
                         addPersonnel.Show();
                    }
                    con2.Close();

                    
                }
                else
                    return;
            }
        }

        public void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Mise a jour des informations afficher lors d'un changement de personnel sélectionner  
            if(listBox1.SelectedIndex != -1)
            {
                int personnel_id = 0;
                try
                {
                    personnel_id = ((personnel)listBox1.SelectedItem).getID();
                }
                catch (Exception)
                {
                    MessageBox.Show("erreur");
                    return;
                }

                // Connexion à la base de donnée MySql afin de récuperer les absences ainsi que la "DATEDEBUT" du personnel sélectionner
                MySqlConnection con = new MySqlConnection(Properties.Resources.connectionString);
                MySqlCommand command = con.CreateCommand();
                con.Open();
                command.CommandText = "SELECT IDMOTIF, DATEDEBUT FROM absence WHERE (IDPERSONNEL LIKE @query) ORDER BY DATEFIN DESC";
                command.Parameters.AddWithValue("@query", personnel_id + "%");

                MySqlDataReader reader = command.ExecuteReader();

                // Ajout des absence du personnel sélectionner dans la listBox2
                listBox2.Items.Clear();
                while (reader.Read())
                {
                    string motif = utils.getAbsence(reader.GetInt32(0));
                    listBox2.Items.Add(motif);
                }

                con.Close();

                // Connexion à la base de donnée MySql afin de récuperer toutes les informations du personnel sélectionner
                MySqlConnection con2 = new MySqlConnection(Properties.Resources.connectionString);
                MySqlCommand command2 = con2.CreateCommand();
                con2.Open();
                command2.CommandText = "SELECT NOM, PRENOM, TEL, MAIL, IDSERVICE FROM personnel WHERE (IDPERSONNEL LIKE @query)";
                command2.Parameters.AddWithValue("@query", personnel_id + "%");

                MySqlDataReader reader2 = command2.ExecuteReader();

                // Affichage des informations du personnel sélectionner
                if (reader2.Read())
                {
                    textBox2.Text = reader2.GetString(0);
                    textBox3.Text = reader2.GetString(1);
                    textBox4.Text = reader2.GetString(2);
                    textBox5.Text = reader2.GetString(3);

                    if (reader2.GetInt32(4) == 1)
                        comboBox1.SelectedIndex = 0;
                    else if (reader2.GetInt32(4) == 2)
                        comboBox1.SelectedIndex = 1;
                    else if (reader2.GetInt32(4) == 3)
                        comboBox1.SelectedIndex = 2;
                    else
                        comboBox1.SelectedIndex = -1;
                }

                con2.Close();
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        // Controle que tous les champs du formulaire sonts remplie
        private bool validateInput()
        {
            if (textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "")
                return false;
            if (comboBox1.SelectedIndex < 0)
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
            }else if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez saisir un personnel a modifier ou ajouter en un nouveau");
                return;
            }
            // Si tous les champs du formulaire sonts rempli demande un confirmation
            else
            {
                var result = MessageBox.Show("Voulez vous enregistrer ces modifications ?", "Modifier un personnel", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    int personnel_id = 0;
                    try
                    {
                        // Récupère l'ID du personnel sélectionner
                        personnel_id = ((personnel)listBox1.SelectedItem).getID();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("erreur");
                        return;
                    }

                    int service = 0;

                    if (comboBox1.SelectedIndex == 0)
                        service = 1;
                    else if (comboBox1.SelectedIndex == 1)
                        service = 2;
                    else if (comboBox1.SelectedIndex == 2)
                        service = 3;

                    // Connexion à la base de donnée MySql afin de modifier les informations du personnel avec les valeurs entrer
                    MySqlConnection con = new MySqlConnection(Properties.Resources.connectionString);

                    string comreq = "UPDATE personnel SET IDSERVICE = '" + service + "', NOM ='" + textBox2.Text + "', PRENOM = '" + textBox3.Text + "', TEL = '" + textBox4.Text + "', MAIL = '" + textBox5.Text + "' WHERE IDPERSONNEL = '" + personnel_id + "'";

                    MySqlCommand command = new MySqlCommand(comreq, con);

                    con.Open();


                    // Confirme la modification et met à jour la liste
                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Compte Modifier");
                        updateList("");
                    }
                    con.Close();
                }
                else
                    return;
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Controle qu'un personnel est sélectionner
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez sélectionner un personnel");
                return;
            }
            else
            {
                // Affiche la page des absences du personnel séletionner
                Form4 absence = new Form4();
                absence.affect(((personnel)listBox1.SelectedItem).getID());
                absence.Show();
            }            
        }
    }
}
