using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace KRA.DataSql
{
    class KraDbInitialiazer :CreateDatabaseIfNotExists<KraContext>
    {
        protected override void Seed(KraContext context)
        {
            base.Seed(context);
        }
    }
}
