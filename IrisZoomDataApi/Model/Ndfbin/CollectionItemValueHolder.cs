using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrisZoomDataApi;
using IrisZoomDataApi.Model.Ndfbin.Types.AllTypes;
using IrisZoomDataApi.Util;

namespace IrisZoomDataApi.Model.Ndfbin
{
    public class CollectionItemValueHolder : IValueHolder
    {
        private NdfValueWrapper _value;

        public CollectionItemValueHolder(NdfValueWrapper value, NdfbinManager manager, long instanceOffset)
        {
            Value = value;
            Manager = manager;
            InstanceOffset = instanceOffset;
        }

        public virtual NdfbinManager Manager { get; private set; }
        public virtual long InstanceOffset { get; private set; }

        public virtual NdfValueWrapper Value
        {
            get { return _value; }
            set
            {
                _value = value; 
            }
        }

        private byte[] _oldVal = new byte[0];

    }
}
