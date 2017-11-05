using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitorLibrary.Model
{
    public class MonitoringDB : DbContext
    {
       // public MonitoringDB() : base("MonitorDB")
        public MonitoringDB() : base("MonitorDB")
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<LicenceKey> LicenceKeys { get; set; }
        public DbSet<UsersGroup> UsersGroups { get; set; }
        
        public static void ReCreareDB()
        {
            var db = new MonitoringDB();

            if (db.Database.Exists())
                db.Database.Delete();
            db.Database.Create();
        }        
    }
}
