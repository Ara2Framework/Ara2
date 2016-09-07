// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ara2.Components;

namespace Ara2.Session1
{
    [Serializable]
    public class SessionJSs:AraObject
    {
        Session Session;

        public SessionJSs(Session vSession) :
            base(null,null)
        {
            Session = vSession;
        }

        [Serializable]
        private struct SJsLoad
        {
            public string Url;
            public int TickId;
            public DateTime DataHora;

            public SJsLoad(string vUrl, int vTickId)
            {
                Url = vUrl;
                TickId = vTickId;
                DataHora = DateTime.Now;
            }
        }

        Dictionary<string,SJsLoad> _JsLoad = new Dictionary<string,SJsLoad>();
        public void AddJs(string vFile)
        {
            if (!_JsLoad.ContainsKey(vFile))
            {
                Tick vTick = Tick.GetTick();
                _JsLoad.Add(vFile,new SJsLoad(vFile, vTick.Id));
                vTick.Script.Send("Ara.AraClass.LoadClass(0," + vTick.Id + ",'" + AraTools.StringToStringJS(vTick.AraPageMain.GetUrlRedirectFiles(vFile)) + "',function () {\n");
                vTick.Script.GetNewLevel();

                vTick.Script.Send(vTick.Script.Level - 1, " ); \n ");
            }

        }

        public bool IsJsLoad(string vFile)
        {
            return _JsLoad.ContainsKey(vFile);
        }
    }
}
