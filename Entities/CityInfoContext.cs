using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filenet.Apis.Entities
{
    public class CityInfoContext : DbContext
    {
        public CityInfoContext(DbContextOptions<CityInfoContext> options)
           : base(options)
        {
            //Database.EnsureCreated();  //creates database

              Database.Migrate();  //handles changes to database --- remember this is a code first model.  change db from one version to another
            //Add-Migration CityInfoDbInitialMigration  run at command line.  Will create the necessary folders "Migrations"
            //http://localhost:23859/api/testdatabase
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointsOfInterest { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("connectionstring");

        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
