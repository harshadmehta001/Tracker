using ClosedXML.Excel;
using KRA.Domain.Contract;
using KRA.Domain.Contracts;
using KRA.Domain.Services;
using KRA.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KRA.UI.Controllers
{
    public class KraScoresController : Controller
    {
        private readonly IKraInputScoresService InputScoreService;
        private readonly IKraScoreCalculator ScoreCalculator;
        private readonly IExcelReportGenerator ReportGenerator;
        private readonly IAccountsService Account;
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public KraScoresController(IKraInputScoresService InputScoreService, IKraScoreCalculator ScoreCalculator, IExcelReportGenerator ReportGenerator, IAccountsService Account)
        {
            this.InputScoreService = InputScoreService;
            this.ScoreCalculator = ScoreCalculator;
            this.ReportGenerator = ReportGenerator;
            this.Account = Account;
        }
        // GET: KraScores
        public ActionResult InputScores()
        {
            return View();
        }
        public ActionResult ADMReport()
        {
            return View();
        }
        public ActionResult ReportTemplate()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult SaveAccountScores(IEnumerable<AccountParametersModel> ParameterScores)
        {
            foreach (var scores in ParameterScores)
            {
                KraInputScoresModel InputScore = new KraInputScoresModel();
                InputScore.AccountParamID = scores.AccountParamID;
                InputScore.Score = scores.Score;
                InputScore.AddedOn = DateTime.Now;
                InputScore.AddedBy = "Logged In User";
                InputScoreService.AddScores(InputScore);
            }
            return null;
        }
        [HttpGet]
        public ActionResult KraScore()
        {

            return View();
        }

        [HttpPost]
        public JsonResult GetFinalScore(AccountParametersModel Param)
        {
            return Json(ScoreCalculator.AccountKraScoreYearly(Param.AccountID, Param.Year), JsonRequestBehavior.AllowGet);
        }
        //Excel
        [HttpGet]
        public ActionResult GetPMReport(int id,int year)
        {
            try
            {
                XLWorkbook Report = ReportGenerator.CreatePMReport(id, year);
                MemoryStream excelStream = new MemoryStream();
                Report.SaveAs(excelStream);
                excelStream.Position = 0;
                AccountsModel account = Account.GetAccount(id);
                string filename = "KraReport_" + account.Name+ "_" + year.ToString() + ".xlsx";
                return File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
            }
            catch (Exception ex)
            {
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
                logger.Error(ex.Source);
                return View("Something Went Wrong");
            }
        }


        [HttpPost]
        public JsonResult GetADMReport(AccountsModel data)
        {
            List<CalculatedScoreYear> admfinal = null;
            try
            {
                List<AccountsModel> accounts = Account.GetAllAccounts(data.ManagerID);
                int[] Accounts = new int[accounts.Count];
                int i = 0;
                foreach (var item in accounts)
                {
                    Accounts[i] = item.AccountID;
                    i++;
                }

                admfinal = ScoreCalculator.KraScoreADMYearly(Accounts, data.Year);
            }
            catch (Exception ex)
            {
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
                logger.Error(ex.Source);

            }
            return Json(admfinal, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetADMReportExcel(int id,int year)
        {
            try
            {
                List<AccountsModel> accounts = Account.GetAllAccounts(id);
                int[] Accounts = new int[accounts.Count];
                int i = 0;
                foreach (var item in accounts)
                {
                    Accounts[i] = item.AccountID;
                    i++;
                }
                XLWorkbook Report = ReportGenerator.CreateADMReport(Accounts, year);
                MemoryStream excelStream = new MemoryStream();
                Report.SaveAs(excelStream);
                excelStream.Position = 0;
                return File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "KraReportADM.xlsx");
            }
            catch (Exception ex)
            {
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
                logger.Error(ex.Source);
                return View("Something Went Wrong");

            }
        }
    }
}