// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using Ara2;

namespace Ara2.Components
{
	[Serializable]
    public class AraTimer : AraObjectClienteServer
    {
        public static bool ArquivosHdCarregado = false;

        public AraTimer(IAraObject ConteinerFather)
            : base(ConteinerFather, "AraTimer")
        {
            Construct();
        }

        private void Construct()
        {
            tick = new AraComponentEvent<Action>(this, "tick", EAraComponentEventTypeThread.ThreadMulti);
            EventInternal += AraTimer_EventInternal;
        }

        public override void LoadJS()
        {
            Tick vTick = Tick.GetTick();
            vTick.Session.AddJs("Ara2/Components/AraTimer/AraTimer.js");
        }

        public void AraTimer_EventInternal(String vFunction)
        {
            switch (vFunction)
            {
                case "tick":
				{
                    lock (this)
                    {
                        if (_Enabled == true && tick.InvokeEvent != null)
                        {
                            try
                            {
                                tick.InvokeEvent();
                            }
                            catch (Exception err)
                            {
                                throw err;
                            }
                            finally
                            {
                                this.TickScriptCall();
                                Tick.GetTick().Script.Send(" if (vObj) vObj.TickEnd(); \n");
                            }
                        }
                    }
                    break;
				}
            }
        }

        #region Eventos
            public AraComponentEvent<Action> tick;
        #endregion

        private int _Interval = 0;
        public int Interval
        {
            set
            {
                if (_Interval != value)
                {
                    _Interval = value;
                    this.TickScriptCall();
                    Tick.GetTick().Script.Send(" vObj.SetInterval(" + _Interval + "); \n");
                }
            }
            get { return _Interval; }
        }


        private bool _Enabled = false;
        public bool Enabled
        {
            get { return _Enabled; }
            set
            {
                lock (this)
                {
                    if (_Interval > 0 || _Enabled == false)
                    {
                        _Enabled = value;
                        this.TickScriptCall();
                        Tick.GetTick().Script.Send(" vObj.SetEnabled(" + (_Enabled == true ? "true" : "false") + "); \n");
                    }
                    else
                        throw new Exception("interval > 0 !");
                }
            }
        }
		
		/// <summary>
		/// The time out in second
		/// </summary>
		private int _Timeout=15;
		public int Timeout
        {
            set
            {
                if (_Timeout != value)
                {
                    _Timeout = value;
                    this.TickScriptCall();
                    Tick.GetTick().Script.Send(" vObj.SetTimeout(" + _Timeout + "); \n");
                }
            }
            get { return _Timeout; }
        }
    }
}
