using Ara2.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using System.Collections;

namespace Ara2.Session4
{
    class SessionThinObject
    {
        private string IdInstance;
        private Type TypeObjext;
        private byte[] ObjectData;

        #region ObjectPackTypes
        static Dictionary<Type, ObjectPackType> ObjectPackTypes = new Dictionary<Type, ObjectPackType>();

        public static ObjectPackType GetObjectPackType(ref Type TypeObjext)
        {
            ObjectPackType vTmp;
            lock (ObjectPackTypes)
            {
                if (ObjectPackTypes.TryGetValue(TypeObjext, out vTmp))
                    return vTmp;

                vTmp = new ObjectPackType(TypeObjext);

                ObjectPackTypes.Add(TypeObjext, vTmp);
            }

            return vTmp;
        }
        #endregion

        public SessionThinObject(IAraObject vObj)
        {
            IdInstance = vObj.InstanceID;
            TypeObjext = vObj.GetType();

            using (MemoryStream stream = new MemoryStream())
            {
                MemoryStream stream2 = stream;
                GetObjectPackType(ref TypeObjext).SerializeObjetct(ref vObj, ref stream2);
                stream2 = null;
                this.ObjectData = stream.ToArray();
            }

            //object vTmp;
            //MemoryStream ms = new MemoryStream(ObjectData);
            //GetObjectPackType(ref TypeObjext).DeserializeObjetct(ref ms, out vTmp);
        }

        public IAraObject ToIAraObject()
        {
            object vTmp;
            MemoryStream ms = new MemoryStream(ObjectData);
            GetObjectPackType(ref TypeObjext).DeserializeObjetct(ref ms, out vTmp);
            return (IAraObject)vTmp;
        }
    }


    class ObjectPackType
    {
        public Type PackType;

        MemberInfo[] Metodos;

        public ObjectPackType(Type vType)
        {
            PackType = vType;

            List<MemberInfo> TmpMembros = new List<MemberInfo>();

            TmpMembros.AddRange(vType.GetRuntimeProperties().Where(a => FiltraMemberNaoSerializaveis(a)));
            TmpMembros.AddRange(vType.GetRuntimeFields().Where(a => FiltraMemberNaoSerializaveis(a)));


            Metodos = TmpMembros.OrderBy(a => a.Name).ToArray();
        }


        private static bool FiltraMemberNaoSerializaveis(PropertyInfo vM)
        {
            if (vM.CanWrite == false
                || vM.CanRead == false
                || (vM.GetGetMethod() != null && vM.GetGetMethod().IsStatic)
                || typeof(IAraObject).IsAssignableFrom(vM.PropertyType))
                return false;
            else
                return FiltraMemberNaoSerializaveis((MemberInfo)vM);
        }

        private static bool FiltraMemberNaoSerializaveis(FieldInfo vM)
        {
            if (vM.IsInitOnly || vM.IsStatic || typeof(IAraObject).IsAssignableFrom(vM.FieldType))
                return false;
            else
                return FiltraMemberNaoSerializaveis((MemberInfo)vM);
        }

        private static bool FiltraMemberNaoSerializaveis(MemberInfo vM)
        {
            if (TemAtributo<NonSerializedAttribute>(vM)
                || TemAtributo<JsonIgnoreAttribute>(vM))
                return false;

            return true;
        }

        private static bool TemAtributo<T>(MemberInfo vM)
        {
            return vM.GetCustomAttribute(typeof(T), true) != null;
        }


        public void SerializeObjetct(ref IAraObject vObj, ref MemoryStream sw)
        {
            var vTmpObj = (object)vObj;
            SerializeObjetct(ref vTmpObj, ref sw);
        }

        public void SerializeObjetct(ref object vObj, ref MemoryStream sw)
        {
            // Bit Null
            if (vObj == null)
            {
                sw.Write(new byte[] { 1 }, 0, 1);
                return;
            }
            else
                sw.Write(new byte[] { 0 }, 0, 1);

            foreach (var vM in Metodos)
            {
                Type vType;
                object vGetValueObj;
                if (vM is PropertyInfo)
                {
                    vType = ((PropertyInfo)vM).PropertyType;
                    vGetValueObj = ((PropertyInfo)vM).GetValue(vObj, null);
                }
                else if (vM is FieldInfo)
                {
                    vType = ((FieldInfo)vM).FieldType;
                    vGetValueObj = ((FieldInfo)vM).GetValue(vObj);
                }
                else
                    throw new Exception(string.Format("Metodo '{0}' inesperado, não é PropertyInfo e FieldInfo", vM.Name));

                if (ToByteSuportado(vType))
                {
                    byte[] vValue = ToBytes(ref vType, ref vGetValueObj);
                    if (vValue != null && vValue.Length > 0)
                    {
                        int vValueL = vValue.Length;
                        sw.Write(BitConverter.GetBytes(vValueL), 0, 4);
                        sw.Write(vValue, 0, vValueL);
                    }
                    else
                        sw.Write(BitConverter.GetBytes((int)0), 0, 4);
                }
                else
                {
                    SessionThinObject.GetObjectPackType(ref vType).SerializeObjetct(ref vGetValueObj, ref sw);
                }
            }
        }

