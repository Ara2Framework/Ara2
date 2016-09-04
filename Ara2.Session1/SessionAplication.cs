// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Ara2.Session1
{
    [Serializable]
    public class SessionsAplication
    {
        #region static



        #endregion

        Session Session;

        public SessionsAplication(Session vSession, int vId,string vUrl)
        {
            Session = vSession;
            _Id = vId;
            _Url = vUrl;
        }

        private int _Id;
        public int Id
        {
            get { return _Id; }
        }

        private string _Url;
        public string Url
        {
            get
            {
                return _Url;
            }
        }

    }

}
