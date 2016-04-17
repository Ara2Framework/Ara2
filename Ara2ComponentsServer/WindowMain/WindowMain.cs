// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Ara2.Dev;
using Ara2.Keyboard;
using System.Collections.Specialized;
using System.Web;
using Ara2.Log;

namespace Ara2.Components
{
    [Serializable]
    [AraDevComponent(vBase:true, vDisplayToolBar:false)]
    public abstract class WindowMain : AraComponentVisualAnchorConteiner, IAraWindowMain
    {
        public WindowMain() :
            this(Tick.GetTick().Session)
        {

        }

        public WindowMain(Session vSession) :
            base(AraObjectClienteServer.Create(null, "div"), null, "AraWindowMain")
        {
            #region Set Eventos
            Click = new AraComponentEvent<EventHandler>(this, "Click");
            ResizeStop = new AraComponentEvent<Action>(this, "ResizeStop");

            KeyDown = new AraComponentEventKey<DArakeyboard>(this, "KeyDown");
            KeyUp = new AraComponentEventKey<DArakeyboard>(this, "KeyUp");
            KeyPress = new AraComponentEventKey<DArakeyboard>(this, "KeyPress");

            PopState = new AraComponentEvent<DPopState>(this, "PopState");
            Menssage = new AraComponentEvent<DMenssage>(this, "Menssage");

            Unload = new AraComponentEvent<Action>(this, "unload");
            Unload += WindowMain_Unload;

            ChangelocationHash = new AraEvent<Action>();
            Active = new AraEvent<Action>();

            this.EventInternal += WindowMain_EventInternal;
            this.SetProperty += WindowMain_SetProperty;
            #endregion

            vSession.AddObject(this);
        }

        public sealed override void LoadJS()
        {
            Tick vTick = Tick.GetTick();           
            vTick.Session.AddJs("Ara2/Components/WindowMain/WindowMain.js");   
        }

        private void WindowMain_EventInternal(string vFunction)
        {
            Tick vTick = Tick.GetTick();
            switch (vFunction.ToUpper())
            {
                case "RESIZESTOP":
                    if (ResizeStop.InvokeEvent != null)
                        ResizeStop.InvokeEvent();
                    break;
                case "UNLOAD":
                    if (Unload.InvokeEvent != null)
                        Unload.InvokeEvent();
                    break;
                case "CLICK":
                    if (Click.InvokeEvent != null)
                        Click.InvokeEvent(this, new EventArgs());
                    break;
                case "KEYDOWN":
                    if (KeyDown.InvokeEvent != null)
                        KeyDown.InvokeEvent(this, new AraKeyboard());
                    break;
                case "KEYUP":
                    if (KeyUp.InvokeEvent != null)
                        KeyUp.InvokeEvent(this, new AraKeyboard());
                    break;
                case "KEYPRESS":
                    if (KeyPress.InvokeEvent != null)
                        KeyPress.InvokeEvent(this, new AraKeyboard());
                    break;
                case "GETCOOKIERETURN":
                    int IdGetCookieReturn = Convert.ToInt32(vTick.Page.Request["id"]);
                    try
                    {
                        GetCookieReturn[IdGetCookieReturn].InvokeEvent(vTick.Page.Request["value"]);
                    }
                    finally
                    {
                        GetCookieReturn.Remove(IdGetCookieReturn);
                    }
                    break;
                case "POPSTATE":
                    if (PopState.InvokeEvent != null)
                    {
                        lock (this)
                        {
                            ExeEventPopState = true;
                            try
                            {
                                Uri vUrl = new Uri(vTick.Page.Request["URL"]);
                                PopState.InvokeEvent(new Uri(vTick.Page.Request["URL"]), HttpUtility.ParseQueryString(vUrl.Query));
                            }
                            finally
                            {
                                ExeEventPopState = false;
                            }
                        }
                    }
                    break;
                case "MENSSAGE":
                    {
                        if (Menssage.InvokeEvent != null)
                        { 
                            try
                            {
                                Menssage.InvokeEvent(vTick.Page.Request["vMenssage"].ToString());
                            }
                            catch(Exception err)
                            {
                                throw new Exception("Erro on event 'Menssage' data '" + vTick.Page.Request["vMenssage"].ToString() + "'.\n" + err.Message);
                            }
                        }
                    }
                    break;
                case "ALERTYESORNO":
                    //resul
                    bool vResul;

                    if ((string)vTick.Page.Request.Params["resul"] == "true")
                        vResul = true;
                    else
                        vResul = false;

                    RunEventAlertYesOrNo(Convert.ToInt32(vTick.Page.Request.Params["key"]), vResul);

                    break;
                case "ASYNCHRONOUSFUNCTION":
                    RunAsynchronousFunction(Convert.ToInt32(vTick.Page.Request.Params["key"]));
                    break;
                case "ALERT":
                    RunEventAlert(Convert.ToInt32(vTick.Page.Request.Params["key"]));
                    break;
                case "ALERTGETSTRING":
                    RunEventAlertGetString(Convert.ToInt32(vTick.Page.Request.Params["key"]), vTick.Page.Request.Params["resul"].ToString());

                    break;
            }
        }

