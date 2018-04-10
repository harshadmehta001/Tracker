
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KRA.Entities;

namespace KRA.Data.Repository
{
  public  interface IAccountParameteresDal
    {
        List<AccountParameters> GetAllAccountParameters(int AccountId,string Quarter,int Year);
        bool AddAccountParameter(AccountParameters Param);
        bool UpdateParameterWeightage(int AccountParamId, int Weightage);
        string[] GetQuarters(int AccountId, int year);
        AccountParameters GetParameter(int AccountParamId);
        List<int> GetYears(int AccountId);
        AccountParameters GetParameter(int AccountId, int ParamId, string Quarter, int Year);
    }
}
