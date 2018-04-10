using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRA.Models
{

    public class KraParametersModel 
    {
        [Key]
        public int ParamID { get; set; }
        public string ParamName { get; set; }
        public string AddedBy { get; set; }
        public DateTime AddedOn { get; set; }
        public virtual ICollection<AccountParametersModel> AccountParam { get; set; }
       
        public KraParametersModel() { }
    }
}
