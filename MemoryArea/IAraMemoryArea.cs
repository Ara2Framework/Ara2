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
        Session GetSession(string vIdSession);
        int CountSession{get;}
        void CleanInactiveSession();
        void SaveSession( Session vSession);
        void CloseSession(string vSessionId);

        string GetNewIdObject(Session Session);
        SessionObject[] GetObjects(Session Session);
        Ara2.Components.IAraObject GetObject(Session Session, string InstanceID);
        void SaveObject(Session Session, Ara2.Components.IAraObject vObject);

        void CloseObject(Session Session, string InstanceID);
    }

}