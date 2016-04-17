// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;


namespace Ara2.Components
{
    [Serializable]
    public class AraScrollBar : IDisposable
    {
        private static bool ArquivosHdCarregado = false;
        public AraScrollBar()
        {
            Tick vTick = Tick.GetTick();
            vTick.Session.AddJs("Ara2/Files/AraScrollBar/AraScrollBar.js");
        }


        public AraScrollBar(IAraComponentVisual vComponentVisual):
            this(vComponentVisual, vComponentVisual.InstanceID)
        {
        }

        public AraScrollBar(IAraComponentVisual vComponentVisual, string ObjectNameHtml)
            :this()
        {
            _ComponentVisual = vComponentVisual;
            _ObjectNameHtml = ObjectNameHtml;
        }

        IAraComponentVisual _ComponentVisual=null;
        public IAraComponentVisual ComponentVisual
        {
            get { return _ComponentVisual; }
            set { 
                _ComponentVisual = value;
                ObjectNameHtml = _ComponentVisual.InstanceID;
            }
        }

        string _ObjectNameHtml = null;
        public string ObjectNameHtml
        {
            get { return _ObjectNameHtml; }
            set { _ObjectNameHtml = value; }
        }

        private string _cursorcolor = "#000000";
        /// <summary>
        /// cursorcolor - change cursor color in hex, default is "#000000"
        /// </summary>
        public string Cursorcolor
        {
            get { return _cursorcolor; }
            set { _cursorcolor = value; }
        }


        private double _cursoropacitymin = 0;
        /// <summary>
        /// cursoropacitymin - change opacity very cursor is inactive (scrollabar "hidden" state), range from 1 to 0, default is 0 (hidden)
        /// </summary>
        public double Cursoropacitymin
        {
            get { return _cursoropacitymin; }
            set { _cursoropacitymin = value; }
        }

        private double _cursoropacitymax = 1;
        /// <summary>
        /// cursoropacitymax - change opacity very cursor is active (scrollabar "visible" state), range from 1 to 0, default is 1 (full opacity)
        /// </summary>
        public double Cursoropacitymax
        {
            get { return _cursoropacitymax; }
            set { _cursoropacitymax = value; }
        }
        
        private int _cursorwidth = 5;
        /// <summary>
        /// cursorwidth - cursor width in pixel, default is 5 (you can write "5px" too)
        /// </summary>
        public int Cursorwidth
        {
            get { return _cursorwidth; }
            set { _cursorwidth = value; }
        }



        public void Commit()
        {
            if (_ObjectNameHtml == null)
                throw new Exception("the AraSrollbar component 'ObjectNameHtml' method not defined");

            List<string> vParameters = new List<string>();
            vParameters.Add("zindex : 999988");

            if (_cursorcolor != "#000000")
                vParameters.Add("cursorcolor : '" + _cursorcolor + "'");

            if (_cursorwidth != 5)
                vParameters.Add("cursorwidth : '" + _cursorwidth + "px'");

            if (_cursoropacitymin != 0)
                vParameters.Add("cursoropacitymin : " + _cursoropacitymin.ToString().Replace(",", "."));

            if (_cursoropacitymax != 1)
                vParameters.Add("cursoropacitymax : " + _cursoropacitymax.ToString().Replace(",", "."));

            Tick.GetTick().Script.Send("Ara.AraScrollBar.Create('" + _ObjectNameHtml + "'," + (vParameters.Any() ? "{" + string.Join(",", vParameters) + "}" : "null") + "); \n");
        }

        public void ToBottom()
        {
            //$('#" + _ObjectNameHtml + "').scrollHeight
            Tick.GetTick().Script.Send("Ara.AraScrollBar.ScrollTop('" + _ObjectNameHtml + "'); \n");
        }

        public void Dispose()
        {
            Tick.GetTick().Script.Send("Ara.AraScrollBar.Remove('" + _ObjectNameHtml + "'); \n");
        }
    }
}
