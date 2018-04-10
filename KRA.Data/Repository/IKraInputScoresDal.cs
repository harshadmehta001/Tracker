using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KRA.Entities;

namespace KRA.Data.Repository
{
    public interface IKraInputScoresDal
    {
        bool AddScore(KraInputScores score);
        KraInputScores GetScore(int AccountParamId);
    }
}
