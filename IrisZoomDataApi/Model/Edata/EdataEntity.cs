using System.Globalization;
using IrisZoomDataApi;

namespace IrisZoomDataApi.Model.Edata
{
    public class EdataEntity
    {
        private int _groupId;
        private string _name;
        private int _fileEntrySize;

        public EdataEntity(EdataManager mgr)
        {
            Manager = mgr;
        }

        public EdataManager Manager
        {
            get;
            protected set;
        }

        public int GroupId
        {
            get { return _groupId; }
            set { _groupId = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int FileEntrySize
        {
            get { return _fileEntrySize; }
            set { _fileEntrySize = value;  }
        }

        public override string ToString()
        {
            return Name.ToString(CultureInfo.CurrentCulture);
        }
    }
}
