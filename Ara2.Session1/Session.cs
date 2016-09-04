// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Ara2.Components;
using System.Threading;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Reflection;

namespace Ara2.Session1
{
    
    [Serializable]
    public class Session : ISession
    {
        public string Id { get; set; }
        int _AppId = 0;
        public int AppId
        {
            get { return _AppId; }
            set { _AppId = value; }
        }

        DateTime _LastCall = DateTime.Now;
        public DateTime LastCall
        {
            get { return _LastCall; }
            set { _LastCall = value; }
        }

        private bool _External=false;

        /// <summary>
        /// Usado para saber quando a sessão é de outro servidor e vc é só um aplicativo 
        /// </summary>
        public bool External
        {
            get { return _External; }
        }

        public AraEvent<Action> OnLoad = new AraEvent<Action>();
        public AraEvent<Action> OnUnload = new AraEvent<Action>();


        AraEvent<Action<IAraObjectClienteServer, string>> _OnReceivesInternalEventBefore = new AraEvent<Action<IAraObjectClienteServer, string>>();
        public AraEvent<Action<IAraObjectClienteServer, string>> OnReceivesInternalEventBefore
        {
            get
            {
                return _OnReceivesInternalEventBefore;
            }
            set
            {
                _OnReceivesInternalEventBefore = value;
            }
        }


        AraEvent<Action<IAraObjectClienteServer, string>> _OnReceivesInternalEventAfter = new AraEvent<Action<IAraObjectClienteServer, string>>();
        public AraEvent<Action<IAraObjectClienteServer, string>> OnReceivesInternalEventAfter
        {
            get
            {
                return _OnReceivesInternalEventAfter;
            }
            set
            {
                _OnReceivesInternalEventAfter = value;
            }
        }
        

        public Session(AraPageMain AraPageMain,string vId,int vAddId):
            this(AraPageMain,vId)
        {
            AppId = vAddId;
            _External = true;
            if (AraPageMain.Page.Request["YourUrlServer"] != null)
                _YourUrlServer = AraPageMain.Page.Request["YourUrlServer"];
            else
            {
                string MyUrl = AraPageMain.Page.Request.Url.ToString();

                if (MyUrl.IndexOf("?") > -1)
                    MyUrl = MyUrl.Substring(0, MyUrl.IndexOf("?"));

                _YourUrlServer = MyUrl;
            }

            
            
            
        }

        public Session(AraPageMain AraPageMain, string vId)
        {
            Id = vId;
        }

        public IAraWindowMain WindowMain
        {
            get
            {
                return (IAraWindowMain)(Objects.First().Value.Object);
            }
        }

        public void ExecuteLoad()
        {
            if (this.OnLoad.InvokeEvent != null)
                this.OnLoad.InvokeEvent();
        }

        private string _YourUrlServer = "";

        public string YourUrlServer
        {
            get
            {
                return _YourUrlServer;
            }
        }

        public void Dispose()
        {
            try
            {
                if (this.OnUnload.InvokeEvent!=null)
                    this.OnUnload.InvokeEvent();
            }
            catch { }

            Aplications.Dispose();
            _Objects.Clear();
        }

        #region Aplication
        AraObjectInstance<SessionAplications> _Aplications = new AraObjectInstance<SessionAplications>();
        public SessionAplications Aplications
        {
            get
            {
                if (_Aplications.Object==null)
                    _Aplications.Object = new SessionAplications(this);

                return _Aplications.Object;
            }
        }
        #endregion

        #region JavaScript
        AraObjectInstance<SessionJSs> _SessionJSs = new AraObjectInstance<SessionJSs>();
        public SessionJSs JSs
        {
            get
            {
                if (_SessionJSs.Object ==null)
                    _SessionJSs.Object = new SessionJSs(this);

                return _SessionJSs.Object;
            }
        }

        public void AddJs(string vJs)
        {
            JSs.AddJs(vJs);
        }
        #endregion