        private void WindowMain_SetProperty(string vName,dynamic vValue)
        { 
           
        }

        private void WindowMain_Unload()
        {
            try
            {
                var vTick = Tick.GetTick();
                vTick.AraPageMain.MemoryArea.CloseSession(vTick.Session.Id);
            }
            catch { }
        }

        #region History
        public void SetHistoryReplaceState(string vValue)
        {
            this.TickScriptCall();
            Tick.GetTick().Script.Send(" try { window.history.replaceState(null, \"Title\",'" + AraTools.StringToStringJS(vValue) + "'); } catch (err) { } \n");
        }
        bool ExeEventPopState = false;

        public void SetHistoryPushState(string vValue)
        {
            if (!ExeEventPopState)
            {
                this.TickScriptCall();
                Tick.GetTick().Script.Send(" try { window.history.pushState(null, \"Title\",'" + AraTools.StringToStringJS(vValue) + "'); } catch (err) { } \n");
            }
        }
        #endregion

        public virtual string GetBodyHtml()
        {
            return null;
        }

        public void Show()
        {
            try
            {
                if (Active.InvokeEvent!=null)
                    Active.InvokeEvent();
            }
            catch (Exception err)
            {
				//throw err;
                throw new Exception("Erro on Active WindowMain\n" + err.Message);
            }
        }

        #region WaitLoading
        Dictionary<int, AraEvent<Action>> _ActionWaitLoading = new Dictionary<int, AraEvent<Action>>();
        int NewActionWaitLoading = 1;

        public void WaitLoading(string vMessage, Action vAction)
        {
            AraEvent<Action> Event = new AraEvent<Action>();
            Event += vAction;
            int vKey = NewActionWaitLoading;
            NewActionWaitLoading++;

            _ActionWaitLoading.Add(vKey, Event);
            
            
            Tick vTick = Tick.GetTick();
            vTick.Script.Send(" Ara.Tools.WarningLoading(" + vTick.Session.AppId + "," + vKey + ",'" + AraTools.StringToStringJS(vMessage) + "'); \n");
        }

        public void RumActionWaitLoading(int vKey)
        {
            try
            {
				Action vTmpAction = _ActionWaitLoading[vKey].InvokeEvent;
			
                if (vTmpAction != null)
                    vTmpAction();

            }
            catch (Exception err)
            {
                throw new Exception("Erro no WaitLoading.\n Erro: " + err.Message);
            }
            finally
            {
                Tick.GetTick().Script.Send(" Ara.Tools.WarningLoading_End(); \n");
                _ActionWaitLoading.Remove(vKey);
            }

        }
        #endregion


        #region Eventos
        [AraDevEvent]
        public AraComponentEvent<Action> ResizeStop { get; set; }
        [AraDevEvent]
        public AraComponentEvent<Action> Unload { get; set; }

