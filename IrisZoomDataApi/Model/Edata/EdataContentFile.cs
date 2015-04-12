using System.Globalization;
using IrisZoomDataApi;

namespace IrisZoomDataApi.Model.Edata
{
    /// <summary>
    /// The reversed struct from Hex Workshop - much love to Hob_gadling for his hard work and help.
    /// The chunks after offset and fileSize are because of long (int64)
    /// 
    /// struct dictFileEntry {
    ///     DWORD groupId;
    ///     DWORD fileEntrySize;
    ///     DWORD offset;
    ///     DWORD chunk2;   
    ///     DWORD fileSize;
    ///     DWORD chunk4;
    ///     blob checksum[16];
    ///     zstring name;
    /// };
    /// </summary>
    public class EdataContentFile : EdataEntity
    {
        private uint _id;
        private string _path;
        private long _offset;
        private long _size;
        private byte[] _checkSum = new byte[16];

        private EdataFileType _fileType = EdataFileType.Unknown;

        public EdataContentFile(EdataManager mgr) : base(mgr)
        {
        }

        public string Path
        {
            get { return _path; }
            set { _path = value;  }
        }

        public long Offset
        {
            get { return _offset; }
            set { _offset = value;  }
        }

        public long Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public byte[] Checksum
        {
            get { return _checkSum; }
            set { _checkSum = value;  }
        }

        public uint Id
        {
            get { return _id; }
            set { _id = value;  }
        }

        public EdataFileType FileType
        {
            get { return _fileType; }
            set { _fileType = value;  }
        }

        public override string ToString()
        {
            return  Path.ToString(CultureInfo.CurrentCulture);
        }
    }
}
