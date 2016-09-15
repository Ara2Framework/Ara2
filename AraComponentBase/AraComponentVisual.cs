// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ara2.Dev;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Ara2.Components
{
    [Serializable]
    public abstract class AraComponentVisual : AraObjectClienteServer, IAraComponentVisual
    {
        protected AraComponentVisual():
            base()
        {

        }

        public AraComponentVisual(string vNameObject, IAraObject vConteinerFather, string vTypeNameJS) :
            base(vNameObject, vConteinerFather, vTypeNameJS)
        {
            _HeightChangeAfter = new AraComponentEvent<Action>(this, "HeightChangeAfter");
            _WidthChangeAfter = new AraComponentEvent<Action>(this, "WidthChangeAfter");

            if (vConteinerFather is AraComponentVisual)
                if (((AraComponentVisual)vConteinerFather).TypePosition == ETypePosition.Static && this.TypePosition == ETypePosition.Absolute)
                    this.TypePosition = ETypePosition.Static;
        }

        public virtual void EventInternal(String vFunction)
        {
            switch (vFunction.ToUpper())
            {
                case "HEIGHTCHANGEAFTER":
                    {
                        lock (this)
                        {
                            if (HeightChangeAfter.InvokeEvent != null)
                                HeightChangeAfter.InvokeEvent();
                        }
                    }
                    break;

                case "WIDTHCHANGEAFTER":
                    {
                        lock (this)
                        {
                            if (WidthChangeAfter.InvokeEvent != null)
                                WidthChangeAfter.InvokeEvent();
                        }
                    }
                    break;
                default:
                    base.EventInternal(vFunction);
                    break;
            }
        }

        public new virtual void SetProperty(string vProperty, dynamic vValeu)
        {
            switch (vProperty)
            {
                case "Left":
                    if (vValeu == null)
                        _Left = null;
                    else
                        _Left = new AraDistance(Convert.ToString(vValeu));
                    break;
                case "Top":
                    if (vValeu == null)
                        _Top = null;
                    else
                        _Top = new AraDistance(Convert.ToString(vValeu));
                    break;
                case "Width":
                    _Width = new AraDistance(Convert.ToString(vValeu));
                    //if (_Width != null && _Width.Value < MinWidth.Value)
                    //{
                    //    _Width.Value = MinWidth.Value;
                    //    Tick vTick = Tick.GetTick();
                    //    this.TickScriptCall();
                    //    vTick.Script.Send(" vObj.SetWidth(" + (_Width == null ? "null" : "'" + _Width + "'") + ",true);\n");
                    //    //vTick.Script.Send(" vObj.ControlVar.SetValueUtm('Width', vObj.Width );\n");
                    //}
                    break;
                case "Height":
                    _Height = new AraDistance(Convert.ToString(vValeu));
                    //if (_Height != null && _Height.Value < MinHeight.Value)
                    //{
                    //    _Height.Value = MinHeight.Value;
                    //    Tick vTick = Tick.GetTick();
                    //    this.TickScriptCall();
                    //    vTick.Script.Send(" vObj.SetHeight(" + (_Height == null ? "null" : "'" + _Height + "'") + ",true); \n");
                    //    //vTick.Script.Send(" vObj.ControlVar.SetValueUtm('Height', vObj.Height );\n");
                    //}
                    break;
                default:
                    {
                        base.SetProperty(vProperty, (object)vValeu);
                    }
                    break;
            }
        }

        private bool _Visible = true;

        [AraDevProperty(true)]
        [PropertySupportLayout]
        [JsonIgnore]
        public bool Visible
        {
            get { return _Visible; }
            set
            {
                _Visible = value;

                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetVisible(" + (_Visible ? "true" : "false") + ");\n");

                if (VisibleChange.InvokeEvent != null)
                    VisibleChange.InvokeEvent();
            }
        }

        [Browsable(false)]
        [JsonIgnore]
        public bool VisibleGlobal
        {
            get {
                if (_Visible == false)
                    return false;

                if (this.ConteinerFather == null)
                    return true;

                return ((AraComponentVisual)this.ConteinerFather).VisibleGlobal;
            }
        }

        [AraDevEvent]
        public AraEvent<Action> VisibleChange = new AraEvent<Action>();

        [AraDevEvent]
        public AraEvent<AraDistance.DChangeDistance> LeftChangeBefore=new AraEvent<AraDistance.DChangeDistance>();

        [AraDevEvent]
        public AraEvent<Action> LeftChangeAfter = new AraEvent<Action>();

        private AraDistance _Left = new AraDistance(0, AraDistance.EUnity.px);

        [AraDevProperty]
        [PropertySupportLayout]
        [JsonIgnore]
        public AraDistance Left
        {
            get { return _Left; }
            set
            {
                if (_Left != value)
                {
                    if (LeftChangeBefore.InvokeEvent != null)
                        LeftChangeBefore.InvokeEvent(value);
                    _Left = value;
                    if (_TypePosition == ETypePosition.Static)
                        _Left = null;

                    Tick vTick = Tick.GetTick();
                    this.TickScriptCall();
                    vTick.Script.Send(" vObj.SetLeft(" + (_Left == null ? "null" : "'" + _Left + "'") + "); \n");
                    vTick.Script.Send(" vObj.ControlVar.SetValueUtm('Left', vObj.Left );\n");

                    if (LeftChangeAfter.InvokeEvent != null)
                        LeftChangeAfter.InvokeEvent();
                }
            }
        }

        [AraDevEvent]
        public AraEvent<AraDistance.DChangeDistance> TopChangeBefore= new AraEvent<AraDistance.DChangeDistance>();

        [AraDevEvent]
        public AraEvent<Action> TopChangeAfter = new AraEvent<Action>();
        
        private AraDistance _Top = new AraDistance(0, AraDistance.EUnity.px);

        [AraDevProperty]
        [PropertySupportLayout]
        [JsonIgnore]
        public AraDistance Top
        {
            get { return _Top; }
            set
            {
                if (_Top != null)
                {
                    if (TopChangeBefore.InvokeEvent != null)
                        TopChangeBefore.InvokeEvent(value);
                    _Top = value;

                    if (_TypePosition == ETypePosition.Static)
                        _Left = null;

                    Tick vTick = Tick.GetTick();
                    this.TickScriptCall();
                    vTick.Script.Send(" vObj.SetTop(" + (_Top == null ? "null" : "'" + _Top + "'") + "); \n");
                    vTick.Script.Send(" vObj.ControlVar.SetValueUtm('Top', vObj.Top );\n");

                    if (TopChangeAfter.InvokeEvent != null)
                        TopChangeAfter.InvokeEvent();
                }
            }
        }

        protected AraDistance _MinWidth = null;
        [AraDevProperty(null)]
        [PropertySupportLayout]
        [JsonIgnore]
        public AraDistance MinWidth
        {
            get { return _MinWidth; }
            set
            {
                //if (this.TypePosition != ETypePosition.Static)
                //    _MinWidth = null;
                //else 
                if (value != null && value.Value != 0)
                    _MinWidth = value;
                else
                    _MinWidth = null;

                if (_MinWidth != null && Width != null && Width.Value != 0 && Width.Value < _MinWidth.Value)
                    Width = _MinWidth;

                Tick vTick = Tick.GetTick();
                this.TickScriptCall();
                vTick.Script.Send(" if (vObj.SetMinWidth) vObj.SetMinWidth(" + (_MinWidth == null ? "null" : "'" + _MinWidth + "'") + ");\n");
            }
        }

        protected AraDistance _MinHeight = null;
        [AraDevProperty(null)]
        [PropertySupportLayout]
        [JsonIgnore]
        public AraDistance MinHeight
        {
            get { return _MinHeight; }
            set
            {
                //if (this.TypePosition != ETypePosition.Static)
                //    _MinHeight = null;
                //else 
                if (value != null && value.Value != 0)
                    _MinHeight = value;
                else
                    _MinHeight = null;

                if (_MinHeight != null && Height != null && Height.Value != 0 && Height.Value < _MinHeight.Value)
                    Height = _MinHeight;

                Tick vTick = Tick.GetTick();
                this.TickScriptCall();
                vTick.Script.Send(" if (vObj.SetMinHeight) vObj.SetMinHeight(" + (_MinHeight == null ? "null" : "'" + _MinHeight + "'") + ");\n");
            }
        }

        
        public static Exception ExceptionOnlyPx = new Exception("this componenete only accepts px");

        private AraEvent<AraDistance.DChangeDistance> _WidthChangeBefore = new AraEvent<AraDistance.DChangeDistance>();

        [AraDevEvent]
        [JsonIgnore]
        public AraEvent<AraDistance.DChangeDistance> WidthChangeBefore
        {
            get { return _WidthChangeBefore; }
            set { _WidthChangeBefore = value; }
        }


        private AraComponentEvent<Action> _WidthChangeAfter;

        [AraDevEvent]
        [JsonIgnore]
        public AraComponentEvent<Action> WidthChangeAfter 
        {
            get { return _WidthChangeAfter; }
            set { _WidthChangeAfter = value; }
        }

        protected AraDistance _Width=new AraDistance(0, AraDistance.EUnity.px);
        
        [AraDevProperty(null)]
        [PropertySupportLayout]
        [JsonIgnore]
        public AraDistance Width
        {
            get { return _Width; }
            set
            {
                if (_Width != value)
                {
                    if (WidthChangeBefore.InvokeEvent != null)
                        WidthChangeBefore.InvokeEvent(value);

                    AraDistance valor = value;
                    if (valor != null && MinWidth !=null && valor.Value < MinWidth.Value)
                        valor = MinWidth;

                    _Width = valor;

                    if (WidthChangeAfter.InvokeEvent != null)
                        WidthChangeAfter.InvokeEvent();

                    Tick vTick = Tick.GetTick();
                    this.TickScriptCall();
                    vTick.Script.Send(" vObj.SetWidth(" + (_Width == null ? "null" : "'" + _Width + "'") + ",true);\n");
                    //vTick.Script.Send(" vObj.ControlVar.SetValueUtm('Width', vObj.Width );\n");

                    
                }
            }
        }


        private AraEvent<AraDistance.DChangeDistance> _HeightChangeBefore = new AraEvent<AraDistance.DChangeDistance>();

        [AraDevEvent]
        [JsonIgnore]
        public AraEvent<AraDistance.DChangeDistance> HeightChangeBefore
        {
            get { return _HeightChangeBefore; }
            set { _HeightChangeBefore = value; }
        }

        private AraComponentEvent<Action> _HeightChangeAfter;

        [AraDevEvent]
        [JsonIgnore]
        public AraComponentEvent<Action> HeightChangeAfter
        {
            get { return _HeightChangeAfter; }
            set { _HeightChangeAfter = value; }
        }

        protected AraDistance _Height = new AraDistance(0, AraDistance.EUnity.px);

        [AraDevProperty(null)]
        [PropertySupportLayout]
        [JsonIgnore]
        public AraDistance Height
        {
            get { return _Height; }
            set
            {
                if (_Height != value)
                {
                    if (HeightChangeBefore.InvokeEvent != null)
                        HeightChangeBefore.InvokeEvent(value);

                    AraDistance valor = value;
                    if (valor != null && MinHeight  != null && valor.Value < MinHeight.Value)
                        valor = MinHeight;

                    _Height = valor;

                    if (HeightChangeAfter.InvokeEvent != null)
                        HeightChangeAfter.InvokeEvent();

                    Tick vTick = Tick.GetTick();
                    this.TickScriptCall();
                    vTick.Script.Send(" vObj.SetHeight(" + (_Height == null ? "null" : "'" + _Height + "'") + ",true);\n");
                    //vTick.Script.Send(" vObj.ControlVar.SetValueUtm('Height', vObj.Height );\n");  
                }
            }
        }

        public abstract override void LoadJS();
        
        public enum ETypePosition
        {
            Static=1,
            Absolute=2,
            Relative=3
        }
        private static Dictionary<int, string> ETypePositionToString = new Dictionary<int, string>
        {
            {1,"static"},
            {2,"absolute"},
            {3,"relative"},
        };

        private ETypePosition _TypePosition = ETypePosition.Absolute;
        [AraDevProperty(ETypePosition.Absolute)]
        [PropertySupportLayout]
        [DefaultValue(ETypePosition.Absolute)]
        [Browsable(true)]
        [JsonIgnore]
        public ETypePosition TypePosition
        {
            get { return _TypePosition; }
            set
            {
                if (value == ETypePosition.Absolute)
                {
                    if (((AraComponentVisual)this.ConteinerFather).TypePosition == ETypePosition.Static)
                        throw new Exception("you can not change to absolute because his father is static container");
                }
                _TypePosition = value;

                if (_TypePosition == ETypePosition.Static)
                {
                    this.Left = null;
                    this.Top = null;
                    this.MinWidth = null;
                    this.MinHeight = null;
                    this.Width = null;
                    this.Height = null;
                }

                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetTypePosition('" + ETypePositionToString[((int)_TypePosition)] + "'); \n");
            }
        }

        private AraResizable _Resizable = null;
        [DefaultValue(null)]
        [JsonIgnore]
        public AraResizable Resizable
        {
            get { return _Resizable; }
            set
            {
                _Resizable = value;               
            }
        }

        private AraDraggable _Draggable = null;
        [DefaultValue(null)]
        [JsonIgnore]
        public AraDraggable Draggable
        {
            get { return _Draggable; }
            set
            {
                _Draggable = value;
            }
        }

        private AraSelectable _Selectable = null;
        [DefaultValue(null)]
        [JsonIgnore]
        public AraSelectable Selectable
        {
            get { return _Selectable; }
            set
            {
                _Selectable = value;
            }
        }

        private int? _ZIndex =null;

        [AraDevProperty(null)]
        [PropertySupportLayout]
        [MergableProperty(false)]
        [JsonIgnore]
        public int? ZIndex
        {
            get { return _ZIndex; }
            set
            {
                _ZIndex = value;

                this.TickScriptCall();
                Tick.GetTick().Script.Send(" $(vObj.Obj).zIndex(" + (ZIndex == null ? "null" : ((int)ZIndex).ToString()) + ");\n");
            }
        }

        

    }
}
