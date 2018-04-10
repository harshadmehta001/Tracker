using KRA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRA.Data.Repository
{
   public interface IAccountTeamSizeDal

    {
        bool AddTeamSize(AccountTeamSize TeamSize);
        AccountTeamSize GetTeamSize(int AccountId, string Quarter, int Year);
    }
}
