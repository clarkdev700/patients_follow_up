using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Optica.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<ActeMedical> ActeMedicals { get; set; }
        public DbSet<Assurance> Assurances { get; set; }
        public DbSet<ControlePat> ControlePats { get; set; }
        public DbSet<DossierPat> DossierPats { get; set; }
        public DbSet<Medecin> Medecins { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Traitement> Traitements { get; set; }
        public DbSet<TraitementActeMedical> TraitementActeMedicals { get; set; }
        public DbSet<Payement> Payements { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DossierPat>()
                .Property(d => d.Statut)
                .HasColumnName("StatutClosed");
        }
    }
}