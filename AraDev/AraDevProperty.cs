// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ara2.Components;

namespace Ara2.Dev
{
    public class SAraDevProperty
    {
        public string InstanceID;
        public string Nome;
        public bool IsDefault;
        public string Value;
        public object ValueObject;
        public bool Editable;
        public object Object;
        public object ValueDefault;
        public System.Reflection.PropertyInfo PropertyInfo;
        public System.Reflection.FieldInfo FieldInfo;
        public Type ValueType;
        public bool Event;
        public bool Hide;
        public AraDevPropertyCustomWindow AraDevPropertyCustomWindow;
        public bool PropertySupportLayout;
        public Attribute[] Attributes;


        public static List<SAraDevProperty> GetPropertys(IAraObject vObjOriginal)
        {
            return GetPropertys("", vObjOriginal, vObjOriginal);
        }

        public static List<SAraDevProperty> GetPropertys(string Perfix, IAraObject vObjOriginal, object vTmpObj)
        {
            List<SAraDevProperty> vReturn = new List<SAraDevProperty>();

            foreach (System.Reflection.PropertyInfo vProperty in vTmpObj.GetType().GetProperties().Where(a => a.GetCustomAttributes(typeof(AraDevProperty), true).Count() > 0))
            {
                AraDevProperty At = (AraDevProperty)vProperty.GetCustomAttributes(typeof(AraDevProperty), true)[0];
                bool IsAraDevClassPropertyString = vProperty.PropertyType.GetCustomAttributes(typeof(AraDevClassPropertyString), true).Count() > 0;
                AraDevPropertyCustomWindow AraDevPropertyCustomWindow = (At is AraDevPropertyCustomWindow ? (AraDevPropertyCustomWindow)At : null);
                bool Hide = vProperty.GetCustomAttributes(typeof(AraDevPropertyHide), true).Count() > 0;
                bool vPropertySupportLayout = vProperty.GetCustomAttributes(typeof(PropertySupportLayout), true).Count() > 0;;

                string vName = (Perfix == "" ? "" : Perfix + ".") + vProperty.Name;
                if (isClass(vProperty.PropertyType) == false || IsAraDevClassPropertyString)
                {
                    object vValueObj = vProperty.GetValue(vTmpObj, null);
                    string vValue;
                    if (vProperty.PropertyType.IsEnum)
                        vValue = (vValueObj == null ? null : Enum.GetName(vProperty.PropertyType, vValueObj));
                    else
                        vValue = (vValueObj == null ? null : ((dynamic)vValueObj).ToString());

                    bool Editavel = AraDevPropertyCustomWindow ==null && (EditableAsString(vProperty.PropertyType) || IsAraDevClassPropertyString);
                    //string vTmpValor = vValue + (Editavel ?"": " " + Propriedades.Buttons.Add(AraGridButton.ButtonIco.pencil, new System.Collections.Hashtable { { "InstanceID", vObjOriginal.InstanceID }, { "name", vName }, { "value", vValue } }, EditProperty) );
                    vReturn.Add(new SAraDevProperty()
                    {
                        InstanceID = vObjOriginal.InstanceID,
                        Nome = vName,
                        IsDefault = (vName == "Name" ? true : (vValueObj == null ? At.ValueDefault == At.ValueDefault : vValueObj.Equals(At.ValueDefault))),
                        Value = vValue,
                        ValueObject = vValueObj,
                        Editable = Editavel,
                        PropertyInfo = vProperty,
                        Object = vTmpObj,
                        ValueDefault = At.ValueDefault,
                        Event = false,
                        ValueType = vProperty.PropertyType,
                        AraDevPropertyCustomWindow = AraDevPropertyCustomWindow,
                        Hide = Hide,
                        PropertySupportLayout = vPropertySupportLayout,
                        Attributes = vProperty.GetCustomAttributes(true).Select(a=>(Attribute)a).ToArray()
                    });

                }
                else
                {
                    object vValueObj = vProperty.GetValue(vTmpObj, null);
                    if (vValueObj != null)
                        vReturn.AddRange(GetPropertys(vName, vObjOriginal, vValueObj));

                }
            }

            foreach (System.Reflection.FieldInfo vField in vTmpObj.GetType().GetFields().Where(a => a.GetCustomAttributes(typeof(AraDevProperty), true).Count() > 0))
            {
                AraDevProperty At = (AraDevProperty)vField.GetCustomAttributes(typeof(AraDevProperty), true)[0];

                bool IsAraDevClassPropertyString = vField.FieldType.GetCustomAttributes(typeof(AraDevClassPropertyString), true).Count() > 0;
                AraDevPropertyCustomWindow AraDevPropertyCustomWindow = (At is AraDevPropertyCustomWindow ? (AraDevPropertyCustomWindow)At : null);
                bool Hide = vField.GetCustomAttributes(typeof(AraDevPropertyHide), true).Count() > 0;
                bool vPropertySupportLayout = vField.GetCustomAttributes(typeof(PropertySupportLayout), true).Count() > 0; ;

                string vName = (Perfix == "" ? "" : Perfix + ".") + vField.Name;
                if (isClass(vField.FieldType) == false || IsAraDevClassPropertyString)
                {
                    object vValueObj = vField.GetValue(vTmpObj);
                    string vValue;
                    if (vField.FieldType.IsEnum)
                        vValue = (vValueObj == null ? null : Enum.GetName(vField.FieldType, vValueObj));
                    else
                        vValue = (vValueObj == null ? null : ((dynamic)vValueObj).ToString());


                    bool Editavel = AraDevPropertyCustomWindow ==null && (EditableAsString(vField.FieldType) || IsAraDevClassPropertyString);

                    //string vTmpValor = vValue + (Editavel?"": " " + Propriedades.Buttons.Add(AraGridButton.ButtonIco.pencil, new System.Collections.Hashtable { { "InstanceID", vObjOriginal.InstanceID }, { "name", vName }, { "value", vValue } }, EditProperty));
                    vReturn.Add(new SAraDevProperty()
                    {
                        InstanceID = vObjOriginal.InstanceID,
                        Nome = vName,
                        IsDefault = (vValueObj == null ? At.ValueDefault == At.ValueDefault : vValueObj.Equals(At.ValueDefault)),
                        Value = vValue,
                        ValueObject = vValueObj,
                        Editable = Editavel,
                        FieldInfo = vField,
                        Object = vTmpObj,
                        ValueDefault = At.ValueDefault,
                        Event = false,
                        ValueType = vField.FieldType,
                        AraDevPropertyCustomWindow = AraDevPropertyCustomWindow,
                        Hide = Hide,
                        PropertySupportLayout = vPropertySupportLayout,
                        Attributes = vField.GetCustomAttributes(true).Select(a => (Attribute)a).ToArray()
                    });
                }
                else
                {
                    object vValueObj = vField.GetValue(vTmpObj);
                    if (vValueObj != null)
                        vReturn.AddRange(GetPropertys(vName, vObjOriginal, vValueObj));

                }
            }

            foreach (System.Reflection.PropertyInfo vProperty in vTmpObj.GetType().GetProperties().Where(a => a.GetCustomAttributes(typeof(AraDevEvent), true).Count() > 0))
            {
                IAraEvent Event = (IAraEvent)vProperty.GetValue(vTmpObj, null);

                string vName = (Perfix == "" ? "" : Perfix + ".") + vProperty.Name;
                string vValue = (Event ==null || Event.Name == null ? "" : Event.Name);
                vReturn.Add(new SAraDevProperty()
                {
                    InstanceID = vObjOriginal.InstanceID,
                    Nome = vName,
                    IsDefault = (vValue == "" || vValue == null),
                    Value = vValue,
                    ValueObject = Event,
                    Editable = false,
                    PropertyInfo = vProperty,
                    Object = vTmpObj,
                    ValueDefault = null,
                    Event = true,
                    ValueType = vProperty.PropertyType,
                    Attributes = vProperty.GetCustomAttributes(true).Select(a => (Attribute)a).ToArray()
                });
            }

            foreach (System.Reflection.FieldInfo vField in vTmpObj.GetType().GetFields().Where(a => a.GetCustomAttributes(typeof(AraDevEvent), true).Count() > 0))
            {
                IAraEvent Event = (IAraEvent)vField.GetValue(vTmpObj);

                string vName = (Perfix == "" ? "" : Perfix + ".") + vField.Name;
                string vValue = (Event ==null || Event.Name == null ? "" : Event.Name);
                vReturn.Add(new SAraDevProperty()
                {
                    InstanceID = vObjOriginal.InstanceID,
                    Nome = vName,
                    IsDefault = (vValue == "" || vValue == null),
                    Value = vValue,
                    ValueObject = Event,
                    Editable = false,
                    FieldInfo = vField,
                    Object = vTmpObj,
                    ValueDefault = null,
                    Event = true,
                    ValueType = vField.FieldType,
                    Attributes = vField.GetCustomAttributes(true).Select(a => (Attribute)a).ToArray()
                });
            }

            //vReturn = vReturn.OrderBy(a => a.nome).ToList();

            return vReturn;
        }

