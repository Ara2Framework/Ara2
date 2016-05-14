// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ara2.Components;
using System.IO;
using System.Reflection;
using Ara2.Json;
using System.Web.Configuration;
using Ara2.Log;

namespace Ara2
{
    public abstract class AraPageMain :  System.Web.UI.Page 
    {


		/// <summary>
		/// Define a classe personalizada de gerenciamento de memória
		/// </summary>
		/// <returns> Retorne uma instancia da sua classe de gerenciamento de menória</returns>
		public virtual string GetSessionMessageNotFound()
		{
			return "Sessão não encontrada. Provavelmente a sessão expirou ou ocorreu um erro interno no servidor.\n A Pagina será reiniciada.";
		}

        /// <summary>
        /// Define a classe personalizada de gerenciamento de memória
        /// </summary>
        /// <returns> Retorne uma instancia da sua classe de gerenciamento de menória</returns>
        public virtual Ara2.Memory.IAraMemoryArea GetMemoryArea()
        {
            throw new Exception("Falta declarar o GetMemoryArea em AraPageMain");
        }

        private static Ara2.Memory.IAraMemoryArea _MemoryArea = null;
        public Ara2.Memory.IAraMemoryArea MemoryArea
        {
            get
            {
                if (_MemoryArea==null)
                    _MemoryArea=GetMemoryArea();
                return _MemoryArea;
            }
        }

        public virtual bool RedirectHttps()
        {
            return false;
        }

        public virtual string ViewPort()
        {
            return "";
        }

        ///// <summary>
        ///// Define a classe personalizada da aplicação de suporte ao tick
        ///// </summary>
        ///// <returns> Retorne o typeof da classe Ex: typeof(SuaClasse)</returns>
        //public virtual object GetNewCustomTick()
        //{
        //    throw new Exception("Falta declarar o GetTypeGlobalTick em AraPageMain");
        //}

        /// <summary>
        /// Define o Window Main 
        /// </summary>
        /// <returns> Retorne o WindowMain</returns>
        public virtual Ara2.Components.WindowMain GetWindowMain(Session Session)
        {
            throw new Exception("Falta declarar o GetWindowMain em AraPageMain");
        }
        
        /// <summary>
        /// Define a Skin padrão a ser carregada
        /// </summary>
        /// <returns> Retorne o WindowMain</returns>
        public virtual string GetJQueryUICss()
        {
            //return "Ara2/Files/jQuery/css/cupertino/jquery-ui-1_9_2_custom.css";
            //return "Ara2/Files/jQuery/css/ui_lightness/jquery-ui-1_9_2_custom.css";
            return "Ara2/Files/jQuery/css/cupertino_1_11/jquery-ui_min.css";
        }

        /// <summary>
        /// Defina o tratamento de erros da aplicação
        /// </summary>
        public virtual void ExceptionAplication(Exception err)
        {
            AraTools.Alert(err.Message);
        }

        /// <summary>
        /// Evento para final de todo tick
        /// </summary>
        public virtual void OnEndTick(Tick vTick)
        {
            
        }

