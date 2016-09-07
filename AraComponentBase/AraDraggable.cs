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
    public class AraDraggable:AraObject
    {
        
        string _ObjectNameHtml = null;

        public AraDraggable(IAraObject vComponentVisual) :
            this(vComponentVisual, vComponentVisual.InstanceID)
        {
            
        }

        public AraDraggable(IAraObject vComponentVisual, string ObjectNameHtml) :
            base(Tick.GetTick().Session.GetNewID(), vComponentVisual)
        {
            _ObjectNameHtml = ObjectNameHtml;

            foreach (var vDragable in this.ConteinerFather.Childs.Where(a => a is AraDraggable && a != this).ToArray())
            {
                vDragable.Dispose();
            }


            Tick vTick = Tick.GetTick();
            vTick.Script.Send(@"
                try
                {
                    $('#" + _ObjectNameHtml + @"').draggable('destroy');
                    $('#" + _ObjectNameHtml + @"').enableSelection();
                    $('#" + _ObjectNameHtml + @"').css('webkit-user-select','text')
                    $('#" + _ObjectNameHtml + @"').css('user-select','text')
                }catch(err){}
                // grid: [ 5, 5 ],
                $('#" + _ObjectNameHtml + @"').disableSelection().css('webkit-user-select','none').css('user-select','none').draggable(
                    {
                        
//                        snap:true,
//                        snapTolerance:5,
                        cancel: null,
                        containment:$('#" + this.ConteinerFather.ConteinerFather.InstanceID + @"'),
                        stop:function( event, ui ) {
                            Ara.GetObject('" + this.ConteinerFather.InstanceID + @"').SetTop(parseInt(ui.position.top,10) + 'px');
                            Ara.GetObject('" + this.ConteinerFather.InstanceID + @"').SetLeft(parseInt(ui.position.left,10) + 'px');
                            
                            Ara.Tick.Send(4, 0, 'Ara', 'OnDraggable', {id:'" + this.InstanceID + @"',OldTop:parseInt(ui.originalPosition.top,10),OldLeft:parseInt(ui.originalPosition.left,10)});
                            
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
                    Tick.GetTick().Script.Send("$('#" + _ObjectNameHtml + "').draggable( 'enable' );");
                else
                    Tick.GetTick().Script.Send("$('#" + _ObjectNameHtml + "').draggable( 'disable' );");
            }
        }

        public delegate void DOnDraggable(object vObj, decimal OldLeft,decimal OldTop);
        public AraEvent<DOnDraggable> OnDraggable = new AraEvent<DOnDraggable>();


    }
}
