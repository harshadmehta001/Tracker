using KRA.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KRA.Entities;

namespace KRA.DataSql
{
    public class KraInputScoresDal : IKraInputScoresDal
    {
        public bool AddScore(KraInputScores score)
        {
            using (KraContext Context = new KraContext())
            {
                KraInputScores Scores = (from Score in Context.Score where Score.AccountParamID == score.AccountParamID select Score).SingleOrDefault();
                if (Scores != null)
                {
                    Scores.Score = score.Score;
                    Context.SaveChanges();
                }
                else
                    Context.Score.Add(score);
                 Context.SaveChanges();
            }
            return true;
        }
        public KraInputScores GetScore(int AccountParamId)
        {
            KraInputScores Scores;
            using (KraContext Context = new KraContext())
            {
                Scores = (from Score in Context.Score where Score.AccountParamID == AccountParamId select Score).SingleOrDefault();
            }
            return Scores;
        }
    }
}