        private static bool AraMemoryAreaCountKeyLoad = false;
        private static string AraMemoryAreaCountKey = null;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.Params["araignore"] != null)
                return;
            else if (Request.Params["DownloadForce"] != null)
            {
                try
                {
                    SendFileDownloadForce( Request.Params["DownloadForce"]);
                    return;
                }
                catch (Exception erro)
                {
                    throw new Exception("Error on DownloadForce '" + Request.Params["DownloadForce"] + "'\n " + erro.Message);
                }
            }
            else if (Request.Params["SendFile"] == "1" && Request.Params["FileKey"] != null)
            {
                try
                {
                    SendFile(Request.Params["FileKey"]);
                    return;
                }
                catch (Exception erro)
                {
                    throw new Exception("Error on SendFile '" + Request.Params["File"] + "'\n " + erro.Message);
                }
            }
            else if (Request.Params["AraMemoryAreaCount"] != null)
            {
                if (AraMemoryAreaCountKeyLoad==false)
                {
                    AraMemoryAreaCountKeyLoad=true;
                    if (File.Exists(Path.Combine( AraTools.GetPath(),"AraMemoryAreaCountKey.ara")))
                        AraMemoryAreaCountKey = File.ReadAllText(Path.Combine( AraTools.GetPath(),"AraMemoryAreaCountKey.ara"));
                }

                if (AraMemoryAreaCountKey != null && AraMemoryAreaCountKey != Request.Params["AraMemoryAreaCountKey"])
                    throw new Exception("AraMemoryAreaCountKey invalid.");

                try
                {
                    MemoryArea.CleanInactiveSession();
                    Response.Write(MemoryArea.CountSession.ToString());
                    return;
                }
                catch (Exception erro)
                {
                    throw new Exception("Error on MemoryArea.CountSession.\n " + erro.Message);
                }
            }
            else
            {
                

                Session Session = null;

                if (Request["SessionId"] != null)
                {

                    Session = Sessions.GetSession(this,Request["SessionId"]);

                    if (Session == null)
                    {
                        //if (Request["AppId"] != "")
                        //    Session = Sessions.NewSession(this, Request["SessionId"], Convert.ToInt32(Request["AppId"]));
                        //else
                        //{
                        if (Request.Params["ARA2"] != "1")
                        {
							Response.Write(@"
                            <html> 
                            <script type='text/javascript'>
							");
							if (GetSessionMessageNotFound()!="")
								Response.Write (@"alert('" + AraTools.StringToStringJS(GetSessionMessageNotFound()) + "');");

							Response.Write (@"
                                setTimeout(function(){ location.reload();},500);
							");

                            Response.Write(@"
							</script>
                            </html>
                            ");
                            return;
                        }
                        else
                        {
							if (GetSessionMessageNotFound()!="")
								Response.Write (@"alert('" + AraTools.StringToStringJS(GetSessionMessageNotFound()) + "');");

							Response.Write (@"
                                setTimeout(function(){ location.reload();},500);
							");

                            return;
                        }
                            
                        //}

                    }
                }
                else
                    Session = Sessions.NewSession(this);

                
                MemoryArea.CleanInactiveSession();

                Tick vTick = Tick.AddTick(new Tick(Session, Page, this));
                //Session.ReviewPointerAraObject();

                if (Request.Params["ARA2"] == "1")
                {
                    if (Convert.ToInt32(Request["AppId"]) == Session.AppId)
                    {
                        try
                        {
                            RecebeTick();
                        }
                        catch (Exception err)
                        {
                            ExceptionAplication(err);
                        }
                        finally
                        {
                            try
                            {
                                vTick.Session.SaveSession();
                            }
                            catch(Exception err) 
                            {
                                ExceptionAplication(new Exception("Erro on save Session.\n" + err.Message));
                            }
                            Tick.DellTick(vTick);
                        }
                    }
                    else
                    {
                        try
                        {
                            // Envia para servidor corespondente
                        }
                        catch (Exception err)
                        {
                            AraTools.Alert(err.Message);
                        }
                        finally
                        {
                            try
                            {
                                vTick.Session.SaveSession();
                            }
                            catch { }
                            Tick.DellTick(vTick);
                        }
                    }
                }
                else // Envia Kernel
                {
                    if (!File.Exists(Path.Combine( AraTools.GetPath() + "AraRedirect.ara")))
                    {
                        if (getInternetExplorerVersion() == null || getInternetExplorerVersion() >= 8)
                        {
                            try
                            {
                                lock (this)
                                {
                                    EnviaKernel();
                                }
                            }
                            catch (Exception err)
                            {
                                Response.Write("<script> document.write(\"" + AraTools.StringToStringJS(err.Message) + "\"); </script>");
                                Response.Write("<script> setTimeout(function(){ location.reload();},500); </script>");
                            }
                        }
                        else
                            Response.Write("<script> window.location ='Ara2/Files/BrowserNotSupported/BrowserNotSupported.html'; </script>");
                    }
                    else
                    {
                        Random RND =new Random();
                        int RNDVal = RND.Next(1,100000);

                        string vUrl = File.ReadAllText(Path.Combine(AraTools.GetPath() + "AraRedirect.ara")).Replace("$RND$", RNDVal.ToString()).Replace("'", "\\'");
                        Response.Write("<script> window.location ='" + vUrl + "'; </script>");
                    }
                    
                   
                }

                
            }

        }


        private float? getInternetExplorerVersion()
        {
            try
            {
                // Returns the version of Internet Explorer or a -1
                // (indicating the use of another browser).
                float? rv = null;
                HttpCapabilitiesBase.BrowserCapabilitiesProvider = new CustomerHttpCapabilitiesProvider();

                System.Web.HttpBrowserCapabilities browser = HttpCapabilitiesBase.BrowserCapabilitiesProvider.GetBrowserCapabilities(Request);
                if (browser.Browser == "IE")
                    rv = (float)(browser.MajorVersion + browser.MinorVersion);
                return rv;
            }
            catch { return null; }
        }

        public virtual void BeforeReceivingTick()
        {

        }

        private void RecebeTick()
        {
            Tick vTick= Tick.GetTick();

            BeforeReceivingTick();

            int[] ExecutedSynchronizewithAraThread = (new List<int>()).ToArray();

            try
            {

                //if (Tick.ReturnisJavascript)
                //    ExecutedSynchronizewithAraThread = AraTools.RumSynchronizewithAraThread();

                vTick.Script.RumLoad();

                #region Receber Variaveis
                string AraTickParametersVarChanges = Page.Request["AraTickParametersVarChanges"];
                if (AraTickParametersVarChanges != null)
                {
                    foreach (dynamic Objs in DynamicJson.Parse(AraTickParametersVarChanges))
                    {
                        IAraObjectClienteServer TmpObj = ((IAraObjectClienteServer)vTick.Session.GetObject(Objs.objid));
                        if (TmpObj != null)
                        {
                            foreach (dynamic Changes in Objs.Changes)
                            {
                                try
                                {
                                    var vEvent = TmpObj.SetProperty.InvokeEvent;
                                    if (vEvent != null)
                                        vEvent(Changes.name, Changes.value);
                                }
                                catch (Exception err)
                                {
                                    ExceptionAplication(new Exception("Erro on SetProperty in '" + Objs.objid + "',type:'" + TmpObj.GetType().Name + "', name:'" + Changes.name + "' value:'" + Changes.value + "'.\n" + err.Message));
                                }
                            }
                        }
                    }

                    try
                    {
                        vTick.Session.SaveSession();
                    }
                    catch (Exception err)
                    {
                        ExceptionAplication(new Exception("Erro SaveSession in RecebeTick.\n" + err.Message));
                    }
                }

                #endregion


                if (Page.Request["ObjId"] == "Ara")
                {
                    RecebeEventoAra(Page.Request["Event"]);
                }
                else
                {
                    #region Executa Evento
                    try
                    {
                        string vEventName = Page.Request["Event"];

                        IAraObjectClienteServer vTmpObject = ((IAraObjectClienteServer)vTick.Session.GetObject(Page.Request["ObjId"]));

                        if (vTmpObject != null)
                        {
                            if (vTick.Session.OnReceivesInternalEventBefore.InvokeEvent != null)
                                vTick.Session.OnReceivesInternalEventBefore.InvokeEvent(vTmpObject, vEventName);

                            vTmpObject.EventInternal.InvokeEvent(Page.Request["Event"]);

                            if (vTick.Session.OnReceivesInternalEventAfter.InvokeEvent != null)
                                vTick.Session.OnReceivesInternalEventAfter.InvokeEvent(vTmpObject, vEventName);
                        }
                        //else
                        //    throw new Exception(" Object not found");
                    }
                    catch (Exception err)
                    {
                        ExceptionAplication(new Exception("Erro on EventInternal in '" + Page.Request["ObjId"] + "'' event '" + Page.Request["Event"] + "'.\n" + err.Message));
                    }
                    #endregion
                }

            }
            catch (Exception err)
            {
                ExceptionAplication(err);
            }


            vTick.Script.RumUnLoad();

            //if (Tick.ReturnisJavascript)
            //    if (ExecutedSynchronizewithAraThread.Length > 0)
            //        AraTools.ClearSynchronizewithAraThread(ExecutedSynchronizewithAraThread);


            vTick.Script.SendScriptsEnd();
        }

        private void EnviaKernel()
        {
            
            Tick vTick = Tick.GetTick();

            


            Response.Write("<!DOCTYPE html>\n");
            Response.Write("<html>\n");
            Response.Write("<head>\n");

            if (Request.Browser.Browser == "IE")
            {
                if (Request.Browser.MajorVersion < 9)
                {
                    Response.Write("<meta http-equiv='X-UA-Compatible' content='chrome=1'>\n");
                    Response.Write("<script type='text/javascript' src='http://ajax.googleapis.com/ajax/libs/chrome-frame/1/CFInstall.min.js'> </script>\n");
                }
            }

            //teste ZOOm
            //Response.Write("<meta name='viewport' content='target-densitydpi=device-dpi; width=device-width; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;' />\n");
            //Response.Write("<meta name='viewport' content='width=300px; ' />\n");
            //Response.Write("<meta name='viewport' content='width=350px;' />\n");
            if (ViewPort() != "")
                Response.Write("<meta name='viewport' content='" + ViewPort() + "' />\n");

            Response.Write("<meta http-equiv=\"Content-language\" content=\"pt-BR\">\n");
            Response.Write("<meta http-equiv=\"X-UA-Compatible\" content=\"IE=EDGE\" /> \n");

            

            #region CSS
            Response.Write("<link rel='stylesheet' type='text/css' href='" + GetJQueryUICss() + "'/>\n");
            Response.Write("<link rel='stylesheet' type='text/css' href='Ara2/Files/jQuery/css/AraJQuery.css'/>\n");
            //Response.Write("<link rel='stylesheet' type='text/css' href='Ara2/Files/JQueryScoolBar/jquery_mCustomScrollbar.css'/>\n");
            #endregion

            #region JS

            //Response.Write("<!--[if lt IE 9]> \n");
            //Response.Write("<script src=\"Ara2/Files/AraKernel/other/ie7/IE9.js\"></script> \n");
            //Response.Write("<![endif]-->\n");

            if (RedirectHttps())
                Response.Write("<script type='text/javascript' src='Ara2/Files/AraKernel/ClassAraHttps.js'></script> \n");

            Response.Write("<script type='text/javascript' src='Ara2/Files/jQuery/jquery-1_11_1_min.js'></script> \n");
            Response.Write("<script type='text/javascript' src='Ara2/Files/jQuery/jquery-migrate-1_2_1_min.js'></script> \n");
            Response.Write("<script type='text/javascript' src='Ara2/Files/jQuery/jquery-ui-1_11_2_custom_min.js'></script> \n");

            //Response.Write("<script type='text/javascript' src='Ara2/Files/jQuery/jquery-1_8_3.js'></script> \n");
            //Response.Write("<script type='text/javascript' src='Ara2/Files/jQuery/jquery-ui-1_9_2_custom.js'></script> \n");
            //Response.Write("<script type='text/javascript' src='Ara2/Files/jQuery/jquery-1_9_1.js'></script> \n");
            //Response.Write("<script type='text/javascript' src='Ara2/Files/jQuery/jquery-ui-1_10_3_custom_min.js'></script> \n");
            //Response.Write("<script type='text/javascript' src='Ara2/Files/jQuery/jquery-migrate-1_2_1_min.js'></script> \n");

            Response.Write("<script type='text/javascript' src='Ara2/Files/jQuery/jquery_ui_touch-punch.js'></script> \n");
            Response.Write("<script type='text/javascript' src='Ara2/Files/jQuery/jquery-cookie.js'></script> \n");

            //Response.Write("<script type='text/javascript' src='Ara2/Files/JQueryScoolBar/jquery_mCustomScrollbar.js'></script> \n");
            Response.Write("<script type='text/javascript' src='Ara2/Files/JQueryScoolBar/jquery_nicescroll.js'></script> \n");
            Response.Write("<script type='text/javascript' src='Ara2/Files/AraKernel/other/json2.js'></script> \n");
            Response.Write("<script type='text/javascript' src='Ara2/Files/AraKernel/other/jquery_selectboxes.js'></script> \n");
            Response.Write("<script type='text/javascript' src='Ara2/Files/AraKernel/other/jquery_printElement.js'></script> \n");

            Response.Write("<script type='text/javascript' src='Ara2/Files/AraKernel/ClassAraAnchor.js'></script> \n");
            Response.Write("<script type='text/javascript' src='Ara2/Files/AraKernel/ClassAraClass.js'></script> \n");
            Response.Write("<script type='text/javascript' src='Ara2/Files/AraKernel/ClassAraComponent.js'></script> \n");
            Response.Write("<script type='text/javascript' src='Ara2/Files/AraKernel/ClassAraGenVarSend.js'></script> \n");
            Response.Write("<script type='text/javascript' src='Ara2/Files/AraKernel/ClassAraTick.js'></script> \n");
            Response.Write("<script type='text/javascript' src='Ara2/Files/AraKernel/ClassAraTools.js'></script> \n");
            Response.Write("<script type='text/javascript' src='Ara2/Files/AraKernel/ClassAra.js'></script> \n");
            
            #endregion
            
            Response.Write("\n");
            Response.Write("</head>\n");

            Response.Write("<div id='DivLoadMain' style='background:white;position: fixed; left: 0px; top: 0px;bottom:1px;right:1px;z-index:1;'> <img src='Ara2/Files/Carregando.gif' style='position: fixed; left: 50%; top: 50%; padding: 10px 15px;'> </div>\n");
            Response.Write("<div id='DivFormModal' class='ui-widget-overlay' style='width: 100%; height: 100%; z-index: 1001;display:none;'></div>\n");
            

            Response.Write("<script>\n");
            Response.Write("var Ara = null;\n");
            Response.Write("$( document ).ready(function() {\n");
            Response.Write(" Ara = new ClassAra('" + vTick.Session.Id + "','" + Page.Request.Url.AbsolutePath + "'); \n");

            WindowMain WindowMain=null;
            try
            {
                try
                {
                    //Reseta Session
                    Sessions.ReNew(this, vTick.Session.Id);
                    vTick.Session = Sessions.GetSession(this, vTick.Session.Id);
                    vTick.Script.RumLoad();
                }
                catch (Exception err)
                {
                    ExceptionAplication(new Exception("Erro on reset session.\n" + err.Message));
                }

                BeforeReceivingTick();

                try
                {
                    WindowMain = GetWindowMain(vTick.Session);
                }
                catch (Exception err)
                {
                    ExceptionAplication(new Exception("Erro on load form Main.\n" + err.Message));
                }

                WindowMain.Show();                
            }
            catch(Exception err)
            {
                ExceptionAplication(new Exception("Erro WindowMain.\n" + err.Message));
            }
            finally
            {
                try
                {
                    vTick.Session.SaveSession();
                }
                catch (Exception errr)
                {
                    ExceptionAplication(new Exception("Erro on save session!\n" + errr.Message));
                }

                vTick.Script.Send(" Ara.EndLoadAra();\n");

                vTick.Script.RumUnLoad();

                vTick.Script.SendScriptsEnd();

                try
                {
                    Tick.DellTick(vTick);
                }
                catch { }

                Response.Write("\n});\n");
                Response.Write(" </script>\n");
                if (WindowMain!=null)
                    Response.Write(WindowMain.GetBodyHtml());

                Response.Write("</html>");
            }

            
        }

        #region OLD

        #region SendFileJS
        //public void SendFileJS(string vFile)
        //{
        //    SendFileJS(false, null, vFile);
        //}

        //public  void SendFileJS(bool? AraClass, int? AppId, string vFile)
        //{
        //    if (AraClass==null)
        //        AraClass = false;

        //    StreamReader stream = null;
        //    if (File.Exists(vFile))
        //        stream = new StreamReader(vFile);
        //    else if (File.Exists(System.IO.Path.Combine(AraTools.GetPath() , vFile.Replace("/", "\\"))))
        //        stream = new StreamReader(System.IO.Path.Combine(AraTools.GetPath(), vFile.Replace("/", "\\")));
        //    else
        //    {
        //        Assembly asm = Assembly.GetExecutingAssembly();
        //        stream = new StreamReader( asm.GetManifestResourceStream(vFile.Replace("/", ".").Replace("\\", ".")));
        //    }

        //    PageAtivaGZip();

        //    if (stream != null)
        //    {
        //        string eTag = Request.Headers["If-None-Match"];

        //        if (string.IsNullOrEmpty(eTag))
        //        {
        //            string vMD5 = AraToolsInternalStatic.GetMD5FileInternal(vFile, stream.BaseStream);
        //            Response.Cache.SetLastModified(DateTime.Now);
        //            Response.Cache.SetETag(vMD5);
        //        }
        //        else
        //        {
        //            if (AraToolsInternalStatic.GetFileInternalByMD5(eTag) == vFile)
        //            {
        //                Response.Clear();
        //                Response.StatusCode = (int)System.Net.HttpStatusCode.NotModified;
        //                Response.SuppressContent = true;
        //                return;
        //            }
        //        }




        //        Response.ContentType = "text/javascript";
        //        Response.Cache.SetCacheability(System.Web.HttpCacheability.Server);
        //        Response.BufferOutput = false;


        //        if (AraClass==true)
        //            Response.Write(" Ara.AraClass.NextClassAppId = " + AppId + ";\n");

        //        try
        //        {
        //            //stream.CopyTo(Response.OutputStream);
        //            Response.Write(stream.ReadToEnd());
        //        }
        //        catch { }

        //        if (AraClass == true)
        //            Response.Write("\n Ara.AraClass.EndLoadClass('" + Page.Request["Url"] + "'); \n");


        //    }
        //    else
        //    {
        //        // Arquivo não encontrado.
        //        Response.Clear();
        //        Response.StatusCode = 404;
        //        Response.End();
        //    }
        //}
        #endregion

        #region GZip

        //private void PageAtivaGZip()
        //{
        //    string AcceptEncoding = Request.Headers["Accept-Encoding"];

        //    if (!string.IsNullOrEmpty(AcceptEncoding) &&
        //             (AcceptEncoding.Contains("gzip") || AcceptEncoding.Contains("deflate")))
        //    {
        //        if (AcceptEncoding.Contains("gzip"))
        //        {
        //            Response.Filter = new System.IO.Compression.GZipStream(Response.Filter,
        //                                        System.IO.Compression.CompressionMode.Compress);
        //            Response.AppendHeader("Content-Encoding", "gzip");
        //        }
        //        else if (AcceptEncoding.Contains("deflate"))
        //        {
        //            Response.Filter = new System.IO.Compression.DeflateStream(Response.Filter,
        //                                        System.IO.Compression.CompressionMode.Compress);
        //            Response.AppendHeader("Content-Encoding", "deflate");
        //        }
                
        //    }
        //}
        #endregion

        #region SendFile
        //public void SendFile( string vFile)
        //{
        //    Assembly asm = Assembly.GetExecutingAssembly();
        //    Stream stream = asm.GetManifestResourceStream(vFile.Replace("/", ".").Replace("\\", "."));

        //    if (stream != null)
        //    {
        //        string eTag = Request.Headers["If-None-Match"];

        //        if (string.IsNullOrEmpty(eTag))
        //        {
        //            string vMD5 = AraToolsInternalStatic.GetMD5FileInternal(vFile, stream);
        //            Response.Cache.SetLastModified(DateTime.Now);
        //            Response.Cache.SetETag(vMD5);
        //        }
        //        else
        //        {
        //            if (AraToolsInternalStatic.GetFileInternalByMD5(eTag) == vFile)
        //            {
        //                Response.Clear();
        //                Response.StatusCode = (int)System.Net.HttpStatusCode.NotModified;
        //                Response.SuppressContent = true;
        //                return;
        //            }
        //        }


        //        System.Text.ASCIIEncoding EncodingToString = new System.Text.ASCIIEncoding();
        //        System.Text.UTF8Encoding EncodingToByte = new System.Text.UTF8Encoding();

        //        /*
        //        string vPath = "";
        //        if (vFile.LastIndexOf("/") > -1)
        //            vPath = Request.Path + "?GetFile=" + vFile.Substring(0, vFile.LastIndexOf("/") + 1);
        //        */
        //        string vPath = "?GetFile=" + vFile.Substring(0, vFile.LastIndexOf("/") + 1);

        //        string Estencao = vFile.Substring(vFile.LastIndexOf("."), vFile.Length - vFile.LastIndexOf(".")).ToLower();

        //        if (AraToolsInternalStatic.ContextTypeDic.ContainsKey(Estencao.ToLower().Substring(1)))
        //            Response.ContentType = AraToolsInternalStatic.ContextTypeDic[Estencao.ToLower().Substring(1)];
        //        else
        //            Response.ContentType = "text/plain";

        //        Response.Cache.SetCacheability(System.Web.HttpCacheability.Public);
        //        Response.BufferOutput = false;

        //        const int buffersize = 1024 * 16;
        //        var buffer = new byte[buffersize];
        //        int count = stream.Read(buffer, 0, buffersize);
        //        while (count > 0)
        //        {
        //            switch (Estencao.ToLower())
        //            {
        //                case ".css":
        //                case ".js":

        //                    string vTmp1 = EncodingToString.GetString(buffer).Replace("$PATH$", vPath);

        //                    if (vTmp1 != EncodingToString.GetString(buffer))
        //                    {
        //                        if (buffersize >= vTmp1.Length)
        //                            buffer = EncodingToByte.GetBytes(vTmp1);
        //                        else
        //                        {
        //                            count += (vTmp1.Length - buffersize);
        //                            buffer = new byte[vTmp1.Length];
        //                            buffer = EncodingToByte.GetBytes(vTmp1);
        //                        }
        //                    }
        //                    break;
        //            }

        //            Response.OutputStream.Write(buffer, 0, count);
        //            buffer = new byte[buffersize];
        //            count = stream.Read(buffer, 0, buffersize);
        //        }
        //    }
        //    else
        //    {
        //        // Arquivo não encontrado.
        //        Response.Clear();
        //        Response.StatusCode = 404;
        //        Response.End();
        //    }
        //}
        #endregion

        #endregion

        public void SendFileDownloadForce(string vFile)
        {
            if (!File.Exists(this.MapPath("~\\") + vFile))
            {
                // Arquivo não encontrado.
                Response.Clear();
                Response.StatusCode = 404;
                Response.End();
                return;
            }


            Stream stream = new StreamReader(MapPath("~\\") + vFile).BaseStream;

            if (stream != null)
            {
                //Response.ContentType = "application/x-msdownload";
                string Estencao = vFile.Substring(vFile.LastIndexOf("."), vFile.Length - vFile.LastIndexOf(".")).ToLower();

                if (AraToolsInternalStatic.ContextTypeDic.ContainsKey(Estencao.ToLower().Substring(1)))
                {
                    Response.ContentType = AraToolsInternalStatic.ContextTypeDic[Estencao.ToLower().Substring(1)];
                }
                else
                    Response.ContentType = "application/x-msdownload";

                Response.AddHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(vFile));
                //Response.Cache.SetCacheability(HttpCacheability.Public);
                Response.BufferOutput = false;

                const int buffersize = 1024 * 16;
                var buffer = new byte[buffersize];
                int count = stream.Read(buffer, 0, buffersize);
                while (count > 0)
                {
                    Response.OutputStream.Write(buffer, 0, count);
                    buffer = new byte[buffersize];
                    count = stream.Read(buffer, 0, buffersize);
                }
            }
            else
            {
                // Arquivo não encontrado.
                Response.Clear();
                Response.StatusCode = 404;
                Response.End();
            }
        }

        #region RandomString
        private static Random random = new Random((int)DateTime.Now.Ticks);//thanks to McAden
        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }
        #endregion

        #region SendFile
        static Dictionary<string,string> _KeySendFile = new Dictionary<string, string>();
        public string GetKeySendFile(string vFile)
        {
            string vRndKey = null;
            lock (_KeySendFile)
            {
                vRndKey = RandomString(20);
                while (_KeySendFile.ContainsKey(vRndKey))
                {
                    System.Threading.Thread.Sleep(50);
                    vRndKey = RandomString(20);
                }
                _KeySendFile.Add(vRndKey, vFile);
            }
            return vRndKey;
        }

        public void SendFile(string vKey)
        {
            try
            {
                //if (vFile.LastIndexOf("&") != -1)
                //    vFile = vFile.Substring(0, vFile.LastIndexOf("&"));
                //!File.Exists(vFile) ||
                string vFile;
                if (!_KeySendFile.TryGetValue(vKey,out vFile) || !File.Exists(vFile))
                {
                    // Arquivo não encontrado.
                    Response.Clear();
                    Response.StatusCode = 404;
                    //Response.Write("ChaveEncontrada :" + (_KeySendFile.Contains(vKey)?"S":"N"));
                    Response.End();
                    return;
                }
                else
                {
                    Stream stream = new StreamReader(vFile).BaseStream;
                    try
                    {

                        //Response.ContentType = "application/x-msdownload";
                        string Estencao = vFile.Substring(vFile.LastIndexOf("."), vFile.Length - vFile.LastIndexOf(".")).ToLower();

                        if (AraToolsInternalStatic.ContextTypeDic.ContainsKey(Estencao.ToLower().Substring(1)))
                            Response.ContentType = AraToolsInternalStatic.ContextTypeDic[Estencao.ToLower().Substring(1)];
                        else
                            Response.ContentType = "application/x-msdownload";

                        //Response.AddHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(vFile));
                        //Response.Cache.SetCacheability(HttpCacheability.Public);
                        Response.BufferOutput = false;

                        const int buffersize = 1024 * 16;
                        var buffer = new byte[buffersize];
                        int count = stream.Read(buffer, 0, buffersize);
                        while (count > 0)
                        {
                            Response.OutputStream.Write(buffer, 0, count);
                            buffer = new byte[buffersize];
                            count = stream.Read(buffer, 0, buffersize);
                        }
                    }
                    finally
                    {
                        stream.Close();
                    }
                }
            }
            finally
            {
                lock(_KeySendFile)
                {
                    _KeySendFile.Remove(vKey);
                }
            }
        }
        #endregion

        public virtual string GetUrlRedirectFiles(string vFile)
        {
            return vFile;
        }

        private void RecebeEventoAra(string vFunction)
        {
            Tick vTick = Tick.GetTick();

            switch (vFunction.ToUpper())
            {
                
                case "ONRESIZE":
                    {
                        IAraObject vObj = vTick.Session.GetObject(Request.Params["id"]);

                        if (vObj !=null && ((AraResizable)vObj).OnResize.InvokeEvent != null)
                            ((AraResizable)vTick.Session.GetObject(Request.Params["id"])).OnResize.InvokeEvent();
                    }
                    break;
                case "ONDRAGGABLE":
                    {
                        AraDraggable vD = ((AraDraggable)vTick.Session.GetObject(Request.Params["id"]));

                        if (vD.OnDraggable.InvokeEvent != null)
                            vD.OnDraggable.InvokeEvent(vD.ConteinerFather, Convert.ToDecimal(Request.Params["OldLeft"]), Convert.ToDecimal(Request.Params["OldTop"]));
                    }
                    break;
                case "SELECTEDSTOP":
                    if (((AraSelectable)vTick.Session.GetObject(Request.Params["id"])).Stop.InvokeEvent != null && Request.Params["ObjsInstanceID"].Trim() != "")
                        ((AraSelectable)vTick.Session.GetObject(Request.Params["id"])).Stop.InvokeEvent(Request.Params["ObjsInstanceID"].Split(',').Select(a=>vTick.Session.GetObject(a)).ToArray());
                    break;
                //case "UNSELECTED":
                //    if (((AraSelectable)vTick.Session.GetObject(Request.Params["id"])).UnSelected.InvokeEvent != null)
                //        ((AraSelectable)vTick.Session.GetObject(Request.Params["id"])).UnSelected.InvokeEvent(vTick.Session.GetObject(Request.Params["idselect"]));
                //    break;    
                case "ANTICLOSESESSION":
                    break;
                case "WARNINGLOADING":
                    vTick.Session.WindowMain.RumActionWaitLoading(Convert.ToInt32(Request.Params["key"]));
                    break;
                //case "ANALYSISRESULT":
                //    Tick.GetTick().Script.CustomerAnalysisShowResult(Request.Params["name"],DynamicJson.Parse( Request.Params["Analysis"]));
                //    break;
            }
        }
    }


    
}
