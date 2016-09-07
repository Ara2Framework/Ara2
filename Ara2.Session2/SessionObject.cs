//// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
//// For licensing, see LICENSE.md or http://www.araframework.com.br/license
//// This file is part of AraFramework project details visit http://www.arafrework.com.br
//// AraFramework - Rafael Leonel Pontani, 2016-4-14
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Ara2.Components;

//namespace Ara2.Session2
//{
//    [Serializable]
//    public class SessionObject: ISessionObject
//    {
//        ISession Session;

//        [NonSerialized]
//        IAraObject _Object = null;

//        public SessionObject(ISession vSession, string vInstanceID)
//        {
//            Session = vSession;
//            _InstanceID = vInstanceID;
//        }

//        public SessionObject(ISession vSession, IAraObject vObject)
//        {
//            Session = vSession;
//            _Object = vObject;
//            _InstanceID = vObject.InstanceID;
//            _Type = vObject.GetType();
//            _NeedSave = true;
//        }


//        public IAraObject Object
//        {
//            get
//            {
//                if (_Object == null)
//                {
//                    _Object = Tick.GetTick().AraPageMain.MemoryArea.GetObject(Session, _InstanceID);
//                    _NeedSave = true;
//                }

//                return _Object;
//            }
//        }

//        string _InstanceID = null;
//        public string InstanceID
//        {
//            get
//            {
//                return _InstanceID;
//            }
//        }

//        private Type _Type=null;
//        public Type Type
//        {
//            get
//            {
//                return _Type;
//            }
//        }

//        bool _NeedSave = false;
//        public bool NeedSave
//        {
//            get
//            {
//                return _NeedSave;
//            }
//        }

//        private object _ObjectLock = new object();

//        public void SaveObject()
//        {
//            lock (_ObjectLock)
//            {
//                if (this._Object != null)
//                {
//                    lock (this._Object)
//                    {
//                        Tick.GetTick().AraPageMain.MemoryArea.SaveObject(Session, this._Object);
//                        this._Object = null;
//                    }
//                }
//            }
//        }
//    }
//}
