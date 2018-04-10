using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRA.Entities
{
   public class Accounts
    {
        [Key]
        public int AccountID {get;set;}
        public string Name {get; set;}
        public int ManagerID {get; set;}
        public virtual ICollection<AccountParameters>  Params { get; set;}
        public virtual ICollection<AccountTeamSize> TeamSize { get; set; }
        public Accounts() { }
    }
}
