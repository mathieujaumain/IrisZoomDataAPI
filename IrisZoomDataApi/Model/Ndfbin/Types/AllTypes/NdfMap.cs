using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrisZoomDataApi.Model.Ndfbin.Types.AllTypes
{
    public class NdfMap : NdfFlatValueWrapper
    {
        private MapValueHolder _key;

        public NdfMap(MapValueHolder key, MapValueHolder value, long offset)
            : base(NdfType.Map, value, offset)
        {
            Key = key;
        }

        public MapValueHolder Key
        {
            get { return _key; }
            set { _key = value;  }
        }

        public override byte[] GetBytes(out bool valid)
        {
            valid = false;

            var mapdata = new List<byte>();

            var key = Key.Value.GetBytes(out valid).ToList();
            var value = ((MapValueHolder)Value).Value.GetBytes(out valid).ToList();

            if (!valid)
                return new byte[0];

            /// TODO Why do we have to add Map here?
            //mapdata.AddRange(BitConverter.GetBytes((uint)NdfType.Map));

            if (Key.Value.Type == NdfType.ObjectReference || Key.Value.Type == NdfType.TransTableReference)
                mapdata.AddRange(BitConverter.GetBytes((uint)NdfType.Reference));

            mapdata.AddRange(BitConverter.GetBytes((uint)Key.Value.Type));
            mapdata.AddRange(key);

            if (((MapValueHolder)Value).Value.Type == NdfType.ObjectReference || ((MapValueHolder)Value).Value.Type == NdfType.TransTableReference)
                mapdata.AddRange(BitConverter.GetBytes((uint)NdfType.Reference));

            mapdata.AddRange(BitConverter.GetBytes((uint)((MapValueHolder)Value).Value.Type));
            mapdata.AddRange(value);

            return mapdata.ToArray();
        }

        public override string ToString()
        {
            return string.Format("Map: {0} : {1}", Key.Value, ((MapValueHolder)Value).Value);
        }
    }
}
