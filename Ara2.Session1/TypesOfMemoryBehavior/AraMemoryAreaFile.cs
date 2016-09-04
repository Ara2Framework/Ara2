// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.IO;
using System.Collections.Generic;
using Ara2.Memory;

namespace Ara2.Session1
{
    public class AraMemoryAreaFile : IAraMemoryArea
    {
        public AraMemoryAreaFile():
            this(AraTools.GetPath())
        {
        }

        public string Path;
        public TimeSpan SessionTimeOut = new TimeSpan(0,30,0);
        static object SpinLock = new object();

        public AraMemoryAreaFile(string vPath, TimeSpan vSessionTimeOut):
            this(vPath)
        {
            SessionTimeOut = vSessionTimeOut;
        }

        public AraMemoryAreaFile(string vPath)
        {
            Path = vPath;

            if (!System.IO.Directory.Exists(System.IO.Path.Combine(Path , "Sessions")))
                System.IO.Directory.CreateDirectory(System.IO.Path.Combine(Path , "Sessions"));
        }

        public string GetNewIdSession()
        {
            int vNumero;
            lock (SpinLock)
			{
	            using (FileStream vFile = File.Open(System.IO.Path.Combine(Path , "Sessions","NewIdSession.bin"), FileMode.OpenOrCreate))
	            {
	                byte[] vNumeroB = new byte[vFile.Length];
	                vFile.Read(vNumeroB, 0, Convert.ToInt32( vFile.Length));
	                
	                if (vFile.Length > 0)
	                    vNumero = BitConverter.ToInt32(vNumeroB,0) + 1;
	                else
	                    vNumero = 1;
	
	                if (Directory.Exists(System.IO.Path.Combine(Path , "Sessions" , vNumero.ToString())))
	                    Directory.Delete(System.IO.Path.Combine(Path , "Sessions" , vNumero.ToString()),true);
	
	                Directory.CreateDirectory(System.IO.Path.Combine(Path , "Sessions" , vNumero.ToString()));
	
	                vNumeroB =  BitConverter.GetBytes(vNumero);
	
	                vFile.SetLength(0);
	                vFile.Write(vNumeroB, 0, vNumeroB.Length);
	                
	            }
	            return vNumero.ToString();
			}
        }

        public string[] GetSessionIDs()
        {
            throw new NotImplementedException();
        }

        public ISession GetSession( string vIdSession)
        {
            lock (SpinLock)
			{
            	return new CustomBinarySerializer<ISession>().DeserializeFromBytes(File.ReadAllBytes(System.IO.Path.Combine(new string[] { Path, "Sessions", vIdSession, vIdSession + ".bin" })));
			}
        }
        public void SaveSession(ISession vSession)
        {
            lock (SpinLock)
			{
                string vSessionId = vSession.Id;
	            byte[] vByts = new CustomBinarySerializer<object>().Serialize2Bytes(vSession);
	
	            File.WriteAllBytes(System.IO.Path.Combine(new string[]{ Path , "Sessions" , vSessionId, vSessionId + ".bin"}), vByts);
			}
        }

        public int CountSession
        {
            get
            {
                return Directory.GetDirectories(System.IO.Path.Combine(Path , "Sessions")).Length;
            }
        }

        public void CleanInactiveSession()
        {
            lock (SpinLock)
            {
                List<DirectoryInfo> TmpD = (new List<DirectoryInfo>((new DirectoryInfo(System.IO.Path.Combine(Path, "Sessions"))).GetDirectories())).FindAll(a => (DateTime.Now - a.LastAccessTime) >= SessionTimeOut);

                foreach (DirectoryInfo DI in TmpD)
                {
                    try
                    {
                        DI.Delete(true);
                    }
                    catch { }
                }
            }

        }

        public void CloseSession(string vIdSession)
        {
            throw new Exception("Falta terminar CloseSession");
        }

        public string GetNewIdObject(ISession Session)
        {
			int? vNumero=null;
			for(int nT=0;nT < 10;nT++)
			{
				System.Threading.Thread TmpTCon = new System.Threading.Thread(() =>
	             {
					try
					{
                        lock (SpinLock)
						{
				            
				            using (FileStream vFile = File.Open(System.IO.Path.Combine(Path , "Sessions","NewIdObject.bin")  , FileMode.OpenOrCreate))
				            {
				                byte[] vNumeroB = new byte[vFile.Length];
				                vFile.Read(vNumeroB, 0, Convert.ToInt32(vFile.Length));
				
				                if (vFile.Length > 0)
				                    vNumero = BitConverter.ToInt32(vNumeroB, 0) + 1;
				                else
				                    vNumero = 1;
				
				                vNumeroB = BitConverter.GetBytes((int)vNumero);
				
				                
				
				                vFile.SetLength(0);
				                vFile.Write(vNumeroB, 0, vNumeroB.Length);
				
				            }
				            
						}
					} catch{}
						
				});
				
				TmpTCon.Start();
				if (!TmpTCon.Join(100) || vNumero ==null){
					TmpTCon.Abort();
					System.Threading.Thread.Sleep(10);
				}
				else
					return vNumero.ToString();
				
				
			}
			
			throw new TimeoutException();
        }
		