        static Type[] TypesSuportados = new Type[]
        {
            typeof(bool     ),
            typeof(char     ),
            typeof(short    ),
            typeof(int      ),
            typeof(long     ),
            typeof(ushort   ),
            typeof(uint     ),
            typeof(ulong    ),
            typeof(float    ),
            typeof(double   ),
            typeof(bool?     ),
            typeof(char?     ),
            typeof(short?    ),
            typeof(int?      ),
            typeof(long?     ),
            typeof(ushort?   ),
            typeof(uint?     ),
            typeof(ulong?    ),
            typeof(float?    ),
            typeof(double?   ),

            typeof(decimal   ),
            typeof(decimal?   ),

            typeof(string   ),
            typeof(Type)
        };

        private static bool ToByteSuportado(Type vObj)
        {
            if (TypesSuportados.Contains(vObj) || vObj.IsEnum || typeof(IEnumerable).IsAssignableFrom(vObj) || typeof(Type).IsAssignableFrom(vObj))
                return true;
            else
                return false;
        }

        static byte[] ByteEmty = new byte[] { 1 };
        private static byte[] ToBytes(ref Type vObjType, ref object vObj)
        {
            if (vObj == null)
                return null;

            if (vObj is string)
                return ByteCombine(ByteEmty, Encoding.ASCII.GetBytes((string)vObj));
            else if (vObj is bool || vObj is bool?)
                return BitConverter.GetBytes((bool)vObj);
            else if (vObj is char || vObj is char?)
                return BitConverter.GetBytes((char)vObj);
            else if (vObj is short || vObj is short?)
                return BitConverter.GetBytes((short)vObj);
            else if (vObj is int || vObj is int?)
                return BitConverter.GetBytes((int)vObj);
            else if (vObj is long || vObj is long?)
                return BitConverter.GetBytes((long)vObj);
            else if (vObj is ushort || vObj is ushort?)
                return BitConverter.GetBytes((ushort)vObj);
            else if (vObj is uint || vObj is uint?)
                return BitConverter.GetBytes((uint)vObj);
            else if (vObj is ulong || vObj is ulong?)
                return BitConverter.GetBytes((ulong)vObj);
            else if (vObj is float || vObj is float?)
                return BitConverter.GetBytes((float)vObj);
            else if (vObj is double || vObj is double?)
                return BitConverter.GetBytes((double)vObj);
            else if (vObj is decimal || vObj is decimal?)
                return BitconverterExt.GetBytes((decimal)vObj);
            else if (vObjType.IsEnum)
                return BitConverter.GetBytes(Convert.ToInt32(vObj));
            else if (typeof(IEnumerable).IsAssignableFrom(vObjType))
            {
                MemoryStream stream = new MemoryStream();
                
                try
                {
                    Type vTypeArray = null;
                    ObjectPackType vObjectPackType = null;
                    bool vToByteSuportado = false;
                    bool ByteEmptyColocado = false;

                    foreach (var vTmp in (IEnumerable)vObj)
                    {
                        if (!ByteEmptyColocado)
                        {
                            ByteEmptyColocado = true;
                            stream.WriteByte(0);
                        }

                        // Bit Null
                        if (vTmp == null)
                            stream.Write(new byte[] { 1 }, 0, 1);
                        else
                        {
                            stream.Write(new byte[] { 0 }, 0, 1);

                            if (vTypeArray == null)
                            {
                                vTypeArray = ((object)vTmp).GetType();
                                vToByteSuportado = ToByteSuportado(vTypeArray);
                            }

                            object vTmp2 = (object)vTmp;

                            if (vToByteSuportado)
                            {
                                byte[] vValue = ToBytes(ref vTypeArray, ref vTmp2);
                                if (vValue != null && vValue.Length > 0)
                                {
                                    int vValueL = vValue.Length;
                                    stream.Write(BitConverter.GetBytes(vValueL), 0, 4);
                                    stream.Write(vValue, 0, vValueL);
                                }
                                else
                                    stream.Write(BitConverter.GetBytes((int)0), 0, 4);
                            }
                            else
                            {
                                if (vObjectPackType == null)
                                {
                                    vTypeArray = vTmp2.GetType();
                                    vObjectPackType = SessionThinObject.GetObjectPackType(ref vTypeArray);
                                }

                                vObjectPackType.SerializeObjetct(ref vTmp2, ref stream);
                            }
                        }
                    }

                    if (!ByteEmptyColocado)
                    {
                        ByteEmptyColocado = true;
                        stream.WriteByte(1);
                    }

                    return stream.ToArray();
                }
                finally
                {
                    stream = null;
                }
            }
            else if (vObjType is Type || typeof(Type).IsAssignableFrom(vObjType))
                return Encoding.ASCII.GetBytes(((Type)vObj).ToString());
            else
                throw new Exception(string.Format("Tipo '{0}' não suportado pelo ToBytes ", vObj.GetType()));
        }

