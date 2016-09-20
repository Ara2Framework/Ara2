using Ara2.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Diagnostics;

namespace Ara2.Session3
{
    class SessionThinObject
    {
        private string IdInstance;
        private Type TypeObjext;
        //private byte[] ObjectData;
        private string ObjectJson;

        static CustomBinarySerializer<IAraObject> Serializer = new CustomBinarySerializer<IAraObject>();

        public SessionThinObject(IAraObject vObj)
        {
            IdInstance = vObj.InstanceID;
            //ObjectData = Serializer.Serialize2Bytes(vObj);
            TypeObjext = vObj.GetType();

            var seetings = new JsonSerializerSettings() { ContractResolver = new IncludePrivateStateContractResolver() };

            ObjectJson = Newtonsoft.Json.JsonConvert.SerializeObject(vObj, seetings);

            //var vTmp = Newtonsoft.Json.JsonConvert.DeserializeObject(ObjectJson, TypeObjext);
            //IAraObject vTmp2 = (IAraObject)vTmp;

        }

        public IAraObject ToIAraObject()
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new IncludePrivateStateContractResolver(),
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            };

            //return Serializer.DeserializeFromBytes(ObjectData);
            return (IAraObject)Newtonsoft.Json.JsonConvert.DeserializeObject(ObjectJson, TypeObjext, settings);
        }
    }
    

    public class IncludePrivateStateContractResolver : DefaultContractResolver
    {
        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            const BindingFlags BindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            var properties = objectType.GetProperties(BindingFlags);//.Where(p => p.HasSetter() && p.HasGetter());
            var fields = objectType.GetFields(BindingFlags);

            var allMembers = properties.Cast<MemberInfo>().Union(fields);
            return allMembers.ToList();
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            if (!prop.Writable)
            {
                var property = member as PropertyInfo;
                if (property != null)
                {
                    prop.Writable = property.HasSetter();
                }
                else
                {
                    var field = member as FieldInfo;
                    if (field != null)
                    {
                        prop.Writable = true;
                    }
                }
            }

            if (!prop.Readable)
            {
                var field = member as FieldInfo;
                if (field != null)
                {
                    prop.Readable = true;
                }
            }

            return prop;
        }
    }

    public static class TypeExtensions
    {
        public static bool HasSetter(this PropertyInfo property)
        {
            //In this way we can check for private setters in base classes
            return property.DeclaringType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                                         .Any(m => m.Name == "set_" + property.Name);
        }

        public static bool HasGetter(this PropertyInfo property)
        {
            //In this way we can check for private getters in base classes
            return property.DeclaringType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                                         .Any(m => m.Name == "get_" + property.Name);
        }
    }

}
