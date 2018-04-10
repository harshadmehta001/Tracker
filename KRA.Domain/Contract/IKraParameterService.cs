using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRA.Domain.Contract
{
   public interface IKraParameterService

    {
        bool AddParameter(Models.KraParametersModel Parameter);
        List<Models.KraParametersModel> GetAllParameters();
    
    }
}
