// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Ara2.Components
{
    [Serializable]
    public class AraComponentEventKey<T> : AraComponentEvent<T>
    {
        public AraComponentEventKey():
            base()
        {
        }

        public AraComponentEventKey(AraObjectClienteServer vObject, string vEventName) :
            base(vObject, vEventName)
        { }

        public AraComponentEventKey(AraObjectClienteServer vObject, string vEventName, T vEvent) :
            base(vObject, vEventName, vEvent)
        { }

        public AraComponentEventKey(AraObjectClienteServer vObject, string vEventName, T vEvent, EAraComponentEventTypeThread vTypeThreadEvent) :
            base(vObject, vEventName, vEvent, vTypeThreadEvent)
        { }

        public AraComponentEventKey(AraObjectClienteServer vObject, string vEventName, EAraComponentEventTypeThread vTypeThreadEvent) :
            base(vObject, vEventName, vTypeThreadEvent)
        { }


        private int[] _Ignore;
        [JsonIgnore]
        public int[] Ignore
        {
            get { return _Ignore; }
            set
            {
                _Ignore = value;

                string TmpScriptArray = "";

                for (int n = 0; n < _Ignore.Length; n++)
                {
                    TmpScriptArray += "'" + _Ignore[n] + "',";
                }
                TmpScriptArray = TmpScriptArray.Substring(0, TmpScriptArray.Length - 1);

                this.Object.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.Events." + this.EventName + ".Ignore = [" + TmpScriptArray + "]; \n");
            }
        }

        private int[] _Only;
        [JsonIgnore]
        public int[] Only
        {
            get { return _Only; }
            set
            {
                _Only = value;

                string TmpScriptArray = "";

                for (int n = 0; n < _Only.Length; n++)
                {
                    TmpScriptArray += "'" + _Only[n] + "',";
                }
                TmpScriptArray = TmpScriptArray.Substring(0, TmpScriptArray.Length - 1);

                this.Object.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.Events." + this.EventName + ".Only = [" + TmpScriptArray + "]; \n");
            }
        }

        private int[] _ReturnFalse;
        [JsonIgnore]
        public int[] ReturnFalse
        {
            get { return _ReturnFalse; }
            set
            {
                _ReturnFalse = value;

                string TmpScriptArray = "";

                for (int n = 0; n < _ReturnFalse.Length; n++)
                {
                    TmpScriptArray += "'" + _ReturnFalse[n] + "',";
                }
                TmpScriptArray = TmpScriptArray.Substring(0, TmpScriptArray.Length - 1);

                this.Object.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.Events." + this.EventName + ".ReturnFalse = [" + TmpScriptArray + "]; \n");
            }
        }

        private int[] _ReturnTrue;
        [JsonIgnore]
        public int[] ReturnTrue
        {
            get { return _ReturnTrue; }
            set
            {
                _ReturnTrue = value;

                string TmpScriptArray = "";

                for (int n = 0; n < _ReturnTrue.Length; n++)
                {
                    TmpScriptArray += "'" + _ReturnTrue[n] + "',";
                }
                TmpScriptArray = TmpScriptArray.Substring(0, TmpScriptArray.Length - 1);

                this.Object.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.Events." + this.EventName + ".ReturnTrue = [" + TmpScriptArray + "]; \n");
            }
        }

        public static AraComponentEventKey<T> operator +(AraComponentEventKey<T> vObj, T vObj2)
        {
            vObj.AddEvent(vObj2);
            return vObj;
        }

        public static AraComponentEventKey<T> operator -(AraComponentEventKey<T> vObj, T vObj2)
        {
            vObj.DelEvent(vObj2);
            return vObj;
        }

    }
}
