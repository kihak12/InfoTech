using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoTech
{
    public class personnel
    {
        int id, service;
        string nom, prenom, tel, mail;

        public personnel(int id, int service, string nom, string prenom, string tel, string mail)
        {
            this.id = id;
            this.service = service;
            this.nom = nom;
            this.prenom = prenom;
            this.tel = tel;
            this.mail = mail;
        }

        // Renvoie le nom + prenom du personnel courrant
        public override string ToString()
        {
            return nom + " - " + prenom;
        }

        // Renvoie l'ID du personnel courrant
        public int getID(){ return id; }
    }
}
