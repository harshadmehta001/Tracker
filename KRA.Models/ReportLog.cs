using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRA.Models
{
    public class ReportLogModel
    {
        [Key]
        public int ReportID { get; set; }
        public string DownloadedBy { get; set; }
        public DateTime DownloadDate { get; set; }
    }
}
