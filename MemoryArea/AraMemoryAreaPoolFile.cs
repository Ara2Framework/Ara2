// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.IO;
using System.Collections.Generic;

namespace Ara2.Memory
{
    public class AraMemoryAreaPoolFile : IAraMemoryArea
    {

        /// <summary>
        /// Padrão 30 minutos
        /// </summary>
        public TimeSpan SessionTimeOut = new TimeSpan(0,30,0);

        /// <summary>
        /// A Pasta do projeto mais "/Sessions/"
        /// </summary>
        string Path;


        private AraMemoryAreaFile AraMemoryAreaFile;

        public AraMemoryAreaPoolFile(string vPath) :
            this()
        {
            Path = vPath;
            AraMemoryAreaFile.Path = Path;
        }

        public AraMemoryAreaPoolFile(string vPath,TimeSpan vSessionTimeOut) :
            this()
        {
            Path = vPath;
            AraMemoryAreaFile.Path = Path;
            SessionTimeOut = vSessionTimeOut;
            AraMemoryAreaFile.SessionTimeOut = SessionTimeOut;
        }

        public AraMemoryAreaPoolFile(TimeSpan vSessionTimeOut):
            this()
        {
            SessionTimeOut = vSessionTimeOut;
        }

        public AraMemoryAreaPoolFile()
        {

            AraMemoryAreaFile = new AraMemoryAreaFile();
			AraMemoryAreaFile.LockFilesObjects = false;
        }

        

        public string GetNewIdSession()
        {
			lock(this)
			{
            	return AraMemoryAreaFile.GetNewIdSession();
			}
        }

        public string[] GetSessionIDs()
        {
            throw new NotImplementedException();
        }

        public void CloseSession(string vIdSession)
        {
            throw new Exception("Falta terminar CloseSession");
        }

        private struct _SSession
        {
            public DateTime LastAccessTime;
            public Session data;

            public _SSession(DateTime vLastAccessTime, Session vdata)
            {
                data = vdata;
                LastAccessTime = vLastAccessTime;
            }
        }

        Dictionary<int, _SSession> _Sessions = new Dictionary<int, _SSession>();

        public Session GetSession(string vIdSession)
        {
			lock (_Sessions)
            {
	            if (_Sessions.ContainsKey(Convert.ToInt32(vIdSession)))
	                return _Sessions[Convert.ToInt32(vIdSession)].data;
                else if (AraMemoryAreaFile.GetSession(vIdSession)!=null)
                {
                    _Sessions.Add(Convert.ToInt32(vIdSession),new _SSession(DateTime.Now, AraMemoryAreaFile.GetSession(vIdSession)));
                    return _Sessions[Convert.ToInt32(vIdSession)].data;
                }
                else
                    return null;
			}
        }
        public void SaveSession(Session vByts)
        {
			lock (_Sessions)
            {
                string vSessionId =vByts.Id;
	            int IntvSessionId = Convert.ToInt32(vSessionId);
	            if (_Sessions.ContainsKey(IntvSessionId))
	                _Sessions[IntvSessionId] = new _SSession(DateTime.Now, vByts);
	            else
	                _Sessions.Add(IntvSessionId, new _SSession(DateTime.Now, vByts));

                new System.Threading.Thread(() =>
                    { 
						int NTentativas=0;
						while(true)
						{
							try
							{
	                        	AraMemoryAreaFile.SaveSession(vByts);
								return;
							}
							catch
							{
								
								NTentativas++;
								if (NTentativas>5);
									return;
								System.Threading.Thread.Sleep(100);
							}
						}
                    }).Start();
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
                foreach (int vId in (new List<int>(_Sessions.Keys)))
                {
                    _SSession DI = _Sessions[vId];
                    if (DateTime.Now - DI.LastAccessTime >= SessionTimeOut)
                    {
                        try
                        {
                            _Sessions.Remove(vId);
                        }
                        catch { }
                    }
                }

                new System.Threading.Thread(() =>
                {
					int NTentativas=0;
					while(true)
					{
						try
						{
                    		AraMemoryAreaFile.CleanInactiveSession();
							return;
						}
						catch
						{
							NTentativas++;
							if (NTentativas>5);
								return;
							
							System.Threading.Thread.Sleep(100);
						}
					}
				}).Start();
            }

        }

        public string GetNewIdObject(Session Session)
        {
			lock(this)
			{
                return AraMemoryAreaFile.GetNewIdObject(Session);
			}
        }

        private class _SessionObject
        {
            public string  InstanceID;
            public Ara2.Components.IAraObject data;
            public int? Lock = null;

            public _SessionObject(string vInstanceID, Ara2.Components.IAraObject vdata)
            {
                InstanceID = vInstanceID;
                data = vdata;
            }
        }

        Dictionary<int, List<_SessionObject>> _Objects = new Dictionary<int, List<_SessionObject>>();

