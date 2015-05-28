using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrisZoomDataApi.Model.Ndfbin.Types.AllTypes
{
    public class NdfFileNameString : NdfString
    {
        public NdfFileNameString(NdfStringReference value, long offset) : base(value, offset)
        {
            Type = NdfType.TableStringFile;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