		private string[] GetFilesDirectory(string vPath,int vTimeOut = 100)
		{
		
			string[] Tmp = new string[]{};
			
			int vNumero;
			for(int nT=0;nT < 10;nT++)
			{
				System.Threading.Thread TmpTCon = new System.Threading.Thread(() =>
	             {
					Tmp= Directory.GetFiles(vPath);
				 });
				
				TmpTCon.Start();
				if (!TmpTCon.Join(vTimeOut)){
					TmpTCon.Abort();
					System.Threading.Thread.Sleep(10);
				}
				else
					return Tmp;
			}
			throw new TimeoutException();
		}
		
        public ISessionObject[] GetObjects(ISession Session)
        {
            lock (SpinLock)
			{
	            List<ISessionObject> Objs = new List<ISessionObject>();
	            foreach (string vFile in  GetFilesDirectory(System.IO.Path.Combine(Path , "Sessions" , Session.Id),100))
	            {
					//"Object_*.bin"
					
	                int IxObject = vFile.LastIndexOf("Object_");
					if (IxObject!=-1)
					{
	                	Objs.Add(new SessionObject(Session, vFile.Substring(IxObject + 7, vFile.Length - IxObject - 7 - 4)));
					}
	            }
	
	            return Objs.ToArray();
			}
        }
		
		public bool LockFilesObjects = true;
		
        Dictionary<string, FileStream> FileStreams = new Dictionary<string, FileStream>();
		
		private FileStream OpenFileTimeOut(string vPathFile,int vTimeOut=100)
		{
			FileStream Tmp=null;
			
			int vNumero;
			for(int nT=0;nT < 10;nT++)
			{
				System.Threading.Thread TmpTCon = new System.Threading.Thread(() =>
	             {
                     try
                     {
                         Tmp = File.Open(vPathFile, FileMode.Open);
                     }
                     catch
                     {
                         Tmp = null;
                     }
				 });
				
				TmpTCon.Start();
                if (!TmpTCon.Join(vTimeOut) || Tmp ==null)
                {
					TmpTCon.Abort();
					System.Threading.Thread.Sleep(10);
				}
				else
					return Tmp;
			}
			throw new TimeoutException();
				
		}
		
        public Ara2.Components.IAraObject GetObject(ISession Session, string InstanceID)
        {
			
            FileStream TmpF=null;

            while (TmpF == null)
            {
                try
                {
                    TmpF = OpenFileTimeOut(System.IO.Path.Combine(Path, "Sessions", Session.Id,"Object_" + InstanceID + ".bin"),100);
                }
                catch { TmpF = null; System.Threading.Thread.Sleep((new Random()).Next (1,10));}
				
            }

            lock (SpinLock)
			{
				if (LockFilesObjects)
					FileStreams.Add(Session.Id + "_" + InstanceID, TmpF);
	            byte[] TmpData = new byte[TmpF.Length];
	            TmpF.Read(TmpData, 0, TmpData.Length);
				
				if (!LockFilesObjects)
				{
					TmpF.Close();
					TmpF=null;
				}
			
            
				return new CustomBinarySerializer<Ara2.Components.IAraObject>().DeserializeFromBytes(TmpData);
			}
        }

        public void SaveObject(ISession Session, Ara2.Components.IAraObject vObject)
        {
            byte[] vObjectBit;

            lock (SpinLock)
            {
                string InstanceID = vObject.InstanceID;

                vObjectBit = new CustomBinarySerializer<object>().Serialize2Bytes(vObject);

                FileStream TmpF;
                if (FileStreams.ContainsKey(Session.Id + "_" + InstanceID))
                    TmpF = FileStreams[Session.Id + "_" + InstanceID];
                else
                    TmpF = File.Open(System.IO.Path.Combine(Path, "Sessions", Session.Id, "Object_" + InstanceID + ".bin"), FileMode.OpenOrCreate);

                TmpF.SetLength(0);
                TmpF.Write(vObjectBit, 0, vObjectBit.Length);
                TmpF.Close();
                if (LockFilesObjects)
                    FileStreams.Remove(Session.Id + "_" + InstanceID);
            }
            //File.WriteAllBytes(Path + "Sessions\\" + Session.Id + "\\Object_" + InstanceID + ".bin", vObject);
        }

        public void CloseObject(ISession Session, string InstanceID)
        {
            throw new Exception("CloseObject no implemented");
            //lock (_Objects)
            //{
            //    _Objects.RemoveAll(a => a == null || (a.IdSession == Session.Id && a.InstanceID == InstanceID));
            //}
        }

    }

}