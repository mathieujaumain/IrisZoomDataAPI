using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrisZoomDataApi.Model.Ndfbin.Types.AllTypes
{
    public class NdfNull : NdfValueWrapper
    {
        public NdfNull(long offset)
            : base(NdfType.Unset, offset)
        {
        }

        public override string ToString()
        {
            return "null";
        }

        public override byte[] GetBytes(out bool valid)
        {
            valid = false;

            return new byte[0];
        }
    }
}
