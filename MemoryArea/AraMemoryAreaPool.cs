// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Ara2.Memory
{
    public class AraMemoryAreaPool : IAraMemoryArea
    {

        public TimeSpan SessionTimeOut = new TimeSpan(0,2,0);

        public AraMemoryAreaPool(TimeSpan vSessionTimeOut):
            this()
        {
            SessionTimeOut = vSessionTimeOut;
        }

        public AraMemoryAreaPool()
        {

            
        }
        Random Rnd = new Random();
        //long _NewIdSession = 0 ;

        public string GetNewIdSession()
        {
            lock (_Sessions)
			{
                //_NewIdSession++;
                string vSessionId;
                do
                {
                    vSessionId = DateTime.Now.Ticks + "_" + Rnd.Next();
                } while (_Sessions.ContainsKey(vSessionId));

                return vSessionId;
			}
        }


        Dictionary<string, _Session> _Sessions = new Dictionary<string, _Session>();

        public string[] GetSessionIDs()
        {
            return _Sessions.Keys.ToArray();
        }

        public Session GetSession(string vIdSession)
        {
            try
            {
                _Session TmpSessions = null;
                if (_Sessions.TryGetValue(vIdSession, out TmpSessions))
                    return TmpSessions.Session;
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }
        public void SaveSession(Session vByts)
        {
            lock (_Sessions)
            {
                string vSessionId = vByts.Id;
                if (!_Sessions.ContainsKey(vSessionId))
                    _Sessions.Add(vSessionId, new _Session(){
                        Session = vByts
                    });
            }
        }

        public int CountSession
        {
            get
            {
				lock (_Sessions)
            	{
                	return _Sessions.Count;
				}
            }
        }

        public void CleanInactiveSession()
        {
            lock (_Sessions)
            {
                foreach (var Id in _Sessions.Values.Where(a => a == null || DateTime.Now - a.Session.LastCall >= SessionTimeOut).Select(a => a.Session.Id).ToArray())
                {
                    CloseSession(Id);
                }
            }
        }

        public void CloseSession(string vIdSession)
        {
            try
            {
                lock (_Sessions)
                {
                    try
                    {
                        _Session vTmp_Session;
                        if (_Sessions.TryGetValue(vIdSession,out vTmp_Session))
                            vTmp_Session.Dispose();
                    }
                    catch { }

                    _Sessions.Remove(vIdSession);
                }
            }
            catch { }
        }

        public string GetNewIdObject(Session Session)
        {
            return _Sessions[Session.Id].GetNewIdObject().ToString();
        }

        private class _Session:IDisposable
        {
            private int NewIdObject;
            public Session Session;
            public Dictionary<string, _SessionObject> Objects = new Dictionary<string,_SessionObject>();

            public int GetNewIdObject()
            {
                int vTmp;
                lock(this)
                {
                    vTmp = NewIdObject;
                    NewIdObject++;
                }
                return vTmp;
            }
            public _SessionObject GetObjectOrNull(string vInstanceID)
            {
                try
                {
                    _SessionObject vTmpObj;
                    if (Objects.TryGetValue(vInstanceID,out vTmpObj))
                        return vTmpObj;
                    else
                        return null;
                }
                catch
                {
                    return null;
                }
            }
            public void AddObject(Ara2.Components.IAraObject vObject)
            {
                lock (Objects)
                {
                    if (!@Objects.ContainsKey(vObject.InstanceID))
                        Objects.Add(vObject.InstanceID, new _SessionObject()
                        {
                            IdSession = Session.Id,
                            InstanceID = vObject.InstanceID,
                            data = vObject
                        });
                    else
                        Objects[vObject.InstanceID].data = vObject;
                }
            }

            public void CloseObject(string InstanceID)
            {
                lock(Objects)
                {
                    Objects.Remove(InstanceID);
                }
            }

            public void Dispose()
            {
                Objects.Clear();
                Session.Dispose();
            }
        }

        private class _SessionObject
        {
            public string IdSession;
            public string InstanceID;
            public Ara2.Components.IAraObject data;
        }

        public SessionObject[] GetObjects(Session Session)
        {
            try
            {
                return _Sessions[Session.Id].Objects.Values.Select(a => new SessionObject(Session, a.data)).ToArray();
            }
            catch
            {
                return null;
            }
        }

        public Ara2.Components.IAraObject GetObject(Session Session, string InstanceID)
        {
            try
            {
                return _Sessions[Session.Id].Objects[InstanceID].data;
            }
            catch { return null; }
        }

        public void SaveObject(Session Session, Ara2.Components.IAraObject vObject)
        {
            var vTmpS = _Sessions[Session.Id];
            var vTmpO = vTmpS.GetObjectOrNull(vObject.InstanceID);
            if (vTmpO == null)
                vTmpS.AddObject(vObject);
        }


        public void CloseObject(Session Session, string InstanceID)
        {

            _Sessions[Session.Id].CloseObject(InstanceID);
        }

    }

}