        [AraDevEvent]
        public AraComponentEvent<EventHandler> Click { get; set; }

        [AraDevEvent]
        public AraComponentEventKey<DArakeyboard> KeyDown { get; set; }
        [AraDevEvent]
        public AraComponentEventKey<DArakeyboard> KeyUp { get; set; }
        [AraDevEvent]
        public AraComponentEventKey<DArakeyboard> KeyPress { get; set; }

        
        [AraDevEvent]
        public AraComponentEvent<DPopState> PopState { get; set; }

        [AraDevEvent]
        public AraEvent<Action> ChangelocationHash { get; set; }

        [AraDevEvent]
        public AraEvent<Action> Active { get; set; }

        [AraDevEvent]
        public AraComponentEvent<DMenssage> Menssage { get; set; }
        #endregion

        #region Cookie
        public void SetCookie(string vName, string vValue, int? ExpiresDays = null, string path = null)
        {
            Tick vTick = Tick.GetTick();
            Dictionary<string, string> vOpts = new Dictionary<string, string>();
            if (ExpiresDays != null)
                vOpts.Add("expires", ExpiresDays.ToString());

            if (path != null)
                vOpts.Add("path", "'" + AraTools.StringToStringJS(path.ToString()) + "'");

            vTick.Script.Send(" $.cookie('" + AraTools.StringToStringJS(vName) + "','" + AraTools.StringToStringJS(vValue) + "'" + (vOpts.Any() ? ",{" + string.Join(",", vOpts.Select(a => a.Key + ":" + a.Value)) + "}" : "") + "); \n");
        }

        

        public int NGetCookieReturn = 1;
        public  Dictionary<int,AraEvent<DGetCookie>> GetCookieReturn = new Dictionary<int,AraEvent<DGetCookie>>();

        public void GetCookie(string vName, DGetCookie vEventReturn)
        {
            int AtualNGetCookieReturn = NGetCookieReturn;
            NGetCookieReturn++;

            try
            {
                AraEvent<DGetCookie> vTmpE = new AraEvent<DGetCookie>();
                vTmpE += vEventReturn;
                GetCookieReturn.Add(AtualNGetCookieReturn, vTmpE);

                Tick vTick = Tick.GetTick();
                vTick.Script.CallObject(this);
                vTick.Script.Send(" vObj.GetCookieReturn(" + AtualNGetCookieReturn + ",'" + AraTools.StringToStringJS(vName) + "'); \n");
            }
            catch { }
        }

        public void DelCookie(string vName, string path = null)
        {
            Tick vTick = Tick.GetTick();
            vTick.Script.Send(" $.removeCookie('" + vName + "'" + (path != null ? ",{ path:'" + AraTools.StringToStringJS(path) + "' }" : "") + "); \n");
        }
        #endregion


        #region Alert

        #region Alert Simples
        public void Alert(string vM)
        {
            Tick vTick = Tick.GetTick();
            vTick.Script.Send(" alert('" + AraTools.StringToStringJS(vM) + "'); ");
        }

        private List<CAlertReturn> AlertReturn = new List<CAlertReturn>();
        private int CAlertReturn_New = 1;

        [Serializable]
        private class CAlertReturn
        {
            public int Key;
            public AraEvent<Delegate> Event;
            public object[] Parametros;
            public DateTime Date;

            public void InvokeEvent()
            {
                Event.InvokeEvent.DynamicInvoke(Parametros);
            }
        }

        public void Alert<T>(string vM, T Retorno, params object[] vParametros)
        {
            Delegate vRetorno = (Delegate)(object)Retorno;

            int TmpKey = CAlertReturn_New;
            CAlertReturn_New++;

            AraEvent<Delegate> Event = new AraEvent<Delegate>();
            Event += vRetorno;

            AlertReturn.Add(new CAlertReturn()
            {
                Key = TmpKey,
                Event = Event,
                Parametros = vParametros,
                Date = DateTime.Now
            });

            Tick vTick = Tick.GetTick();
            vTick.Script.Send(" alert('" + AraTools.StringToStringJS(vM) + "'); \n");
            vTick.Script.Send(" Ara.Tick.Send(4, " + vTick.Session.AppId + ",'" + this.InstanceID + "', 'Alert', { key: " + TmpKey + "});");
        }

