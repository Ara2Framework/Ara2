// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Reflection;
using Ara2.Dev;

namespace Ara2.Components
{  
    [Serializable]
    [AraDevComponent(vConteiner:false,vResizable:true,vDraggable:true)]
    public class AraButton : AraComponentVisualAnchorConteiner, IAraDev
    {
        public object Tag;

        public AraButton(IAraObject ConteinerFather)
            : this(AraObjectClienteServer.Create(ConteinerFather, "a", new Dictionary<string, string> { {"href","#"}}), ConteinerFather)
        {
        }

        public AraButton(IAraObject ConteinerFather,string vHref)
            : this(AraObjectClienteServer.Create(ConteinerFather, "a", new Dictionary<string, string> { { "href", vHref } }), ConteinerFather)
        {

        }

        public AraButton(IAraObject ConteinerFather, string vHref, string vText, ButtonIco vIco)
            : this(AraObjectClienteServer.Create(ConteinerFather, "a", new Dictionary<string, string> { { "href", vHref } }), ConteinerFather)
        {
            Text = vText;
            Ico = vIco;
            Construct();
        }

        public AraButton(string vId, IAraObject ConteinerFather)
            : base(vId, ConteinerFather, "AraButton")
        {
            Construct();
        }

        public AraButton(IAraObject ConteinerFather, ButtonIco vIco)
            : this(ConteinerFather)
        {
            Ico = vIco;
            Construct();
        }

        public AraButton(IAraObject ConteinerFather, string vText, ButtonIco vIco)
            : this(ConteinerFather,"#", vText, vIco)
        {
  
        }

        private void Construct()
        {
            Click = new AraComponentEvent<EventHandler>(this,"Click");
            this.EventInternal += AraButton_EventInternal;

            this._MinWidth = 10;
            this._MinHeight = 25;
            this._Width = 90;
            this._Height = 25;
            this._Text = " ";
        }

        public override void LoadJS()
        {
            Tick vTick = Tick.GetTick();
            vTick.Session.AddJs("Ara2/Components/AraButton/AraButton.js");
        }


        public void AraButton_EventInternal(String vFunction)
        {
            switch (vFunction.ToUpper())
            {
                case "CLICK":
                    if (Enabled)
                        Click.InvokeEvent(this, new EventArgs());
                break;
            }
        }

        #region Eventos
        [AraDevEvent]
        public AraComponentEvent<EventHandler> Click { get; set; }
        #endregion



        private string _Text=" ";
        [AraDevProperty(" ")]
        [PropertySupportLayout]
        public string Text
        {
            set
            {
                _Text = value;

                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetValue(" + (_Text ==null?"null":"'" + AraTools.StringToStringJS(_Text) + "'") + "); \n");
            }
            get { return _Text; }
        }



        private int Border = 5;

        private bool _Enabled = true;
        [AraDevProperty(true)]
        public bool Enabled
        {
            get { return _Enabled; }
            set
            {
                _Enabled = value;
                
                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetEnabled(" + (_Enabled == true ? "true" : "false") + "); \n");
            }
        }

