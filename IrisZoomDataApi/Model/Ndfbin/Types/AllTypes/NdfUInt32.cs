using System;

namespace IrisZoomDataApi.Model.Ndfbin.Types.AllTypes
{
    public class NdfUInt32 : NdfFlatValueWrapper
    {
        public NdfUInt32(uint value, long offset)
            : base(NdfType.UInt32, value, offset)
        {

        }

        public override byte[] GetBytes(out bool valid)
        {
            valid = true;

            try
            {
                return BitConverter.GetBytes(Convert.ToUInt32(Value));
            }
            catch (Exception e)
            {
                valid = false;
                return new byte[0];
            }
        }

        public new uint Value
        {
            get { return (uint)base.Value; }
            set { base.Value = value; }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
