// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;
using Newtonsoft.Json;

namespace Ara2.Components
{

    [Serializable]
    //[TypeConverter(typeof(AraEventConverter))]
    [Category("Events")]
    public class AraEvent<T> : IAraEvent
    {

        [NonSerialized]
        [JsonIgnore]
        protected T _Event;

        [NonSerialized]
        [JsonIgnore]
        protected bool _EventsLoad = false;


        public static AraEvent<T> operator +(AraEvent<T> vObj, T vObj2)
        {
            vObj.AddEvent(vObj2);
            return vObj;
        }

        public static AraEvent<T> operator -(AraEvent<T> vObj, T vObj2)
        {
            vObj.DelEvent(vObj2);
            return vObj;
        }



        public void AddEvent(T vObj2)
        {
            //try
            //{

            //    //System.Type vType = ((Delegate)(object)vObj2).Method.ReflectedType;
            //    //object Tmp = TempAAA.Cast<vType>(((Delegate)(object)vObj2).Target);
            //    MethodInfo Method = ((Ara2.Components.AraObject)(((Delegate)(object)vObj2).Target)).GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).First(a => a.Name == ((Delegate)(object)vObj2).Method.Name);
            //}
            //catch
            //{
            //    throw new Exception("Method '" + ((Delegate)(object)vObj2).Method.Name + "' in '" + ((Delegate)(object)vObj2).Target.GetType().Name + "' is not public");
            //}


            //Só para carregar
            T Tmp = this.InvokeEvent;

            this._Event = (T)(object)MulticastDelegate.Combine((MulticastDelegate)(object)this._Event, (MulticastDelegate)(object)vObj2);
            if (this._Name==null)
                this._Name = ((MulticastDelegate)(object)this._Event).Method.Name;
            UpdateNameObjects();
            InvokeChangeEnabled();
        }

        public void DelEvent(T vObj2)
        {
            //Só para carregar
            T Tmp = this.InvokeEvent;

            this._Event = (T)(object)MulticastDelegate.Remove((MulticastDelegate)(object)this._Event, (MulticastDelegate)(object)vObj2);
            UpdateNameObjects();
            InvokeChangeEnabled();
        }



        [Serializable]
        protected class AraObjectEvent
        {
            public string Name;
            public string Method;
            public Type[] Types;

            public AraObjectEvent()
            {

            }

            public AraObjectEvent(string vName, string vMetode, Type[] vTypes)
            {
                Name = vName;
                Method = vMetode;
                Types = vTypes;
            }
        }

        [JsonProperty]
        protected List<AraObjectEvent> _NameObjects = new List<AraObjectEvent>();

        [JsonIgnore]
        private List<AraObjectEvent> NameObjects
        {
            get
            {
                return _NameObjects;
            }
            set
            {
                _NameObjects = value;
            }
        }

        [Browsable(false)]
        [JsonIgnore]
        public int Count
        {
            get { return (NameObjects != null ? NameObjects.Count : 0); }
        }

        [Browsable(false)]
        [JsonIgnore]
        public bool Enabled
        {
            get
            {
                return Count > 0;
            }
        }


        private AraEvent<Action> _ChangeEnabled = null;
        [Browsable(false)]
        [JsonIgnore]
        public AraEvent<Action> ChangeEnabled
        {
            get
            {
                if (_ChangeEnabled == null) _ChangeEnabled = new AraEvent<Action>();
                return _ChangeEnabled;
            }
            set
            {
                _ChangeEnabled = value;
            }
        }

        protected void InvokeChangeEnabled()
        {
            if (_ChangeEnabled != null && this.ChangeEnabled.InvokeEvent != null)
                this.ChangeEnabled.InvokeEvent();
        }



        private void UpdateNameObjects()
        {
            List<AraObjectEvent> TmpNameObjects = new List<AraObjectEvent>();
            if (_Event != null)
            {
                foreach (Delegate Tmp in ((MulticastDelegate)(object)_Event).GetInvocationList())
                {
                    string vInstanceID = ((IAraObject)Tmp.Target).InstanceID;
                    if (!TmpNameObjects.Where(a=>a.Name == vInstanceID && a.Method == Tmp.Method.Name).Any())
                        TmpNameObjects.Add(new AraObjectEvent(vInstanceID, Tmp.Method.Name, Tmp.Method.GetParameters().ToList().Select(a => a.ParameterType).ToArray()));
                }
            }

            NameObjects = TmpNameObjects;
            TmpNameObjects = null;
        }

