using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Optica.Models
{
    public class Stats
    {
    }
    public class AnalyseStats
    {
        public string Acte { get; set; }
        public int Qte { get; set; }
        public double Vpourcent { get; set; }
    }

    public class DetteStats
    {
        public string Origine { get; set; }
        public float Montant { get; set; }
        public double VPourcent { get; set; }
    }

    public class datePlage
    {
        public string Jour { get; set; }
        public DateTime? Deb { get; set; }
        public DateTime? End { get; set; }
    }

    public class DayGroupStat
    {
        public string Jour { get; set; }
        public AnalyseStats V1 { get; set; }
        public AnalyseStats V2 { get; set; }
        public AnalyseStats V3 { get; set; }
        public AnalyseStats V4 { get; set; }
        public AnalyseStats V5 { get; set; }
    }

    public class AssurDette
    {
        public string NomAssur { get; set; }
        public string CodeAssur { get; set; }
        public double Montant { get; set; }
        public double Taux { get; set; }
    }
}