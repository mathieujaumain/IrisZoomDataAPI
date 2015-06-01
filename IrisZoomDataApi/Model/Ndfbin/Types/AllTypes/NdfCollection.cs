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

        public  NdfValueWrapper GetValueFromQuery(string query)
        {
            string rest = string.Empty;
            string next = NdfQueryReader.ParseNextStep(query, out rest);

            long index = -1;
            if (long.TryParse(next, out index)) // can be  a list of map so we need to find a way to use "next" as a key and (even worst, sometimes a long is a key map)
            {
                NdfValueWrapper val = this.InnerList[(int) index].Value;

                switch (val.Type)
                {
                    case NdfType.ObjectReference:
                        NdfObjectReference reference = val as NdfObjectReference;
                        return reference.Instance.GetValueFromQuery(rest);

                    case NdfType.MapList:
                        NdfMapList mapList = val as NdfMapList;
                        return mapList.GetValueFromQuery(rest);

                    case NdfType.List:
                        NdfCollection list = val as NdfCollection;
                        return list.GetValueFromQuery(rest);

                    case NdfType.Map:
                        NdfMap map = val as NdfMap;
                        return (map.Value as MapValueHolder).Value;
                       

                    default:
                        return val;
                }
            }
            else
            {
                // do a list from the maps of the inner list
                List<CollectionItemValueHolder> maps = _innerList.FindAll(x => x.Value.Type == NdfType.Map);
                try
                {
                    NdfValueWrapper selectedmap = maps.Find(x => (x.Value as NdfMap).Key.Value.ToString() == next).Value; // wat

                    NdfMap mapVal = selectedmap as NdfMap;
                    MapValueHolder valholder = mapVal.Value as MapValueHolder;

                    return valholder.Value;
                    
                }
                catch { }
            }

            throw (new Exception("Something went wrong with this path: " + query != string.Empty ? query : "empty path"));
        }

       public bool TryGetValueFromQuery(string query, out NdfValueWrapper value) 
       {
            string rest = string.Empty;
            string next = NdfQueryReader.ParseNextStep(query, out rest);

           // verify next is in the from "[ i ]"
           bool isIndex = false;
            string[] parts = next.Split(new string[] { "[", "]" }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length > 0)
            {
                isIndex = true;
            }

            long index = -1;
            if (isIndex && long.TryParse(parts[0], out index))
            {
                NdfValueWrapper val = null;

                try { val = this.InnerList[(int)index].Value; }
                catch { value = null; return false; }

                switch (val.Type)
                {
                    case NdfType.ObjectReference:
                        NdfObjectReference reference = val as NdfObjectReference;
                        return reference.Instance.TryGetValueFromQuery(rest, out value);

                    case NdfType.MapList:
                        NdfMapList mapList = val as NdfMapList;
                        return mapList.TryGetValueFromQuery(rest, out value);

                    case NdfType.List:
                        NdfCollection list = val as NdfCollection;
                        return list.TryGetValueFromQuery(rest, out value);

                    case NdfType.Map:
                        NdfMap map = val as NdfMap;
                        return map.TryGetValueFromQuery(rest, out value);

                    case Types.NdfType.Unknown:
                        break;
                    case Types.NdfType.Unset:
                        break;

                    default:
                        value = val;
                        return true;
                }
            }
            else
            {
                // do a list from the maps of the inner list
                List<CollectionItemValueHolder> maps = _innerList.FindAll(x => x.Value.Type == NdfType.Map);
                try
                {
                    NdfValueWrapper selectedmap = maps.Find(x => (x.Value as NdfMap).Key.Value.ToString() == next).Value; // wat

                    NdfMap mapVal = selectedmap as NdfMap;
                    MapValueHolder valholder = mapVal.Value as MapValueHolder;

                    value = valholder.Value;
                    return true;
                }
                catch { value = null; return false; }
            }


            value = null;
            return false;
            //throw (new Exception("Something went wrong with this path: " + propertyPath != string.Empty ? propertyPath : "empty path"));
        }
    }
}
