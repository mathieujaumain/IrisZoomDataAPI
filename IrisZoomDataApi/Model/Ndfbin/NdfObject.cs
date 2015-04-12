using System.Collections.Generic;
using System;
using IrisZoomDataApi.Model.Ndfbin.Types.AllTypes;

namespace IrisZoomDataApi.Model.Ndfbin
{
    public class NdfObject 
    {
        private readonly List<NdfPropertyValue> _propertyValues = new List<NdfPropertyValue>();
        private NdfClass _class;
        private byte[] _data;
        private uint _id;

        public NdfClass Class
        {
            get { return _class; }
            set { _class = value; }
        }

        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public List<NdfPropertyValue> PropertyValues
        {
            get { return _propertyValues; }
        }

        public uint Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public long Offset { get; set; }

        /// <summary>
        /// Return value from property name
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public NdfValueWrapper GetValueOfProperty(string propertyName)
        {
            foreach (NdfPropertyValue propval in PropertyValues)
            {
                if (propval.Property.Name == propertyName)
                {
                    return propval.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// Return property from name.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public NdfPropertyValue GetProperty(string propertyName)
        {
            foreach (NdfPropertyValue propval in PropertyValues)
            {
                if (propval.Property.Name == propertyName)
                {
                    return propval;
                }
            }
            return null;
        }

        public bool GetProperty(string propName, out NdfPropertyValue value)
        {
            foreach (NdfPropertyValue propval in PropertyValues)
            {
                if (propval.Property.Name == propName)
                {
                    value = propval;
                    return true;
                }
            }
            value = null;
            return false;
        }

        public bool TryGetValueFromPath(string propertyPath, out NdfValueWrapper value)
        {
            string[] pathParts = propertyPath.Split(new string[] { "." }, System.StringSplitOptions.RemoveEmptyEntries);
            int count = pathParts.Length;
            if (count > 0)
            {
                NdfPropertyValue nextproperty = GetProperty(pathParts[0]);

                if (GetProperty(pathParts[0], out nextproperty))
                {
                    propertyPath.Replace(pathParts[0] + ".", string.Empty);
                    switch (nextproperty.Type)
                    {
                        case Types.NdfType.ObjectReference:
                            NdfObjectReference reference = nextproperty.Value as NdfObjectReference;
                            return reference.Instance.TryGetValueFromPath(propertyPath, out value);

                        case Types.NdfType.MapList:
                            NdfMapList mapList = nextproperty.Value as NdfMapList;
                            return mapList.TryGetValueFromPath(propertyPath, out value); // TODO

                        case Types.NdfType.List:
                            NdfCollection list = nextproperty.Value as NdfCollection;
                            return list.TryGetValueFromPath(propertyPath, out value); // TODO

                        case Types.NdfType.Unknown :
                            break;
                        case Types.NdfType.Unset:
                            break;


                        default:
                            value = nextproperty.Value;
                            return true;
                    }
                }
            }
            value = null;
            return false;
        }


        public bool TryGetValueFromPath<T>(string propertyPath, out T value) where T : NdfValueWrapper
        {
            NdfValueWrapper val = null;
            value = null;
            if(TryGetValueFromPath(propertyPath, out val))
            {
                value = val as T;
                if (value != null)
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Return value from path going from this object
        /// </summary>
        /// <param name="propertyPath"> Path formated like <ObjectReference>.<ListProperty>.<ListIndex>.<PropertyName> </param>
        /// <returns></returns>
        public NdfValueWrapper GetValueFromPath(string propertyPath)
        {
            string[] pathParts = propertyPath.Split(new string[]{"."}, System.StringSplitOptions.RemoveEmptyEntries);
            int count = pathParts.Length;
            if (count > 0)
            {
                NdfPropertyValue nextproperty = GetProperty(pathParts[0]);
                if (nextproperty == null)
                {
                    propertyPath.Replace(pathParts[0] + ".", string.Empty);
                    switch (nextproperty.Type)
                    {
                        case Types.NdfType.ObjectReference:
                            NdfObjectReference reference = nextproperty.Value as NdfObjectReference;
                            return reference.Instance.GetValueFromPath(propertyPath);

                        case Types.NdfType.MapList:
                            NdfMapList mapList = nextproperty.Value as NdfMapList;
                            return mapList.getValueFromPath(propertyPath); 

                        case Types.NdfType.List:
                            NdfCollection list = nextproperty.Value as NdfCollection;
                            return list.getValueFromPath(propertyPath); 

                        default:
                            return nextproperty.Value;
                    }
                }

            }

            throw(new Exception("Something went wrong with this path: " + propertyPath != string.Empty ? propertyPath:"Empty Path"));
        }
    }
}
