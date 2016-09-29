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
                StreamWriter sw = new StreamWriter(stream);

                GetObjectPackType(ref TypeObjext).SerializeObjetct(ref vObj, ref sw);
                sw.Dispose();
                sw = null;

                this.ObjectData = stream.ToArray();
            }
        }

        public IAraObject ToIAraObject()
        {
            IAraObject vTmp;
            GetObjectPackType(ref TypeObjext).DeserializeObjetct(ObjectData, out vTmp);
            return vTmp;
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

            TmpMembros.AddRange(vType.GetRuntimeProperties().Where(a=> FiltraMemberNaoSerializaveis(a)));
            TmpMembros.AddRange(vType.GetRuntimeFields().Where(a => FiltraMemberNaoSerializaveis(a)));
            

            Metodos = TmpMembros.OrderBy(a => a.Name).ToArray();
        }


        private static bool FiltraMemberNaoSerializaveis(PropertyInfo vM)
        {
            if (vM.CanWrite==false 
                || vM.CanRead==false 
                || (vM.GetGetMethod() !=null && vM.GetGetMethod().IsStatic)
                || vM.PropertyType.IsAssignableFrom(typeof(IAraObject)))
                return false;
            else
                return FiltraMemberNaoSerializaveis((MemberInfo)vM);
        }

        private static bool FiltraMemberNaoSerializaveis(FieldInfo vM)
        {
            if (vM.IsInitOnly || vM.IsStatic || vM.FieldType.IsAssignableFrom(typeof(IAraObject)))
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


        public void SerializeObjetct(ref IAraObject vObj,ref StreamWriter sw)
        {
            var vTmpObj = (object)vObj;
            SerializeObjetct(ref vTmpObj, ref sw);
        }

        public void SerializeObjetct(ref object vObj, ref StreamWriter sw)
        {
            if (vObj==null)
            {
                sw.Write(BitConverter.GetBytes((int)0));
                return;
            }

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
                    byte[] vValue = ToBytes(ref vType,ref vGetValueObj);
                    if (vValue != null && vValue.Length > 0)
                    {
                        sw.Write(BitConverter.GetBytes(vValue.Length));
                        sw.Write(vValue);
                    }
                    else
                        sw.Write(BitConverter.GetBytes((int)0));
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
        };

        private static bool ToByteSuportado(Type vObj)
        {
            if (TypesSuportados.Contains(vObj) || vObj.IsEnum || typeof(IEnumerable).IsAssignableFrom(vObj))
                return true;
            else
                return false;
        }

        private static byte[] ToBytes(ref Type vObjType, ref object vObj)
        {
            if (vObj == null)
                return null;

            if (vObj is bool || vObj is bool?)
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
            else if (vObj is string)
                return Encoding.ASCII.GetBytes((string)vObj);
            else if (vObjType.IsEnum)
                return BitConverter.GetBytes(Convert.ToInt32(vObj));
            else if (typeof(IEnumerable).IsAssignableFrom(vObjType))
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    StreamWriter sw = new StreamWriter(stream);


                    ObjectPackType vObjectPackType = null;

                    foreach (var vTmp in (IEnumerable)vObj)
                    {
                        object vTmp2 = (object)vTmp;

                        if (vObjectPackType == null)
                        {
                            Type vTypeEnum = vTmp2.GetType();
                            vObjectPackType = SessionThinObject.GetObjectPackType(ref vTypeEnum);
                        }

                        vObjectPackType.SerializeObjetct(ref vTmp2, ref sw);
                    }

                    sw.Dispose();
                    sw = null;

                    return stream.ToArray();
                }
            }
            else
                throw new Exception(string.Format("Tipo '{0}' não suportado pelo ToBytes ", vObj.GetType()));
        }

        public void DeserializeObjetct(byte[] vObjB,out IAraObject vObj)
        {
            throw new NotImplementedException();
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
