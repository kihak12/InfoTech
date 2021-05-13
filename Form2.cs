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

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Aucun personnel sélectionner");
                return;
            }
            else
            {
                var result = MessageBox.Show("Voulez vous supprimer ce personnel ?\n\rCette action éffacera également les absences de ce personnel.", "Supprimer un personnel", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    int personnel_id = 0;
                    try
                    {
                        personnel_id = ((personnel)listBox1.SelectedItem).getID();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("erreur");
                    }

                    MySqlConnection con2 = new MySqlConnection(Properties.Resources.connectionString);
                    MySqlCommand command2 = con2.CreateCommand();
                    con2.Open();
                    command2.CommandText = "DELETE FROM absence WHERE IDPERSONNEL = @id_personnel";
                    command2.Parameters.AddWithValue("@id_personnel", personnel_id);

                    command2.ExecuteNonQuery();

                    MySqlConnection con = new MySqlConnection(Properties.Resources.connectionString);
                    MySqlCommand command = con.CreateCommand();
                    con.Open();
                    command.CommandText = "DELETE FROM personnel WHERE IDPERSONNEL = @id_personnel";
                    command.Parameters.AddWithValue("@id_personnel", personnel_id);

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

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

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

                MySqlConnection con = new MySqlConnection(Properties.Resources.connectionString);
                MySqlCommand command = con.CreateCommand();
                con.Open();
                command.CommandText = "SELECT IDMOTIF, DATEDEBUT FROM absence WHERE (IDPERSONNEL LIKE @query) ORDER BY DATEFIN DESC";
                command.Parameters.AddWithValue("@query", personnel_id + "%");

                MySqlDataReader reader = command.ExecuteReader();

                listBox2.Items.Clear();
                while (reader.Read())
                {
                    string motif = utils.getAbsence(reader.GetInt32(0));
                    listBox2.Items.Add(motif);
                }

                con.Close();

                MySqlConnection con2 = new MySqlConnection(Properties.Resources.connectionString);
                MySqlCommand command2 = con2.CreateCommand();
                con2.Open();
                command2.CommandText = "SELECT NOM, PRENOM, TEL, MAIL, IDSERVICE FROM personnel WHERE (IDPERSONNEL LIKE @query)";
                command2.Parameters.AddWithValue("@query", personnel_id + "%");

                MySqlDataReader reader2 = command2.ExecuteReader();

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
            if (!validateInput())
            {
                MessageBox.Show("Veuillez remplir tous les champs");
                return;
            }
            else
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

                int service = 0;

                if (comboBox1.SelectedIndex == 0)
                    service = 1;
                else if (comboBox1.SelectedIndex == 1)
                    service = 2;
                else if (comboBox1.SelectedIndex == 2)
                    service = 3;

                    MySqlConnection con = new MySqlConnection(Properties.Resources.connectionString);

                string comreq = "UPDATE personnel SET IDSERVICE = '" + service + "', NOM ='" + textBox2.Text + "', PRENOM = '" + textBox3.Text + "', TEL = '" + textBox4.Text + "', MAIL = '" + textBox5.Text + "' WHERE IDPERSONNEL = '" + personnel_id + "'";

                MySqlCommand command = new MySqlCommand(comreq, con);

                con.Open();


                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Compte Modifier");
                    updateList("");
                }
                con.Close();
            }
        }
    }
}
