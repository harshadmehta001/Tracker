using KRA.Domain.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KRA.Models;
using System.Web.Script.Serialization;
using KRA.Domain.Contracts;
using System.IO;
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]

namespace KRA.UI.Controllers
{
    public class ParametersController : Controller
    {
        private readonly IKraParameterService ParamService;
        private readonly IAccountsService ServiceAccounts;
        private readonly IAccountParameterService AccountParams;
        private readonly IParameterBoundsService ParameterBounds;
        private readonly IAccountTeamSizeService TeamSizeService;

        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ParametersController(IKraParameterService ParamService, IAccountsService ServiceAccounts, IAccountParameterService AccountParams, IParameterBoundsService ParameterBounds, IAccountTeamSizeService TeamSizeService)
        {
            this.ParamService = ParamService;
            this.ServiceAccounts = ServiceAccounts;
            this.AccountParams = AccountParams;
            this.ParameterBounds = ParameterBounds;
            this.TeamSizeService = TeamSizeService;
        }

        //Manage Paremeter View
        [HttpGet]
        public ActionResult ManageParameters()
        {
            return View();
        }

        //ManageDefinations View
        public ActionResult ManageParameterDefinations()
        {
            return View();
        }
        public ActionResult ManageParmeterDefinationsCopy()
        {
            return View();
        }

        //Returns Parmeters Master List
        [HttpGet]
        public JsonResult GetAccountParameters()
        {
            List<KraParametersModel> Parameters=null;
            try
            {
                Parameters = ParamService.GetAllParameters();
            }
            catch (Exception ex) {
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
                logger.Error(ex.Source);
                return Json("Error Occured", JsonRequestBehavior.AllowGet);

            }
            return Json(Parameters, JsonRequestBehavior.AllowGet);
        }

