using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRA.Entities
{
    public class KraParameters
    {
        [Key]
        public int ParamID { get; set; }
        public string ParamName { get; set; }
        public string AddedBy { get; set; }
        public string Category { get; set; }
        public DateTime AddedOn { get; set; }
        public virtual ICollection<AccountParameters> AccountParam { get; set; }
       
        public KraParameters() { }
    }
}
