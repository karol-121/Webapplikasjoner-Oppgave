using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Webapplication.Models
{
    public class Cruise
    {
        public int Id { get; set; }
        virtual public Route Route { get; set; }
        public int Departure_Weekday { get; set; }
        public int Departure_Daytime { get; set; }
        public int Max_Passengers { get; set; }
        public int Passeger_Price { get; set; }
        public int Pet_Price { get; set; }
        public int Vehicle_Price { get; set; }
    }

    public class Route
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; } //Finne løsningen slikk at id blir ikke nødvendig. I dette tilfelle kan orgin og destination felt til sammen bli en primar key.
        public string Origin { get; set; }
        public string Destination { get; set; }
    }
    public class CruiseContext : DbContext
    {
        public CruiseContext (DbContextOptions<CruiseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Cruise> Cruises { get; set; }

        public DbSet<Route> Routes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
        
    }
}
