using KRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRA.Domain.Contract
{
    public interface IAccountTeamSizeService
    {
        bool UpdateTeamSize(AccountTeamSizeModel TeamSize);
        AccountTeamSizeModel GetTeamSize(int AccountId, string Quarter, int Year);
    }
}
