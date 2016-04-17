// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ara2
{
    delegate void AraDelegate_Click(AraEventMouse EventMouse);

    public class AraEventMouse
    {
        public int? Button = null;
        public int? layerX = null; //	48	Number
        public int? layerY = null; //	9	Number
        public int? clientX = null; //	545	Number
        public int? clientY = null; //	187	Number                
        public int? offsetX = null; //	31	Number
        public int? offsetY = null; //	5	Number
        public int? pageX = null; //	545	Number
        public int? pageY = null; //	187	Number
        public int? screenX = null; //	545	Number
        public int? screenY = null; //	274	Number
        public int? x = null; //	545	Number
        public int? y = null; //	187	Number
        public bool? altKey = null; //	false	Boolean
        public bool? ctrlKey = null; //	false	Boolean
        public bool? shiftKey = null; //	false	Boolean

        public AraEventMouse()
        {
            Tick vTick = Tick.GetTick();

            try
            {
                Button = Convert.ToInt16(vTick.Page.Request["Mouse_which"]);
                if (Button == 0) Button = 1;
            }
            catch { }

            try
            {
                layerX = Convert.ToInt32(vTick.Page.Request["Mouse_layerX"]);
            }
            catch { }

            try
            {
                layerY = Convert.ToInt32(vTick.Page.Request["Mouse_layerY"]);
            }
            catch { }

            try
            {
                clientX = Convert.ToInt32(vTick.Page.Request["Mouse_clientX"]);
            }
            catch { }
            try
            {
                clientY = Convert.ToInt32(vTick.Page.Request["Mouse_clientY"]);
            }
            catch { }
            try
            {
                offsetX = Convert.ToInt32(vTick.Page.Request["Mouse_offsetX"]);
            }
            catch { }
            try
            {
                offsetY = Convert.ToInt32(vTick.Page.Request["Mouse_offsetY"]);
            }
            catch { }
            try
            {
                pageX = Convert.ToInt32(vTick.Page.Request["Mouse_pageX"]);
            }
            catch { }
            try
            {
                pageY = Convert.ToInt32(vTick.Page.Request["Mouse_pageY"]);
            }
            catch { }
            try
            {
                screenX = Convert.ToInt32(vTick.Page.Request["Mouse_screenX"]);
            }
            catch { }
            try
            {
                screenY = Convert.ToInt32(vTick.Page.Request["Mouse_screenY"]);
            }
            catch { }
            try
            {
                x = Convert.ToInt32(vTick.Page.Request["Mouse_x"]);
            }
            catch { }
            try
            {
                y = Convert.ToInt32(vTick.Page.Request["Mouse_y"]);
            }
            catch { }
            try
            {
                altKey = Convert.ToBoolean(vTick.Page.Request["Mouse_altKey"]);
            }
            catch { }
            try
            {
                ctrlKey = Convert.ToBoolean(vTick.Page.Request["Mouse_ctrlKey"]);
            }
            catch { }
            try
            {
                shiftKey = Convert.ToBoolean(vTick.Page.Request["Mouse_shiftKey"]);
            }
            catch { }
        }
    }

}
