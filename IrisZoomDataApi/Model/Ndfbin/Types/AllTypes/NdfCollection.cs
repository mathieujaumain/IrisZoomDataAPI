using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Data;
using System.Windows.Input;

namespace IrisZoomDataApi.Model.Ndfbin.Types.AllTypes
{
    public class NdfCollection : NdfValueWrapper, IList<CollectionItemValueHolder>, IList
    {
        private readonly List<CollectionItemValueHolder> _innerList 
            = new List<CollectionItemValueHolder>();

        public NdfCollection(long offset)
            : base(NdfType.List, offset)
        {

        }

        public NdfCollection(IEnumerable<CollectionItemValueHolder> list, long offset)
            : this(offset)
        {
            if (list != null)
                foreach (CollectionItemValueHolder wrapper in list)
                    InnerList.Add(wrapper);
        }

        public List<CollectionItemValueHolder> InnerList
        {
            get { return _innerList; }
        }


        public override string ToString()
        {
            return string.Format("Collection[{0}]", InnerList.Count);
        }

        public override byte[] GetBytes(out bool valid)
        {
            valid = true;

            bool itemValid;

            var data = new List<byte>();

            data.AddRange(BitConverter.GetBytes(InnerList.Count));

            foreach (var valueHolder in InnerList)
            {
                var valueDat = valueHolder.Value.GetBytes(out itemValid);

                if (!itemValid)
                    continue;

                if (valueHolder.Value.Type == NdfType.ObjectReference || valueHolder.Value.Type == NdfType.TransTableReference)
                    data.AddRange(BitConverter.GetBytes((uint)NdfType.Reference));

                data.AddRange(BitConverter.GetBytes((uint)valueHolder.Value.Type));
                data.AddRange(valueDat);
            }

            return data.ToArray();
        }

        #region IList<NdfValueWrapper> Members

        public int IndexOf(CollectionItemValueHolder item)
        {
            return InnerList.IndexOf(item);
        }

        public void Insert(int index, CollectionItemValueHolder item)
        {
            InnerList.Insert(index, item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void RemoveAt(int index)
        {
            var el = InnerList[index];

            InnerList.RemoveAt(index);

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public CollectionItemValueHolder this[int index]
        {
            get { return InnerList[index]; }
            set { InnerList[index] = value; }
        }

        public void Add(CollectionItemValueHolder item)
        {
            InnerList.Add(item);

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void Clear()
        {
            InnerList.Clear();

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(CollectionItemValueHolder item)
        {
            return InnerList.Contains(item);
        }

        public void CopyTo(CollectionItemValueHolder[] array, int arrayIndex)
        {
            InnerList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return InnerList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(CollectionItemValueHolder item)
        {
            var res = InnerList.Remove(item);

            if (res)
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

            return res;
        }

        public IEnumerator<CollectionItemValueHolder> GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }

        #endregion

        #region IList

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, e);
        }

        public int Add(object value)
        {
            var val = value as CollectionItemValueHolder;
            if (val == null)
                return -1;

            Add(val);

            return IndexOf(value);
        }

        public bool Contains(object value)
        {
            var val = value as CollectionItemValueHolder;
            if (val == null)
                return false;

            return InnerList.Contains(value as CollectionItemValueHolder);
        }

        public int IndexOf(object value)
        {
            return IndexOf(value as CollectionItemValueHolder);
        }

        public void Insert(int index, object value)
        {
            Insert(index, value as CollectionItemValueHolder);
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public void Remove(object value)
        {
            Remove(value as CollectionItemValueHolder);
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = value as CollectionItemValueHolder;
            }
        }

        public void CopyTo(System.Array array, int index)
        {
            throw new System.NotImplementedException();
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return this; }
        }

        #endregion

        public  NdfValueWrapper getValueFromPath(string propertyPath)
        {
            string[] pathParts = propertyPath.Split(new string[] { "." }, System.StringSplitOptions.RemoveEmptyEntries);
            long index = -1;
            int count = pathParts.Length;
            if (count > 0 && long.TryParse( pathParts[0], out index))
            {

                NdfValueWrapper val = this.InnerList[(int) index].Value;
                propertyPath.Replace(pathParts[0] + ".", string.Empty);
                switch (val.Type)
                {
                    case NdfType.ObjectReference:
                        NdfObjectReference reference = val as NdfObjectReference;
                        return reference.Instance.GetValueFromPath(propertyPath);

                    case NdfType.MapList:
                        NdfMapList mapList = val as NdfMapList;
                        return mapList.getValueFromPath(propertyPath);

                    case NdfType.List:
                        NdfCollection list = val as NdfCollection;
                        return list.getValueFromPath(propertyPath);

                    default:
                        return val;
                }
            }

            throw (new Exception("Something went wrong with this path: " + propertyPath != string.Empty ? propertyPath : "empty path"));
        }

        public bool TryGetValueFromPath(string propertyPath, out NdfValueWrapper value)
        {
            string[] pathParts = propertyPath.Split(new string[] { "." }, System.StringSplitOptions.RemoveEmptyEntries);
            long index = -1;
            int count = pathParts.Length;
            if (count > 0 && long.TryParse( pathParts[0], out index))
            {
                NdfValueWrapper val = null;

                try { val = this.InnerList[(int)index].Value; }
                catch { value = null; return false; }

                propertyPath.Replace(pathParts[0] + ".", string.Empty);

                switch (val.Type)
                {
                    case NdfType.ObjectReference:
                        NdfObjectReference reference = val as NdfObjectReference;
                        return reference.Instance.TryGetValueFromPath(propertyPath, out value);

                    case NdfType.MapList:
                        NdfMapList mapList = val as NdfMapList;
                        return mapList.TryGetValueFromPath(propertyPath, out value);

                    case NdfType.List:
                        NdfCollection list = val as NdfCollection;
                        return list.TryGetValueFromPath(propertyPath, out value);

                    case Types.NdfType.Unknown:
                        break;
                    case Types.NdfType.Unset:
                        break;

                    default:
                        value = val;
                        return true;
                }
            }
            value = null;
            return false;
            //throw (new Exception("Something went wrong with this path: " + propertyPath != string.Empty ? propertyPath : "empty path"));
        }
    }
}
