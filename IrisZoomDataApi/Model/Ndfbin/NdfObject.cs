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

            NdfPropertyValue propval = GetProperty(propertyName);
            if (propval != null)
                return propval.Value;
            return null;
        }

        /// <summary>
        /// Return property from name.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public NdfPropertyValue GetProperty(string propertyName)
        {
            return PropertyValues.Find(x => x.Property.Name == propertyName);
        }

        /// <summary>
        /// Return true if it succeded getting a property and output it.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public bool TryGetProperty(string propName, out NdfPropertyValue value)
        {
            value = GetProperty(propName);
            return value != null;
        }

        /// <summary>
        /// Return true if it succeded getting a property from the query and output it.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValueFromQuery(string query, out NdfValueWrapper value)
        {
            string rest = string.Empty;
            string next = NdfQueryReader.ParseNextStep(query, out rest);

            if(!string.IsNullOrEmpty(next))
            {
                NdfPropertyValue nextproperty;

                if (TryGetProperty(next, out nextproperty))
                {
                    

                    switch (nextproperty.Type)
                    {
                        case Types.NdfType.ObjectReference:
                            NdfObjectReference reference = nextproperty.Value as NdfObjectReference;
                            if (string.IsNullOrEmpty(rest))
                            {
                                value = reference;
                                return true;
                            }
                            return reference.Instance.TryGetValueFromQuery(rest, out value);

                        case Types.NdfType.MapList:
                            NdfMapList mapList = nextproperty.Value as NdfMapList;
                            if (string.IsNullOrEmpty(rest))
                            {
                                value = mapList;
                                return true;
                            }
                            return mapList.TryGetValueFromQuery(rest, out value);

                        case Types.NdfType.List:
                            NdfCollection list = nextproperty.Value as NdfCollection;
                            if (string.IsNullOrEmpty(rest))
                            {
                                value = list;
                                return true;
                            }
                            return list.TryGetValueFromQuery(rest, out value);

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

        /// <summary>
        /// Return true if it succeded getting a property from the query and cast it into T, then output it.
        /// </summary>
        /// <param name="propertyPath"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValueFromQuery<T>(string query, out T value) where T : NdfValueWrapper
        {
            NdfValueWrapper val = null;
            value = null;
            if (TryGetValueFromQuery(query, out val))
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
        public NdfValueWrapper GetValueFromQuery(string query)
        {
            string rest = string.Empty;
            string next = NdfQueryReader.ParseNextStep(query, out rest);

            if (!string.IsNullOrEmpty(next))
            {
                NdfPropertyValue nextproperty = GetProperty(next);
                if (nextproperty == null)
                {
                    switch (nextproperty.Type)
                    {
                        case Types.NdfType.ObjectReference:
                            NdfObjectReference reference = nextproperty.Value as NdfObjectReference;
                            return reference.Instance.GetValueFromQuery(rest);

                        case Types.NdfType.MapList:
                            NdfMapList mapList = nextproperty.Value as NdfMapList;
                            return mapList.GetValueFromQuery(rest); 

                        case Types.NdfType.List:
                            NdfCollection list = nextproperty.Value as NdfCollection;
                            return list.GetValueFromQuery(rest); 

                        default:
                            return nextproperty.Value;
                    }
                }
            }

            throw(new Exception("Something went wrong with this path: " + query != string.Empty ? query:"Empty Path"));
        }
    }
}
