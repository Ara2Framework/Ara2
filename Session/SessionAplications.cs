// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ara2.Components;

namespace Ara2.SessionAplication
{
    [Serializable]
    public class SessionAplications:AraObject
    {
        Session Session;

        public SessionAplications(Session vSession):
            base(AraObject.Create(null).InstanceID,null)
        {
            Session = vSession;
        }

        List<SessionsAplication> _Aplications = new List<SessionsAplication>();
        private int _AplicationsNewID = 0;

        public void OpenAplication(string vUrl)
        {
            if (Session.AppId == 0)
            {
                int _AppId = _AplicationsNewID;
                _AplicationsNewID++;

                SessionsAplication SessionsAplication = new SessionsAplication(Session, _AppId, vUrl);
                _Aplications.Add(SessionsAplication);

                Tick vTick = Tick.GetTick();
                vTick.Script.Send(" Ara.OpenAplication('" + AraTools.StringToStringJS(vUrl) + "'," + _AppId + ");");
            }
            else
            {
                // Solicita ao servidor principal que abra a nova app
                Tick vTick = Tick.GetTick();
                vTick.Script.Send(" Ara.OpenAplication('" + AraTools.StringToStringJS(vUrl) + "');");
            }
        }
    }
}
