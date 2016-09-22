// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Ara2.Memory;
using System.Web;

namespace Ara2.Session4
{
    public class AraMemoryAreaPool : IAraMemoryArea
    {
        public TimeSpan SessionTimeOut = new TimeSpan(0,2,0);

        const string PrefixSession = "S_";

        public AraMemoryAreaPool(TimeSpan vSessionTimeOut):
            this()
        {
            SessionTimeOut = vSessionTimeOut;
        }

        public AraMemoryAreaPool()
        {

            
        }

        public ISession NewSession(AraPageMain AraPageMain)
        {
            return NewSession(AraPageMain, GetNewIdSession());
        }
        
        public ISession NewSession(AraPageMain AraPageMain,string vIdSession)
        {
            CloseSession(vIdSession);
            ISession TmpSession = new Session(AraPageMain, vIdSession);
            HttpRuntime.Cache.Insert(TmpSession.Id, TmpSession);
            TmpSession.ExecuteLoad();
            TmpSession.SaveSession();
            TmpSession.LastCall = DateTime.Now;
            
            return TmpSession;
        }

        //Random Rnd = new Random();
        //long _NewIdSession = 0 ;

        public string GetNewIdSession()
        {
            //         lock (_Sessions)
            //{
            //             //_NewIdSession++;
            //             string vSessionId;
            //             do
            //             {
            //                 vSessionId = DateTime.Now.Ticks + "_" + Rnd.Next();
            //             } while (_Sessions.ContainsKey(vSessionId));

            //             return vSessionId;
            //}
            return string.Concat(PrefixSession, Guid.NewGuid().ToString().Replace("-", "_"));
        }


        //Dictionary<string, _Session> _Sessions = new Dictionary<string, _Session>();

        public string[] GetSessionIDs()
        {
            List<string> vSessionsIDs = new List<string>();
            var vTmpCache = HttpRuntime.Cache.GetEnumerator();
            while(vTmpCache.MoveNext())
            {
                if (vTmpCache.Key.ToString().StartsWith(PrefixSession))
                    vSessionsIDs.Add(vTmpCache.Key.ToString());
            }

            return vSessionsIDs.ToArray();
        }

        public ISession GetSession(string vIdSession)
        {
            try
            {
                var vTmpS = (ISession)HttpRuntime.Cache.Get(vIdSession);
                if(vTmpS!=null) vTmpS.LastCall = DateTime.Now;
                return vTmpS;

                //_Session TmpSessions = null;
                //if (_Sessions.TryGetValue(vIdSession, out TmpSessions))
                //{
                //    TmpSessions.Session.LastCall = DateTime.Now;
                //    return TmpSessions.Session;
                //}
                //else
                //    return null;
            }
            catch
            {
                return null;
            }
        }
        public void SaveSession(ISession vByts)
        {
            //lock (_Sessions)
            //{
            //    string vSessionId = vByts.Id;
            //    if (!_Sessions.ContainsKey(vSessionId))
            //        _Sessions.Add(vSessionId, new _Session(){
            //            Session = vByts
            //        });
            //}
        }

        public int CountSession
        {
            get
            {
                int vCount=0;
                var vTmpCache = HttpRuntime.Cache.GetEnumerator();
                while (vTmpCache.MoveNext())
                {
                    if (vTmpCache.Key.ToString().StartsWith(PrefixSession))
                        vCount++;
                }

                return vCount;
            }
        }

        DateTime UltimaLimpesa = DateTime.Now;
        public void CleanInactiveSession()
        {
            lock(this)
            {
                if ((DateTime.Now - UltimaLimpesa) >= SessionTimeOut)
                {
                    UltimaLimpesa = DateTime.Now;

                    var vTmpCache = HttpRuntime.Cache.GetEnumerator();
                    while (vTmpCache.MoveNext())
                    {
                        if (vTmpCache.Key.ToString().StartsWith(PrefixSession))
                        {
                            if (DateTime.Now - ((ISession)vTmpCache.Value).LastCall >= SessionTimeOut)
                                CloseSession(((ISession)vTmpCache.Value));
                        }
                    }
                }
            }
        }