        public static byte[] ByteCombine(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }

        private static object ToObject(ref Type vObjType, ref byte[] vBytes)
        {
            if (vBytes == null)
                return null;


            if (vObjType == typeof(string))
            {
                if (vBytes.Length > 1)
                    return Encoding.ASCII.GetString(vBytes.Skip(1).ToArray());
                else
                    return string.Empty;
            }
            else if (vObjType == typeof(bool) || vObjType == typeof(bool?))
                return BitConverter.ToBoolean(vBytes, 0);
            else if (vObjType == typeof(char) || vObjType == typeof(char?))
                return BitConverter.ToChar(vBytes, 0);
            else if (vObjType == typeof(short) || vObjType == typeof(short?))
                return (short)BitConverter.ToInt16(vBytes, 0);
            else if (vObjType == typeof(int) || vObjType == typeof(int?))
                return BitConverter.ToInt32(vBytes, 0);
            else if (vObjType == typeof(long) || vObjType == typeof(long?))
                return BitConverter.ToInt64(vBytes, 0);
            else if (vObjType == typeof(ushort) || vObjType == typeof(ushort?))
                return BitConverter.ToUInt16(vBytes, 0);
            else if (vObjType == typeof(uint) || vObjType == typeof(uint?))
                return BitConverter.ToUInt32(vBytes, 0);
            else if (vObjType == typeof(ulong) || vObjType == typeof(ulong?))
                return BitConverter.ToUInt64(vBytes, 0);
            else if (vObjType == typeof(float) || vObjType == typeof(float?))
                return BitConverter.ToSingle(vBytes, 0);
            else if (vObjType == typeof(double) || vObjType == typeof(double?))
                return BitConverter.ToDouble(vBytes, 0);
            else if (vObjType == typeof(decimal) || vObjType == typeof(decimal?))
                return BitconverterExt.ToDecimal(vBytes);
            else if (vObjType.IsEnum)
                return Enum.ToObject(vObjType, BitConverter.ToInt32(vBytes, 0));
            else if (typeof(IEnumerable).IsAssignableFrom(vObjType))
            {


                Type vType = (vObjType.GetGenericArguments().Any() ? vObjType.GetGenericArguments()[0] : vObjType.GetElementType());
                var listType = Activator.CreateInstance(typeof(List<>).MakeGenericType(vType));
                var vObjectPackType = SessionThinObject.GetObjectPackType(ref vType);
                bool vToByteSuportado = ToByteSuportado(vType);

                //foreach (var vTmp in (IEnumerable)vObj)
                MemoryStream ms = new MemoryStream(vBytes);

                // Byte Empty
                if (ms.ReadByte() == 0)
                {
                    try
                    {
                        do
                        {

                            if (ms.ReadByte() == 0)
                            {
                                //var vTmpBs = vBytes.Skip(PRead).ToArray();
                                object vObj;
                                if (vToByteSuportado)
                                {
                                    byte[] bufferLen = new byte[4];
                                    ms.Read(bufferLen, 0, 4);
                                    int Len = BitConverter.ToInt32(bufferLen, 0);

                                    if (Len > 0)
                                    {
                                        byte[] vObjBSub = new byte[Len]; //.Skip(PosR).Take(Len).ToArray();
                                        ms.Read(vObjBSub, 0, Len);

                                        vObj = ToObject(ref vType, ref vObjBSub);
                                    }
                                    else
                                        vObj = null;
                                }
                                else
                                    vObjectPackType.DeserializeObjetct(ref ms, out vObj);

                                ((IList)listType).Add(vObj);
                            }
                            else
                                ((IList)listType).Add(null);
                        } while (ms.Position < vBytes.Length);
                    }
                    finally
                    {
                        ms = null;
                    }

                }

                if (vObjType.IsArray)
                {
                    //return listType.GetType().GetMethod("ToArray").Invoke(listType, null);
                    return ((dynamic)listType).ToArray();
                }
                else
                    return Convert.ChangeType(listType, vObjType);
            }
            else if (vObjType is Type || typeof(Type).IsAssignableFrom(vObjType))
                return Type.GetType(Encoding.ASCII.GetString(vBytes));
            else
                throw new Exception(string.Format("Tipo '{0}' não suportado pelo ToBytes ", vObjType));
        }