        public void RunEventAlert(int vKey)
        {
            try
            {
                AlertReturn
                    .Where(a => a.Key == vKey)
                    .First()
                    .InvokeEvent();
            }
            finally
            {
                AlertReturn.RemoveAll(a => a.Key == vKey);
            }

        }
        #endregion

        #region AlertYesOrNo
        private List<CAlertYesOrNo> AlertYesOrNoReturn = new List<CAlertYesOrNo>();
        private int CAlertYesOrNo_New = 1;
        [Serializable]
        private class CAlertYesOrNo
        {
            public int Key;
            public AraEvent<Delegate> Event;
            public object[] Parametros;
            public DateTime Date;

            public void InvokeEvent(bool vResult)
            {
                List<object> vTmp = new List<object>() { vResult };
                vTmp.AddRange(Parametros);

                Event.InvokeEvent.DynamicInvoke(vTmp.ToArray());
            }
        }

        public void AlertYesOrNo<T>(string vM, T EventReturn, params object[] vParametros)
        {
            Delegate vRetorno = (Delegate)(object)EventReturn;
            if (!vRetorno.Method.GetParameters()[0].ParameterType.Equals(typeof(bool)))
                throw new Exception("first parameter is not bool.");

            int TmpKey = CAlertYesOrNo_New;
            CAlertYesOrNo_New++;

            AraEvent<Delegate> Event = new AraEvent<Delegate>();
            Event += vRetorno;

            AlertYesOrNoReturn.Add(new CAlertYesOrNo()
            {
                Key = TmpKey,
                Event = Event,
                Parametros = vParametros,
                Date = DateTime.Now
            });

            Tick vTick = Tick.GetTick();
            vTick.Script.Send(" var vAlertResu = confirm('" + AraTools.StringToStringJS(vM) + "');");
            vTick.Script.Send(" setTimeout(function(){ Ara.Tick.Send(4, " + vTick.Session.AppId + ", '" + this.InstanceID + "', 'AlertYesOrNo', { key: " + TmpKey + ",resul:vAlertResu});},1000);");
        }

        private void RunEventAlertYesOrNo(int vKey, bool vResul)
        {
            try
            {
                AlertYesOrNoReturn
                    .Where(a => a.Key == vKey)
                    .First()
                    .InvokeEvent(vResul);               
            }
            finally
            {
                AlertYesOrNoReturn.RemoveAll(a => a.Key == vKey);
            }
        }
        #endregion

        #region  AsynchronousFunction
        private List<CAsynchronousFunction> AsynchronousFunctionReturn = new List<CAsynchronousFunction>();
        private int AsynchronousFunctionReturn_count = 1;
        [Serializable]
        private class CAsynchronousFunction
        {
            public int Key;
            public AraEvent<Delegate> Event;
            public object[] Parametros;
            public DateTime Date;

            public void InvokeEvent()
            {
                Event.InvokeEvent.DynamicInvoke(Parametros);
            }
        }

        public void AsynchronousFunction(Action EventReturn)
        {
            AsynchronousFunction(EventReturn);
        }

        public void AsynchronousFunction<T>(T vEventReturn, params object[] vParametros) 
        {
            Delegate EventReturn = (Delegate)(object)vEventReturn;

            int TmpKey = AsynchronousFunctionReturn_count;
            AsynchronousFunctionReturn_count++;

            AraEvent<Delegate> Event = new AraEvent<Delegate>();
            Event += EventReturn;

            AsynchronousFunctionReturn.Add(new CAsynchronousFunction()
            {
                Key = TmpKey,
                Event = Event,
                Parametros = vParametros,
                Date = DateTime.Now
            });

            Tick vTick = Tick.GetTick();
            vTick.Script.Send(" Ara.Tick.Send(1, " + vTick.Session.AppId + ", '" + this.InstanceID + "', 'AsynchronousFunction', { key: " + TmpKey + " });");
        }

        

