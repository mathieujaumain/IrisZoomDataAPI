﻿using System;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using IrisZoomDataApi;
using IrisZoomDataApi.Model.Ndfbin.Types.AllTypes;

namespace IrisZoomDataApi.Model.Ndfbin.Types
{
    public static class NdfTypeManager
    {
        public static NdfType GetType(byte[] data)
        {
            if (data.Length != 4)
                return NdfType.Unknown;

            uint value = BitConverter.ToUInt32(data, 0);

            if (Enum.IsDefined(typeof(NdfType), value))
                return (NdfType)value;

            return NdfType.Unknown;
        }

        public static NdfValueWrapper GetValue(byte[] data, NdfType type, NdfbinManager mgr, long pos)
        {
            //if (data.Length != SizeofType(type))
            //    return null;

            switch (type)
            {
                case NdfType.Boolean:
                    return new NdfBoolean(BitConverter.ToBoolean(data, 0), pos);
                case NdfType.Int8:
                    return new NdfInt8(data[0], pos);
                case NdfType.Int16:
                    return new NdfInt16(BitConverter.ToInt16(data, 0), pos);
                case NdfType.Int32:
                    return new NdfInt32(BitConverter.ToInt32(data, 0), pos);
                case NdfType.UInt32:
                    return new NdfUInt32(BitConverter.ToUInt32(data, 0), pos);
                case NdfType.Float32:
                    return new NdfSingle(BitConverter.ToSingle(data, 0), pos);
                case NdfType.Float64:
                    return new NdfDouble(BitConverter.ToDouble(data, 0), pos);
                case NdfType.Float64_2:
                    return new NdfDouble_2(BitConverter.ToDouble(data, 0), pos);
                case NdfType.TableStringFile:
                    var id = BitConverter.ToInt32(data, 0);
                    return new NdfFileNameString(mgr.Strings[id], pos);
                case NdfType.TableString:
                    var id2 = BitConverter.ToInt32(data, 0);
                    return new NdfString(mgr.Strings[id2], pos);
                case NdfType.Color32:
                    return new NdfColor32(Color.FromArgb(data[0], data[1], data[2], data[3]), pos);
                case NdfType.Color128:
                    return new NdfColor128(data, pos);
                case NdfType.Vector:
                    var px = data.Take(4).ToArray();
                    var py = data.Skip(4).Take(4).ToArray();
                    var pz = data.Skip(8).ToArray();
                    return new NdfVector(new Point3D(BitConverter.ToSingle(px, 0),
                                       BitConverter.ToSingle(py, 0),
                                       BitConverter.ToSingle(pz, 0)), pos);

                case NdfType.ObjectReference:
                    var instId = BitConverter.ToUInt32(data.Take(4).ToArray(), 0);
                    var clsId = BitConverter.ToUInt32(data.Skip(4).ToArray(), 0);
                    var cls = mgr.Classes.SingleOrDefault(x => x.Id == clsId); // mgr.Classes[(int)clsId]; due to deadrefs...
                    return new NdfObjectReference(cls, instId, pos, cls == null);

                case NdfType.Map:
                    return new NdfMap(new MapValueHolder(null, mgr, 0), new MapValueHolder(null, mgr, 0), pos);
                case NdfType.Guid:
                    return new NdfGuid(new Guid(data), pos);

                case NdfType.WideString:
                    return new NdfWideString(Encoding.Unicode.GetString(data), 0);

                case NdfType.TransTableReference:
                    var id3 = BitConverter.ToInt32(data, 0);
                    return new NdfTrans(mgr.Trans[id3], pos);

                case NdfType.LocalisationHash:
                    return new NdfLocalisationHash(data, 0);

                case NdfType.Unset:
                    return new NdfNull(pos);

                default:
                    return null;
            }
        }

        public static uint SizeofType(NdfType type)
        {
            switch (type)
            {
                case NdfType.Boolean:
                case NdfType.Int8:
                    return 1;
                case NdfType.Int16:
                    return 2;
                case NdfType.Int32:
                case NdfType.UInt32:
                case NdfType.Float32:
                case NdfType.TableStringFile:
                case NdfType.TableString:
                case NdfType.Color32:
                case NdfType.WideString:
                    return 4;
                case NdfType.LocalisationHash:
                case NdfType.ObjectReference:
                    return 8;
                case NdfType.Vector:
                    return 12;
                case NdfType.Color128:
                case NdfType.Guid:
                    return 16;

                case NdfType.Map:
                    return 0;
                case NdfType.List:
                case NdfType.MapList:
                    return 4;

                case NdfType.Float64:
                case NdfType.Float64_2:
                    return 8;

                case NdfType.TransTableReference:
                    return 4;

                default:
                    return 0;
            }
        }
    }
}
