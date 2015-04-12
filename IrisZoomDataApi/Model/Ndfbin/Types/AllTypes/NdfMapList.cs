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

        public NdfMap GetMap(string key)
        {
            NdfMap map;
            NdfStringReference str;
            foreach (CollectionItemValueHolder itemval in InnerList)
            {
                map = itemval.Value as NdfMap;
                str = ((NdfString)map.Key.Value).Value as NdfStringReference; // Bleh
                if (str.Value == key)
                    return map;
            }
            return null;
        }

        public bool TryGetMap(string key, out NdfMap value)
        {

            NdfStringReference str;
            foreach (CollectionItemValueHolder itemval in InnerList)
            {
                NdfMap map = itemval.Value as NdfMap;
                str = ((NdfString)map.Key.Value).Value as NdfStringReference; // Bleh
                if (str.Value == key)
                {
                    value = map;
                    return true;
                }
            }
            value = null;
            return false;
        }

        public NdfValueWrapper getValueFromPath(string propertyPath)
        {
            string[] pathParts = propertyPath.Split(new string[]{"."}, System.StringSplitOptions.RemoveEmptyEntries);
            int count = pathParts.Length;
            if (count > 0)
            {
                NdfMap nextMap = GetMap(pathParts[0]);
                propertyPath.Replace(pathParts[0] + ".", string.Empty);
                switch (nextMap.Type)
                {
                    case NdfType.ObjectReference:
                        NdfObjectReference reference = nextMap.Value as NdfObjectReference;
                        return reference.Instance.GetValueFromPath(propertyPath);

                    case NdfType.MapList:
                        NdfMapList mapList = nextMap.Value as NdfMapList;
                        return mapList.getValueFromPath(propertyPath);

                    case NdfType.List:
                        NdfCollection list = nextMap.Value as NdfCollection;
                        return list.getValueFromPath(propertyPath); // TODO

                    default:
                        return nextMap;
                }
            }

            throw (new Exception("Something went wrong with this path: " + propertyPath != string.Empty ? propertyPath : "empty path"));
        }

        public bool TryGetValueFromPath(string propertyPath, out NdfValueWrapper value)
        {
            string[] pathParts = propertyPath.Split(new string[] { "." }, System.StringSplitOptions.RemoveEmptyEntries);
            int count = pathParts.Length;
            if (count > 0)
            {
                NdfMap nextMap = GetMap(pathParts[0]);
                if (TryGetMap(pathParts[0], out nextMap))
                {
                    propertyPath.Replace(pathParts[0] + ".", string.Empty);
                    switch (nextMap.Type)
                    {
                        case NdfType.ObjectReference:
                            NdfObjectReference reference = nextMap.Value as NdfObjectReference;
                            return reference.Instance.TryGetValueFromPath(propertyPath, out value);

                        case NdfType.MapList:
                            NdfMapList mapList = nextMap.Value as NdfMapList;
                            return mapList.TryGetValueFromPath(propertyPath, out value);

                        case NdfType.List:
                            NdfCollection list = nextMap.Value as NdfCollection;
                            return list.TryGetValueFromPath(propertyPath, out value); // TODO

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
