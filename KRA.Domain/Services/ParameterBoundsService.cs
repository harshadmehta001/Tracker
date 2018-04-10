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
    public class ParameterBoundsService : IParameterBoundsService

    {
        private readonly IParameterBoundsDal ParameterBoundsDal;
        private readonly IAccountParameterService Parmservice;
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ParameterBoundsService(IParameterBoundsDal ParameterBoundsDal, IAccountParameterService Parmservice)
        {

            this.ParameterBoundsDal = ParameterBoundsDal;
            this.Parmservice = Parmservice;
        }

        public Entities.ParameterBounds ParamBoundsModelToEntityMapper(ParamaterBoundsModel Bounds)
        {

            Entities.ParameterBounds EntityBounds = new Entities.ParameterBounds()
            {
                AccountParamID = Bounds.AccountParamID,
                MaxValue = Bounds.MaxValue,
                MinValue = Bounds.MinValue,
                Result = Bounds.Result,
                AddedOn = DateTime.Now,
                DefinitionID = Bounds.DefinitionID,
            };
            return EntityBounds;
        }


        public bool AddBounds(ParamaterBoundsModel Bounds)
        {
            try
            {
                Entities.ParameterBounds EntityBounds = ParamBoundsModelToEntityMapper(Bounds);
                ParameterBoundsDal.AddParameterBound(EntityBounds);
                return true;

            }

            catch (Exception ex)
            {
                logger.Info("Error in Get AddBounds");
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
                logger.Error(ex.Source);
                return false;
            }
        }

        public List<ParamaterBoundsModel> GetBounds(int AccountParamId)
        {
            List<ParamaterBoundsModel> AllBounds;
            try
            {
                AllBounds = (from bounds in ParameterBoundsDal.GetParameterBounds(AccountParamId)
                             select new Models.ParamaterBoundsModel
                             {

                                 AccountParamID = bounds.AccountParamID,
                                 DefinitionID = bounds.DefinitionID,
                                 MaxValue = bounds.MaxValue,
                                 MinValue = bounds.MinValue,
                                 Result = bounds.Result

                             }).ToList();

            }

            catch (Exception ex)
            {
                logger.Info("Error in Get GetBounds");
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
                logger.Error(ex.Source);
                AllBounds = null;
            }
            return AllBounds;
        }

        public List<ParamaterBoundsModel> CopyPreviousParameterBounds(AccountParametersModel parameter)
        {

            List<AccountParametersModel> parameters = Parmservice.GetAccountParameters(parameter.AccountID, parameter.Quarter, parameter.Year);
            int paramid = Parmservice.GetParameter(parameter.AccountParamID).ParamID;
            AccountParametersModel AccountParamId = (from id in parameters where id.ParamID == paramid select id).SingleOrDefault();

            List<ParamaterBoundsModel> bounds = GetBounds(AccountParamId.AccountParamID);


            return bounds;

        }
    }
}
