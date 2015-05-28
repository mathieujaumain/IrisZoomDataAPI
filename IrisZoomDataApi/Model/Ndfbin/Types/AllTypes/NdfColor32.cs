using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace IrisZoomDataApi.Model.Ndfbin.Types.AllTypes
{
    public class NdfColor32 : NdfFlatValueWrapper
    {
        public NdfColor32(Color value, long offset)
            : base(NdfType.Color32, value, offset)
        {

        }

        public override byte[] GetBytes(out bool valid)
        {
            valid = true;

            var col = (Color)Value;

            var colorArray = new byte[] { col.A, col.R, col.G, col.B };

            return colorArray;
        }

        public new Color Value
        {
            get { return (Color)base.Value; }
            set { base.Value = value; }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
