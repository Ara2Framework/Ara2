﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ara2.Components;
using System.Web;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Ara2.Session3
{
    public class Session : ISession
    {
        const string PrefixObject = "O_";

        public Session(AraPageMain vAraPageMain, string vSession)
        {
            AraPageMain = vAraPageMain;
            Id = vSession;
            OnReceivesInternalEventAfter = new AraEvent<Action<IAraObjectClienteServer, string>>();
            OnReceivesInternalEventBefore = new AraEvent<Action<IAraObjectClienteServer, string>>();
        }

        

        public string Id { get; set; }

        public DateTime LastCall { get; set; }

        public AraEvent<Action<IAraObjectClienteServer, string>> OnReceivesInternalEventAfter { get; set; }

        public AraEvent<Action<IAraObjectClienteServer, string>> OnReceivesInternalEventBefore { get; set; }


        AraPageMain AraPageMain;
        public IAraWindowMain WindowMain
        {
            get
            {
                return AraPageMain.GetWindowMain(this);
            }
        }

        #region JS
        List<string> _JsLoad = new List<string>();
        public void AddJs(string vFile)
        {
            if (!_JsLoad.Contains(vFile))
            {
                Tick vTick = Tick.GetTick();
                _JsLoad.Add(vFile);
                vTick.Script.Send("Ara.AraClass.LoadClass(0," + vTick.Id + ",'" + AraTools.StringToStringJS(vTick.AraPageMain.GetUrlRedirectFiles(vFile)) + "',function () {\n");
                vTick.Script.GetNewLevel();

                vTick.Script.Send(vTick.Script.Level - 1, " ); \n ");
            }
        }
        #endregion

        #region CSS
        List<string> _CssLoad = new List<string>();
        public void AddCss(string vCss)
        {
            if (!_CssLoad.Contains(vCss))
            {
                Tick vTick = Tick.GetTick();
                vTick.Script.Send("Ara.AddCss('" + AraTools.StringToStringJS(vTick.AraPageMain.GetUrlRedirectFiles(vCss)) + "');\n");
                _CssLoad.Add(vCss);
            }
        }
        #endregion

        #region Add Object

        Dictionary<string, IAraObject> Objetos = new Dictionary<string, IAraObject>();

        public void AddObject(IAraWindowMain WindowMain)
        {
            AddObject(WindowMain, null);
        }

        public void AddObject(IAraObject vObj, IAraObject vConteinerFather)
        {
            lock(Objetos)
            {
                if (!Objetos.ContainsKey(vObj.InstanceID))
                    Objetos.Add(vObj.InstanceID, vObj);
                else
                    Objetos[vObj.InstanceID] = vObj;
            }
        }
        
        public void AddObject(IAraObject vObj, IAraObject vConteinerFather, string vTypeNameJS)
        {
            string ConteinerFatherName = (vConteinerFather != null ? "'" + ((IAraObject)vConteinerFather).InstanceID + "'" : "null");

            if (vObj is AraObjectClienteServer)
                ((AraObjectClienteServer)vObj).LoadJS();

            AddObject(vObj, vConteinerFather);
            Tick vTick = Tick.GetTick();
            vTick.Script.Send(" Ara.AddObject(0,'" + vObj.InstanceID + "'," + (vTypeNameJS != null ? "'" + vTypeNameJS + "'" : "null") + "," + ConteinerFatherName + "); \n");
        }
        #endregion


        public void DellObject(IAraObject vObj)
        {
            string vInstanceID = vObj.InstanceID;

            lock(Objetos)
            {
                if (vObj.ConteinerFather != null)
                    vObj.ConteinerFather.RemuveChild(vObj);
                Tick vTick = Tick.GetTick();
                vTick.Script.Send(" Ara.DelObject('" + AraTools.StringToStringJS(vInstanceID) + "');\n");
                HttpRuntime.Cache.Remove(vObj.InstanceID);
                Objetos.Remove(vObj.InstanceID);
            }
        }

        public void ExecuteLoad()
        {
            
            
        }

        public string GetNewID()
        {
            return string.Concat(PrefixObject, Guid.NewGuid().ToString().Replace("-","_"));
        }

        public IAraObject GetObject(string vInstanceID)
        {
            try
            {
                IAraObject vTmpObject;
                lock (Objetos)
                {
                    if (Objetos.TryGetValue(vInstanceID, out vTmpObject))
                        return vTmpObject;
                    else
                    {
                        var vObjSessionThinObject = (SessionThinObject)HttpRuntime.Cache.Get(vInstanceID);
                        if (vObjSessionThinObject != null)
                        {
                            vTmpObject = vObjSessionThinObject.ToIAraObject();

                            Objetos.Add(vInstanceID, vTmpObject);

                            return vTmpObject;
                        }
                        else
                            return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public void SaveSession()
        {
            //throw new NotImplementedException();
            lock(Objetos)
            {
                if (Objetos.Keys.Any())
                {
                    foreach (var vKey in Objetos.Keys.ToArray())
                    {
                        var vObj = Objetos[vKey];
                        if (vObj!=null)
                            AddObjectToCache(vObj, vObj.ConteinerFatherInstanceID);
                        Objetos.Remove(vKey);
                    }
                }
            }
        }

        private void AddObjectToCache(IAraObject vObj,string vConteinerFatherInstanceID)
        {
            if (HttpRuntime.Cache.Get(vObj.InstanceID) != null)
                HttpRuntime.Cache.Remove(vObj.InstanceID);

            if (vConteinerFatherInstanceID != null)
                HttpRuntime.Cache.Insert(vObj.InstanceID, new SessionThinObject(vObj));//, new System.Web.Caching.CacheDependency(null, new string[] { vConteinerFatherInstanceID }));
            else
                HttpRuntime.Cache.Insert(vObj.InstanceID, new SessionThinObject(vObj));//, new System.Web.Caching.CacheDependency(null, new string[] { this.Id }));
        }

        public bool ExistsObject(string vInstanceID)
        {
            return GetObject(vInstanceID) != null;
        }

        public void Dispose()
        {
            //foreach (var vObj in HttpRuntime.Cache)
            //{
            //    if (vObj is IAraObject)
            //    {
            //        DellObject((IAraObject)vObj);
            //    }
            //}
        }
    }
}