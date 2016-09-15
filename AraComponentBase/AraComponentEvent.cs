// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Ara2.Components
{
    public enum EAraComponentEventTypeThread
    {
        ThreadMulti = 1,
        ThreadSingle = 2,
        ThreadClick = 3
    }

    //[TypeConverter(typeof(AraEventConverter))]
    [Category("Events")]
    [Serializable]
    public class AraComponentEvent<T>:AraEvent<T>
    {
        public static AraComponentEvent<T> operator +(AraComponentEvent<T> vObj, T vObj2)
        {
            vObj.AddEvent(vObj2);
            return vObj;
        }

        public static AraComponentEvent<T> operator -(AraComponentEvent<T> vObj, T vObj2)
        {
            vObj.DelEvent(vObj2);
            return vObj;
        }

        protected AraComponentEvent():
            base()
        {
            
        }

        public AraComponentEvent(IAraObjectClienteServer vObject, string vEventName)
        {
            _Object = new AraObjectInstance<IAraObjectClienteServer>(vObject);
            _EventName = vEventName;
            _EventsLoad = true;
        }


        public AraComponentEvent(IAraObjectClienteServer vObject, string vEventName, T vEvent)
        {
            _Event = vEvent;
            _Object = new AraObjectInstance<IAraObjectClienteServer>(vObject);
            _EventName = vEventName;
            _EventsLoad = true;
        }

        public AraComponentEvent(IAraObjectClienteServer vObject, string vEventName, T vEvent, EAraComponentEventTypeThread vTypeThreadEvent) :
            this(vObject, vEventName, vEvent)
        {
            TypeThreadEvent = vTypeThreadEvent;
            _EventsLoad = true;
        }

        public AraComponentEvent(IAraObjectClienteServer vObject, string vEventName, EAraComponentEventTypeThread vTypeThreadEvent)
        {
            _Object = new AraObjectInstance<IAraObjectClienteServer>(vObject);
            _EventName = vEventName;
            TypeThreadEvent = vTypeThreadEvent;
            _EventsLoad = true;
        }

        private AraObjectInstance<IAraObjectClienteServer> _Object;

        [JsonIgnore]
        public IAraObjectClienteServer Object
        {
            get
            {
                return (IAraObjectClienteServer)_Object.Object;
            }
        }


        public string _EventName;

        [JsonIgnore]
        public string EventName
        {
            get
            {
                return _EventName;
            }
            set
            {
                _EventName = value;
            }
        }


        private EAraComponentEventTypeThread _TypeThreadEvent = EAraComponentEventTypeThread.ThreadSingle;

        [JsonIgnore]
        public EAraComponentEventTypeThread TypeThreadEvent
        {
            get { return _TypeThreadEvent; }
            set
            {
                _TypeThreadEvent = value;
                UpdateValueThreadType();
            }
        }

        public void AddEvent(T vObj2)
        {
            base.AddEvent(vObj2);
            this.UpdateValueEnabled();
        }

        public void DelEvent(T vObj2)
        {
            base.DelEvent(vObj2);
            this.UpdateValueEnabled();
        }

        protected void UpdateValueEnabled()
        {
            
            Tick vTick = Tick.GetTick();
            Object.TickScriptCall();
            vTick.Script.Send(" if (vObj.Events." + _EventName + ".SetEnabled) { vObj.Events." + _EventName + ".SetEnabled(" + (Enabled ? "true" : "false") + "); } else {vObj.Events." + _EventName + ".Enabled = " + (Enabled ? "true" : "false") + "}; \n");
        }

        protected void UpdateValueThreadType()
        {
            Tick vTick = Tick.GetTick();
            ((IAraObjectClienteServer)Object).TickScriptCall();
            vTick.Script.Send(" vObj.Events." + _EventName + ".ThreadType = " + (int)TypeThreadEvent + "; \n");
        }

        

    }

}
