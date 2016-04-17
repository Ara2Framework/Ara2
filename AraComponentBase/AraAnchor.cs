// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ara2.Dev;
using System.ComponentModel;
using System.Globalization;
using System.Dynamic;

namespace Ara2.Components
{
    [Serializable]
    [Category("Layout")]
    [TypeConverter(typeof(AraAnchorConverter))]
    public class AraAnchor : AraObject
    {

        public AraAnchor(IAraObjectClienteServer vComponent) :
            base(null, vComponent)
        {
            this.ConteinerFather.ChangeConteinerFatherBefore += Component_ChangeConteinerFatherBefore;
            this.ConteinerFather.ChangeConteinerFatherAfter += Component_ChangeConteinerFatherAfter;

            ((IAraObjectClienteServer)this.ConteinerFather).TickScriptCall();
            Tick.GetTick().Script.Send(" vObj.Anchor.SetConteinerFather(" + (this.ConteinerFather.ConteinerFather == null ? "null" : "Ara.GetObject('" + this.ConteinerFather.ConteinerFather.InstanceID + "')") + ");\n");
        }
        
        private decimal? _Left = null;
        [AraDevProperty]
        [PropertySupportLayout]
        [Category("Anchor")]
        [RefreshProperties(RefreshProperties.All)]
        public decimal? Left
        {
            get { return _Left; }
            set
            {
                if (_Left != value)
                {
                    _Left = value;

                    Tick vTick = Tick.GetTick();
                    ((IAraObjectClienteServer)this.ConteinerFather).TickScriptCall();
                    vTick.Script.Send(" vObj.Anchor.SetLeft(" + _Left + "); \n");
                }
            }
        }

        private decimal? _Top = null;
        [AraDevProperty]
        [PropertySupportLayout]
        [Category("Anchor")]
        [RefreshProperties(RefreshProperties.All)]
        public decimal? Top
        {
            get { return _Top; }
            set
            {
                if (_Top != value)
                {
                    _Top = value;

                    Tick vTick = Tick.GetTick();
                    ((IAraObjectClienteServer)this.ConteinerFather).TickScriptCall();
                    vTick.Script.Send(" vObj.Anchor.SetTop(" + _Top + "); \n");
                }
            }
        }

        private decimal? _Right = null;
        [AraDevProperty]
        [PropertySupportLayout]
        [Category("Anchor")]
        [RefreshProperties(RefreshProperties.All)]
        public decimal? Right
        {
            get { return _Right; }
            set
            {
                if (_Right != value)
                {
                    _Right = value;

                    Tick vTick = Tick.GetTick();
                    ((IAraObjectClienteServer)this.ConteinerFather).TickScriptCall();
                    vTick.Script.Send(" vObj.Anchor.SetRight(" + _Right + "); \n");
                }
            }
        }

        private decimal? _Bottom;
        [AraDevProperty]
        [PropertySupportLayout]
        [Category("Anchor")]
        [RefreshProperties(RefreshProperties.All)]
        public decimal? Bottom
        {
            get { return _Bottom; }
            set
            {
                if (_Bottom != value)
                {
                    _Bottom = value;

                    Tick vTick = Tick.GetTick();
                    ((IAraObjectClienteServer)this.ConteinerFather).TickScriptCall();
                    vTick.Script.Send(" vObj.Anchor.SetBottom(" + _Bottom + "); \n");
                }
            }
        }

        private bool _CenterH = false;
        [AraDevProperty(false)]
        [PropertySupportLayout]
        [Category("Center")]
        [RefreshProperties(RefreshProperties.All)]
        public bool CenterH
        {
            get { return _CenterH; }
            set
            {
                if (_CenterH != value)
                {
                    _CenterH = value;
                    if (_CenterH)
                    {
                        _Left = null;
                        _Right = null;
                    }

                    Tick vTick = Tick.GetTick();
                    ((IAraObjectClienteServer)this.ConteinerFather).TickScriptCall();
                    vTick.Script.Send(" vObj.Anchor.SetCenterH(" + (_CenterH ? "true" : "false") + "); \n");
                }
            }
        }

        private bool _CenterV = false;
        [AraDevProperty(false)]
        [PropertySupportLayout]
        [Category("Center")]
        [RefreshProperties(RefreshProperties.All)]
        public bool CenterV
        {
            get { return _CenterV; }
            set
            {
                if (_CenterV != value)
                {
                    _CenterV = value;
                    if (_CenterV)
                    {
                        _Top = null;
                        _Bottom = null;
                    }

                    Tick vTick = Tick.GetTick();
                    ((IAraObjectClienteServer)this.ConteinerFather).TickScriptCall();
                    vTick.Script.Send(" vObj.Anchor.SetCenterV(" + (_CenterV ? "true" : "false") + "); \n");
                }
            }
        }

        private void Component_ChangeConteinerFatherBefore(IAraObject ToConteinerFather)
        {

            if (((AraComponentVisual)this.ConteinerFather).TypePosition == AraComponentVisual.ETypePosition.Absolute && ToConteinerFather is AraComponentVisual)
            {
                if (((AraComponentVisual)ToConteinerFather).TypePosition == AraComponentVisual.ETypePosition.Static)
                    throw new Exception("you can not change to absolute because his father is static container");
            }
        }

