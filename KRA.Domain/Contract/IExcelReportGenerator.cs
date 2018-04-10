using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRA.Domain.Contract
{
   public interface IExcelReportGenerator
    {
         XLWorkbook CreatePMReport(int AccountId, int Year);
        XLWorkbook CreateADMReport(int[] AccountId, int Year);
    }
}
