using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrisZoomDataApi.Model.Ndfbin.Types.AllTypes
{
    public class NdfObjectReference : NdfValueWrapper
    {
        private uint _instanceId;
        private bool _isDead;
        public NdfClass _class;

        public NdfObjectReference(NdfClass cls, uint instance, long offset, bool isDead = false)
            : base(NdfType.ObjectReference, offset)
        {
            Class = cls;
            InstanceId = instance;

            _isDead = isDead;
        }

        public NdfClass Class
        {
            get { return _class; }
            protected set { _class = value; }
        }

        public uint InstanceId
        {
            get { return _instanceId; }
            set
            {
                _instanceId = value;
            }
        }

        public NdfObject Instance
        {
            get { return Class.Instances.SingleOrDefault(x => x.Id == InstanceId); }
            set
            {
                if (!Class.Instances.Contains(value))
                    InstanceId = Class.Instances.First().Id;
                else
                    InstanceId = value.Id;
            }
        }

        public override string ToString()
        {
            if (Class == null)
                return string.Format("Class does not exist : {0}", InstanceId);

            return string.Format("{0} : {1} - {2}", Class.Id, InstanceId, Class.Name);

        }

        public override byte[] GetBytes(out bool valid)
        {
            valid = true;

            var refereceData = new List<byte>();

            refereceData.AddRange(BitConverter.GetBytes(InstanceId));

            refereceData.AddRange(_isDead ? new byte[] { 0xFF, 0xFF, 0xFF, 0xFF } : BitConverter.GetBytes(Class.Id));

            return refereceData.ToArray();
        }
    }
}
