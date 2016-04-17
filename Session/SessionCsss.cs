// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ara2.Components;
using System.Collections;

namespace Ara2
{
    [Serializable]
    public class SessionCsss:AraObject
    {

        Session Session;

        public SessionCsss(Session vSession) :
            base(null,null)
        {
            Session = vSession;
        }

        List<string> CssLoad = new List<string>();
        public void AddCss(string vFile)
        {
            if (!CssLoad.Exists(a => a == vFile))
            {
                Tick vTick = Tick.GetTick();
                vTick.Script.Send("Ara.AddCss('" + AraTools.StringToStringJS(vFile) + "');\n");
                CssLoad.Add(vFile);
            }
        }
    }
}
