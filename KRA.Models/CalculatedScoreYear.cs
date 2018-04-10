using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRA.Models
{
    public class CalculatedScoreYear
    {
        public int AccountParamID { get; set; }
        public float[] Score{ get; set; }
        public string ParameterName { get; set; }
        public int AccountID { get; set; }
        public int Weightage { get; set; }
    }
}
