using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KRA.Entities;
namespace KRA.Data.Repository
{
 public interface IParameterBoundsDal
    {
        bool AddParameterBound(ParameterBounds bounds);
        List<ParameterBounds> GetParameterBounds(int AccountParamID);
       
    }
}
