using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace InfoTech
{
    public class utils
    {
        //Hash le mot de passe entrer par l'utilisateur
        public static string hashPassword(string password)
        {
            SHA256CryptoServiceProvider sha1 = new SHA256CryptoServiceProvider();

            byte[] password_bytes = Encoding.ASCII.GetBytes(password);
            byte[] encrypted_bytes = sha1.ComputeHash(password_bytes);
            return Convert.ToBase64String(encrypted_bytes);
        }

        // Retourne l'absence du personnel
        public static string getAbsence(int value)
        {
            string motif;
            if (value == 1)
                motif = "vacances";
            else if (value == 2)
                motif = "maladie";
            else if (value == 3)
                motif = "motif familial";
            else
            {
                motif = "congé parental";
            }

            return motif;
        }

        // Conversion du format de la date(dd-mm-yyyy) en format date de la bdd (yyyy-mm-dd)
        public static DateTime convertDate(string date)
        {
            DateTime dt = DateTime.ParseExact(date, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            return dt;
        }

    }
}