        //Returns the list of Accounts
        [HttpGet]
        public JsonResult GetAccounts()
        {
            List<Models.AccountsModel> AllAccounts;
            try
            {
                AllAccounts = ServiceAccounts.GetAllAccounts();
            }
            catch (Exception ex)
            {
                logger.Info("Error in Get Accounts");
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
                logger.Error(ex.Source);
                AllAccounts = null;
            }
            return Json(AllAccounts, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetAccounts(AccountsModel data)
        {
            List<Models.AccountsModel> AllAccounts;
            try
            {
                AllAccounts = ServiceAccounts.GetAllAccounts(data.ManagerID);
            }
            catch (Exception ex)
            {
                logger.Info("Error in Get Accounts");
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
                logger.Error(ex.Source);
                AllAccounts = null;
            }
            return Json(AllAccounts, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveAccountParameters(AccountParameterJson data)
        {
            List<AccountParametersModel> Jsonlist = new List<AccountParametersModel>();
            string errormessage = "Error Occured";
            try
            {
                int NoOfParameters = data.Paramters.Length;
                for (int i = 0; i < NoOfParameters; i++)
                {
                    AccountParametersModel Parameter = new AccountParametersModel();
                    Parameter.ParamID = data.Paramters[i];
                    Parameter.Quarter = data.Quarter;
                    Parameter.AccountID = data.ProjectId;
                    Parameter.Year = data.Year;


                    AccountParams.AddAccountParam(Parameter);
                    Jsonlist.Add(Parameter);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
                logger.Error(ex.Source);

                return Json(errormessage, JsonRequestBehavior.AllowGet);
            }

            return Json(Jsonlist, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string AddNewParameter(KraParametersModel param)
        {
            try
            {
                param.AddedOn = DateTime.Now;
                param.AddedBy = "LoggedInUser";
                if (ParamService.AddParameter(param))
                {

                    return "Parameter Added";
                }
                else
                {
                    return "Parameter Already Exist";
                   
                }
            }
        catch(Exception ex)
            {

                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
                logger.Error(ex.Source);

                return "Error";
            }
            }

        [HttpPost]
        public JsonResult GetAccountParameters(AccountParametersModel Param)
        {
            List<AccountParametersModel> Parameters = null;
            try
            {
                Parameters = AccountParams.GetAccountParameters(Param.AccountID, Param.Quarter, Param.Year);
            }
            catch (Exception ex)
            {
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
                logger.Error(ex.Source);
                return Json("Error Occured", JsonRequestBehavior.AllowGet);

            }
            return Json(Parameters, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public void SaveWeightage(AccountParametersModel data)

        {
            try
            {
                AccountParams.UpdateParameterWeightage(data.AccountParamID, Convert.ToInt32(data.Weightage));
            }
            catch(Exception ex)
            {
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
                logger.Error(ex.Source);

            }
            }

        public void SaveParameterBounds(IEnumerable<ParamaterBoundsModel> Bounds)
        {
            foreach (var parmbounds in Bounds)
            {
                try
                {
                    if (parmbounds != null)
                    {
                        parmbounds.DefinitionID = 0;
                        ParameterBounds.AddBounds(parmbounds);
                    }
                }
                catch(Exception ex)
                {
                    logger.Error(ex.InnerException);
                    logger.Error(ex.Message);
                    logger.Error(ex.Source);
                }
                }
        }
        [HttpPost]
        public JsonResult CopyPreviousParameterBounds(AccountParametersModel parameter)
        { List<ParamaterBoundsModel> bounds = null;

            try
            {
                bounds = ParameterBounds.CopyPreviousParameterBounds(parameter);
               
               
            }
            catch(Exception ex) {
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
                logger.Error(ex.Source);
            }

            return Json(bounds,JsonRequestBehavior.AllowGet);
            }

        [HttpPost]
        public JsonResult GetParameterBounds(AccountParametersModel Parameter)
        {
            List<ParamaterBoundsModel> bounds = ParameterBounds.GetBounds(Parameter.AccountParamID);
            return Json(bounds, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetQuarters(AccountParametersModel Account)
        {
            string response;
            string[] Quarters = AccountParams.GetQuarters(Account.AccountID,Account.Year);
            if (Quarters != null)
            {
                return Json(Quarters, JsonRequestBehavior.AllowGet);
            }
            else
                response = "Please Select the parameters";

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetYears(AccountsModel Account)
        {
            List<int> years = AccountParams.GetYears(Account.AccountID);

            return Json(years, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult GetQuarterJson(AccountParametersModel Account)
        {
            List<Object> list = new List<Object>();
            try
            {
                string[] arr = AccountParams.GetQuarters(Account.AccountID,Account.Year);
                for (int i = 0; i < arr.Length; i++)
                {
                    var obj = new { Quarter = arr[i] };
                    list.Add(obj);

                }
            }
            catch (Exception ex) {
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
                logger.Error(ex.Source);
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public void UpdateTeamSize(AccountTeamSizeModel data)
        {
            try
            {
                data.AddedOn = DateTime.Now;
                data.AddedBy = "Admin";
               
                TeamSizeService.UpdateTeamSize(data);
            }
            catch (Exception ex)
            {
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
                logger.Error(ex.Source);
            }
        }

        [HttpPost]
        public JsonResult GetTeamSize(AccountTeamSizeModel data)
        {
                AccountTeamSizeModel TeamSize = TeamSizeService.GetTeamSize(data.AccountID, data.Quarter,data.Year);

            if (TeamSize != null)
            {
                return Json(TeamSize.TeamSize, JsonRequestBehavior.AllowGet);
            }
            else
                return null;
            }

        [HttpPost]
        public JsonResult GetWeightage(AccountParametersModel parameter)
        {
            string weightage=null;
            try
            {
                weightage = AccountParams.GetParameter(parameter.AccountParamID).Weightage;
            }
            catch (Exception ex) {
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
                logger.Error(ex.Source);
            }
            return Json(weightage, JsonRequestBehavior.AllowGet);
        }

    }
}