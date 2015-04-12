using System;
using System.Windows.Data;
using System.Windows.Input;
using IrisZoomDataApi;
using IrisZoomDataApi.Model.Ndfbin.Types;
using IrisZoomDataApi.Model.Ndfbin.Types.AllTypes;
using IrisZoomDataApi.Util;

namespace IrisZoomDataApi.Model.Ndfbin
{
    public class NdfPropertyValue : IValueHolder
    {
        private NdfObject _instance;
        private NdfProperty _property;
        private NdfValueWrapper _value;
        private byte[] _valueData;

        public NdfPropertyValue(NdfObject instance)
        {
            _instance = instance;
        }


        public NdfType Type
        {
            get
            {
                if (Value == null)
                    return NdfType.Unset;
                else
                    return Value.Type;
            }
        }

        public byte[] ValueData
        {
            get { return _valueData; }
            set
            {
                _valueData = value; 
            }
        }

        public string BinValue
        {
            get { return Utils.ByteArrayToBigEndianHeyByteString(ValueData); }
        }

        public NdfValueWrapper Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public NdfbinManager Manager
        {
            get { return Property.Class.Manager; }
        }

        public long InstanceOffset
        {
            get
            {
                return Instance.Offset;
            }
        }

        public NdfProperty Property
        {
            get { return _property; }
            set { _property = value;}
        }

        public NdfObject Instance
        {
            get { return _instance; }
            set
            {
                _instance = value;
            }
        }

        private byte[] _oldVal = new byte[0];

    }
}