        public enum ButtonIco
        {
            None,
            carat_1_n,
            carat_1_ne,
            carat_1_e,
            carat_1_se,
            carat_1_s,
            carat_1_sw,
            carat_1_w,
            carat_1_nw,
            carat_2_n_s,
            carat_2_e_w,
            triangle_1_n,
            triangle_1_ne,
            triangle_1_e,
            triangle_1_se,
            triangle_1_s,
            triangle_1_sw,
            triangle_1_w,
            triangle_1_nw,
            triangle_2_n_s,
            triangle_2_e_w,
            arrow_1_n,
            arrow_1_ne,
            arrow_1_e,
            arrow_1_se,
            arrow_1_s,
            arrow_1_sw,
            arrow_1_w,
            arrow_1_nw,
            arrow_2_n_s,
            arrow_2_ne_sw,
            arrow_2_e_w,
            arrow_2_se_nw,
            arrowstop_1_n,
            arrowstop_1_e,
            arrowstop_1_s,
            arrowstop_1_w,
            arrowthick_1_n,
            arrowthick_1_ne,
            arrowthick_1_e,
            arrowthick_1_se,
            arrowthick_1_s,
            arrowthick_1_sw,
            arrowthick_1_w,
            arrowthick_1_nw,
            arrowthick_2_n_s,
            arrowthick_2_ne_sw,
            arrowthick_2_e_w,
            arrowthick_2_se_nw,
            arrowthickstop_1_n,
            arrowthickstop_1_e,
            arrowthickstop_1_s,
            arrowthickstop_1_w,
            arrowreturnthick_1_w,
            arrowreturnthick_1_n,
            arrowreturnthick_1_e,
            arrowreturnthick_1_s,
            arrowreturn_1_w,
            arrowreturn_1_n,
            arrowreturn_1_e,
            arrowreturn_1_s,
            arrowrefresh_1_w,
            arrowrefresh_1_n,
            arrowrefresh_1_e,
            arrowrefresh_1_s,
            arrow_4,
            arrow_4_diag,
            extlink,
            newwin,
            refresh,
            shuffle,
            transfer_e_w,
            transferthick_e_w,
            folder_collapsed,
            folder_open,
            document,
            document_b,
            note,
            mail_closed,
            mail_open,
            suitcase,
            comment,
            person,
            print,
            trash,
            locked,
            unlocked,
            bookmark,
            tag,
            home,
            flag,
            calendar,
            cart,
            pencil,
            clock,
            disk,
            calculator,
            zoomin,
            zoomout,
            search,
            wrench,
            gear,
            heart,
            star,
            link,
            cancel,
            plus,
            plusthick,
            minus,
            minusthick,
            close,
            closethick,
            key,
            lightbulb,
            scissors,
            clipboard,
            copy,
            contact,
            image,
            video,
            script,
            alert,
            info,
            notice,
            help,
            check,
            bullet,
            radio_off,
            radio_on,
            pin_w,
            pin_s,
            play,
            pause,
            seek_next,
            seek_prev,
            seek_end,
            seek_start,
            seek_first,
            stop,
            eject,
            volume_off,
            volume_on,
            power,
            signal_diag,
            signal,
            battery_0,
            battery_1,
            battery_2,
            battery_3,
            circle_plus,
            circle_minus,
            circle_close,
            circle_triangle_e,
            circle_triangle_s,
            circle_triangle_w,
            circle_triangle_n,
            circle_arrow_e,
            circle_arrow_s,
            circle_arrow_w,
            circle_arrow_n,
            circle_zoomin,
            circle_zoomout,
            circle_check,
            circlesmall_plus,
            circlesmall_minus,
            circlesmall_close,
            squaresmall_plus,
            squaresmall_minus,
            squaresmall_close,
            grip_dotted_vertical,
            grip_dotted_horizontal,
            grip_solid_vertical,
            grip_solid_horizontal,
            gripsmall_diagonal_se,
            grip_diagonal_se
        }
        
        private ButtonIco _Ico = ButtonIco.None;
        [AraDevProperty(ButtonIco.None)]
        [PropertySupportLayout]
        public ButtonIco Ico 
        {
            get
            {
                return _Ico;
            }
            set
            {
                _Ico = value;

                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetIcoPrimary(" + (_Ico == ButtonIco.None ? "false" : "'ui-icon-" + (_Ico.ToString().Replace("_", "-")) + "'") + "); \n");
            }
        }

        private ButtonIco _IcoSecondary = ButtonIco.None;
        [AraDevProperty(ButtonIco.None)]
        [PropertySupportLayout]
        public ButtonIco IcoSecondary
        {
            get
            {
                return _IcoSecondary;
            }
            set
            {
                _IcoSecondary = value;

                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetIcoSecondary(" + (_IcoSecondary == ButtonIco.None ? "false" : "'ui-icon-" + (_IcoSecondary.ToString().Replace("_", "-")) + "'") + "); \n");
            }
        }



        #region Ara2Dev
        private string _Name = "";
        [AraDevProperty("")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private AraEvent<DStartEditPropertys> _StartEditPropertys = null;
        public AraEvent<DStartEditPropertys> StartEditPropertys
        {
            get
            {
                if (_StartEditPropertys == null)
                {
                    _StartEditPropertys = new AraEvent<DStartEditPropertys>();
                    this.Click += this_ClickEdit;
                }

                return _StartEditPropertys;
            }
            set
            {
                _StartEditPropertys = value;
            }
        }
        private void this_ClickEdit(object sender, EventArgs e)
        {
            if (_StartEditPropertys.InvokeEvent != null)
                _StartEditPropertys.InvokeEvent(this);
        }
        
        private AraEvent<DStartEditPropertys> _ChangeProperty = new AraEvent<DStartEditPropertys>();
        public AraEvent<DStartEditPropertys> ChangeProperty
        {
            get
            {
                return _ChangeProperty;
            }
            set
            {
                _ChangeProperty = value;
            }
        }
        #endregion


    }
}
