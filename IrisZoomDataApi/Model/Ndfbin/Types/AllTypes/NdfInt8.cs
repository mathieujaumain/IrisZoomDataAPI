using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrisZoomDataApi.Model.Ndfbin.Types.AllTypes
{
    public class NdfInt8 : NdfFlatValueWrapper
    {
        public NdfInt8(byte value, long offset)
            : base(NdfType.Int8, value, offset)
        {
            
        }

        public override byte[] GetBytes(out bool valid)
        {
            valid = true;

            try
            {
                return new byte[1] { (byte)Value };
            }
            catch (Exception e)
            {
                valid = false;
                return new byte[0];
            }
        }

        public new byte Value
        {
            get { return (byte)base.Value; }
            set { base.Value = value; }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
