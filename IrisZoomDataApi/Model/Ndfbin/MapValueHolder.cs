using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IrisZoomDataApi;
using IrisZoomDataApi.Model.Ndfbin.Types.AllTypes;

namespace IrisZoomDataApi.Model.Ndfbin
{
    public class MapValueHolder : CollectionItemValueHolder
    {
        public MapValueHolder(NdfValueWrapper value, NdfbinManager manager, long instanceOffset) : base(value, manager, instanceOffset)
        {
        }
    }
}