        public bool Equals(T vEvent)
        {
            if (_Event != null)
                return ((MulticastDelegate)(object)_Event).GetInvocationList().Where(a => a.Equals(vEvent)).Count() > 0;
            else
                return false;
        }


        private static bool EqualArrayType(ParameterInfo[] ParameterInfo, Type[] bType)
        {
            if (ParameterInfo.Length != bType.Length)
                return false;

            int n = 0;
            foreach (ParameterInfo TmpParameterInfo in ParameterInfo)
            {
                if (TmpParameterInfo.ParameterType != bType[n])
                    return false;
                n++;
            }

            return true;
        }

        private static MethodInfo GetMethod(object vObj, AraObjectEvent Tmp)
        {
            MethodInfo Method = vObj.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault(a => a.Name == Tmp.Method && EqualArrayType(a.GetParameters(), Tmp.Types));
            if (Method != null)
                return Method;
            else if (vObj.GetType().BaseType != null)
                return GetMethod(vObj.GetType().BaseType, Tmp);
            else
                return null;
        }

        private static MethodInfo GetMethod(Type vTypeObj, AraObjectEvent Tmp)
        {
            MethodInfo Method = vTypeObj.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault(a => a.Name == Tmp.Method && EqualArrayType(a.GetParameters(), Tmp.Types));
            if (Method != null)
                return Method;
            else if (vTypeObj.BaseType != null)
                return GetMethod(vTypeObj.BaseType, Tmp);
            else
                return null;
        }

        [Browsable(false)]
        [JsonIgnore]
        public T InvokeEvent
        {
            get
            {
                Tick vTick = Tick.GetTick();

                if (_EventsLoad == false)
                {
                    if (NameObjects != null)
                    {
                        foreach (AraObjectEvent Tmp in NameObjects)
                        {
                            object vObj = vTick.Session.GetObject(Tmp.Name);
                            if (vObj != null)
                                this._Event = (T)(object)MulticastDelegate.Combine((MulticastDelegate)(object)this._Event, (MulticastDelegate)(object)MulticastDelegate.CreateDelegate(typeof(T), vObj, GetMethod(vObj, Tmp)));
                        }
                    }
                    _EventsLoad = true;
                }

                if (this._Event != null)
                {
                    foreach (System.Delegate D in ((dynamic)this._Event).GetInvocationList())
                    {
                        IAraObject vObj = (IAraObject)D.Target;
                        if (!vTick.Session.ExistsObject(vObj.InstanceID) )
                        {
                            this._Event = (T)(object)MulticastDelegate.Remove((MulticastDelegate)(object)this._Event, D);
                            UpdateNameObjects();
                        }
                    }
                }

                return _Event;
            }
        }

        #region AraDev
        private string _Name = null;

        [Ara2.Dev.AraDevProperty]
        [Browsable(false)]
        [JsonIgnore]
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                if (_Name != null && _Name.Trim() == "")
                    _Name = null;

            }
        }
        #endregion


    }

    internal class AraEventConverter : ExpandableObjectConverter
    {
        //public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        //{
        //    return true;
        //}

        //public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        //{
        //    return new AraDistance("");
        //}


        //public object ConvertFromString(string text)
        //{
        //    return (object)(new AraDistance(text));
        //}

        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType)
        {
            return true;
        }


        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            var vValueS = (string)value;

            if (string.IsNullOrEmpty(vValueS))
                ((dynamic)context.PropertyDescriptor.GetValue(context.Instance)).Name = null;
            else
                ((dynamic)context.PropertyDescriptor.GetValue(context.Instance)).Name = ((dynamic)context.Instance).Name + "_" + context.PropertyDescriptor.Name;

            return context.PropertyDescriptor.GetValue(context.Instance);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                         CultureInfo culture,
                                         object value,
                                         Type destinationType)
        {
            if (destinationType == typeof(string) && value!=null)
                return ((dynamic)value).Name;
            else
                return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