        //public int DeserializeObjetct(byte[] vObjB,out IAraObject vObj)
        //{
        //    return DeserializeObjetct(vObjB,out vObj);
        //}
        static byte[] ArrayByteNUll = null;

        public void DeserializeObjetct(ref MemoryStream vObjB, out object vObj)
        {
            int Len;
            vObj = null;

            // Verifica Bit NUll
            if (vObjB.ReadByte() == 1)
                return;

            // Tenho que acrecentar um byte para saber se o objeto é null
            vObj = Activator.CreateInstance(PackType, true);


            foreach (var vM in Metodos)
            {
                Type vType;
                if (vM is PropertyInfo)
                {
                    vType = ((PropertyInfo)vM).PropertyType;
                    object vTmpO;
                    if (ToByteSuportado(vType))
                    {
                        byte[] bufferLen = new byte[4];
                        vObjB.Read(bufferLen, 0, 4);
                        Len = BitConverter.ToInt32(bufferLen, 0);

                        if (Len > 0)
                        {
                            byte[] vObjBSub = new byte[Len]; //.Skip(PosR).Take(Len).ToArray();
                            vObjB.Read(vObjBSub, 0, Len);

                            vTmpO = ToObject(ref vType, ref vObjBSub);
                        }
                        else
                            vTmpO = ToObject(ref vType, ref ArrayByteNUll);
                    }
                    else
                    {
                        //byte[] vObjBSub  = new byte[ //= vObjB.Skip(PosR).ToArray();
                        SessionThinObject.GetObjectPackType(ref vType).DeserializeObjetct(ref vObjB, out vTmpO);
                    }

                    ((PropertyInfo)vM).SetValue(vObj, vTmpO);
                }
                else if (vM is FieldInfo)
                {
                    vType = ((FieldInfo)vM).FieldType;
                    object vTmpO;
                    if (ToByteSuportado(vType))
                    {
                        //Len = BitConverter.ToInt32(vObjB, PosR);
                        byte[] bufferLen = new byte[4];
                        vObjB.Read(bufferLen, 0, 4);
                        Len = BitConverter.ToInt32(bufferLen, 0);

                        if (Len > 0)
                        {
                            //byte[] vObjBSub = vObjB.Skip(PosR).Take(Len).ToArray();
                            byte[] vObjBSub = new byte[Len]; //.Skip(PosR).Take(Len).ToArray();
                            vObjB.Read(vObjBSub, 0, Len);

                            vTmpO = ToObject(ref vType, ref vObjBSub);
                        }
                        else
                            vTmpO = ToObject(ref vType, ref ArrayByteNUll);
                    }
                    else
                    {
                        //byte[] vObjBSub = vObjB.Skip(PosR).ToArray();
                        SessionThinObject.GetObjectPackType(ref vType).DeserializeObjetct(ref vObjB, out vTmpO);
                    }

                    ((FieldInfo)vM).SetValue(vObj, vTmpO);
                }
                else
                    throw new Exception(string.Format("Metodo '{0}' inesperado, não é PropertyInfo e FieldInfo", vM.Name));
            }
        }

    }

    public class BitconverterExt
    {
        public static byte[] GetBytes(decimal dec)
        {
            //Load four 32 bit integers from the Decimal.GetBits function
            Int32[] bits = decimal.GetBits(dec);
            //Create a temporary list to hold the bytes
            List<byte> bytes = new List<byte>();
            //iterate each 32 bit integer
            foreach (Int32 i in bits)
            {
                //add the bytes of the current 32bit integer
                //to the bytes list
                bytes.AddRange(BitConverter.GetBytes(i));
            }
            //return the bytes list as an array
            return bytes.ToArray();
        }
        public static decimal ToDecimal(byte[] bytes)
        {
            //check that it is even possible to convert the array
            if (bytes.Count() != 16)
                throw new Exception("A decimal must be created from exactly 16 bytes");
            //make an array to convert back to int32's
            Int32[] bits = new Int32[4];
            for (int i = 0; i <= 15; i += 4)
            {
                //convert every 4 bytes into an int32
                bits[i / 4] = BitConverter.ToInt32(bytes, i);
            }
            //Use the decimal's new constructor to
            //create an instance of decimal
            return new decimal(bits);
        }
    }


}
