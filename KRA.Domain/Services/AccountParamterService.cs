using KRA.Domain.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KRA.Models;
using KRA.Data.Repository;

namespace KRA.Domain.Services
{
    public class AccountParamterService : IAccountParameterService
    {
        private readonly IAccountParameteresDal AccountParameterDal;
        private readonly IKraParameterService ParametersService;
        private readonly IKraInputScoresService InputScoreService;
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AccountParamterService(IAccountParameteresDal AccountParameterDal, IKraParameterService ParametersService, IKraInputScoresService InputScoreService)
        {
            this.AccountParameterDal = AccountParameterDal;
            this.ParametersService = ParametersService;
            this.InputScoreService = InputScoreService;
        }

        private Models.AccountParametersModel AccountParametersEntityModelMapper(Entities.AccountParameters AccountParameter)
        {
            Models.AccountParametersModel Params = null;
            if (AccountParameter != null)
            {
                Params = new Models.AccountParametersModel
                {
                    AccountParamID = AccountParameter.AccountParamID,
                    AccountID = AccountParameter.AccountID,
                    ParamID = AccountParameter.ParamID,
                    Quarter = AccountParameter.Quarter,
                    Weightage = AccountParameter.Weightage.ToString()
                };
            }
            return Params;
        }

        private Entities.AccountParameters AccountParametersModelToEntityMapper(Models.AccountParametersModel AccountParameter)
        {
            Entities.AccountParameters Params = null;
            if (AccountParameter != null)
            {
                Params = new Entities.AccountParameters
                {
                    AccountParamID = AccountParameter.AccountParamID,
                    AccountID = AccountParameter.AccountID,
                    ParamID = AccountParameter.ParamID,
                    Quarter = AccountParameter.Quarter,
                    Weightage = Convert.ToInt32(AccountParameter.Weightage),
                    Year = AccountParameter.Year,
                };
            }
            return Params;
        }

        public bool AddAccountParam(AccountParametersModel AccountParameter)
        {
            if (AccountParameterDal.AddAccountParameter(AccountParametersModelToEntityMapper(AccountParameter)))
            {
                logger.Info("AccountParam Added" + AccountParameter.ToString());
                return true;
            }
            else
                logger.Info("Exception Thrown" + AccountParameter.ToString());
            return false;
        }
        public List<AccountParametersModel> GetAccountParameters(int AccountId, string Quarter, int Year)
        {
            List<AccountParametersModel> Params;
            try
            {
                List<Entities.AccountParameters> AllParams = AccountParameterDal.GetAllAccountParameters(AccountId, Quarter, Year);
                List<KraParametersModel> Parameters = ParametersService.GetAllParameters();
                Params = (from parameters in AllParams
                          join param in Parameters on parameters.ParamID equals param.ParamID
                          select new AccountParametersModel
                          {
                              AccountID = parameters.AccountID,
                              ParamID = parameters.ParamID,
                              AccountParamID = parameters.AccountParamID,
                              ParameterName = param.ParamName,
                              Weightage = parameters.Weightage.ToString(),
                              Score = 0

                          }).ToList();
                foreach (var items in Params)
                {
                    if (InputScoreService.GetScores(items.AccountParamID) != null)
                    {
                        items.Score = InputScoreService.GetScores(items.AccountParamID).Score;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Get GetAccountParameters");
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
                logger.Error(ex.Source);
                Params = null;
            }
            return Params;
        }

        public bool UpdateParameterWeightage(int AccountParameterId, int Weightage)
        {
            if (AccountParameterDal.UpdateParameterWeightage(AccountParameterId, Weightage))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string[] GetQuarters(int AccountId, int Year)
        {
            string[] quarters;
            try
            {
                quarters = AccountParameterDal.GetQuarters(AccountId, Year);
            }
            catch (Exception ex)
            {
                logger.Info("Error in Get GetAccountParameters");
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
                logger.Error(ex.Source);
                quarters = null;
            }
            return quarters;
        }
        public List<int> GetYears(int AccountId)
        {
            List<int> years;
            try
            {
                years = AccountParameterDal.GetYears(AccountId);
                foreach (var year in years)
                {
                    if (year == DateTime.Now.Year)
                    {
                        years.Remove(year);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Info("Error in Get GetAccountParameters");
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
                logger.Error(ex.Source);
                years = null;
            }
            return years;
        }

        public AccountParametersModel GetParameter(int AccountParameterId)
        {
            AccountParametersModel parameter = AccountParametersEntityModelMapper(AccountParameterDal.GetParameter(AccountParameterId));
            return parameter;
        }

        public AccountParametersModel GetParameter(int AccountId, int ParamId, string Quarter, int Year)
        {
            AccountParametersModel parameter = AccountParametersEntityModelMapper(AccountParameterDal.GetParameter(AccountId, ParamId, Quarter, Year));
            return parameter;
        }

        public List<AccountParametersModel> GetParameters(int AccountId, int Year)
        {
            string[] Quarters = GetQuarters(AccountId, Year);

            List<AccountParametersModel>[] parameter = new List<AccountParametersModel>[Quarters.Length];
            List<AccountParametersModel> templist = new List<AccountParametersModel>();
            for (int i = 0; i < Quarters.Length; i++)
            {
                parameter[i] = GetAccountParameters(AccountId, Quarters[i], Year);
                foreach (var items in parameter[i])
                {
                    templist.Add(items);
                }
            }
            var Parameters = templist.GroupBy(a => a.ParameterName).Select(g => g.First()).ToList();
            return Parameters;
        }
    }
}
