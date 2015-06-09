using System.Collections.Generic;
using System.Windows.Media;
using IrisZoomDataApi.BL.DDS;
using PixelFormats = IrisZoomDataApi.BL.DDS.PixelFormats;

namespace IrisZoomDataApi.Model.Texture
{
    public class TgvFile
    {
        private PixelFormats _format;
        private uint _height;
        private uint _imageHeight;
        private uint _imageWidth;
        private bool _isCompressed;

        private ushort _mipMapCount;

        private List<uint> _offsets = new List<uint>();
        private string _pixelFormatStr;
        private List<uint> _sizes = new List<uint>();
        private byte[] _sourceChecksum;
        private uint _version;
        private uint _width;

        private readonly List<TgvMipMap> _mipMaps = new List<TgvMipMap>();

        public uint Version
        {
            get { return _version; }
            set
            {
                _version = value;
            }
        }

        public bool IsCompressed
        {
            get { return _isCompressed; }
            set
            {
                _isCompressed = value;
             }
        }

        public uint Width
        {
            get { return _width; }
            set
            {
                _width = value;
            }
        }

        public uint Height
        {
            get { return _height; }
            set
            {
                _height = value;
            }
        }

        public uint ImageWidth
        {
            get { return _imageWidth; }
            set
            {
                _imageWidth = value;
            }
        }

        public uint ImageHeight
        {
            get { return _imageHeight; }
            set
            {
                _imageHeight = value;
            }
        }

        public ushort MipMapCount
        {
            get { return _mipMapCount; }
            set
            {
                _mipMapCount = value;
            }
        }

        public PixelFormats Format
        {
            get { return _format; }
            set
            {
                _format = value;
            }
        }

        public byte[] SourceChecksum
        {
            get { return _sourceChecksum; }
            set
            {
                _sourceChecksum = value;
            }
        }

        public List<uint> Offsets
        {
            get { return _offsets; }
            set
            {
                _offsets = value;
            }
        }

        public List<uint> Sizes
        {
            get { return _sizes; }
            set
            {
                _sizes = value;
            }
        }

        public string PixelFormatStr
        {
            get { return _pixelFormatStr; }
            set
            {
                _pixelFormatStr = value;
            }
        }

        public List<TgvMipMap> MipMaps
        {
            get { return _mipMaps; }
        }
    }
}