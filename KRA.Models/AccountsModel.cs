using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRA.Models
{
    public class AccountsModel
    {
        [Key]
        public int AccountID {  get;set;}
        public string Name {get; set;}
        public int ManagerID {get; set;}
        public int Year { get; set; }
        public virtual ICollection<AccountParametersModel>  Params { get; set;}
        public AccountsModel() { }
    }
}