        public void CloseSession(string vIdSession)
        {
            try
            {
                CloseSession(GetSession(vIdSession));
                //lock (_Sessions)
                //{
                //    try
                //    {
                //        _Session vTmp_Session;
                //        if (_Sessions.TryGetValue(vIdSession,out vTmp_Session))
                //            vTmp_Session.Dispose();
                //    }
                //    catch { }

                //    _Sessions.Remove(vIdSession);
                //}
            }
            catch { }
        }

        public void CloseSession(ISession Session)
        {
            try
            {
                if (Session != null)
                {
                    lock (Session)
                    {
                        string vIdSession = Session.Id;
                        Session.Dispose();
                        HttpRuntime.Cache.Remove(vIdSession);
                    }
                }

            }
            catch { }
        }

        //public string GetNewIdObject(ISession Session)
        //{
        //    return _Sessions[Session.Id].GetNewIdObject().ToString();
        //}

        //private class _Session:IDisposable
        //{
        //    private int NewIdObject;
        //    public ISession Session;
        //    public Dictionary<string, _SessionObject> Objects = new Dictionary<string,_SessionObject>();

        //    public int GetNewIdObject()
        //    {
        //        int vTmp;
        //        lock(this)
        //        {
        //            vTmp = NewIdObject;
        //            NewIdObject++;
        //        }
        //        return vTmp;
        //    }
        //    public _SessionObject GetObjectOrNull(string vInstanceID)
        //    {
        //        try
        //        {
        //            _SessionObject vTmpObj;
        //            if (Objects.TryGetValue(vInstanceID,out vTmpObj))
        //                return vTmpObj;
        //            else
        //                return null;
        //        }
        //        catch
        //        {
        //            return null;
        //        }
        //    }
        //    public void AddObject(Ara2.Components.IAraObject vObject)
        //    {
        //        lock (Objects)
        //        {
        //            if (!@Objects.ContainsKey(vObject.InstanceID))
        //                Objects.Add(vObject.InstanceID, new _SessionObject()
        //                {
        //                    IdSession = Session.Id,
        //                    InstanceID = vObject.InstanceID,
        //                    data = vObject
        //                });
        //            else
        //                Objects[vObject.InstanceID].data = vObject;
        //        }
        //    }

        //    public void CloseObject(string InstanceID)
        //    {
        //        lock(Objects)
        //        {
        //            Objects.Remove(InstanceID);
        //        }
        //    }

        //    public void Dispose()
        //    {
        //        Objects.Clear();
        //        Session.Dispose();
        //    }
        //}

        //private class _SessionObject
        //{
        //    public string IdSession;
        //    public string InstanceID;
        //    public Ara2.Components.IAraObject data;
        //}

        //public ISessionObject[] GetObjects(ISession Session)
        //{
        //    try
        //    {
        //        return _Sessions[Session.Id].Objects.Values.Select(a => new SessionObject(Session, a.data)).ToArray();
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}

        //public Ara2.Components.IAraObject GetObject(ISession Session, string InstanceID)
        //{
        //    try
        //    {
        //        return _Sessions[Session.Id].Objects[InstanceID].data;
        //    }
        //    catch { return null; }
        //}

        //public void SaveObject(ISession Session, Ara2.Components.IAraObject vObject)
        //{
        //    var vTmpS = _Sessions[Session.Id];
        //    var vTmpO = vTmpS.GetObjectOrNull(vObject.InstanceID);
        //    if (vTmpO == null)
        //        vTmpS.AddObject(vObject);
        //}


        //public void CloseObject(ISession Session, string InstanceID)
        //{

        //    _Sessions[Session.Id].CloseObject(InstanceID);
        //}

    }

}