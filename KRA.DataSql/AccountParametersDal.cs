using KRA.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;
using KRA.Entities;

namespace KRA.DataSql
{
    public class AccountParametersDal : IAccountParameteresDal
    {
        public bool AddAccountParameter(AccountParameters Param)
        {
            using (KraContext Context = new KraContext())
            {
                AccountParameters Parameter = (from parameter in Context.AccountParameter where parameter.AccountParamID == Param.AccountParamID select parameter).SingleOrDefault();
                if (Parameter != null)
                {
                    Parameter.ParamID = Param.ParamID;
                    Parameter.Quarter = Param.Quarter;
                    Context.SaveChanges();
                }
                else
                {
                    Context.AccountParameter.Add(Param);
                    Context.SaveChanges();
                }
            }
            return true;
        }

        public List<AccountParameters> GetAllAccountParameters(int AccountId, string Quarter, int Year)
        {
            List<AccountParameters> AllParameters;
            using (KraContext Context = new KraContext())
            {
                AllParameters = (from Params in Context.AccountParameter where Params.AccountID == AccountId && Params.Quarter == Quarter && Params.Year == Year select Params).ToList();
            }
            return AllParameters;

        }

        public string[] GetQuarters(int AccountId, int year)
        {
            string[] quarters;
            using (KraContext Context = new KraContext())
            {
                quarters = (from parameters in Context.AccountParameter where parameters.AccountID == AccountId && parameters.Year == year select parameters.Quarter).Distinct().ToArray();

            }
            return quarters;
        }
        public List<int> GetYears(int AccountId)
        {
            List<int> years;
            using (KraContext Context = new KraContext())
            {
                years = (from parameters in Context.AccountParameter where parameters.AccountID == AccountId orderby parameters.Year ascending select parameters.Year).Distinct().ToList();

            }
            return years;
        }

        public bool UpdateParameterWeightage(int AccountParamId, int Weightage)
        {
            using (KraContext Context = new KraContext())
            {
                AccountParameters Parameter = Context.AccountParameter.Find(AccountParamId);
                Parameter.Weightage = Weightage;
                Context.SaveChanges();
            }
            return true;
        }

        public AccountParameters GetParameter(int AccountParamId)
        {
            AccountParameters parameter = null;
            using (KraContext Context = new KraContext())
            {
                parameter = Context.AccountParameter.Find(AccountParamId);
            }
            return parameter;
        }

        public AccountParameters GetParameter(int AccountId, int ParamId, string Quarter, int Year)
        {
            AccountParameters parameter = null;
            using (KraContext Context = new KraContext())
            {
                parameter = (from bs in Context.AccountParameter where bs.AccountID == AccountId && bs.ParamID == ParamId && bs.Quarter == Quarter && bs.Year == Year select bs).FirstOrDefault();
            }
            return parameter;
        }
    }
}

