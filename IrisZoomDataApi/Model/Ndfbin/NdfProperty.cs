using System.Globalization;
using IrisZoomDataApi.Util;

namespace IrisZoomDataApi.Model.Ndfbin
{
    public class NdfProperty
    {
        private NdfClass _class;
        private int _id;
        private string _name;
        private long _offset;

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }

        public string BinId
        {
            get { return Utils.Int32ToBigEndianHexByteString(Id); }
        }

        public NdfClass Class
        {
            get { return _class; }
            set
            {
                _class = value;
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

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }

        public override string ToString()
        {
            return Name.ToString(CultureInfo.InvariantCulture);
        }
    }
}