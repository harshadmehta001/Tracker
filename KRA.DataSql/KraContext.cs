using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using KRA.Entities;


namespace KRA.DataSql
{
    class KraContext : DbContext
    {
        public KraContext() : base("DbConnection2") {

        }

        public DbSet<Accounts> Account { get; set; }
        public DbSet<AccountParameters>  AccountParameter{get; set;}
        public DbSet<AccountTeamSize> TeamSize { get; set; }
        public DbSet<KraInputScores> Score { get; set; }
        public DbSet<KraParameters> Params { get; set; }
        public DbSet<ParameterBounds> Bounds { get; set; }
        public DbSet<ReportLog> Reportlogs { get; set; }

    }
}
