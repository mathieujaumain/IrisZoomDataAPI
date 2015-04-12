using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrisZoomDataApi.Model.Ndfbin.Types.AllTypes
{
    class NdfTrans : NdfFlatValueWrapper
    {
        public NdfTrans(NdfTranReference value, long offset)
            : base(NdfType.TransTableReference, value, offset)
        {
        }

        public override byte[] GetBytes(out bool valid)
        {
            valid = true;

            return BitConverter.GetBytes(((NdfTranReference)Value).Id);
        }
    }

}
