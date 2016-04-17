// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ara2.Json;
using Ara2.Dev;
using System.ComponentModel;
using System.Globalization;
using System.Collections;
using System.Dynamic;
using System.Globalization;

namespace Ara2.Components
{
    [Serializable]
    [AraDevClassPropertyString]
    [TypeConverter(typeof(AraDistanceConverter))]
    [Category("Layout")]
    public class AraDistance
    {
        public delegate void DChangeDistance(AraDistance ToDistance);

        public enum EUnity
        {
            px=1,
            percent=2,
            em=3
        }

        private static Dictionary<int, string> EUnityString = new Dictionary<int, string>
        {
            {1,"px"},
            {2,"%"},
            {3,"em"},
        };

        private decimal _Value;
        //[RefreshProperties(RefreshProperties.All)]
        [Browsable(false)]
        public decimal Value
        {
            get { return _Value; }
            //set{_Value = value;}
        }
        
        private EUnity _Unity ;
        //[RefreshProperties(RefreshProperties.All)]
        [Browsable(false)]
        public EUnity Unity
        {
            get { return _Unity; }
            //set{_Unity = value;}
        }

        

        public AraDistance(string vValue)
        {
            if (vValue != null && vValue != "NaNpx")
            {
                if (vValue.Length > 1 && string.Compare(vValue.Substring(vValue.Length - 2, 2), "px", true) == 0)
                    _Unity = EUnity.px;
                else if (vValue.Length > 2 && string.Compare(vValue.Substring(vValue.Length - 1, 1), "%", true) == 0)
                    _Unity = EUnity.percent;
                else if (vValue.Length > 2 && string.Compare(vValue.Substring(vValue.Length - 2, 2), "em", true) == 0)
                    _Unity = EUnity.em;
                else if (AraTools.IsDecimal(vValue) || AraTools.IsInt(vValue))
                {
                    _Value = Convert.ToDecimal(vValue);
                    _Unity = EUnity.px;
                    return;
                }
                else
                    throw new Exception("AraDistance Valor invalido '" + vValue + "'");

                _Value = Convert.ToDecimal(vValue.Substring(0, vValue.Length - EUnityString[(int)Unity].Length).Replace(".", ","));
            }
            else
            {
                _Value = 0;
                _Unity = EUnity.px;
            }
        }

        public AraDistance(decimal vValue, EUnity vUnity)
        {
            _Value = vValue;
            _Unity = vUnity;
        }


        public static AraDistance operator +(AraDistance vObj, decimal vObj2)
        {
            return new AraDistance(vObj.Value + vObj2,vObj.Unity);
        }

        public static AraDistance operator -(AraDistance vObj, decimal vObj2)
        {
            return new AraDistance(vObj.Value - vObj2, vObj.Unity);
        }

        public static implicit operator AraDistance(decimal value)
        {
            return new AraDistance(value, EUnity.px);
        }

        public static implicit operator string(AraDistance value)
        {
            return value.ToString();
        }

        public string ToString()
        {
            return Value.ToString().Replace(".", "").Replace(",", ".") + EUnityString[(int)Unity];
        }
    }

    internal class AraDistanceConverter : ExpandableObjectConverter
    {
        //public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
        //{
        //    return true;
        //}

        //public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues)
        //{
        //    return new AraDistance("");
        //}


        public object ConvertFromString(string text)
        {
            return (object)(new AraDistance(text));
        }

        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType)
        {
            return true;
        }

        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            return new AraDistance((string)value);
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                         CultureInfo culture,
                                         object value,
                                         Type destinationType)
        {

            if (destinationType == typeof(string) && value is AraDistance)
                return ((AraDistance)value).ToString();
            else if (destinationType == typeof(string) && value is DynamicObject)
                return ((dynamic)value).ToString();
            else
                return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
