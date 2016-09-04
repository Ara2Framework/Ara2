// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Ara2
{
    [Serializable]
    public static class Sessions
    {
        static public void ReNew(AraPageMain AraPageMain, string vIdSession, int vAppId)
        {
            AraPageMain.MemoryArea.CloseSession(vIdSession);
            ISession TmpSession = AraPageMain.NewSession(AraPageMain, vIdSession, vAppId);
            TmpSession.SaveSession();
        }

        static public ISession GetSession(AraPageMain AraPageMain, string vSessionID)
        {
            ISession TmpSession;
            try
            {
                TmpSession = AraPageMain.MemoryArea.GetSession( vSessionID);
            }
            catch
            {
                TmpSession = null;
            }

            if (TmpSession != null)
                TmpSession.LastCall = DateTime.Now;
            return TmpSession;
        }

        static public ISession NewSession(AraPageMain PageMain)
        {
            // Desativei este recurso pois pode ocasionar falha de segurança
            //if (string.IsNullOrEmpty(PageMain.Page.Request["SetSessionID"]))
            var vIdSession = PageMain.MemoryArea.GetNewIdSession();
            PageMain.MemoryArea.CloseSession(vIdSession);
            ISession TmpSession = PageMain.NewSession(PageMain, vIdSession, 0);
            TmpSession.ExecuteLoad();
            TmpSession.SaveSession();
            return TmpSession;

            //else
            //    return NewSession(PageMain, PageMain.Page.Request["SetSessionID"], 0);
        }
    }

}
