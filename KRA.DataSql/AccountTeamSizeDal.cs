using KRA.Data.Repository;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KRA.Entities;

namespace KRA.DataSql
{
  public  class AccountTeamSizeDal : IAccountTeamSizeDal
    {
        public bool AddTeamSize(AccountTeamSize TeamSize)
        {
            using (KraContext Context = new KraContext()) {
                AccountTeamSize size = (from teamsize in Context.TeamSize where teamsize.AccountId == TeamSize.AccountId && teamsize.Quarter == TeamSize.Quarter select teamsize).SingleOrDefault();
                if (size != null)
                {
                    size.TeamSize = TeamSize.TeamSize;

                    Context.SaveChanges();

                }
                else {
                    Context.TeamSize.Add(TeamSize);
                    Context.SaveChanges();
                }
            }
            return true;
        }

        public AccountTeamSize GetTeamSize(int AccountId, string Quarter, int Year)
        {
            AccountTeamSize TeamSize;
            using(KraContext Context = new KraContext())
            {
                TeamSize = (from teamsize in Context.TeamSize where teamsize.AccountId == AccountId && teamsize.Quarter == Quarter && teamsize.Year == Year select teamsize).FirstOrDefault();

            }
            return TeamSize;
        }

    }
}
