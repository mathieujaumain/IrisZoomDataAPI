using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrisZoomDataApi.Model.Ndfbin.Types.AllTypes
{
    public class NdfDouble_2 : NdfDouble
    {
        public NdfDouble_2(double value, long offset) : base(value, offset)
        {
            Type = NdfType.Float64_2;
        }

        public new double Value
        {
            get { return Convert.ToDouble(Value); }
            set { base.Value = value; }
        }
    }
}
