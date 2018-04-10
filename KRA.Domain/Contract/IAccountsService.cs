using KRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRA.Domain.Contracts
{
   public interface IAccountsService
    {
        bool AddAccount(AccountsModel Account);
         List<AccountsModel> GetAllAccounts();
        List<AccountsModel> GetAllAccounts(int Admid);
        AccountsModel GetAccount(int AccountId);

    }
}
