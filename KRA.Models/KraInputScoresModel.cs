using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRA.Models
{
    public class KraInputScoresModel
    {
        [Key]
        public int ScoreID { get; set; }
        public int AccountParamID { get; set; }
        public float Score { get; set; }
        public int Year { get; set; }
        public DateTime AddedOn { get; set; }
        public string AddedBy { get; set; }
        public virtual ICollection<KraInputScoresModel> Scores { get; set; }
    }
}
