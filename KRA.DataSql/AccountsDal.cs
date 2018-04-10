using KRA.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KRA.Entities;

namespace KRA.DataSql
{
   public class AccountsDal : IAccountsDal

    {
        public bool AddAccount(Accounts account)
        {
            using (KraContext Context = new KraContext()) {
                
                Context.Account.Add(account);
                Context.SaveChanges();
            }
            return true;
        }

        public Accounts GetAccount(int accountid)
        {
            Accounts Account;
            using (KraContext Context = new KraContext())
            {
            Account = Context.Account.Find(accountid);
            }
            return Account;
        }

        public List<Accounts> GetAllAccounts()
       {
            List<Accounts> AccountList;
            using (KraContext Context = new KraContext()) {
                AccountList = (from accounts in Context.Account select accounts).ToList();
            }
            return AccountList;
        }

        public List<Accounts> GetAllAccounts(int ADMId)
        {
            List<Accounts> AccountList;
            using (KraContext Context = new KraContext())
            {
                AccountList = (from accounts in Context.Account where accounts.ManagerID ==ADMId select accounts).ToList();
            }
            return AccountList;
        }
    }
}
