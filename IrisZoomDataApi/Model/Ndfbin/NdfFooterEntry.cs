namespace IrisZoomDataApi.Model.Ndfbin
{
    public class NdfFooterEntry
    {
        private string _name;
        private long _offset;
        private long _size;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
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

        public long Size
        {
            get { return _size; }
            set
            {
                _size = value;
            }
        }
    }
}