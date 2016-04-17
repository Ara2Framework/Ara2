// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ara2.Keyboard
{
    public delegate void DArakeyboard(object Object, AraKeyboard vAraKeyboard);

    public class AraKeyboard
    {
        public int Key
        {
            get
            {
                return Convert.ToInt16(Tick.GetTick().Page.Request["Key"]);
            }
        }

        public bool ctrlKey
        {
            get
            {
                return Tick.GetTick().Page.Request["ctrlKey"]=="true";
            }
        }

        public bool altKey
        {
            get
            {
                return Tick.GetTick().Page.Request["altKey"] == "true";
            }
        }

        public bool shiftKey
        {
            get
            {
                return Tick.GetTick().Page.Request["shiftKey"] == "true";
            }
        }
        
    }
}
