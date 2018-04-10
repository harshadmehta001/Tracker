using KRA.Domain.Contract;
using KRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRA.Domain.Services
{
    public class KraScoreCalculator : IKraScoreCalculator
    {
        private readonly IAccountParameterService AccountParams;
        private readonly IKraInputScoresService InputScores;
        private readonly IParameterBoundsService ParamBounds;
        private readonly IAccountTeamSizeService Teamsize;
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public KraScoreCalculator(IAccountParameterService AccountParams, IKraInputScoresService InputScores, IParameterBoundsService ParamBounds, IAccountTeamSizeService Teamsize)
        {
            this.AccountParams = AccountParams;
            this.InputScores = InputScores;
            this.ParamBounds = ParamBounds;
            this.Teamsize = Teamsize;
        }


        #region GetAccountScores
        public Dictionary<int, Entities.KraInputScores> GetAccountScores(int AccountId, string Quarter, int Year)
        {

            Dictionary<int, Entities.KraInputScores> dictionary = new Dictionary<int, Entities.KraInputScores>();
            List<AccountParametersModel> Parameters = AccountParams.GetAccountParameters(AccountId, Quarter, Year);
            foreach (var parameter in Parameters)
            {

                dictionary.Add(parameter.AccountParamID, InputScores.GetScores(parameter.AccountParamID));

            }
            return dictionary;

        }
        #endregion

        public Dictionary<int, List<Models.ParamaterBoundsModel>> GetParameterBounds(int AccountId, string Quarter, int Year)
        {
            List<AccountParametersModel> Parameters = AccountParams.GetAccountParameters(AccountId, Quarter, Year);

            Dictionary<int, List<Models.ParamaterBoundsModel>> ParameterBounds = new Dictionary<int, List<ParamaterBoundsModel>>();
            foreach (var parameter in Parameters)
            {
                ParameterBounds.Add(parameter.AccountParamID, ParamBounds.GetBounds(parameter.AccountParamID));
            }
            return ParameterBounds;
        }
        #region CaluclateProjectScore
        public List<CalculatedScoreModel> CalculateProjectKraScore(int AccountId, string Quarter, int Year)
        {
            List<AccountParametersModel> Parameters = AccountParams.GetAccountParameters(AccountId, Quarter, Year);

            List<CalculatedScoreModel> FinalScores = new List<CalculatedScoreModel>();
            try
            {
                foreach (var parameter in Parameters)
                {
                    List<ParamaterBoundsModel> BoundsList = ParamBounds.GetBounds(parameter.AccountParamID);
                    Entities.KraInputScores Scores = InputScores.GetScores(parameter.AccountParamID);
                    int Finalscore = 0;

                    foreach (var bounds in BoundsList)
                    {

                        if (bounds.MaxValue > Scores.Score && Scores.Score >= bounds.MinValue)
                        {
                            Finalscore = bounds.Result;
                        }
                        else if (bounds.MaxValue >= Scores.Score && Scores.Score > bounds.MinValue)
                        {
                            Finalscore = bounds.Result;
                        }
                    }
                    CalculatedScoreModel Ob = new CalculatedScoreModel();
                    Ob.FinalScore = (float)(Finalscore * Convert.ToInt32(parameter.Weightage)) / 100;
                    Ob.AccountParamID = parameter.AccountParamID;
                    Ob.ParameterName = parameter.ParameterName;
                    Ob.AccountID = parameter.AccountID;
                    Ob.Weightage = Convert.ToInt32(parameter.Weightage);
                    FinalScores.Add(Ob);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
                logger.Error(ex.Source);
                FinalScores = null;
            }
            return FinalScores;
        }
        #endregion

        //Overloaded Method, if we require report for whole Year. 

        #region CaluclateProjectScore(for Year)
        public List<List<CalculatedScoreModel>> CalculateProjectKraScore(int AccountId, int Year)
        {
            List<List<CalculatedScoreModel>> KraScores = new List<List<CalculatedScoreModel>>();
            string[] Quarters = AccountParams.GetQuarters(AccountId, Year);
            for (int i = 0; i < Quarters.Length; i++)
            {
                List<CalculatedScoreModel> QuarterScore = CalculateProjectKraScore(AccountId, Quarters[i], Year);
                if (QuarterScore.Count != 0)
                {
                    KraScores.Add(QuarterScore);
                }
            }

            return KraScores;
        }
        #endregion



        public List<CalculatedScoreYear> AccountKraScoreYearly(int AccountId, int Year)
        {
            List<CalculatedScoreYear> Scores;
            try
            {
                string[] Quarters = AccountParams.GetQuarters(AccountId, Year);
                Scores = new List<CalculatedScoreYear>();
                var Parameters = AccountParams.GetParameters(AccountId, Year);
                foreach (var items in Parameters)
                {
                    CalculatedScoreYear Ob = new CalculatedScoreYear();
                    Ob.ParameterName = items.ParameterName;
                    Ob.Score = new float[Quarters.Length];
                    Ob.Weightage = Convert.ToInt32(items.Weightage);
                    for (int i = 0; i < Quarters.Length; i++)
                    {
                        List<CalculatedScoreModel> QuarterScore = CalculateProjectKraScore(AccountId, Quarters[i], Year);
                        if (QuarterScore != null)
                        {
                            CalculatedScoreModel ob = (from bs in QuarterScore where bs.ParameterName == items.ParameterName select bs).FirstOrDefault();
                            if (ob != null)
                            {
                                Ob.Score[i] = ob.FinalScore;
                            }
                            else
                            {
                                Ob.Score[i] = 0;
                            }

                        }
                    }

                    Scores.Add(Ob);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.InnerException);
                logger.Error(ex.Message);
                logger.Error(ex.Source);
                Scores = null;
            }
            return Scores;
        }


        public CalculatedScoreYear PMFinalScore(int AccountId, int Year)
        {
            CalculatedScoreYear[] Scores = AccountKraScoreYearly(AccountId, Year).ToArray();
            CalculatedScoreYear FinalScore = new CalculatedScoreYear();
            FinalScore.ParameterName = "FinalScore";
            int length = Scores[0].Score.Length;
            FinalScore.Score = new float[length];
            for (int j = 0; j < length; j++)
            {
                FinalScore.Weightage += Scores[j].Weightage;
                for (int i = 0; i < Scores.Length; i++)
                {
                    FinalScore.Score[j] += Scores[i].Score[j];
                }
                Math.Round(FinalScore.Score[j]);
            }
            return FinalScore;
        }


        public CalculatedScoreYear ADMFinalScore(int[] AccountId, int Year)
        {
            CalculatedScoreYear[] Scores = KraScoreADMYearly(AccountId, Year).ToArray();
            CalculatedScoreYear FinalScore = new CalculatedScoreYear();
            FinalScore.ParameterName = "FinalScore";
            int length = Scores[0].Score.Length;
            FinalScore.Score = new float[length];
            for (int j = 0; j < length; j++)
            {
                FinalScore.Weightage += Scores[j].Weightage;
                for (int i = 0; i < Scores.Length; i++)
                {
                    FinalScore.Score[j] += Scores[i].Score[j];
                }
                Math.Round(FinalScore.Score[j]);
            }
            return FinalScore;
        }


        public List<CalculatedScoreModel> KraScoreWithTeamSize(int AccountId, string Quarter, int Year)
        {
            List<CalculatedScoreModel> calculatedScore = CalculateProjectKraScore(AccountId, Quarter, Year);
            AccountTeamSizeModel accountteamsize = Teamsize.GetTeamSize(AccountId, Quarter, Year);
            int teamsize = 1;
            if (accountteamsize != null)
            {
                teamsize = accountteamsize.TeamSize;
            }

            foreach (var items in calculatedScore)
            {
                items.FinalScore = items.FinalScore * teamsize;
            }
            return calculatedScore;
        }
        //to be completed
        //to be tested
        public List<CalculatedScoreModel> KraScoreADM(int[] AccountId, string Quarter, int Year)
        {
            List<CalculatedScoreModel>[] list = new List<CalculatedScoreModel>[AccountId.Length];
            List<CalculatedScoreModel> final = new List<CalculatedScoreModel>();
            int totalsize = 0;
            int[] size = new int[AccountId.Length];

            for (int i = 0; i < AccountId.Length; i++)
            {
                totalsize += Teamsize.GetTeamSize(AccountId[i], Quarter, Year).TeamSize;
                list[i] = KraScoreWithTeamSize(AccountId[i], Quarter, Year);
                size[i] = list[i].Count;
            }
            Array.Sort(size);

            for (int j = 0; j < size[0]; j++)
            {
                CalculatedScoreModel ob = new CalculatedScoreModel();

                for (int i = 0; i < list.Length; i++)
                {
                    CalculatedScoreModel[] scoresob = list[i].ToArray();
                    ob.FinalScore += scoresob[j].FinalScore;
                    ob.ParameterName = scoresob[j].ParameterName;
                    ob.Weightage = scoresob[j].Weightage;
              }
                ob.FinalScore = ob.FinalScore / totalsize;
                final.Add(ob);
            }
            return final;
        }

        public List<CalculatedScoreYear> KraScoreADMYearly(int[] AccountId, int Year)
        {
            List<CalculatedScoreYear> Scores = new List<CalculatedScoreYear>();

            if (AccountId != null)
            {
                string[] quarters = AccountParams.GetQuarters(AccountId[0], Year);
                List<AccountParametersModel>[] parameters = new List<AccountParametersModel>[AccountId.Length];  //Gets the parameters for all the accounts
                List<AccountParametersModel> templist = new List<AccountParametersModel>();
                for (int i = 0; i < AccountId.Length; i++)
                {
                    parameters[i] = AccountParams.GetParameters(AccountId[i], Year);
                    foreach (var items in parameters[i])
                    {
                        templist.Add(items);
                    }
                }

                var Parameters = templist.GroupBy(a => a.ParameterName).Select(g => g.First()).ToList();
                foreach (var items in Parameters)
                {
                    CalculatedScoreYear Ob = new CalculatedScoreYear();
                    Ob.ParameterName = items.ParameterName;
                    Ob.Score = new float[quarters.Length];
                    Ob.Weightage = Convert.ToInt32(items.Weightage);
                    for (int i = 0; i < quarters.Length; i++)
                    {
                        List<CalculatedScoreModel> QuarterScore = KraScoreADM(AccountId, quarters[i], Year);
                        if (QuarterScore != null)
                        {
                            CalculatedScoreModel ob = (from bs in QuarterScore where bs.ParameterName == items.ParameterName select bs).FirstOrDefault();
                            if (ob != null)
                            {
                                Ob.Score[i] = ob.FinalScore;
                            }
                            else
                            {
                                Ob.Score[i] = 0;
                            }
                        }
                    }
                    Scores.Add(Ob);
                }
            }
            return Scores;
        }
    }
}
