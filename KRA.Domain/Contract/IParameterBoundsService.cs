using KRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRA.Domain.Contract
{
   public    interface IParameterBoundsService
    {
        bool AddBounds(ParamaterBoundsModel Bounds);
        List<ParamaterBoundsModel> GetBounds(int AccountParamId);
        List<ParamaterBoundsModel> CopyPreviousParameterBounds(AccountParametersModel parameter);


    }
}
