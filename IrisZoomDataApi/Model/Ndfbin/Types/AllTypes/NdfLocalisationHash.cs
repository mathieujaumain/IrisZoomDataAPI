using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrisZoomDataApi.Util;

namespace IrisZoomDataApi.Model.Ndfbin.Types.AllTypes
{
    public class NdfLocalisationHash : NdfFlatValueWrapper
    {
        public NdfLocalisationHash(byte[] value, long offset)
            : base(NdfType.LocalisationHash, value, offset)
        {

        }

        public new byte[] Value
        {
            get { return (byte[])base.Value; }
            set
            {
                base.Value = value;
            }
        }

        public override byte[] GetBytes(out bool valid)
        {
            valid = true;

            return Value;
        }

        public override string ToString()
        {
            return Utils.ByteArrayToBigEndianHeyByteString(Value);
        }
    }
}
