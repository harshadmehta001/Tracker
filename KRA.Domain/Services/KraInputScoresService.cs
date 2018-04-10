using KRA.Domain.Contract;
using System;
using System.Collections.Generic;
using KRA.Entities;
using KRA.Data.Repository;

namespace KRA.Domain.Services
{
    public class KraInputScoresService : IKraInputScoresService
    {
        private readonly IKraInputScoresDal InputScoresDal;
        private readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(KraInputScoresService));
        public KraInputScoresService(IKraInputScoresDal InputScoresDal)
        {
            this.InputScoresDal = InputScoresDal;
        }
        public Entities.KraInputScores InputScoresEntityToModelMapper(Models.KraInputScoresModel Scores)
        {
            Entities.KraInputScores EntityScore = new Entities.KraInputScores()
            {
                AccountParamID = Scores.AccountParamID,
                AddedBy = Scores.AddedBy,
                AddedOn = Scores.AddedOn,
                Score = Scores.Score,
                Year = Scores.Year
            };
            return EntityScore;
        }

        public bool AddScores(Models.KraInputScoresModel Score)
        {

            Entities.KraInputScores Scores = InputScoresEntityToModelMapper(Score);
            if (InputScoresDal.AddScore(Scores))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public KraInputScores GetScores(int AccountParamId)
        {
            KraInputScores Scores;
            try
            {
                Scores = InputScoresDal.GetScore(AccountParamId);
            }
            catch (Exception ex)
            {
                logger.Error("Exception in Get Scores Method");
                logger.Error(ex.ToString());
                Scores = null;
            }
            return Scores;
        }
    }
}
