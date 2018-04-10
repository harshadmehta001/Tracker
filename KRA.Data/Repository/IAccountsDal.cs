using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KRA.Entities;

namespace KRA.Data.Repository
{
    public interface IAccountsDal
    {
        bool AddAccount(Accounts account);
        Accounts GetAccount(int accountid);
        List<Accounts> GetAllAccounts();
        List<Accounts> GetAllAccounts(int ADMId);

    }
}
