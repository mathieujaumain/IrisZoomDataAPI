using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrisZoomDataApi.Model.Texture
{
    public class TgvMipMap
    {
        private uint _offset;
        private uint _size;
        private int _mipWidth;
        private byte[] _content;

        public TgvMipMap()
        {
        }

        public TgvMipMap(uint offset, uint size, ushort mipWidth)
        {
            Offset = offset;
            Size = size;
            MipWidth = mipWidth;
        }

        public uint Offset
        {
            get { return _offset; }
            set { _offset = value;  }
        }

        public uint Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public int MipWidth
        {
            get { return _mipWidth; }
            set { _mipWidth = value; }
        }

        public byte[] Content
        {
            get { return _content; }
            set { _content = value; }
        }
    }
}