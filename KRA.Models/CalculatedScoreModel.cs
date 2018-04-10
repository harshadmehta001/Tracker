using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRA.Models
{
   public class CalculatedScoreModel
    {
        public int AccountParamID { get; set; }
        public string ParameterName { get; set; }
        public int Weightage { get; set; }

        public float FinalScore { get; set; }
        public int AccountID { get; set; }
    }
}
