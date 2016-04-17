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
        static public void ReNew(AraPageMain AraPageMain, string vIdSession)
        {
            AraPageMain.MemoryArea.CloseSession(vIdSession);
            Session TmpSession = new Session(AraPageMain, vIdSession);
            TmpSession.SaveSession();
        }

        static public Session GetSession(AraPageMain AraPageMain, string vSessionID)
        {
            Session TmpSession;
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

        static public Session NewSession(AraPageMain PageMain)
        {
            if (string.IsNullOrEmpty(PageMain.Page.Request["SetSessionID"]))
                return NewSession(PageMain, PageMain.MemoryArea.GetNewIdSession(), 0);
            else
                return NewSession(PageMain, PageMain.Page.Request["SetSessionID"], 0);
        }

        static public Session NewSession(AraPageMain AraPageMain, string vIdSession, int vAppId)
        {
            AraPageMain.MemoryArea.CloseSession(vIdSession);
            Session TmpSession = new Session(AraPageMain, vIdSession, vAppId);
            TmpSession.ExecuteLoad();
            TmpSession.SaveSession();
            return TmpSession;
        }


    }

}
