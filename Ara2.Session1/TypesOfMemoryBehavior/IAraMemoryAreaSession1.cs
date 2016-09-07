using Ara2.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ara2.Session1
{
    public interface IAraMemoryAreaSession1: IAraMemoryArea
    {
        string GetNewIdObject(ISession Session);
        ISessionObject[] GetObjects(ISession Session);
        Ara2.Components.IAraObject GetObject(ISession Session, string InstanceID);
        void SaveObject(ISession Session, Ara2.Components.IAraObject vObject);

        void CloseObject(ISession Session, string InstanceID);
    }
}
