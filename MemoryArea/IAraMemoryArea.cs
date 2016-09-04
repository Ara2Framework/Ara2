// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;

namespace Ara2.Memory
{
    public interface IAraMemoryArea
    {
        string GetNewIdSession();
        string[] GetSessionIDs();
        ISession GetSession(string vIdSession);
        int CountSession{get;}
        void CleanInactiveSession();
        void SaveSession(ISession vSession);
        void CloseSession(string vSessionId);

        string GetNewIdObject(ISession Session);
        ISessionObject[] GetObjects(ISession Session);
        Ara2.Components.IAraObject GetObject(ISession Session, string InstanceID);
        void SaveObject(ISession Session, Ara2.Components.IAraObject vObject);

        void CloseObject(ISession Session, string InstanceID);

        ISession NewSession(AraPageMain AraPageMain);
        ISession NewSession(AraPageMain araPageMain, string vSession, int AppId);
    }

}