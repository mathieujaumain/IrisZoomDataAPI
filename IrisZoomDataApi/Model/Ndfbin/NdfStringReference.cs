using System.Globalization;

namespace IrisZoomDataApi.Model.Ndfbin
{
    public class NdfStringReference
    {
        private int _id;
        private long _offset;
        private string _value;

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }

        public long Offset
        {
            get { return _offset; }
            set
            {
                _offset = value;
            }
        }

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
            }
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}