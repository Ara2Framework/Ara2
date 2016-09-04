using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ara2.Components;

namespace Ara2.Session2
{
    public class Session : ISession
    {
        public Session(IAraWindowMain vWindowMain)
        {
            _WindowMain = vWindowMain;
        }

        public int AppId { get; set; }

        public string Id { get; set; }

        public DateTime LastCall { get; set; }

        public AraEvent<Action<IAraObjectClienteServer, string>> OnReceivesInternalEventAfter { get; set; }

        public AraEvent<Action<IAraObjectClienteServer, string>> OnReceivesInternalEventBefore { get; set; }


        IAraWindowMain _WindowMain;
        public IAraWindowMain WindowMain
        {
            get
            {
                return _WindowMain;
            }
        }

        public void AddJs(string vJs)
        {
            throw new NotImplementedException();
        }

        public void AddObject(IAraWindowMain WindowMain)
        {
            throw new NotImplementedException();
        }

        public void AddObject(IAraObject vObj, IAraObject vConteinerFather)
        {
            throw new NotImplementedException();
        }

        public void AddObject(IAraObject vObj, IAraObject vConteinerFather, string vTypeNameJS)
        {
            throw new NotImplementedException();
        }

        public void DellObject(IAraObject vObj)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void ExecuteLoad()
        {
            throw new NotImplementedException();
        }

        public string GetNewID()
        {
            throw new NotImplementedException();
        }

        public IAraObject GetObject(string vInstanceID)
        {
            throw new NotImplementedException();
        }

        public void SaveSession()
        {
            throw new NotImplementedException();
        }

        public bool ExistsObject(string vInstanceID)
        {
            throw new NotImplementedException();
        }

        public void AddCss(string vCss)
        {
            throw new NotImplementedException();
        }
    }
}