        private static bool isClass(Type vType)
        {
            return vType.IsClass && vType != typeof(string);
        }

        public static bool EditableAsString(Type vType)
        {
            return
            vType == typeof(string) ||
            vType == typeof(int) || vType == typeof(Int16) || vType == typeof(Int64) ||
            vType == typeof(int?) || vType == typeof(Int16?) || vType == typeof(Int64?) ||
            vType == typeof(decimal) || vType == typeof(decimal?) ||
            vType == typeof(float) || vType == typeof(float?) ||
            vType == typeof(double) || vType == typeof(double?)
                //vType == typeof(AraDistance) ||
                //vType.GetInterface("IAraEvent") != null
            ;

        }

        public static void SetProperty(IAraDev vObj, string vPropriedade, string vValue)
        {
            //List<SAraDevProperty> vTmp = SAraDevProperty.GetPropertys(vObj);
            SAraDevProperty vTmpPropriedade = SAraDevProperty.GetPropertys(vObj).First(a => a.Nome == vPropriedade);

            if (vTmpPropriedade.Event)
            {
                if (vTmpPropriedade.PropertyInfo != null)
                    ((IAraEvent)vTmpPropriedade.PropertyInfo.GetValue(vTmpPropriedade.Object, null)).Name = (string)vValue;
                else
                    ((IAraEvent)vTmpPropriedade.FieldInfo.GetValue(vTmpPropriedade.Object)).Name = (string)vValue;
            }
            else
            {
                if (vTmpPropriedade.PropertyInfo != null)
                {
                    if (SAraDevProperty.EditableAsString(vTmpPropriedade.PropertyInfo.PropertyType))
                        vTmpPropriedade.PropertyInfo.SetValue(vTmpPropriedade.Object, ChangeType(vValue, vTmpPropriedade.PropertyInfo.PropertyType), null);
                    else if (vTmpPropriedade.PropertyInfo.PropertyType.Equals(typeof(bool)))
                    {
                        vTmpPropriedade.PropertyInfo.SetValue(vTmpPropriedade.Object, Convert.ToBoolean(vValue), null);
                    }
                    else if (vTmpPropriedade.PropertyInfo.PropertyType.IsEnum)
                    {
                        Type vType = vTmpPropriedade.PropertyInfo.PropertyType;
                        vTmpPropriedade.PropertyInfo.SetValue(vTmpPropriedade.Object, Enum.Parse(vType, vValue), null);
                    }
                    else
                    {
                        Type vType = vTmpPropriedade.PropertyInfo.PropertyType;
                        System.Reflection.ConstructorInfo Contruct = vType.GetConstructor(new Type[] { typeof(string) });
                        vTmpPropriedade.PropertyInfo.SetValue(vTmpPropriedade.Object, Contruct.Invoke(new object[] { vValue }), null);
                    }
                }
                else
                {
                    if (SAraDevProperty.EditableAsString(vTmpPropriedade.FieldInfo.FieldType))
                        vTmpPropriedade.FieldInfo.SetValue(vTmpPropriedade.Object, ChangeType(vValue, vTmpPropriedade.FieldInfo.FieldType));
                    else if (vTmpPropriedade.FieldInfo.FieldType.Equals(typeof(bool)))
                    {
                        vTmpPropriedade.FieldInfo.SetValue(vTmpPropriedade.Object, Convert.ToBoolean(vValue));
                    }
                    else if (vTmpPropriedade.FieldInfo.FieldType.IsEnum)
                    {
                        Type vType = vTmpPropriedade.FieldInfo.FieldType;
                        vTmpPropriedade.FieldInfo.SetValue(vTmpPropriedade.Object, Enum.Parse(vType, vValue));
                    }
                    else
                    {
                        Type vType = vTmpPropriedade.FieldInfo.FieldType;
                        System.Reflection.ConstructorInfo Contruct = vType.GetConstructor(new Type[] { typeof(string) });
                        vTmpPropriedade.FieldInfo.SetValue(vTmpPropriedade.Object, Contruct.Invoke(new object[] { vValue }));
                    }
                }
            }
        }

        private static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t); ;
            }

            return Convert.ChangeType(value, t);
        }
    }
}
