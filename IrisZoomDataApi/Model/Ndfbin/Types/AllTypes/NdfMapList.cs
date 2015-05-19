using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrisZoomDataApi.Model.Ndfbin.Types.AllTypes
{
    public class NdfMapList : NdfCollection
    {
        public NdfMapList(long offset) : base(offset)
        {
            Type = NdfType.MapList;
        }

        public override byte[] GetBytes(out bool valid)
        {
            valid = true;

            bool itemValid;

            var data = new List<byte>();

            data.AddRange(BitConverter.GetBytes(InnerList.Count));

            foreach (var valueHolder in InnerList)
            {
                var valueDat = valueHolder.Value.GetBytes(out itemValid);

                if (!itemValid)
                    continue;

                //if (valueHolder.Value.Type == NdfType.ObjectReference || valueHolder.Value.Type == NdfType.TransTableReference)
                //    data.AddRange(BitConverter.GetBytes((uint)NdfType.Reference));

                //data.AddRange(BitConverter.GetBytes((uint)valueHolder.Value.Type));
                data.AddRange(valueDat);
            }

            return data.ToArray();
        }

        private NdfStringReference FromItem2StringRef(NdfValueWrapper val)
        {
            NdfMap map = val as NdfMap;
            return ((NdfString)map.Key.Value).Value as NdfStringReference; // Bleh
        }

        public NdfMap GetMap(string key)
        {

            NdfMap map;
            try
            {
                map = InnerList.First(x => FromItem2StringRef(x.Value).Value == key).Value as NdfMap;
            }
            catch
            {
                map = null;
            }

            return map;
        }

        public bool TryGetMap(string key, out NdfMap value)
        {

            value = GetMap(key);
            if (value != null)
                return true;
            return false;
        }

        public NdfValueWrapper GetValueFromQuery(string query)
        {
            string rest = string.Empty;
            string next = NdfQueryReader.ParseNextStep(query, out rest);

            if (!string.IsNullOrEmpty(next))
            {
                NdfMap nextMap = GetMap(next);
                switch (nextMap.Type)
                {
                    case NdfType.ObjectReference:
                        NdfObjectReference reference = nextMap.Value as NdfObjectReference;
                        return reference.Instance.GetValueFromQuery(rest);

                    case NdfType.MapList:
                        NdfMapList mapList = nextMap.Value as NdfMapList;
                        return mapList.GetValueFromQuery(rest);

                    case NdfType.List:
                        NdfCollection list = nextMap.Value as NdfCollection;
                        return list.GetValueFromQuery(rest); // TODO

                    default:
                        return nextMap;
                }
            }

            throw (new Exception("Something went wrong with this path: " + query != string.Empty ? query : "empty path"));
        }

        public bool TryGetValueFromQuery(string propertyPath, out NdfValueWrapper value)
        {
            string rest = string.Empty;
            string next = NdfQueryReader.ParseNextStep(propertyPath, out rest);

            if (!string.IsNullOrEmpty(next))
            {
                NdfMap nextMap;
                if (TryGetMap(next, out nextMap))
                {
                    switch (nextMap.Type)
                    {
                        case NdfType.ObjectReference:
                            NdfObjectReference reference = nextMap.Value as NdfObjectReference;
                            return reference.Instance.TryGetValueFromQuery(rest, out value);

                        case NdfType.MapList:
                            NdfMapList mapList = nextMap.Value as NdfMapList;
                            return mapList.TryGetValueFromQuery(rest, out value);

                        case NdfType.List:
                            NdfCollection list = nextMap.Value as NdfCollection;
                            return list.TryGetValueFromQuery(rest, out value);

                        case Types.NdfType.Unknown:
                            break;
                        case Types.NdfType.Unset:
                            break;

                        default:
                            value = nextMap;
                            return true;
                    }
                }
            }
            value = null;
            return false;
        }
    }
}
