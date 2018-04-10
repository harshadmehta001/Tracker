using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using KRA.Domain.Contract;
using KRA.Entities;
using KRA.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRA.Domain.Services
{
    public class ExcelReportGenerator : IExcelReportGenerator
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IAccountParameterService AccountParams;
        private readonly IKraInputScoresService InputScores;
        private readonly IKraScoreCalculator Krascores;
        public ExcelReportGenerator(IKraScoreCalculator Krascores, IAccountParameterService AccountParams, IKraInputScoresService InputScores)
        {
            this.Krascores = Krascores;
            this.AccountParams = AccountParams;
            this.InputScores = InputScores;
        }
        public DataTable GetScoresDataTable(int AccountId, int Year)
        {
            DataTable scorestable = new DataTable();
            scorestable.Columns.Add("Parameter Name");
            scorestable.Columns.Add("Weightage");
            scorestable.Columns.Add("Q1");
            scorestable.Columns.Add("Q2");
            scorestable.Columns.Add("Q3");
            scorestable.Columns.Add("Q4");



            try
            {
                string[] Quarters = AccountParams.GetQuarters(AccountId, Year);
                List<AccountParametersModel> Parameters = AccountParams.GetParameters(AccountId, Year);
                List<CalculatedScoreYear> list = new List<CalculatedScoreYear>();
                foreach (var items in Parameters)
                {
                    CalculatedScoreYear ob = new CalculatedScoreYear();
                    ob.ParameterName = items.ParameterName;
                    ob.Weightage = Convert.ToInt32(items.Weightage);
                    ob.Score = new float[Quarters.Length];


                    for (int i = 0; i < Quarters.Length; i++)
                    {

                        AccountParametersModel parameter =
                                                    AccountParams.GetParameter(AccountId, items.ParamID, Quarters[i], Year);
                        if (parameter != null)
                        {
                            KraInputScores score = InputScores.GetScores(parameter.AccountParamID);
                            if (score != null)
                            {

                                ob.Score[i] = score.Score;
                            }
                            else
                            {
                                ob.Score[i] = 0;
                            }
                        }

                        else
                        {
                            ob.Score[i] = 0;
                        }
                    }
                    list.Add(ob);

                }

                foreach (var items in list)
                {
                    int size = items.Score.Length;
                    if (size == 1)
                    {
                        scorestable.Rows.Add(items.ParameterName, items.Weightage, items.Score[0]);

                    }
                    else if (size == 2)
                    {
                        scorestable.Rows.Add(items.ParameterName, items.Weightage, items.Score[0], items.Score[1]);
                    }
                    else if (size == 3)
                    {
                        scorestable.Rows.Add(items.ParameterName, items.Weightage, items.Score[0], items.Score[1], items.Score[2]);
                    }
                    else if (size == 4)
                    {
                        scorestable.Rows.Add(items.ParameterName, items.Weightage, items.Score[0], items.Score[1], items.Score[2], items.Score[3]);
                    }
                }


            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                logger.Error(ex.Message);
                logger.Error(ex.Source);
                scorestable = null;

            }

            return scorestable;
        }
        public void ApplyStyle(IXLWorksheet ws, string cell)
        {
            ws.Cell(cell).Style.Fill.SetBackgroundColor(XLColor.YellowRyb);
            ws.Cell(cell).Style.Font.SetBold(true);
            ws.Cell(cell).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Cell(cell).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
            ws.Cell(cell).Style.Border.SetTopBorder(XLBorderStyleValues.Thin);
            ws.Cell(cell).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin);
            ws.Cell(cell).Style.Border.SetRightBorder(XLBorderStyleValues.Thin);

        }
        public void ApplyStyle(IXLWorksheet ws, int row, int column)
        {
            ws.Cell(row, column).Style.Fill.SetBackgroundColor(XLColor.YellowRyb);
            ws.Cell(row, column).Style.Font.SetFontColor(XLColor.Black);
            ws.Cell(row, column).Style.Font.SetBold(true);
        }
        public void ApplyBorder(IXLWorksheet ws, int row, int column)
        {
            ws.Cell(row, column).Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
            ws.Cell(row, column).Style.Border.SetTopBorder(XLBorderStyleValues.Thin);
            ws.Cell(row, column).Style.Border.SetLeftBorder(XLBorderStyleValues.Thin);
            ws.Cell(row, column).Style.Border.SetRightBorder(XLBorderStyleValues.Thin);
            ws.Cell(row, column).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            
        }


        public void CreateWorksheet(XLWorkbook wb, int AccountId, int Year)
        {
            var ws = wb.Worksheets.Add("Score" + AccountId);

            // From a list of strings
            var list = Krascores.AccountKraScoreYearly(AccountId, Year);

            ws.Range("B3:G3").Row(1).Merge();
            ws.Cell("B3").AsRange().AddToNamed("Titles");
            ws.Column(3).Width = 10;
            ws.Column(2).Width = 20;

            ws.Cell("B3").Value = "Score";
            ws.Cell("B3").Style.Fill.SetBackgroundColor(XLColor.Blue);
            ws.Cell("B3").Style.Font.SetBold(true);
            ws.Cell("B3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Cell("B3").Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
            ws.Cell("B3").Style.Border.SetTopBorder(XLBorderStyleValues.Thin);
            ws.Cell("B3").Style.Border.SetLeftBorder(XLBorderStyleValues.Thin);
            ws.Cell("B3").Style.Border.SetRightBorder(XLBorderStyleValues.Thin);
            ws.Cell("B3").Style.Font.SetFontColor(XLColor.White);

            ApplyStyle(ws, "B4");
            ws.Cell("B4").Value = "Parameter";
            ApplyStyle(ws, "C4");
            ws.Cell("C4").Value = "Weightage";
            ApplyStyle(ws, "D4");
            ws.Cell("D4").Value = "Q1";
            ApplyStyle(ws, "E4");
            ws.Cell("E4").Value = "Q2";
            ApplyStyle(ws, "F4");
            ws.Cell("F4").Value = "Q3";
            ApplyStyle(ws, "G4");

            ws.Cell("G4").Value = "Q4";
            DataTable dt = new DataTable();
            dt.Columns.Add();
            dt.Columns.Add();
            dt.Columns.Add();
            dt.Columns.Add();
            dt.Columns.Add();
            dt.Columns.Add();

            foreach (var items in list)
            {
                int size = items.Score.Length;
                if (size == 1)
                {
                    dt.Rows.Add(items.ParameterName, items.Weightage + "%", Math.Round(items.Score[0]) + "%").ClearErrors();

                }
                else if (size == 2)
                {
                    dt.Rows.Add(items.ParameterName, items.Weightage + "%", Math.Round(items.Score[0]) + "%", Math.Round(items.Score[1]) + "%").ClearErrors();
                }
                else if (size == 3)
                {
                    dt.Rows.Add(items.ParameterName, items.Weightage + "%", Math.Round(items.Score[0]) + "%", Math.Round(items.Score[1]) + "%", Math.Round(items.Score[2]) + "%").ClearErrors();
                }
                else if (size == 4)
                {
                    dt.Rows.Add(items.ParameterName, items.Weightage + "%", Math.Round(items.Score[0]) + "%", Math.Round(items.Score[1]) + "%", Math.Round(items.Score[2]) + "%", Math.Round(items.Score[3]) + "%");
                }
            }

            CalculatedScoreYear TotalScore = Krascores.PMFinalScore(AccountId, Year);
            int qsize = TotalScore.Score.Length;
            if (qsize == 1)
            {
                dt.Rows.Add(TotalScore.ParameterName, TotalScore.Weightage + "%", Math.Round(TotalScore.Score[0]) + "%");

            }
            else if (qsize == 2)
            {
                dt.Rows.Add(TotalScore.ParameterName, TotalScore.Weightage + "%", Math.Round(TotalScore.Score[0]) + "%", Math.Round(TotalScore.Score[1]) + "%");

            }
            else if (qsize == 3)
            {
                dt.Rows.Add(TotalScore.ParameterName, TotalScore.Weightage + "%", Math.Round(TotalScore.Score[0]) + "%", Math.Round(TotalScore.Score[1]) + "%", Math.Round(TotalScore.Score[2]) + "%");

            }
            else if (qsize == 4)
            {
                dt.Rows.Add(TotalScore.ParameterName, TotalScore.Weightage + "%", Math.Round(TotalScore.Score[0]) + "%", Math.Round(TotalScore.Score[1]) + "%", Math.Round(TotalScore.Score[2]) + "%", Math.Round(TotalScore.Score[3]) + "%");

            }
            ws.Cell("B5").Value = dt.AsEnumerable();
            int rowsize = dt.Rows.Count;

            for (int i = 5; i < rowsize + 5; i++)
            {
                for (int j = 2; j < 8; j++)
                {
                    ApplyBorder(ws, i, j);
                    if (i == rowsize + 5 - 1)
                    {
                        ApplyStyle(ws, i, j);
                    }

                }

            }
            ws.Range("B16:G16").Row(1).Merge();
            ws.Cell("B16").AsRange().AddToNamed("Titles");
            ws.Cell("B16").Value = "Input Score";
            ws.Cell("B16").Style.Fill.SetBackgroundColor(XLColor.Blue);

            ws.Cell("B16").Style.Font.SetBold(true);
            ws.Cell("B16").Style.Font.SetFontColor(XLColor.White);

            ws.Cell("B16").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            ws.Cell("B16").Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
            ws.Cell("B16").Style.Border.SetTopBorder(XLBorderStyleValues.Thin);
            ws.Cell("B16").Style.Border.SetLeftBorder(XLBorderStyleValues.Thin);
            ws.Cell("B16").Style.Border.SetRightBorder(XLBorderStyleValues.Thin);
            DataTable inputscores = GetScoresDataTable(AccountId, Year);

            ws.Cell("B17").Value = "Parameter";
            ws.Cell("B17").Style.Fill.SetBackgroundColor(XLColor.YellowRyb);
            ApplyStyle(ws, "B17");
            ws.Cell("C17").Value = "Weightage";
            ApplyStyle(ws, "C17");
            ws.Cell("D17").Value = "Q1";
            ApplyStyle(ws, "D17");

            ApplyStyle(ws, "E17");
            ws.Cell("E17").Value = "Q2";
            ApplyStyle(ws, "F17");
            ws.Cell("F17").Value = "Q3";
            ApplyStyle(ws, "G17");
            ws.Cell("G17").Value = "Q4";
            ws.Cell("B18").Value = inputscores.AsEnumerable();
            rowsize = inputscores.Rows.Count;
            for (int i = 18; i < rowsize + 18; i++)
            {
                for (int j = 2; j < 8; j++)
                {
                    ApplyBorder(ws, i, j);

                }
            }
        }


        public XLWorkbook CreatePMReport(int AccountId, int Year)
        {
            var wb = new XLWorkbook();
            wb.Author = "KraTracker";
            wb.Properties.Company = "IRIS";
            {

                CreateWorksheet(wb, AccountId, Year);

            }
            return wb;
        }


        public XLWorkbook CreateADMReport(int[] AccountId, int Year)
        {
            var wb = new XLWorkbook();
            wb.Author = "KraTracker";
            wb.Properties.Company = "IRIS";
            {
                var ws = wb.Worksheets.Add("Kra Adm Score");
                var list = Krascores.KraScoreADMYearly(AccountId, Year);
                ws.Range("B3:G3").Row(1).Merge();
                ws.Cell("B3").AsRange().AddToNamed("Titles");
                ws.Column(3).Width = 10;
                ws.Column(2).Width = 20;
                ws.Cell("B3").Value = "Final Score";
                ws.Cell("B3").Style.Fill.SetBackgroundColor(XLColor.Blue);
                ws.Cell("B3").Style.Font.SetBold(true);
                ws.Cell("B3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                ws.Cell("B3").Style.Border.SetBottomBorder(XLBorderStyleValues.Thin);
                ws.Cell("B3").Style.Border.SetTopBorder(XLBorderStyleValues.Thin);
                ws.Cell("B3").Style.Border.SetLeftBorder(XLBorderStyleValues.Thin);
                ws.Cell("B3").Style.Border.SetRightBorder(XLBorderStyleValues.Thin);
                ws.Cell("B3").Style.Font.SetFontColor(XLColor.White);

                ApplyStyle(ws, "B4");
                ws.Cell("B4").Value = "Parameter";
                ApplyStyle(ws, "C4");
                ws.Cell("C4").Value = "Weightage";
                ApplyStyle(ws, "D4");
                ws.Cell("D4").Value = "Q1";
                ApplyStyle(ws, "E4");
                ws.Cell("E4").Value = "Q2";
                ApplyStyle(ws, "F4");
                ws.Cell("F4").Value = "Q3";
                ApplyStyle(ws, "G4");

                ws.Cell("G4").Value = "Q4";
                DataTable dt = new DataTable();
                dt.Columns.Add();
                dt.Columns.Add();
                dt.Columns.Add();
                dt.Columns.Add();
                dt.Columns.Add();
                dt.Columns.Add();

                foreach (var items in list)
                {
                    int size = items.Score.Length;
                    if (size == 1)
                    {
                        dt.Rows.Add(items.ParameterName, items.Weightage + "%", Math.Round(items.Score[0]) + "%").ClearErrors();

                    }
                    else if (size == 2)
                    {
                        dt.Rows.Add(items.ParameterName, items.Weightage + "%", Math.Round(items.Score[0]) + "%", Math.Round(items.Score[1]) + "%").ClearErrors();
                    }
                    else if (size == 3)
                    {
                        dt.Rows.Add(items.ParameterName, items.Weightage + "%", Math.Round(items.Score[0]) + "%", Math.Round(items.Score[1]) + "%", Math.Round(items.Score[2]) + "%").ClearErrors();
                    }
                    else if (size == 4)
                    {
                        dt.Rows.Add(items.ParameterName, items.Weightage + "%", Math.Round(items.Score[0]) + "%", Math.Round(items.Score[1]) + "%", Math.Round(items.Score[2]) + "%", Math.Round(items.Score[3]) + "%");
                    }
                }

                CalculatedScoreYear TotalScore = Krascores.ADMFinalScore(AccountId, Year);
                int qsize = TotalScore.Score.Length;
                if (qsize == 1)
                {
                    dt.Rows.Add(TotalScore.ParameterName, TotalScore.Weightage + "%", Math.Round(TotalScore.Score[0]) + "%");

                }
                else if (qsize == 2)
                {
                    dt.Rows.Add(TotalScore.ParameterName, TotalScore.Weightage + "%", Math.Round(TotalScore.Score[0]) + "%", Math.Round(TotalScore.Score[1]) + "%");

                }
                else if (qsize == 3)
                {
                    dt.Rows.Add(TotalScore.ParameterName, TotalScore.Weightage + "%", Math.Round(TotalScore.Score[0]) + "%", Math.Round(TotalScore.Score[1]) + "%", Math.Round(TotalScore.Score[2]) + "%");

                }
                else if (qsize == 4)
                {
                    dt.Rows.Add(TotalScore.ParameterName, TotalScore.Weightage + "%", Math.Round(TotalScore.Score[0]) + "%", Math.Round(TotalScore.Score[1]) + "%", Math.Round(TotalScore.Score[2]) + "%", Math.Round(TotalScore.Score[3]) + "%");

                }
                ws.Cell("B5").Value = dt.AsEnumerable();
                int rowsize = dt.Rows.Count;
                for (int i = 5; i < rowsize + 5; i++)
                {
                    for (int j = 2; j < 8; j++)
                    {
                        ApplyBorder(ws, i, j);
                        if (i == rowsize + 5 - 1)
                        {
                            ApplyStyle(ws, i, j);
                        }
                    }

                }

            }
            for (int i = 0; i < AccountId.Length; i++)
            {

                CreateWorksheet(wb, AccountId[i], Year);

            }
            return wb;
        }
    }
}
