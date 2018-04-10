using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRA.Entities
{
    public class AccountParameters

    {
        [Key]
        public int AccountParamID { get; set; }
        public int ParamID { get; set; }
        public int AccountID { get; set; }
        public string Quarter { get; set; }
        public  int Year { get; set; }
        public int  Weightage {get; set;}
        public virtual ICollection<ParameterBounds> Bounds { get; set;}
        public virtual ICollection<KraInputScores> Scores { get; set; }

        public AccountParameters() { }
    }
}