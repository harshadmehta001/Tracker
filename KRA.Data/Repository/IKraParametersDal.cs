using KRA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KRA.Data.Repository
{
   public interface IKraParametersDal
    {
        bool AddParameter(KraParameters Parameter);
        List<KraParameters> GetParameters();
    }
}
