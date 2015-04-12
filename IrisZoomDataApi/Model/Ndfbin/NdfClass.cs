using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using IrisZoomDataApi;
using IrisZoomDataApi.Model.Ndfbin.Types;
using IrisZoomDataApi.Model.Ndfbin.Types.AllTypes;

namespace IrisZoomDataApi.Model.Ndfbin
{
    public class NdfClass
    {
        private int _id;
        private long _offset;
        private string _name;
        private readonly List<NdfProperty> _properties = new List<NdfProperty>();
        private readonly List<NdfObject> _instances = new List<NdfObject>();

        public NdfClass(NdfbinManager mgr)
        {
            Manager = mgr;
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public long Offset
        {
            get { return _offset; }
            set { _offset = value;}
        }

        public string Name
        {
            get { return _name; }
            set { _name = value;}
        }

        public List<NdfProperty> Properties
        {
            get { return _properties; }
        }

        public List<NdfObject> Instances
        {
            get { return _instances; }
        }

        public NdfbinManager Manager { get; protected set; }

        public override string ToString()
        {
            return Name.ToString(CultureInfo.InvariantCulture);
        }
    }
}
