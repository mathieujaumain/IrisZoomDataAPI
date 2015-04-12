using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrisZoomDataApi.Model.Ndfbin.Types.AllTypes
{
    public class NdfGuid : NdfFlatValueWrapper
    {
        public NdfGuid(Guid value, long offset)
            : base(NdfType.Guid, value, offset)
        {

        }

        public override byte[] GetBytes(out bool valid)
        {
            valid = true;

            return Guid.Parse(Value.ToString()).ToByteArray();
        }

        public new Guid Value
        {
            get { return Guid.Parse(base.Value.ToString()); }
            set { base.Value = value; }
        }
    }
}
