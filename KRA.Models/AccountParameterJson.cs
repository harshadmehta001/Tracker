using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRA.Models
{
 public  class AccountParameterJson
    {
        public int ProjectId { get; set; }
        public int [] Paramters { get; set; }
        public string Quarter { get; set; }
        public int Year { get; set; }
    }
}
