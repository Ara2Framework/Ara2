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

namespace Ara2
{

    public interface ISession : IDisposable
    {
        string Id { get; set; }
        int AppId { get; set; }
        DateTime LastCall { get; set; }

        AraEvent<Action<IAraObjectClienteServer, string>> OnReceivesInternalEventBefore { get; set; }
        AraEvent<Action<IAraObjectClienteServer, string>> OnReceivesInternalEventAfter { get; set; }

        void AddObject(IAraWindowMain WindowMain);
        void AddObject(IAraObject vObj, IAraObject vConteinerFather);
        void AddObject(IAraObject vObj, IAraObject vConteinerFather, String vTypeNameJS);
        void DellObject(IAraObject vObj);
        string GetNewID();
        IAraObject GetObject(string vInstanceID);
        bool ExistsObject(string vInstanceID);

        void SaveSession();
        void AddJs(string vJs);
        void AddCss(string vCss);
        void ExecuteLoad();

        //Dictionary<string, ISessionObject> Objects { get; }

        IAraWindowMain WindowMain { get; }
    }
}

    