        private void RunAsynchronousFunction(int vKey)
        {
            try
            {
                var Event = AsynchronousFunctionReturn.Where(a => a.Key == vKey).First();
                Event.InvokeEvent();
            }
            catch (Exception err)
            {
                throw new Exception("Error on RunAsynchronousFunction.\n" + err.Message);
            }
            finally
            {
                AsynchronousFunctionReturn.RemoveAll(a => a.Key == vKey);
            }

        }
        #endregion

        #region GetString
        private List<CAlertGetString> AlertGetStringReturn = new List<CAlertGetString>();
        private int AlertGetString_New = 1;
        [Serializable]
        private class CAlertGetString
        {
            public int Key;
            public AraEvent<Delegate> Event;
            public object[] Parametros;
            public DateTime Date;

            public void InvokeEvent(string vResult)
            {
                List<object> vTmp = new List<object>() { vResult };
                vTmp.AddRange(Parametros);

                Event.InvokeEvent.DynamicInvoke(vTmp.ToArray());
            }
        }


        public void AlertGetString<T>(string vM, string vValuedefault, T EventReturn, params object[] vParametros)
        {
            Delegate vRetorno = (Delegate)(object)EventReturn;
            if (!vRetorno.Method.GetParameters()[0].ParameterType.Equals(typeof(string)))
                throw new Exception("first parameter is not string.");

            int TmpKey = AlertGetString_New;
            AlertGetString_New++;

            AraEvent<Delegate> Event = new AraEvent<Delegate>();
            Event += vRetorno;

            AlertGetStringReturn.Add(new CAlertGetString()
            {
                Key = TmpKey,
                Event = Event,
                Parametros = vParametros,
                Date = DateTime.Now
            });

            Tick vTick = Tick.GetTick();
            vTick.Script.Send(" Ara.Tick.Send(4, " + vTick.Session.AppId + ", '" + this.InstanceID + "', 'AlertGetString', { key: " + TmpKey + ",resul:prompt('" + AraTools.StringToStringJS(vM) + "','" + AraTools.StringToStringJS(vValuedefault) + "')});");
        }

        private void RunEventAlertGetString(int vKey, string vResul)
        {
            try
            {
                vResul = vResul == "null" ? "" : vResul;
                AlertGetStringReturn
                    .Where(a => a.Key == vKey)
                    .First()
                    .InvokeEvent(vResul);
            }
            finally
            {
                AlertYesOrNoReturn.RemoveAll(a => a.Key == vKey);
            }

        }
        #endregion

        #endregion

        public static WindowMain GetInstance()
        {
            return Tick.GetTick().Session.WindowMain;
        }


        #region ShowDiv
        List<AraObjectInstance<IDivModal>> DivsModal = new List<AraObjectInstance<IDivModal>>();
        List<AraObjectInstance<IDivModal>> DivsModalHide = new List<AraObjectInstance<IDivModal>>();

        AraObjectInstance<IAraComponentVisualAnchor> _DivCanvas = new AraObjectInstance<IAraComponentVisualAnchor>();
        public IAraComponentVisualAnchor DivCanvas
        {
            get { return _DivCanvas.Object; }
            set { _DivCanvas.Object = value; }
        }

        private decimal? GetDivCanvasBottom()
        {
            ITaskbar Taskbar = (ITaskbar)this.Childs.Where(a => a is ITaskbar).FirstOrDefault();
            if (Taskbar != null)
                return (Taskbar.Visible ? (Taskbar.Height.Value + 5) : 0);
            else
                return 0;
        }

