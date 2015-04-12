namespace IrisZoomDataApi.Model.Ndfbin.Types.AllTypes
{
    public abstract class NdfFlatValueWrapper : NdfValueWrapper
    {
        private object _value;

        protected NdfFlatValueWrapper(NdfType type, object value, long offset)
            : base(type, offset)
        {
            Value = value;
        }

        public object Value
        {
            get { return _value; }
            set { _value = value;  }
        }


        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