        #region CSS
        AraObjectInstance<SessionCsss> _SessionCsss = new AraObjectInstance<SessionCsss>();
        public SessionCsss Csss
        {
            get
            {
                if (_SessionCsss.Object==null)
                    _SessionCsss.Object = new SessionCsss(this);
                
                return _SessionCsss.Object;
            }
        }

        public void AddCss(string vJs)
        {
            Csss.AddCss(vJs);
        }
        #endregion


        #region Objects

        public string GetNewID()
        {
            return "A" + this.AppId + "O" + Tick.GetTick().AraPageMain.MemoryArea.GetNewIdObject(this);
        }

        [NonSerialized]
        Dictionary<string,ISessionObject> _Objects = null;

        public Dictionary<string, ISessionObject> Objects
        {
            get
            {
                if (_Objects==null)
                    _Objects = Tick.GetTick().AraPageMain.MemoryArea.GetObjects(this).ToDictionary(a=>a.InstanceID,a=>a);

                return _Objects;
            }
        }

        public List<ISessionObject> GetObjectsByType(Type vType)
        {
            return _Objects.Values.Where(a=> a.Type == vType || a.Type.GetInterface(vType.FullName)!=null).ToList();
        }

        public void AddObject(IAraWindowMain WindowMain)
        {
            if (!Objects.ContainsKey(WindowMain.InstanceID))
                Objects.Add(WindowMain.InstanceID, new SessionObject(this, WindowMain));
            else
                Objects[WindowMain.InstanceID] =  new SessionObject(this, WindowMain);
        }

        public void AddObject(IAraObject vObj, IAraObject vConteinerFather)
        {
            if (!Objects.ContainsKey(vObj.InstanceID))
                Objects.Add(vObj.InstanceID, new SessionObject(this, vObj));
            else
                Objects[vObj.InstanceID] = new SessionObject(this, vObj);

        }

        public void AddObject(IAraObject vObj, IAraObject vConteinerFather, String vTypeNameJS)
        {
            string ConteinerFatherName = (vConteinerFather != null ? "'" + ((IAraObject)vConteinerFather).InstanceID + "'" : "null");

            if (vObj is AraObjectClienteServer)
                ((AraObjectClienteServer)vObj).LoadJS();

            AddObject(vObj, vConteinerFather);
            Tick vTick = Tick.GetTick();
            vTick.Script.Send(" Ara.AddObject(" + vTick.Session.AppId + ",'" + vObj.InstanceID + "'," + (vTypeNameJS != null ? "'" + vTypeNameJS + "'" : "null") + "," + ConteinerFatherName + "); \n");
        }

        public void DellObject(IAraObject vObj)
        {
            string vInstanceID = vObj.InstanceID;

            if (vObj.ConteinerFather!=null)
                vObj.ConteinerFather.RemuveChild(vObj);
            Tick vTick = Tick.GetTick();
            vTick.Script.Send(" Ara.DelObject('" + AraTools.StringToStringJS(vInstanceID) + "');\n");
            Objects.Remove(vInstanceID);
            vTick.AraPageMain.MemoryArea.CloseObject(this, vInstanceID);
        }

        public IAraObject GetObject(string vInstanceID)
        {
            try
            {
                ISessionObject vTmpSObj=null;
                if (Objects.TryGetValue(vInstanceID, out vTmpSObj))
                    return vTmpSObj.Object;
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        

        public void SaveObjects()
        {
            ISessionObject[] vTmpObjs;
            lock(Objects)
            {
                vTmpObjs = Objects.Values.Where(a=>a.NeedSave).ToArray();
            }

            foreach (var vTmpObj in vTmpObjs)
                vTmpObj.SaveObject();
        }

        public void SaveSession()
        {
            Tick vTick = Tick.GetTick();

            if (vTick != null)
            {
                vTick.AraPageMain.MemoryArea.SaveSession(this); 
                SaveObjects();
            }
        }

        public object CustomSession=null;
    }

    

}
