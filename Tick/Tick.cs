// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

namespace Ara2
{
    public class Tick : IDisposable
    {
        #region Ticks
        private static Dictionary<int,Tick> Ticks = new Dictionary<int,Tick>();

        public static Tick GetTick()
        {
            return GetTick(Thread.CurrentThread.ManagedThreadId);
        }

        public static Tick GetTick(int vThreadId)
        {
            try
            {
                Tick vTmpTick;
                if (Ticks.TryGetValue(vThreadId,out vTmpTick))
                    return vTmpTick;
                else
                    return null;
            }
            catch { return null; }
        }


        public static Tick AddTick(Tick vTick)
        {
            if (!Ticks.Keys.Contains(Thread.CurrentThread.ManagedThreadId))
                Ticks.Add(Thread.CurrentThread.ManagedThreadId,vTick);
            else
            {
                Ticks[Thread.CurrentThread.ManagedThreadId] = vTick;
            }
            return vTick;
        }

        public static void DellTick(Tick vTick)
        {
            int vThreadId = vTick.ThreadId;
            try
            {
                vTick.AraPageMain.OnEndTick(vTick);
            }
            catch { }

            try
            {
                vTick.Dispose();                
            }
            catch { }

            try
            {
                Ticks.Remove(vThreadId);
            }
            catch { }
        }

        #endregion

        public AraPageMain AraPageMain;
        public int ThreadId;
        public ISession Session;
        public System.Web.UI.Page Page;
        public Script Script;
        public bool AbsoluteEndOfTheResponses = false;
        public bool ReturnisJavascript = true;

        public Tick(ISession vSession, System.Web.UI.Page vPage, AraPageMain vAraPageMain)
        {
            AraPageMain = vAraPageMain;
            Session = vSession;
            ThreadId = Thread.CurrentThread.ManagedThreadId;
            Page = vPage;
            Script = new Script(this);

            _Id = Convert.ToInt32(Page.Request["TickId"]);
        }



        //public event Action OnLoad;
        //public event Action OnUnload;
        public object CustomTick;

        private int _Id;
        public int Id
        {
            get
            {
                return _Id;
            }
        }

        public void Dispose()
        {
            if (Id != -1)
                Script.Send(" Ara.Tick.DelTick(" + Id + ");\n");


            if (CustomTick != null)
            {
                if (CustomTick is IDisposable)
                    ((IDisposable)CustomTick).Dispose();
                CustomTick = null;
            }
        }

    }       
}
