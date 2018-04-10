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
    public class KraParameterService : IKraParameterService
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IKraParametersDal ParamDal;
        public KraParameterService(IKraParametersDal ParamDal)
        {
            this.ParamDal = ParamDal;
        }
        #region Mapper
        public Entities.KraParameters ParameterModelToEntityMapper(Models.KraParametersModel Param)
        {
            Entities.KraParameters EntityParams = new Entities.KraParameters()
            {
                ParamID = Param.ParamID,
                ParamName = Param.ParamName,
                AddedBy = Param.AddedBy,
                AddedOn = Param.AddedOn
            };
            return EntityParams;
        }
        #endregion
        #region AddParameter

        public bool AddParameter(KraParametersModel Parameter)
        {
            Parameter.AddedOn = DateTime.Now;
            Entities.KraParameters Param = ParameterModelToEntityMapper(Parameter);

            if (ParamDal.AddParameter(Param))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        
        #endregion

        public List<KraParametersModel> GetAllParameters()
        {
            List<KraParametersModel> AllParameters;
            try
            {

                AllParameters = (from parm in ParamDal.GetParameters()
                                 select new Models.KraParametersModel
                                 {
                                     AddedBy = parm.AddedBy,
                                     ParamID = parm.ParamID,
                                     ParamName = parm.ParamName,
                                     AddedOn = parm.AddedOn

                                 }).ToList();
            }
            catch (Exception ex)
            {
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
                logger.Error(ex.Source);
                AllParameters = null;
            }
            return AllParameters;
        }
    }
}
