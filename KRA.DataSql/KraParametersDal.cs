using KRA.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KRA.Entities;

namespace KRA.DataSql
{
    public class KraParametersDal : IKraParametersDal
    {
        public bool AddParameter(KraParameters Parameter)
        {
            var result = true;
          using(KraContext Context = new KraContext())
            {
                KraParameters parameter = (from bs in Context.Params where bs.ParamName.ToLower() == Parameter.ParamName.ToLower() || bs.ParamName.ToUpper() == Parameter.ParamName.ToUpper() select bs).SingleOrDefault();
                if (parameter != null)
                {
                    result = false;
                }
                else
                {
                    Context.Params.Add(Parameter);
                    Context.SaveChanges();
                }
            }
            return result;
        }

        public List<KraParameters> GetParameters()
        {
            List<KraParameters> AllParameters;
            using (KraContext Context = new KraContext()) {
                AllParameters = (from prams in Context.Params select prams).ToList();
            }
            return AllParameters;
        }
    }
}
