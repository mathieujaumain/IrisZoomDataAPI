using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrisZoomDataApi.Model.Ndfbin.Types.AllTypes
{
    public class NdfWideString : NdfFlatValueWrapper
    {
        public NdfWideString(string value, long offset)
            : base(NdfType.WideString, value, offset)
        {

        }

        public override byte[] GetBytes(out bool valid)
        {
            valid = true;
            var data = new List<byte>();

            var val = (string)Value;

            var valBytes = Encoding.Unicode.GetBytes(val);

            data.AddRange(BitConverter.GetBytes(valBytes.Length));
            data.AddRange(valBytes);

            return data.ToArray();
        }

        public new string Value
        {
            get { return (string)base.Value; }
            set { base.Value = value; }
        }

        public override string ToString()
        {
            return Value.ToString();
        }

    }
}
