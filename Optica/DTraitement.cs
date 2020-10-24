using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optica
{
    public  class DTraitement
    {
        public string Title(string prenom)
        {
            string[] ch = prenom.Split(' ');
            string chfinal = "";
            int chLength =  ch.Count();
            if (chLength > 0)
            {
                for (int i = 0; i < chLength; i++) 
                {
                    string x = ch[i].ToLower();
                    chfinal +=" "+ x[0].ToString().ToUpper() + x.Substring(1);  //string.Format(chfinal, ch[0].ToUpper());
                }
            }
            return chfinal;
        }


        public string getNumeroMatricule(string nom, string prenom)
        {
            //var r = new Random();
            //var num = r.Next(2, 9);
            var dx = DateTime.Now;
            int dy = int.Parse(dx.Year.ToString().Substring(2));
            string ch1 = (dx.Day + dx.Month + dy) + dx.ToString("Hms") ;
            char[] delimiter = new char[] { ' ', '-','.','\'' };
            var n = nom.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            var p = prenom.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            string chaine = null;
            int lg = n.Count();
            if (lg > 0)
            {
                foreach (var i in n)
                {
                    chaine += i.Substring(0, 1);
                }
            }
            if (p.Count() > 0)
                chaine += p[0].Substring(0, 1).ToUpper();
            //num += 7;
            return ch1 + chaine;
        }
    }
}