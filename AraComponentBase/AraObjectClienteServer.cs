// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Reflection;
using Ara2.Dev;


namespace Ara2.Components
{

    [Serializable]
    public abstract class AraObjectClienteServer : AraObject,IAraObjectClienteServer
    {
        #region Static
        public static string Create(IAraObject ConteinerFather, string vNodeName)
        {
            return Create(ConteinerFather,vNodeName,(System.Collections.Generic.Dictionary<string, string>)null);
        }

        public static string Create(IAraObject ConteinerFather, string vNodeName, System.Collections.Generic.Dictionary<string, string> Attributes)
        {
            string vId=Tick.GetTick().Session.GetNewID();
            return Create(ConteinerFather,vNodeName, vId, Attributes);
        }

        public static string Create(IAraObject ConteinerFather, string vNodeName, string vId)
        {
            return Create(ConteinerFather, vNodeName, vId, null);
        }

        public static string Create(IAraObject ConteinerFather, string vNodeName, string vId, System.Collections.Generic.Dictionary<string, string> Attributes)
        {
            StringBuilder vTmpB = new StringBuilder();
            vTmpB.Append("Ara.CreateObject(" + (ConteinerFather != null ? "'" + AraTools.StringToStringJS(ConteinerFather.InstanceID) + "'" : "null") + ",'" + AraTools.StringToStringJS(vNodeName) + "','" + AraTools.StringToStringJS(vId) + "',");
            
            if (Attributes != null)
            {
                vTmpB.Append("{");
                if (Attributes.Any())
                    vTmpB.Append(string.Join(",", Attributes.Select(a => a.Key + ":'" + AraTools.StringToStringJS(a.Value) + "'")));
                vTmpB.Append("}");
            }
            else
                vTmpB.Append("null");

            vTmpB.Append(");\n");

            Tick.GetTick().Script.Send(vTmpB.ToString());
            return vId;
        }
        #endregion

        AraContainer AraContainer = new AraContainer();
        // (vObj==null? Tick.GetTick().Session.GetNewID():vObj.Name)

        //public AraObjectClienteServer(IAraObject vObj, IAraObject vConteinerFather) :
        //    this(vObj.InstanceID, vConteinerFather)
        //{
        //}


        protected AraObjectClienteServer():
            base()
        {

        }

        public AraObjectClienteServer(IAraObject vObj, IAraObject vConteinerFather, string vTypeNameJS) :
            this((vObj==null?null:vObj.InstanceID), vConteinerFather, vTypeNameJS)
        {
        }

        public AraObjectClienteServer(IAraObject vConteinerFather, string vTypeNameJS) :
            this((string)null, vConteinerFather, vTypeNameJS)
        {
        }

        public AraObjectClienteServer(string vName, IAraObject vConteinerFather, string vTypeNameJS) :
            base(vName, vConteinerFather)
        {

            //if (ConteinerFather != null && ConteinerFather is IAraContainerClient)
            //    ((IAraContainerClient)ConteinerFather).AddChild(this);
            Tick.GetTick().Session.AddObject(this, vConteinerFather, vTypeNameJS);

            
        }
        
        public void TickScriptCall()
        {
            Tick.GetTick().Script.CallObject(this);
        }

        public void Dispose()
        {
            
            base.Dispose();
        }


        public void CssAddClass(string vClassName)
        {
            this.TickScriptCall();
            Tick.GetTick().Script.Send("$(vObj.Obj).addClass('" + AraTools.StringToStringJS(vClassName) + "');");
        }

        public void CssRemoveClass(string vClassName)
        {
            this.TickScriptCall();
            Tick.GetTick().Script.Send("$(vObj.Obj).removeClass('" + AraTools.StringToStringJS(vClassName) + "');");
        }

        public void Style(string vName,string vValue)
        {
            this.TickScriptCall();
            Tick.GetTick().Script.Send("$(vObj.Obj).css('" + AraTools.StringToStringJS(vName) + "','" + AraTools.StringToStringJS(vValue) + "');");
        }



        //private AraEvent<DComponentEventInternal> _EventInternal = new AraEvent<DComponentEventInternal>();
        //[AraDevEvent]
        //public AraEvent<DComponentEventInternal> EventInternal
        //{
        //    get { return _EventInternal; }
        //    set { _EventInternal = value; }
        //}

        //private AraEvent<DComponentProperty> _SetProperty = new AraEvent<DComponentProperty>();
        //[AraDevEvent]
        //public AraEvent<DComponentProperty> SetProperty
        //{
        //    get { return _SetProperty; }
        //    set { _SetProperty = value; }
        //}
        public virtual void EventInternal(string vFunction)
        {

        }

        public abstract void LoadJS();

        public virtual void SetProperty(string vProperty, dynamic vValue)
        {
            this.TickScriptCall();

            Tick.GetTick().Script.Send(" vObj.ControlVar.SetValueUtm('" + vProperty + "'," + (vValue == null ? "null" : "'" + AraTools.StringToStringJS(vValue.ToString()) + "'") + "); \n");

            if (vProperty == "IsDestroyed()")
            {
                if (vValue == true)
                {
                    this.Dispose();
                }
            }
        }
        

        
    }
}
