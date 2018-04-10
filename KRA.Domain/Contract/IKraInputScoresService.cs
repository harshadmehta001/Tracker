using KRA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRA.Domain.Contract
{
   public interface IKraInputScoresService
    {
        bool AddScores(Models.KraInputScoresModel Score);
        KraInputScores GetScores(int AccountParamId);
    }
}
