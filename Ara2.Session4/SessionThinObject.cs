using Ara2.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

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
            }

            vTmp = new ObjectPackType(TypeObjext);
            lock (ObjectPackTypes)
            {
                ObjectPackTypes.Add(TypeObjext, vTmp);
            }

            return vTmp;
        }
        #endregion

        public SessionThinObject(IAraObject vObj)
        {
            IdInstance = vObj.InstanceID;
            TypeObjext = vObj.GetType();

            GetObjectPackType(ref TypeObjext).SerializeObjetct(vObj, out this.ObjectData);
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


        public void SerializeObjetct(IAraObject vObj,out byte[] vObjB)
        {
            SerializeObjetct((object)vObj, out vObjB);
        }

        public void SerializeObjetct(object vObj, out byte[] vObjB)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (var sw = new StreamWriter(stream))
                {
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
                            byte[] vValue = ToBytes(vGetValueObj);
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
                            byte[] vTmpB;
                            SessionThinObject.GetObjectPackType(ref vType).SerializeObjetct(vGetValueObj, out vTmpB);
                            if (vTmpB != null || vTmpB.Length > 0)
                            {
                                sw.Write(BitConverter.GetBytes(vTmpB.Length));
                                sw.Write(vTmpB);
                            }
                            else
                                sw.Write(BitConverter.GetBytes((int)0));
                        }
                    }
                }

                vObjB = stream.ToArray();
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

            typeof(string   ),
        };

        private static bool ToByteSuportado(Type vObj)
        {
            if (TypesSuportados.Contains(vObj))
                return true;
            else
                return false;
        }

        private static byte[] ToBytes(object vObj)
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
            else if (vObj is string)
                return Encoding.ASCII.GetBytes((string)vObj);
            else
                throw new Exception(string.Format("Tipo '{0}' não suportado pelo ToBytes ", vObj.GetType()));
        }

        public void DeserializeObjetct(byte[] vObjB,out IAraObject vObj)
        {
            throw new NotImplementedException();
        }

    }


}
