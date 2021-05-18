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

        // Initialisation de la liste des absence du personnel reconnu grâce à son ID(para)
        public void affect(int para)
        {
            initList(para);
        }

        public void initList(int id)
        {
            // Connexion à la base de donnée MySql afin de récuperer toutes les informations du personnel grâce à son ID
            MySqlConnection con = new MySqlConnection(Properties.Resources.connectionString);
            MySqlCommand command = con.CreateCommand();
            con.Open();
            command.CommandText = "SELECT PRENOM, NOM, TEL, MAIL, IDPERSONNEL FROM personnel WHERE (IDPERSONNEL LIKE @query)";
            command.Parameters.AddWithValue("@query", id + "%");

            MySqlDataReader reader = command.ExecuteReader();

            // Affichage des inforamtion du personnel
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
            // Connexion à la base de donnée MySql afin de récuperer toutes les absences du personnel sélectionner
            MySqlConnection con = new MySqlConnection(Properties.Resources.connectionString);
            MySqlCommand command = con.CreateCommand();
            con.Open();
            command.CommandText = "SELECT IDMOTIF, DATEDEBUT, DATEFIN FROM absence WHERE (IDPERSONNEL LIKE @query) ORDER BY DATEFIN DESC";
            command.Parameters.AddWithValue("@query", id + "%");

            MySqlDataReader reader = command.ExecuteReader();

            // Affichage des absences dans la listBox1
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
            // Modification des informations afficher lors du changement d'une absence sélectionner
            if (listBox1.SelectedIndex != -1)
            {
                // Récuperation de l'absence sélectionner grâce à l'ID du personnel et la date du début de l'absence sélectionner
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

                // Conversion du format de la date afficher (dd-mm-yyyy) en format date de la bdd (yyyy-mm-dd)
                DateTime dt = utils.convertDate(date_debut);


                // Connexion à la base de donnée MySql afin de récuperer les détails de l'absences sélectionner selon l'ID et la date début
                MySqlConnection con2 = new MySqlConnection(Properties.Resources.connectionString);
                MySqlCommand command2 = con2.CreateCommand();
                con2.Open();
                command2.CommandText = "SELECT * FROM absence WHERE (IDPERSONNEL LIKE @personnel) AND (DATEDEBUT LIKE '" + dt.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                command2.Parameters.AddWithValue("@personnel", id  + "%");

                MySqlDataReader reader2 = command2.ExecuteReader();
                
                // Affichage des informations de l'absence
                if (reader2.Read())
                {
                    label14.Text = reader2.GetString(1);
                    label15.Text = reader2.GetString(3);
                    label16.Text = utils.getAbsence(reader2.GetInt32(2));

                }

                // Affichage de la zone de modification de l'absence sélectionner 
                InitModifierAbsence(id, dt);

                con2.Close();
            }
        }

        // Affichage de la zone de modification de l'absence sélectionner
        private void InitModifierAbsence(string id, DateTime selected)
        {
            // Connexion à la base de donnée MySql afin de récuperer les détails de l'absences sélectionner selon l'ID et la date début
            MySqlConnection con2 = new MySqlConnection(Properties.Resources.connectionString);
            MySqlCommand command2 = con2.CreateCommand();
            con2.Open();
            command2.CommandText = "SELECT * FROM absence WHERE (IDPERSONNEL LIKE @personnel) AND (DATEDEBUT LIKE '" + selected.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            command2.Parameters.AddWithValue("@personnel", id + "%");

            MySqlDataReader reader2 = command2.ExecuteReader();

            // Affichage des informations de l'absence
            if (reader2.Read())
            {
                comboBox1.SelectedIndex = reader2.GetInt32(2)-1;
                dateTimePicker1.Value = reader2.GetDateTime(1);
                dateTimePicker2.Value = reader2.GetDateTime(3);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Controle que tous les champs du formulaire sonts rempli
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Aucune absence sélectionner");
                return;
            }
            else
            {
                // Demande une confirmation de suppression
                var result = MessageBox.Show("Voulez vous supprimer cette absence ?", "Supprimer une absence", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    // Récuperation de l'absence sélectionner grâce à l'ID du personnel et la date du début de l'absence sélectionner
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

                    DateTime dt = utils.convertDate(date_debut);

                    // Connexion à la base de donnée MySql afin de supprimer l'absences sélectionner grâce à l'ID et la date début
                    MySqlConnection con2 = new MySqlConnection(Properties.Resources.connectionString);
                    MySqlCommand command2 = con2.CreateCommand();
                    con2.Open();
                    command2.CommandText = "DELETE FROM absence WHERE (IDPERSONNEL LIKE @personnel) AND (DATEDEBUT LIKE '" + dt.ToString("yyyy-MM-dd HH:mm:ss") + "')";
                    command2.Parameters.AddWithValue("@personnel", id);

                    // Confirme la suppression
                    if (command2.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Absence Supprimer !");
                        this.Hide();
                        int id2 = int.Parse(id);
                        Form4 absence = new Form4();
                        absence.affect(id2);
                        absence.Show();
                    }
                    con2.Close();


                }
                else
                    return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Affichage de la fenêtre d'ajout d'absence
            int id = int.Parse(label_id.Text);
            Form5 add = new Form5();
            add.affect(id);
            this.Hide();
            add.Show();
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

        private void button3_Click(object sender, EventArgs e)
        {
            // Controle que tous les champs du formulaire sonts rempli
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Aucune absence sélectionner");
            }
            else if (!validateInput())
            {
                MessageBox.Show("Veuillez compléter tous les formulaire");
            }
            else if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                MessageBox.Show("Erreur, la date de début ne peut pas être suppérieur à la date de fin");
            }
            else
            {
                // Demande un confirmation de modification
                var result = MessageBox.Show("Voulez vous enregistrer ces modifications ?", "Modifier une absence", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    string motif;
                    string id = label_id.Text;
                    string date_debut = listBox1.SelectedItem.ToString();
                    DateTime date_select = utils.convertDate(date_debut);

                    if (comboBox1.SelectedIndex == 0)
                        motif = "1";
                    else if (comboBox1.SelectedIndex == 1)
                        motif = "2";
                    else if (comboBox1.SelectedIndex == 2)
                        motif = "3";
                    else
                        motif = "4";

                    // Connexion à la base de donnée MySql afin de mettre à jour les détails de l'absences sélectionner selon l'ID et la date début
                    MySqlConnection con = new MySqlConnection(Properties.Resources.connectionString);

                    string comreq = "UPDATE absence SET IDMOTIF = '" + motif + "', DATEDEBUT = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd HH:mm:ss") + "', DATEFIN = '" + dateTimePicker2.Value.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE IDPERSONNEL = '" + id + "' AND DATEDEBUT = '" + date_select.ToString("yyyy-MM-dd HH:mm:ss") + "'";

                    MySqlCommand command = new MySqlCommand(comreq, con);

                    con.Open();

                    // Confirme la modification de l'absence et retourne a la fenêtre précédante
                    if (command.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Absence Modifier");
                    }
                    Form4 renew = new Form4();
                    this.Hide();
                    renew.affect(int.Parse(id));
                    renew.Show();
                    con.Close();
                }
            }


        }
    }
}
