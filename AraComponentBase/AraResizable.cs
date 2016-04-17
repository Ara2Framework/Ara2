// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Ara2.Components
{
    [Serializable]
    public class AraResizable:AraObject
    {
        string _ObjectNameHtml = null;

        public AraResizable(IAraObject vComponentVisual) :
            this(vComponentVisual, vComponentVisual.InstanceID)
        {
            
        }

        public AraResizable(IAraObject vComponentVisual, string ObjectNameHtml) :
            base(Tick.GetTick().Session.GetNewID(), vComponentVisual)
        {
            _ObjectNameHtml = ObjectNameHtml;

            foreach (var vResizable in this.ConteinerFather.Childs.Where(a => a is AraResizable && a != this).ToArray())
            {
                vResizable.Dispose();
            }

            Tick vTick = Tick.GetTick();
            vTick.Script.Send(@"
                try
                {
                    $('#" + _ObjectNameHtml + @"').resizable('destroy');
                }catch(err){}
                $('#" + _ObjectNameHtml + @"').resizable(
                    {
                        stop:function( event, ui ) {
                            Ara.GetObject('" + this.ConteinerFather.InstanceID + @"').SetWidth(parseInt(ui.size.width,10) + 'px');
                            Ara.GetObject('" + this.ConteinerFather.InstanceID + @"').SetHeight(parseInt(ui.size.height,10) + 'px');
                            Ara.Tick.Send(4, " + vTick.Session.AppId + @", 'Ara', 'OnResize', {id:'" + this.InstanceID + @"'});
                        }
                    }
                );
            ");

            //vTick.Script.Send(@"$('#" + _ObjectNameHtml + @"').resizable();");

            
        }

        private bool _Enable = true;
        public bool Enable
        {
            get { return _Enable; }
            set
            {
                _Enable = value;

                if (_Enable)
                    Tick.GetTick().Script.Send("$('#" + _ObjectNameHtml + "').resizable( 'enable' );");
                else
                    Tick.GetTick().Script.Send("$('#" + _ObjectNameHtml + "').resizable( 'disable' );");
            }
        }

        public AraEvent<Action> OnResize = new AraEvent<Action>();


    }
}
