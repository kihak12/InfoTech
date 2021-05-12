using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;

namespace InfoTech
{
    public class utils
    {
        public static string hashPassword(string password)
        {
            SHA256CryptoServiceProvider sha1 = new SHA256CryptoServiceProvider();

            byte[] password_bytes = Encoding.ASCII.GetBytes(password);
            byte[] encrypted_bytes = sha1.ComputeHash(password_bytes);
            return Convert.ToBase64String(encrypted_bytes);
        }

        public static string getAbsence(int value)
        {
            string motif;
            if (value == 1)
                motif = "vacances";
            else if (value == 2)
                motif = "maladie";
            else if (value == 1)
                motif = "motif familial";
            else
            {
                motif = "congé parental";
            }

            return motif;
        }
        
}
}
