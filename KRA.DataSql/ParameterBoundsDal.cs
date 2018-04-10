using KRA.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KRA.Entities;

namespace KRA.DataSql
{
    public class ParameterBoundsDal : IParameterBoundsDal
    {
        public bool AddParameterBound(ParameterBounds bounds)
        {
            using (KraContext Context = new KraContext())
            {
                ParameterBounds bound = Context.Bounds.Find(bounds.DefinitionID);
                if (bound == null)
                {

                    Context.Bounds.Add(bounds);
                    Context.SaveChanges();
                }
                else
                {
                    bound.MinValue = bounds.MinValue;
                    bound.MaxValue = bounds.MaxValue;
                    bound.Result = bounds.Result;
                    Context.SaveChanges();
                }
            }
            return true;
        }

        public List<ParameterBounds> GetParameterBounds(int AccountParamID)
        {
            List<ParameterBounds> AllBounds;
            using (KraContext Context = new KraContext())
            {
                AllBounds = (from bounds in Context.Bounds where bounds.AccountParamID == AccountParamID select bounds).ToList();

            }
            return AllBounds;
        }
    }
}