        public SessionObject[] GetObjects(Session Session)
        {
            if (_Objects.ContainsKey(Convert.ToInt32(Session.Id)))
            {
                List<SessionObject> Tmp = new List<SessionObject>();
                foreach (_SessionObject Obj in _Objects[Convert.ToInt32(Session.Id)].ToArray())
                {
                    Tmp.Add(new SessionObject(Session, Obj.InstanceID.ToString()));
                }

                return Tmp.ToArray();
            }
            else if (AraMemoryAreaFile.GetObjects(Session).Length>0)
            {
                List<_SessionObject> Tmp = new List<_SessionObject>();

                foreach(SessionObject TmpObj in AraMemoryAreaFile.GetObjects(Session))
                {
					Ara2.Components.IAraObject vObj = TmpObj.Object;
					if (vObj!=null)
                    	Tmp.Add(new _SessionObject(TmpObj.InstanceID,vObj));
                }

                if (_Objects.ContainsKey(Convert.ToInt32(Session.Id)))
                    _Objects[Convert.ToInt32(Session.Id)] = Tmp;
                else
                    _Objects.Add(Convert.ToInt32(Session.Id), Tmp);
                return GetObjects(Session);
            }
            else
                return (new List<SessionObject>()).ToArray();
        }


        public Ara2.Components.IAraObject GetObject(Session Session, string InstanceID)
        {
            if (!_Objects.ContainsKey(Convert.ToInt32(Session.Id)))
                _Objects.Add(Convert.ToInt32(Session.Id), new List<_SessionObject>());

            _SessionObject Obj = _Objects[Convert.ToInt32(Session.Id)].Find(a => a.InstanceID == InstanceID); 
			
			
            if (Obj == null)
            {
				lock(this)
				{
					try
					{
		                Ara2.Components.IAraObject TmpObj  = AraMemoryAreaFile.GetObject(Session, InstanceID);
		                if (TmpObj != null)
		                {
			                if (!_Objects.ContainsKey(Convert.ToInt32(Session.Id)))
			                    _Objects.Add(Convert.ToInt32(Session.Id), new List<_SessionObject>());
			                _Objects[Convert.ToInt32(Session.Id)].Add(new _SessionObject(TmpObj.InstanceID, TmpObj));
                            _Objects[Convert.ToInt32(Session.Id)].Find(a => a.InstanceID == InstanceID).Lock = System.Threading.Thread.CurrentThread.ManagedThreadId;
			                return TmpObj;
		                }
						else
		                    throw new Exception("Object '" + InstanceID + "' não encontrado na session '" + Session.Id + "'");
					}
					catch(Exception err)
					{
						return null;
						//System.Diagnostics.Debugger.Break();
					}
				}
            }
            

            while (true)
			{
			    lock (this)
			    {
                    
                    if (Obj.Lock != null ||  ThreadAtiva(System.Threading.Thread.CurrentThread.ManagedThreadId)==false)
			        {
                        Obj.Lock = System.Threading.Thread.CurrentThread.ManagedThreadId;
			            return Obj.data;
			        }                        
			    }
			    System.Threading.Thread.Sleep((new Random()).Next (1,10));
            }

            
        }

        private bool ThreadAtiva(int Id)
        {
            System.Diagnostics.ProcessThreadCollection Threads = System.Diagnostics.Process.GetCurrentProcess().Threads;

            for (int n = 0; n < Threads.Count; n++)
            {
                if (Threads[n].Id == Id)
                    return true;
            }

            return false;
        }

        public void SaveObject(Session Session,Ara2.Components.IAraObject vObject)
        {
            lock (this)
            {
                string InstanceID=vObject.InstanceID;
                     
                if (!_Objects.ContainsKey(Convert.ToInt32(Session.Id)))
                    _Objects.Add(Convert.ToInt32(Session.Id), new List<_SessionObject>());

                _SessionObject Obj = _Objects[Convert.ToInt32(Session.Id)].Find(a => a.InstanceID == InstanceID);
                if (Obj == null)
                    _Objects[Convert.ToInt32(Session.Id)].Add(new _SessionObject(InstanceID,vObject));
                else
                {
                    Obj.data = vObject;
                    Obj.Lock = null;
                }

                
            }
			
			new System.Threading.Thread(() =>
                {
					int NTentativas=0;
					while(true)
					{
						try
						{
                    		AraMemoryAreaFile.SaveObject(Session, vObject);
							return;
						}
						catch
						{
							NTentativas++;
							if (NTentativas>5);
								return;
							
							System.Threading.Thread.Sleep(100);
						}
					}
                }).Start();
            //File.WriteAllBytes(Path + "Sessions\\" + Session.Id + "\\Object_" + InstanceID + ".bin", vObject);
        }

        public void CloseObject(Session Session, string InstanceID)
        {
            lock (_Objects)
            {
                _Objects[Convert.ToInt32(Session.Id)].RemoveAll(a => a == null || a.InstanceID == InstanceID);
            }
        }

    }

}