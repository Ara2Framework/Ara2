// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Ara2
{
    public class AraTools
    {
        #region Alert

        #region Alert Simples
        static public void Alert(string vM)
        {
            Components.WindowMain.GetInstance().Alert(vM);
        }

        static public void Alert(string vM, Action Retorno)
        {
            Alert<Action>(vM, Retorno);
        }

        static public void Alert<T>(string vM, T Retorno, params object[] vParametros)
        {
            Components.WindowMain.GetInstance().Alert(vM, Retorno, vParametros);
        }
        #endregion

        #region AlertYesOrNo
        static public void AlertYesOrNo(string vM,Action<bool> EventReturn)
        {
            AlertYesOrNo<Action<bool>>(vM, EventReturn);
        }

        static public void AlertYesOrNo<T>(string vM, T EventReturn, params object[] vParametros)
        {
            Components.WindowMain.GetInstance().AlertYesOrNo<T>(vM, EventReturn, vParametros);
        }
        #endregion

        #region AsynchronousFunction
        static public void AsynchronousFunction(Action EventReturn)
        {
            AsynchronousFunction<Action>(EventReturn);
        }

        static public void AsynchronousFunction<T>(T EventReturn, params object[] vParametros) 
        {
            Components.WindowMain.GetInstance().AsynchronousFunction<T>(EventReturn, vParametros);
        }
        #endregion

        #region AlertGetString
        static public void AlertGetString(string vM, Action<string> EventReturn)
        {
            AlertGetString<Action<string>>(vM, "", EventReturn);
        }

        static public void AlertGetString(string vM, string vValuedefault, Action<string> EventReturn)
        {
            AlertGetString<Action<string>>(vM, vValuedefault, EventReturn);
        }

        static public void AlertGetString<T>(string vM, T EventReturn, params object[] vParans)
        {
            AlertGetString<T>(vM, "", EventReturn, vParans);
        }

        static public void AlertGetString<T>(string vM, string vValuedefault, T EventReturn,params object[] vParans)
        {
            Components.WindowMain.GetInstance().AlertGetString(vM, vValuedefault, EventReturn, vParans);
        }
        #endregion

        #endregion


        static public string StreamToString(Stream vStream)
        {
            using (StreamReader SR = new StreamReader(vStream))
            {
                string tmp = SR.ReadToEnd();
                SR.Dispose();
                SR.Close();
                return tmp;
            }
        }

        static public void DebuggerJS()
        {
            Tick.GetTick().Script.Send("\ndebugger;\n");
        }

        static public bool IsDate(string vDate)
        {
            DateTime Tmp;
            return DateTime.TryParse(vDate, out Tmp);
        }

        static public bool IsDecimal(string vDecimal)
        {
            decimal Tmp;
            return Decimal.TryParse(vDecimal, out Tmp);
        }

        static public bool IsBool(string vtext)
        {
            bool Tmp;
            return bool.TryParse(vtext, out Tmp);
        }

        static public bool IsInt(string vDecimal)
        {
            try
            {
                if (IsDecimal(vDecimal))
                {
                    decimal Tmp = decimal.Parse(vDecimal);
                    int Tmp2 = (int)Tmp;
                    if ((Tmp - Tmp2) != 0)
                        return false;
                    else
                        return true;
                }
                else
                {
                    int Tmp;
                    return int.TryParse(vDecimal,out Tmp);
                }
            }
            catch
            {
                return false;
            }
        }

        static public int DiferencaEmDiasUteis(DateTime initialDate, DateTime finalDate)
        {
            int days = 0;
            int daysCount = 0;
            days = initialDate.Subtract(finalDate).Days;

            if (days < 0)
                days = days * -1;

            for (int i = 1; i <= days; i++)
            {
                initialDate = initialDate.AddDays(1);
                if (initialDate.DayOfWeek != DayOfWeek.Sunday &&
                    initialDate.DayOfWeek != DayOfWeek.Saturday)
                    daysCount++;
            }
            return daysCount;
        }


        public static string HttpPost(string URI, Dictionary<string, string> Parameters)
        {
            string ParametersString = "";

            if (Parameters.Count > 0)
            {
                foreach (string vkey in Parameters.Keys)
                {
                    ParametersString += vkey + "=" + System.Web.HttpUtility.UrlEncode(Parameters[vkey]) + "&";
                }
                ParametersString = ParametersString.Substring(0, ParametersString.Length - 1);
            }

            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            //req.Proxy = new System.Net.WebProxy(ProxyString, true);

            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            //We need to count how many bytes we’re sending. Post’ed Faked Forms should be name=value&
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(ParametersString);
            req.ContentLength = bytes.Length;
            System.IO.Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length); //Push it out there
            os.Close();
            System.Net.WebResponse resp = req.GetResponse();
            if (resp == null) return null;

            string vTmp002;
            using (System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream()))
            {
                vTmp002 = sr.ReadToEnd().Trim();
                sr.Dispose();
                sr.Close();
            }
            return vTmp002;
        }

        //static public void ExecuteAraExec(string vFile)
        //{
        //    try
        //    {

        //        Ara2.VAkos.Xmlconfig ExecFile = new Ara2.VAkos.Xmlconfig(vFile, false);
        //        string vUrl = ExecFile.Settings["url"].Value;
        //        ExecFile = null;

        //        OpenRemoteApplication(vUrl);
        //    }
        //    catch (Exception Erro)
        //    {
        //        AraTools.Alert("O arquivo '" + vFile + "' não é um AraExec.\n Erro: " + Erro.Message);
        //    }

        //}

        //static public void OpenRemoteApplication(string vUrlApp)
        //{
        //    OpenRemoteApplication(vUrlApp, "");
        //}

        //static public void OpenRemoteApplication(string vUrlApp, string vParan)
        //{
        //    Tick Tick = Tick.GetTick();
        //    string vCodForm = Tick.Application.Forms.GetNewCodForm();

        //    string vUrlToGetAccess = "";
        //    string YourUrlServer = vUrlApp;
        //    /*
        //    if (YourUrlServer.IndexOf("http://") > -1)
        //        YourUrlServer = YourUrlServer.Substring("http://".Length, YourUrlServer.Length - "http://".Length);
        //    YourUrlServer = YourUrlServer.Substring(0, YourUrlServer.IndexOf('/'));
        //    */

        //    Dictionary<string, string> vParans = new Dictionary<string, string>();

        //    vParans.Add("OpenRemoteApplication", "1");
        //    vParans.Add("SessionID", Tick.Application.Session.Id);
        //    vParans.Add("CodFormMain", vCodForm);
        //    vParans.Add("UrlServerApp", vUrlToGetAccess);
        //    vParans.Add("YourUrlServer", YourUrlServer);
        //    vParans.Add("Paran", vParan);

        //    string vResposta = HttpPost(vUrlApp, vParans);

        //    Tick.Application.SubApplications.Add(vCodForm, vUrlApp);

        //    if (vResposta == "OK")
        //        Tick.Script.Send(" Ara.OpenRemoteApplication('" + StringToStringJS(vUrlApp) + "','" + vCodForm + "0.'); \n");
        //    else
        //        AraTools.Alert("Erro ao abrir a Aplicação.\n Erro: " + vResposta);
        //}



        static public string GetPath()
        {
            string vTmp;
            try
            {
                vTmp = Tick.GetTick().Page.MapPath("~\\");
            }
            catch
            {
                try
                {
                    vTmp = System.Web.HttpContext.Current.Server.MapPath("~\\");
                }
                catch
                {
                    try
                    {
                        vTmp = System.Web.HttpRuntime.AppDomainAppPath;
                    }
                    catch
                    {
                        vTmp = Directory.GetCurrentDirectory();
                    }
                }
            }

            if (vTmp.Substring(vTmp.Length - 2, 2) == "/\\")
                vTmp = vTmp.Substring(0, vTmp.Length - 1);

            return vTmp;
        }

        //        static public string GetPath(System.Web.UI.Page Page)
        //        {
        //            string vRetorno = "";
        //            try
        //            {
        //                vRetorno = Page.MapPath("~\\");
        //            }
        //            catch { vRetorno = ""; }
        //
        //            if (vRetorno == "")
        //                vRetorno = Page.MapPath("/");
        //            return vRetorno;
        //        }


        
        public static void WarningLoading(string vMessage, Action vAction)
        {
            Tick.GetTick().Session.WindowMain.WaitLoading(vMessage, vAction);
        }


        static public string TreatEmptyVoid(object vValue)
        {
            if (vValue == DBNull.Value)
                return "";
            else if (vValue == null)
                return "";
            else
                return Convert.ToString(vValue);
        }

        static public string GetLinkDownloadForce(string vPath)
        {
            return "?DownloadForce=" + vPath;
        }

        static public void FileWriteAllText(string vFile, string vText)
        {
            using (StreamWriter W = new StreamWriter(vFile))
            {
                W.Write(vText);
                W.Dispose();
            }
        }
        static public string FileReaderAllText(string vFile)
        {
            using (StreamReader R = new StreamReader(vFile))
            {
                string vTmp = R.ReadToEnd();
                R.Dispose();
                return vTmp;
            }
        }

        private const string StringToIDJSCaracteres = "ABCDEFGHIJLMNOPQRSTUVXZabcdefghijlmnopqrstuvxz0123456789";

        static public string StringToIDJS(string vString)
        {
            if (vString != null)
            {
                string Tmp = "";
                
                foreach (char vChar in vString.ToArray<char>())
                {
                    if (StringToIDJSCaracteres.IndexOf(vChar) > -1)
                        Tmp += vChar;
                    else
                        Tmp += "_" + ((int)vChar).ToString();
                }

                return Tmp;
            }
            else
                return "Null";
        }

        /// <summary>
        /// Determines whether the supplied object is a .NET numeric system type
        /// </summary>
        /// <param name="val">The object to test</param>
        /// <returns>true=Is numeric; false=Not numeric</returns>
        public static bool IsNumericType(object val)
        {
            if (val == null)
                return false;

            // Test for numeric type, returning true if match
            if
                (
                val is double || val is float || val is int || val is long || val is decimal ||
                val is short || val is uint || val is ushort || val is ulong || val is byte ||
                val is sbyte
                )
                return true;

            // Not numeric
            return false;
        }

        public static Boolean Between(DateTime input, DateTime minDate, DateTime maxDate)
        {
            // SQL takes limit in !
            return input >= minDate && input <= maxDate;
        }

        static public string StringToStringJS(string s)
        {
            if (s == null || s.Length == 0)
            {
                return "";
            }
            char c;
            int i;
            int len = s.Length;
            StringBuilder sb = new StringBuilder(len + 4);
            string t;

            //sb.Append('"');
            for (i = 0; i < len; i += 1)
            {
                c = s[i];
                if ((c == '\\') || (c == '"') || (c == '>'))
                {
                    sb.Append('\\');
                    sb.Append(c);
                }
                else if (c == '\b')
                    sb.Append("\\b");
                else if (c == '\t')
                    sb.Append("\\t");
                else if (c == '\n')
                    sb.Append("\\n");
                else if (c == '\f')
                    sb.Append("\\f");
                else if (c == '\r')
                    sb.Append("\\r");
                else if (c == '!')
                    sb.Append("\\!");
                else
                {
                    if (c < ' ')
                    {
                        //t = "000" + Integer.toHexString(c); 
                        string tmp = new string(c, 1);
                        t = "000" + int.Parse(tmp, System.Globalization.NumberStyles.HexNumber);
                        sb.Append("\\u" + t.Substring(t.Length - 4));
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
            }
            //sb.Append('"');
            return sb.ToString().Replace("'", "\\'");
        }

        static public void ManifestResourceStreamToPathFile(Assembly asm, string vName)
        {
            ManifestResourceStreamToPathFile(Tick.GetTick().Page, asm, vName);
        }

        private static bool ManifestResourceStreamToPathFile_FileLoad = false;

        [Serializable]
        private class CMRSToPathFile
        {
            public string Name { get; set; }
            public DateTime Date { get; set; }
        }

        private static List<CMRSToPathFile> _ManifestResourceStreamToPathFile_Files = new List<CMRSToPathFile>();
        private static List<CMRSToPathFile> ManifestResourceStreamToPathFile_Files
        {
            get
            {
                if (!ManifestResourceStreamToPathFile_FileLoad)
                {
                    string vFile = Path.Combine(GetPath(), "Ara2", "ManifestResourceStreamToPathFile2.json");
                    if (File.Exists(vFile))
                    {
                        lock (_ManifestResourceStreamToPathFile_Files)
                        {
                            _ManifestResourceStreamToPathFile_Files.Clear();
                            try
                            {
                                foreach (dynamic vTmpD2 in Json.DynamicJson.Parse(File.ReadAllText(vFile)))
                                {
                                    if (vTmpD2 != null)
                                    {
                                        _ManifestResourceStreamToPathFile_Files.Add(new CMRSToPathFile()
                                        {
                                            Name = vTmpD2.Name,
                                            Date = Convert.ToDateTime(vTmpD2.Date)
                                        });
                                    }
                                }
                            }
                            catch { }
                        }
                    }

                    ManifestResourceStreamToPathFile_FileLoad = true;
                }

                return _ManifestResourceStreamToPathFile_Files;
            }
        }
        private static void ManifestResourceStreamToPathFile_AddFile(CMRSToPathFile vFileTmp)
        {
            lock (_ManifestResourceStreamToPathFile_Files)
            {
                _ManifestResourceStreamToPathFile_Files.RemoveAll(a => a.Name == vFileTmp.Name);
                vFileTmp.Date = DateTime.Now;
                _ManifestResourceStreamToPathFile_Files.Add(vFileTmp);
            }

            ManifestResourceStreamToPathFile_Save();
        }

        private static Thread ManifestResourceStreamToPathFile_Save_Thread = null;
        private static void ManifestResourceStreamToPathFile_Save()
        {
            if (ManifestResourceStreamToPathFile_Save_Thread == null || ManifestResourceStreamToPathFile_Save_Thread.IsAlive==false)
            {
                ManifestResourceStreamToPathFile_Save_Thread = new Thread(ManifestResourceStreamToPathFile_SaveAny);
                ManifestResourceStreamToPathFile_Save_Thread.Start(GetPath());
            }            
        }

        private static void ManifestResourceStreamToPathFile_SaveAny(object vPath)
        {
            var vSleep = DateTime.Now.AddSeconds(30);

            while (vSleep > DateTime.Now)
                Thread.Sleep(1000);

            lock (_ManifestResourceStreamToPathFile_Files)
            {
                string vFile = Path.Combine(vPath.ToString(), "Ara2", "ManifestResourceStreamToPathFile2.json");
                File.WriteAllText(vFile, Json.DynamicJson.Serialize(_ManifestResourceStreamToPathFile_Files));
            }
        }

        static public void ManifestResourceStreamToPathFile(System.Web.UI.Page Page, Assembly asm, string vName)
        {
            DateTime CreationTime = (new FileInfo(asm.Location).CreationTime);
            if (ManifestResourceStreamToPathFile_Files.Exists(a => a!=null && a.Name == vName && a.Date == CreationTime))
                return;

            Exception vErro;
            int NTentaticas = 0;
            do
            {
                if (NTentaticas > 0)
                    System.Threading.Thread.Sleep(200);

                vErro = null;
                try
                {
                    ManifestResourceStreamToPathFileTentativa(Page, asm, vName);
                }
                catch (Exception err)
                { vErro = err; }

                NTentaticas++;
            } while (vErro != null && NTentaticas <= 10);

            if (vErro != null)
                throw vErro;
            else
            {
                ManifestResourceStreamToPathFile_AddFile(new CMRSToPathFile()
                {
                    Name = vName,
                    Date = (new FileInfo(asm.Location).CreationTime)
                });
            }
        }

        static private void ManifestResourceStreamToPathFileTentativa(System.Web.UI.Page Page, Assembly asm, string vName)
        {
            if (vName.IndexOf("*") > -1)
            {
                string vFileTmp = vName.Replace("/", ".");
                
                foreach (string vFile in asm.GetManifestResourceNames().ToList().FindAll(a => vFileTmp.Split('*').ToList().Exists(b => a.Contains(b) && b != "")))
                {
                    string vFile2 = vFile.Replace(".", "/");
                    vFile2 = vFile2.Substring(0, vFile2.LastIndexOf("/")) + "." + vFile2.Substring(vFile2.LastIndexOf("/")+1, vFile2.Length - vFile2.LastIndexOf("/")-1);
                    ManifestResourceStreamToPathFileTentativa(Page, asm, vFile2);
                }
            }
            else
            {
                Stream stream = asm.GetManifestResourceStream(vName.Replace("/", "."));


                if (stream != null)
                {
                    List<string> Tmp = new List<string>();
                    Tmp.Add(GetPath());
                    Tmp.AddRange(vName.Replace("'", ".").Replace("/", "\\").Split('\\'));

                    string vFileD = System.IO.Path.Combine(Tmp.ToArray());


                    if (File.Exists(vFileD))
                        File.Delete(vFileD);
                    else
                    {
                        string vFileName = vName.Substring(vName.LastIndexOf("/") + 1, vName.Length - vName.LastIndexOf("/") - 1);
                        string vFilePath = vName.Substring(0, vName.LastIndexOf("/") + 1);

                        
                        string vPathAtual = GetPath();
                        if (!Directory.Exists(System.IO.Path.Combine(vPathAtual, vFilePath)))
                        {
                            string[] PathFile = vName.Split('/');
                            foreach (string vPath in PathFile)
                            {
                                if (vPath != vFileName)
                                {
                                    vPathAtual = System.IO.Path.Combine(vPathAtual, vPath);

                                    if (!Directory.Exists(vPathAtual))
                                        Directory.CreateDirectory(vPathAtual);
                                }
                            }
                        }
                    }

                    using (StreamWriter streamW = new StreamWriter(vFileD))
                    {
                        stream.CopyTo(streamW.BaseStream);
                    }
                }
                else
                    throw new Exception("ManifestResourceStreamToPathFile not fount " + vName);

            }

            
        }

        public static string UrlServer
        {
            get
            {

                try
                {

                    Tick vTick = Tick.GetTick();
                    string vUrl = vTick.Page.Request.Url.ToString();

                    if (vUrl.IndexOf("?") > -1)
                        vUrl = vUrl.Substring(0, vUrl.IndexOf("?"));

                    return vUrl;
                }
                catch (Exception error)
                {
                    AraTools.Alert("Error on AraTools get UrlServer.\n Error: " + error.Message);
                    return "";
                }

            }
        }

        public static EDeviceType DeviceType
        {
            get
            {
                EDeviceType? _UserAgentToDeviceType;
                string userAgent = Tick.GetTick().Page.Request.UserAgent;
                //AraTools.Alert("userAgent = '" + userAgent + "'");
                if (userAgent.ToLowerInvariant().Contains("blackberry"))
                    _UserAgentToDeviceType = EDeviceType.Phone;
                else if (userAgent.ToLowerInvariant().Contains("iphone"))
                    _UserAgentToDeviceType = EDeviceType.Phone;
                else if (userAgent.ToLowerInvariant().Contains("ipad"))
                    _UserAgentToDeviceType = EDeviceType.Tablet;
                else if (userAgent.ToLowerInvariant().Contains("android"))
                {
                    if (userAgent.ToLowerInvariant().Contains("mobile"))
                        _UserAgentToDeviceType = EDeviceType.Phone;
                    else
                        _UserAgentToDeviceType = EDeviceType.Tablet;
                }
                else
                    _UserAgentToDeviceType = EDeviceType.Desktop;

                return (EDeviceType)_UserAgentToDeviceType;
            }
        }

        public static decimal ToDecimal(object Obj)
        {
            if (Obj != null && Obj != DBNull.Value && IsDecimal(Obj.ToString()))
                return Convert.ToDecimal(Obj);
            return (decimal)0.0;
        }

        public static string ToFormattedDecimal(object Obj)
        {
            return ToDecimal(Obj).ToString("n");
        }

        public static int ToInt(object Obj)
        {
            if (Obj != null && Obj != DBNull.Value && IsInt(Obj.ToString()))
                return Convert.ToInt32(Obj);
            return 0;
        }

        public static DateTime? ToDate(object Obj)
        {
            if (Obj != null && Obj != DBNull.Value && IsDate(Obj.ToString()))
                return Convert.ToDateTime(Obj);
            return null;
        }

        public static string ToFormattedDate(object Obj)
        {
            DateTime? Date = ToDate(Obj);
            if (Date != null)
                return ((DateTime)Date).ToString("dd/MM/yyyy");
            else
                return "";
        }

        public static void Print(string vUrl)
        {
            string vScript = "";

            vScript = " var w = window.open('" + AraTools.StringToStringJS(vUrl) + "');\n";
            vScript += "setTimeout(function(){  w.print(); w.close();},1000);\n";
            //vScript += "    w.close();\n";

            Tick.GetTick().Script.Send(vScript);
        }

        #region Cookie
        public static void SetCookie(string vName, string vValue, int? ExpiresDays = null, string path = null)
        {
            Tick.GetTick().Session.WindowMain.SetCookie(vName, vValue, ExpiresDays, path);
        }
        public static void DelCookie(string vName, string path = null)
        {
            Tick.GetTick().Session.WindowMain.DelCookie(vName, path);
        }
        public static void GetCookie(string vName, Ara2.Components.DGetCookie vEventReturn)
        {
            Tick.GetTick().Session.WindowMain.GetCookie(vName, vEventReturn);
        }
        #endregion


    }

    public enum EDeviceType
    {
        Desktop,
        Tablet,
        Phone
    }
}
