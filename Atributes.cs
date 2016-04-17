// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ara2
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = true)]
    public class AraFieldAlias : Attribute
    {
        public AraFieldAlias(string vAlias)
        {
            Alias = vAlias;
        }
        public string Alias = null;
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property  | AttributeTargets.Interface, AllowMultiple = true)]
    public class AraFieldHide : Attribute
    {
    }

    
    public interface IAraFieldFormat
    {
        double Ordem { get; set; }
        string ToString(object vObj);
        string ToString(object vObjTag, object vObj);
        //string ToString(System.Nullable<T> vObj);
    }

    
    public class AraFieldFormatDouble : AraFieldFormatDecimal
    {
        public AraFieldFormatDouble(double vOrdem = 0) :
            base(vOrdem)
        {
        }

        public AraFieldFormatDouble(int vNumberDecimalPlaces, double vOrdem = 0)
            : base(vNumberDecimalPlaces, vOrdem)
        {
        }

        public virtual string ToString(double vValue)
        {
            return ToString(Convert.ToDecimal(vValue));
        }

        public virtual string ToString(double? vValue)
        {
            if (vValue == null)
                return "";
            else
                return ToString(Convert.ToDecimal(vValue));
        }

        public override string ToString(object vObjRef, object vValue)
        {
            if (vValue == null)
                return "";
            else
                return ToString(vObjRef,Convert.ToDecimal(vValue));
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = true)]
    public class AraFieldFormatDecimal : Attribute, IAraFieldFormat
    {
        double _Ordem = 0;
        /// <summary>
        /// default: 0
        /// </summary>
        /// <value>0</value>
        public double Ordem
        {
            get { return _Ordem; }
            set { _Ordem = value; }
        }

        public int NumberDecimalPlaces = 2;

        public AraFieldFormatDecimal(double vOrdem = 0)
        {
            Ordem = vOrdem;
        }

        public AraFieldFormatDecimal(int vNumberDecimalPlaces, double vOrdem = 0)
            :this(vOrdem)
        {
            NumberDecimalPlaces = vNumberDecimalPlaces;
        }

        public virtual string ToString(decimal vValue)
        {
            return vValue.ToString("n" + NumberDecimalPlaces);
        }
        public virtual string ToString(decimal? vValue)
        {
            if (vValue == null)
                return "";
            else
                return ToString((decimal)vValue);
        }

        public virtual string ToString(object vValue)
        {
            if (vValue == null)
                return "";
            else if (vValue is decimal)
                return ToString((decimal)vValue);
            else if (vValue is decimal?)
                return ToString((decimal?)vValue);
            else
                throw new Exception("Erro on formart AraFieldFormatDecimal '" + vValue.ToString() +"'.");
        }

        public virtual string ToString(object vObjRef, decimal vValue)
        {
            return ToString(vValue);
        }

        public virtual string ToString(object vObjRef, decimal? vValue)
        {
            return ToString(vValue);
        }

        public virtual string ToString(object vObjRef, object vValue)
        {
            return ToString( vValue);
        }
    }

    public class AraFieldFormatDateTimeHHMM : AraFieldFormatDateTime
    {
        public AraFieldFormatDateTimeHHMM(double vOrdem = 0)
            : base("dd/MM/yyyy HH:mm", vOrdem)
        {

        }
    }

    public class AraFieldFormatDateTimeHHMMSS : AraFieldFormatDateTime
    {
        public AraFieldFormatDateTimeHHMMSS(double vOrdem = 0)
            : base("dd/MM/yyyy HH:mm:ss", vOrdem)
        {

        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = true)]
    public class AraFieldFormatDateTime : Attribute,IAraFieldFormat
    {
        public string Format = "dd/MM/yyyy";

        double _Ordem = 0;
        /// <summary>
        /// default: 0
        /// </summary>
        /// <value>0</value>
        public double Ordem
        {
            get { return _Ordem; }
            set { _Ordem = value; }
        }

        public AraFieldFormatDateTime(double vOrdem = 0)
        {
            Ordem = vOrdem;
        }

        public AraFieldFormatDateTime(string vFormat, double vOrdem = 0)
            :this(vOrdem)
        {
            Format = vFormat;
        }

        public virtual string ToString(DateTime vValue)
        {
            return vValue.ToString(Format);
        }
        public virtual string ToString(DateTime? vValue)
        {
            if (vValue == null)
                return "";
            else
                return ToString((DateTime)vValue);
        }

        public virtual string ToString(object vValue)
        {
            if (vValue == null)
                return "";
            else if (vValue is DateTime)
                return ToString((DateTime)vValue);
            else if (vValue is DateTime?)
                return ToString((DateTime?)vValue);
            else
                throw new Exception("Erro on formart AraFieldFormatDateTime '" + vValue.ToString() + "'.");
        }


        public virtual string ToString(object vObjRef, DateTime vValue)
        {
            return ToString(vValue);
        }

        public virtual string ToString(object vObjRef, DateTime? vValue)
        {
            return ToString(vValue);
        }

        public virtual string ToString(object vObjRef, object vValue)
        {
            return ToString(vValue);
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Interface, AllowMultiple = true)]
    public class AraFieldFormatNumberColorPositiveNegative : Attribute, IAraFieldFormat
    {
        double _Ordem = 1;
        /// <summary>
        /// default: 1
        /// </summary>
        /// <value>1</value>
        public double Ordem
        {
            get { return _Ordem; }
            set { _Ordem = value; }
        }

        static string ConstColorPositive = "blue";
        static string ConstColorNegative = "red";

        public string ColorPositive = ConstColorPositive;
        public string ColorNegative = ConstColorNegative;

        public AraFieldFormatNumberColorPositiveNegative()
        {

        }

        public AraFieldFormatNumberColorPositiveNegative(string vColorPositive, string vColorNegative)
        {
            ColorPositive = vColorPositive;
            ColorNegative = vColorNegative;

        }

        public string ToString(object vValue)
        {
            if (AraTools.IsDecimal(vValue.ToString()))
            {
                decimal vValorD = Convert.ToDecimal(vValue);
                if (vValorD != 0)
                {
                    string Color = (vValorD > 0 ? ColorPositive : ColorNegative);

                    return "<font color='" + Color + "'>"
                        + vValue.ToString()
                        + "<font>";
                }
                else
                    return vValue.ToString();
            }
            else
                return vValue.ToString();
        }

        public string ToString(object Obj, object vValue)
        {
            return ToString(vValue);
        }
    }

}