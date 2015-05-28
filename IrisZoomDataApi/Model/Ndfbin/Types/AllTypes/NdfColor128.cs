using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using IrisZoomDataApi.Util;

namespace IrisZoomDataApi.Model.Ndfbin.Types.AllTypes
{
    public class NdfColor128 : NdfFlatValueWrapper 
    {
        public NdfColor128(byte[] value, long offset) : base(NdfType.Color128, value, offset)
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
            return string.Format("Vec4: {0}", Utils.ByteArrayToBigEndianHeyByteString(Value));
        }

    }
}
