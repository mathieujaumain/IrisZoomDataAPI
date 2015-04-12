using System;

namespace IrisZoomDataApi.Model.Ndfbin.Types.AllTypes
{
    public class NdfString : NdfFlatValueWrapper
    {
        public NdfString(NdfStringReference value, long offset)
            : base(NdfType.TableString, value, offset)
        {
        }

        public override byte[] GetBytes(out bool valid)
        {
            valid = true;

            return BitConverter.GetBytes(((NdfStringReference)Value).Id);
        }
    }
}
