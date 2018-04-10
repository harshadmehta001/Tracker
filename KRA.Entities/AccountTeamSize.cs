using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRA.Entities
{
    public class AccountTeamSize
    {
        [Key]
        public int TeamSizeID { get; set; }
        public int AccountId { get; set; }
        public int TeamSize { get; set; }
        public string Quarter { get; set; }
        public int Year { get; set; }
        public DateTime AddedOn { get; set; }
        public string AddedBy { get; set; }

        public AccountTeamSize() { }
    }
}
