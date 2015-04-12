using IrisZoomDataApi.Util;

namespace IrisZoomDataApi.Model.Trad
{
    public class TradEntry
    {
        private byte[] _hash;
        private string _hashView;
        private uint _offsetDic;
        private uint _offsetCont;
        private uint _contLen;
        private string _content;

        private bool _userCreated;

        public string HashView
        {
            get
            {
                return _hashView;
            }
            protected set
            {
                _hashView = value;
            }
        }

        public byte[] Hash
        {
            get { return _hash; }
            set
            {
                _hash = value;
                HashView = Utils.ByteArrayToBigEndianHeyByteString(_hash);
            }
        }

        public uint OffsetDic
        {
            get { return _offsetDic; }
            set { _offsetDic = value; }
        }

        public uint OffsetCont
        {
            get { return _offsetCont; }
            set { _offsetCont = value; }
        }

        public uint ContLen
        {
            get { return _contLen; }
            set { _contLen = value; }
        }

        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
            }
        }

        public bool UserCreated
        {
            get { return _userCreated; }
            set { _userCreated = value; }
        }
    }
}