        public void ShowDiv(IAraComponentVisualAnchor vForm)
        {
            if (DivCanvas != null)
            {
                DivCanvas.Dispose();
                DivCanvas = null;
            }

            DivCanvas = vForm;
            DivCanvas.Anchor.Left = 0;
            DivCanvas.Anchor.Right = 0;
            DivCanvas.Anchor.Top = 0;
            DivCanvas.Anchor.Bottom = GetDivCanvasBottom();
            DivCanvas.Visible = true;
            this.MinWidth = DivCanvas.MinWidth;
            this.MinHeight = DivCanvas.MinHeight;
        }

        public void ShowDivModal(IDivModal vForm)
        {
            DivsModal.ForEach((a) => { if (a.Object.Visible) a.Object.Visible = false; });

            if (!DivsModalHide.Exists(a => a.InstanceID == vForm.InstanceID))
            {
                if (vForm.ConteinerFather.InstanceID != this.InstanceID)
                    vForm.ConteinerFather = this;
                vForm.Anchor.Left = 0;
                vForm.Anchor.Right = 0;
                vForm.Anchor.Top = 0;
                vForm.Anchor.Bottom = GetDivCanvasBottom();
                vForm.Visible = true;
                vForm.ZIndex = 100;
                vForm.StyleContainer = true;

                DivsModal.Add(new AraObjectInstance<IDivModal>()
                {
                    Object = vForm
                });
            }
            else
            {
                DivsModalHide.RemoveAll(a => a.InstanceID == vForm.InstanceID);

                DivsModal.Add(new AraObjectInstance<IDivModal>()
                {
                    Object = vForm
                });
                vForm.Visible = true;
            }

            vForm.Unload = new AraEvent<Action<object>>();
            this.MinWidth = vForm.MinWidth;
            this.MinHeight = vForm.MinHeight;

            DivCanvas.Visible = false;
        }

        public void CloseDivModal(IDivModal vForm)
        {
            CloseDivModal(vForm, null);
        }

        public void CloseDivModal(IDivModal vForm, object vObjReturn)
        {
            string vID = vForm.InstanceID;
            if (vForm.Unload.InvokeEvent != null)
                vForm.Unload.InvokeEvent(vObjReturn);

            AraObjectInstance<IDivModal> InsConteiner = DivsModal.Where(a => a.Object.InstanceID == vID).First();
            InsConteiner.Object.Dispose();
            DivsModal.Remove(InsConteiner);

            if (DivsModal.Count == 0)
            {
                DivCanvas.Visible = true;
                this.MinWidth = DivCanvas.MinWidth;
                this.MinHeight = DivCanvas.MinHeight;
            }
            else
            {
                IDivModal vDivAtual = DivsModal[DivsModal.Count - 1].Object;
                vDivAtual.Visible = true;
                this.MinWidth = vDivAtual.MinWidth;
                this.MinHeight = vDivAtual.MinHeight;
            }
        }

        public void HideDivModal(IDivModal vForm)
        {
            HideDivModal(vForm, null);
        }

        public void HideDivModal(IDivModal vForm, object vObjReturn)
        {
            string vID = vForm.InstanceID;
            if (vForm.Unload.InvokeEvent != null)
                vForm.Unload.InvokeEvent(vObjReturn);

            DivsModalHide.Add(new AraObjectInstance<IDivModal>() { Object = vForm });
            DivsModal.RemoveAll(a => a.Object.InstanceID == vID);
            vForm.Visible = false;

            if (DivsModal.Count == 0)
            {
                DivCanvas.Visible = true;
                this.MinWidth = DivCanvas.MinWidth;
                this.MinHeight = DivCanvas.MinHeight;
            }
            else
            {
                IDivModal vDivAtual = DivsModal[DivsModal.Count - 1].Object;
                vDivAtual.Visible = true;
                this.MinWidth = vDivAtual.MinWidth;
                this.MinHeight = vDivAtual.MinHeight;
            }
        }
        #endregion

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

        private System.Collections.Hashtable AraDevEvents = new System.Collections.Hashtable();

        #endregion

    }
}
