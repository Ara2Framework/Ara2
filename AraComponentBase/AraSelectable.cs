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
    public class AraSelectable:AraObject
    {
        string _ObjectNameHtml = null;

        public AraSelectable(IAraObject vComponentVisual) :
            this(vComponentVisual, vComponentVisual.InstanceID)
        {
            
        }

        public AraSelectable(IAraObject vComponentVisual, string ObjectNameHtml) :
            base(Tick.GetTick().Session.GetNewID(), vComponentVisual)
        {
            _ObjectNameHtml = ObjectNameHtml;
            
            Tick vTick = Tick.GetTick();
            vTick.Script.Send(@"
                try
                {
                    $('#" + _ObjectNameHtml + @"').selectable('destroy');
                }catch(err){}
                $('#" + _ObjectNameHtml + @"').selectable(
                    {
                        filter:'.selectable_" + this.InstanceID + @"',
                        stop: function( event, ui ) {
                            
                            var vIds = [];
                            var vTmp = $('.selectable_" + this.InstanceID + @".ui-selected').map(function(){ vIds.push($(this).attr('id')); });
                            $('.selectable_" + this.InstanceID + @".ui-selected').removeClass('ui-selected');
                            Ara.Tick.Send(4, " + vTick.Session.AppId + @", 'Ara', 'SELECTEDSTOP', {id:'" + this.InstanceID + @"',ObjsInstanceID:vIds.toString()});
                        }
/*
                        selected:function( event, ui ) {
                            var TmpId = ui.selected.id;
                            if (TmpId!="""" && TmpId!=""" + _ObjectNameHtml + @""")
                            {
                                if (Ara.GetObject(TmpId)!=null)
                                    Ara.Tick.Send(4, " + vTick.Session.AppId + @", 'Ara', 'Selected', {id:'" + this.InstanceID + @"',idselect:TmpId});
                            }                           
                        },
                        unselected:function( event, ui ) {
                            var TmpId = ui.unselected.id;
                            if (TmpId!="""" && TmpId!=""" + _ObjectNameHtml + @""")
                            {
                                if (Ara.GetObject(TmpId)!=null)
                                    Ara.Tick.Send(4, " + vTick.Session.AppId + @", 'Ara', 'UnSelected', {id:'" + this.InstanceID + @"',idselect:TmpId});
                            }                           
                        }
*/
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
                    Tick.GetTick().Script.Send("$('#" + _ObjectNameHtml + "').selectable( 'enable' );");
                else
                    Tick.GetTick().Script.Send("$('#" + _ObjectNameHtml + "').selectable( 'disable' );");
            }
        }

        public enum Etolerance
        {
            fit,
            touch
        }

        private Etolerance _tolerance = Etolerance.touch;
        public Etolerance tolerance
        {
            get { return _tolerance; }
            set
            {
                _tolerance = value;

                Tick.GetTick().Script.Send("$('#" + _ObjectNameHtml + "').selectable( { tolerance: \"" + (_tolerance == Etolerance.fit ? "fit" : "touch") + "\" } );");
            }
        }

        public void AddObjectSelectable(IAraComponentVisual vObj)
        {
            vObj.CssAddClass("selectable_" + this.InstanceID);
        }

        public void RemoveObjectSelectable(IAraComponentVisual vObj)
        {
            vObj.CssRemoveClass("selectable_" + this.InstanceID);
        }

        public delegate void DOnSelect(IAraObject[] vObjects);
        public AraEvent<DOnSelect> Stop = new AraEvent<DOnSelect>();
    }
}
