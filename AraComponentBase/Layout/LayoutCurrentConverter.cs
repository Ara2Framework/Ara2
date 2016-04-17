// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace Ara2.Components
{
    
    public class LayoutCurrentConverter : StringConverter
    {
        public static readonly string NovoEditar = "<Novo\\Editar Layout>";
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            //true means show a combobox
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            //true will limit to list. false will show the list, but allow free-form entry
            return true;
        }

        public override
            System.ComponentModel.TypeConverter.StandardValuesCollection
            GetStandardValues(ITypeDescriptorContext context)
        {


            List<string> strCollection = new List<string>();
            strCollection.Add(LayoutCurrentConverter.NovoEditar);

            var vLayoutsS = ((dynamic)context.Instance).LayoutsString;
            if (vLayoutsS != null)
            {
                var vAraLayouts = (new CustomBinarySerializer<AraLayouts>()).DeserializeFromBytes(Convert.FromBase64String(vLayoutsS));
                if (vAraLayouts != null)
                {
                    foreach (var vL in vAraLayouts)
                    {
                        strCollection.Add((vL.Name == null ? "Default" : vL.Name));
                    }
                }
            }

            return new StandardValuesCollection(strCollection);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                if ((string)value == NovoEditar)
                    return value;
                else if ((string)value == "Default")
                    return value;
                else
                {

                    var vAraLayouts = (new CustomBinarySerializer<AraLayouts>()).DeserializeFromBytes(Convert.FromBase64String(((dynamic)context.Instance).LayoutsString));
                    foreach (var vL in vAraLayouts)
                    {
                        if (vL.Name == (string)value)
                            return vL;
                    }

                    return null;
                }
            }
            else
                return value;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {

            if (value == null)
                return string.Empty;

            if (value.GetType() == typeof(string) && destinationType == typeof(string))
                return value;

            try
            {
                return ((dynamic)value).Name;
            }
            catch
            {
                return null;
            }
        }

    }
    
}
