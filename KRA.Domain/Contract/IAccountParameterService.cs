using KRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRA.Domain.Contract
{
    public interface IAccountParameterService
    {
         bool AddAccountParam(AccountParametersModel AccountParameter);
         List<AccountParametersModel> GetAccountParameters(int AccountId, string Quarter, int Year);
         bool UpdateParameterWeightage(int AccountParameterId, int Weightage);
        string[] GetQuarters(int AccountId, int Year);
        AccountParametersModel GetParameter(int AccountParameterId);
        List<AccountParametersModel> GetParameters(int AccountId, int Year);
        List<int> GetYears(int AccountId);
        AccountParametersModel GetParameter(int AccountId, int ParamId, string Quarter,int Year);
    }
}
