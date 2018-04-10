using KRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRA.Domain.Contract
{
  public  interface IKraScoreCalculator
    {
        Dictionary<int, Entities.KraInputScores> GetAccountScores(int AccountId, string Quarter, int Year);
        Dictionary<int, List<Models.ParamaterBoundsModel>> GetParameterBounds(int AccountId, string Quarter, int Year);
         List <CalculatedScoreModel> CalculateProjectKraScore(int AccountId, string Quarter, int Year);
        List<List<CalculatedScoreModel>> CalculateProjectKraScore(int AccountId, int Year);
        List<CalculatedScoreYear> AccountKraScoreYearly(int AccountId, int Year);
        CalculatedScoreYear PMFinalScore(int AccountId, int Year);
        List<CalculatedScoreModel> KraScoreADM(int[] AccountId, string Quarter, int Year);
        List<CalculatedScoreYear> KraScoreADMYearly(int[] AccountId, int Year);
        CalculatedScoreYear ADMFinalScore(int[] AccountId, int Year);

    }
}