        private void Component_ChangeConteinerFatherAfter()
        {
            ((IAraObjectClienteServer)this.ConteinerFather).TickScriptCall();
            Tick.GetTick().Script.Send(" vObj.Anchor.SetConteinerFather(" + (this.ConteinerFather.ConteinerFather == null ? "null" : "Ara.GetObject('" + this.ConteinerFather.ConteinerFather.InstanceID + "')") + ");\n");
        }


        private string DecimalToString(decimal? vValor)
        {
            if (vValor == null)
                return "null";
            else
                return Convert.ToDecimal(vValor).ToString().Replace(".", "").Replace(",", ".");
        }



        public void Reflesh()
        {
            Tick vTick = Tick.GetTick();
            ((IAraObjectClienteServer)this.ConteinerFather).TickScriptCall();
            vTick.Script.Send(" vObj.Anchor.Reflesh(); \n");
        }

        public string ToString()
        {
            List<string> Parans = new List<string>();
            if (this.Left !=null)
                Parans.Add("L: " + this.Left);
            if (this.Top != null)
                Parans.Add("T: " + this.Top);
            if (this.Right != null)
                Parans.Add("R: " + this.Right);
            if (this.Bottom != null)
                Parans.Add("B: " + this.Bottom);
            if (this.CenterV != false)
                Parans.Add("CV: " + this.CenterV);
            if (this.CenterH != false)
                Parans.Add("CH: " + this.CenterH);

            return string.Join(";", Parans);
        }
    }

    internal class AraAnchorConverter : ExpandableObjectConverter
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
            if (sourceType == typeof(string))
                return true;
            else
                return false;
        }
        static List<object> LockObjecsConvertForm = new List<object>();

        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            dynamic ObjAnchor = context.PropertyDescriptor.GetValue(context.Instance);

            //Evita Dupla Conversção na hora do set de cada propriedade
            bool EstaSendoConvertido =false;
            lock (LockObjecsConvertForm)
            {
                EstaSendoConvertido = LockObjecsConvertForm.Exists(a=>a.Equals((dynamic)ObjAnchor));
                if (!EstaSendoConvertido)
                    LockObjecsConvertForm.Add(ObjAnchor);
            }

            if (!EstaSendoConvertido)
            {
                try
                {

                    if (value is string)
                    {
                        foreach (var vParam in ((string)value).Split(';'))
                        {
                            var vParam2 = vParam.Split(':');
                            if (vParam2.Count() == 2)
                            {
                                switch (vParam2[0].ToUpper().Trim())
                                {
                                    case "L":
                                    case "LEFT":
                                        {
                                            if (vParam2[1].Trim().ToUpper() == "NULL")
                                                ObjAnchor.Left = null;
                                            else
                                            {
                                                decimal vValor;
                                                if (decimal.TryParse(vParam2[1].Trim(), out vValor))
                                                        ObjAnchor.Left = vValor;
                                            }
                                        }
                                        break;
                                    case "T":
                                    case "TOP":
                                        {
                                            if (vParam2[1].Trim().ToUpper() == "NULL")
                                                ObjAnchor.Top = null;
                                            else
                                            {
                                                decimal vValor;
                                                if (decimal.TryParse(vParam2[1].Trim(), out vValor))
                                                    ObjAnchor.Top = vValor;
                                            }
                                        }
                                        break;

                                    case "R":
                                    case "RIGHT":
                                        {
                                            if (vParam2[1].Trim().ToUpper() == "NULL")
                                                ObjAnchor.Right = null;
                                            else
                                            {
                                                decimal vValor;
                                                if (decimal.TryParse(vParam2[1].Trim(), out vValor))
                                                        ObjAnchor.Right = vValor;
                                            }
                                        }
                                        break;
                                    case "B":
                                    case "BOTTOM":
                                        {
                                            if (vParam2[1].Trim().ToUpper() == "NULL")
                                                ObjAnchor.Bottom = null;
                                            else
                                            {
                                                decimal vValor;
                                                if (decimal.TryParse(vParam2[1].Trim(), out vValor))
                                                    ObjAnchor.Bottom = vValor;
                                            }
                                        }
                                        break;
                                    case "CH":
                                    case "CENTERH":
                                        {
                                            bool vValor;
                                            if (bool.TryParse(vParam2[1].Trim(), out vValor))
                                                ObjAnchor.CenterH = vValor;
                                        }
                                        break;
                                    case "CV":
                                    case "CENTERV":
                                        {
                                            bool vValor;
                                            if (bool.TryParse(vParam2[1].Trim(), out vValor))
                                                ObjAnchor.CenterV = vValor;
                                        }
                                        break;
                                }
                            }
                        }
                    }

                }
                finally
                {
                    LockObjecsConvertForm.Remove(ObjAnchor);
                }
            }

            return ObjAnchor;
        }

        public override object ConvertTo(ITypeDescriptorContext context,
                                         CultureInfo culture,
                                         object value,
                                         Type destinationType)
        {

            if (destinationType == typeof(string) && value is AraAnchor)
                return ((AraDistance)value).ToString();
            else if (destinationType == typeof(string) && value is DynamicObject)
                return ((dynamic)value).ToString();
            else
                return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}

