using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrisZoomDataApi;
using IrisZoomDataApi.Model.Ndfbin.Types.AllTypes;

namespace IrisZoomDataApi.Model.Ndfbin
{
    public interface IValueHolder
    {
        NdfValueWrapper Value { get; set; }

        NdfbinManager Manager { get; }

        long InstanceOffset { get; }
    }
}
