using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRA.Models
{
    public class AccountParametersModel

    {
        [Key]
        public int AccountParamID { get; set; }
        public int ParamID { get; set; }
        public int AccountID { get; set; }
        public string Quarter { get; set; }
        public string ParameterName { get; set; }
        public string Weightage { get; set; }
        public int Year { get; set; }
        public float Score { get; set; }
        public virtual ICollection<ParamaterBoundsModel> Bounds { get; set;}
        public virtual ICollection<KraInputScoresModel> Scores { get; set; }

        public AccountParametersModel() { }
    }
}