using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ara2.Components;
using System.Web;

namespace Ara2.Session2
{
    public class Session : ISession
    {
        public Session(AraPageMain vAraPageMain, string vSession,int vAppId)
        {
            AraPageMain = vAraPageMain;
            Id = vSession;
            AppId = vAppId;
            OnReceivesInternalEventAfter = new AraEvent<Action<IAraObjectClienteServer, string>>();
            OnReceivesInternalEventBefore = new AraEvent<Action<IAraObjectClienteServer, string>>();
        }

        public int AppId { get; set; }

        public string Id { get; set; }

        public DateTime LastCall { get; set; }

        public AraEvent<Action<IAraObjectClienteServer, string>> OnReceivesInternalEventAfter { get; set; }

        public AraEvent<Action<IAraObjectClienteServer, string>> OnReceivesInternalEventBefore { get; set; }


        AraPageMain AraPageMain;
        IAraWindowMain _WindowMain;
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
                vTick.Script.Send("Ara.AraClass.LoadClass(" + vTick.Session.AppId + "," + vTick.Id + ",'" + AraTools.StringToStringJS(vTick.AraPageMain.GetUrlRedirectFiles(vFile)) + "',function () {\n");
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
        public void AddObject(IAraWindowMain WindowMain)
        {
            AddObject(WindowMain, null);
        }

        public void AddObject(IAraObject vObj, IAraObject vConteinerFather)
        {
            if (vConteinerFather!=null)
                HttpRuntime.Cache.Insert(vObj.InstanceID, vObj, new System.Web.Caching.CacheDependency(null,new string[] { vConteinerFather.InstanceID }));
            else
                HttpRuntime.Cache.Insert(vObj.InstanceID, vObj);
        }

        public void AddObject(IAraObject vObj, IAraObject vConteinerFather, string vTypeNameJS)
        {
            string ConteinerFatherName = (vConteinerFather != null ? "'" + ((IAraObject)vConteinerFather).InstanceID + "'" : "null");

            if (vObj is AraObjectClienteServer)
                ((AraObjectClienteServer)vObj).LoadJS();

            AddObject(vObj, vConteinerFather);
            Tick vTick = Tick.GetTick();
            vTick.Script.Send(" Ara.AddObject(" + vTick.Session.AppId + ",'" + vObj.InstanceID + "'," + (vTypeNameJS != null ? "'" + vTypeNameJS + "'" : "null") + "," + ConteinerFatherName + "); \n");
        }
        #endregion


        public void DellObject(IAraObject vObj)
        {
            HttpRuntime.Cache.Remove(vObj.InstanceID);
        }

        public void ExecuteLoad()
        {
            
        }

        public string GetNewID()
        {
            //return Guid.NewGuid().ToString().Replace("-","_");
            return "A" + this.AppId + "O" + Tick.GetTick().AraPageMain.MemoryArea.GetNewIdObject(this);
        }

        public IAraObject GetObject(string vInstanceID)
        {
            try
            {
                return (IAraObject)HttpRuntime.Cache.Get(vInstanceID);
            }
            catch
            {
                return null;
            }
        }

        public void SaveSession()
        {
            //throw new NotImplementedException();
        }

        public bool ExistsObject(string vInstanceID)
        {
            return GetObject(vInstanceID) != null;
        }

        public void Dispose()
        {

            foreach (var vObj in HttpRuntime.Cache)
            {
                if (vObj is IAraObject)
                {
                    HttpRuntime.Cache.Remove(((IAraObject)vObj).InstanceID);
                }
            }
        }
    }
}
