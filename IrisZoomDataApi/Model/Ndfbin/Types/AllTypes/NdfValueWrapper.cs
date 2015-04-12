namespace IrisZoomDataApi.Model.Ndfbin.Types.AllTypes
{
    public abstract class NdfValueWrapper {
        private NdfType _type;

        protected NdfValueWrapper(NdfType type, long offset) 
        {
            Type = type;
            OffSet = offset;
        }

        public NdfType Type
        {
            get { return _type; }
            protected set { _type = value;  }
        }

        public long OffSet
        {
            get;
            set;
        }

        public abstract byte[] GetBytes(out bool valid);

    }
}
