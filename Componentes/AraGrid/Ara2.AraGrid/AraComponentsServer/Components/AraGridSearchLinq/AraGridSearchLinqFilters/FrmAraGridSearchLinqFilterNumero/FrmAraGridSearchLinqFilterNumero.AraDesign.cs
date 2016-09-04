
/*
    NÂO ALTERAR ESTE ARQUIVO SEM O EDITOR ARA.DEV !
    DO NOT CHANGE THIS FILE WITHOUT THE EDITOR ARA.DEV!

 _   _          ____             _   _______ ______ _____            _____    ______  _____ _______ ______            _____   ____  _    _ _______      ______  
| \ | |   /\   / __ \      /\   | | |__   __|  ____|  __ \     /\   |  __ \  |  ____|/ ____|__   __|  ____|     /\   |  __ \ / __ \| |  | |_   _\ \    / / __ \ 
|  \| |  /  \ | |  | |    /  \  | |    | |  | |__  | |__) |   /  \  | |__) | | |__  | (___    | |  | |__       /  \  | |__) | |  | | |  | | | |  \ \  / / |  | |
| . ` | / /\ \| |  | |   / /\ \ | |    | |  |  __| |  _  /   / /\ \ |  _  /  |  __|  \___ \   | |  |  __|     / /\ \ |  _  /| |  | | |  | | | |   \ \/ /| |  | |
| |\  |/ ____ \ |__| |  / ____ \| |____| |  | |____| | \ \  / ____ \| | \ \  | |____ ____) |  | |  | |____   / ____ \| | \ \| |__| | |__| |_| |_   \  / | |__| |
|_| \_/_/    \_\____/  /_/    \_\______|_|  |______|_|  \_\/_/    \_\_|  \_\ |______|_____/   |_|  |______| /_/    \_\_|  \_\\___\_\\____/|_____|   \/   \____/ 
                                                                                                                                                                

Ara2.Dev 1.0

*/



using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ara2;
using Ara2.Components;


namespace AraDesign
{
    [Serializable]
    public abstract class FrmAraGridSearchLinqFilterNumeroAraDesign : Ara2.Components.AraWindow
    {
    
       #region Objects
       private AraObjectInstance<Ara2.Components.AraSplitContainers> _A0O295 = new AraObjectInstance<Ara2.Components.AraSplitContainers>();
       public Ara2.Components.AraSplitContainers A0O295
       {
          get { return _A0O295.Object; }
          set { _A0O295.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraTabs> _TabsTiposFiltro = new AraObjectInstance<Ara2.Components.AraTabs>();
       public Ara2.Components.AraTabs TabsTiposFiltro
       {
          get { return _TabsTiposFiltro.Object; }
          set { _TabsTiposFiltro.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraTab> _TabSimples = new AraObjectInstance<Ara2.Components.AraTab>();
       public Ara2.Components.AraTab TabSimples
       {
          get { return _TabSimples.Object; }
          set { _TabSimples.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraTab> _TabMultiplos = new AraObjectInstance<Ara2.Components.AraTab>();
       public Ara2.Components.AraTab TabMultiplos
       {
          get { return _TabMultiplos.Object; }
          set { _TabMultiplos.Object = value; }
       }
       private AraObjectInstance<Ara2.Grid.AraGridSearchLinq> _GridFiltroSimples = new AraObjectInstance<Ara2.Grid.AraGridSearchLinq>();
       public Ara2.Grid.AraGridSearchLinq GridFiltroSimples
       {
          get { return _GridFiltroSimples.Object; }
          set { _GridFiltroSimples.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraButton> _bAdicionarAoFiltro = new AraObjectInstance<Ara2.Components.AraButton>();
       public Ara2.Components.AraButton bAdicionarAoFiltro
       {
          get { return _bAdicionarAoFiltro.Object; }
          set { _bAdicionarAoFiltro.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraLabel> _lFiltro = new AraObjectInstance<Ara2.Components.AraLabel>();
       public Ara2.Components.AraLabel lFiltro
       {
          get { return _lFiltro.Object; }
          set { _lFiltro.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraTextBox> _txtCondicao = new AraObjectInstance<Ara2.Components.AraTextBox>();
       public Ara2.Components.AraTextBox txtCondicao
       {
          get { return _txtCondicao.Object; }
          set { _txtCondicao.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraLabel> _A0O264 = new AraObjectInstance<Ara2.Components.AraLabel>();
       public Ara2.Components.AraLabel A0O264
       {
          get { return _A0O264.Object; }
          set { _A0O264.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraSplitContainer> _A0O301 = new AraObjectInstance<Ara2.Components.AraSplitContainer>();
       public Ara2.Components.AraSplitContainer A0O301
       {
          get { return _A0O301.Object; }
          set { _A0O301.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraSplitContainer> _A0O311 = new AraObjectInstance<Ara2.Components.AraSplitContainer>();
       public Ara2.Components.AraSplitContainer A0O311
       {
          get { return _A0O311.Object; }
          set { _A0O311.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraButton> _bConfirmar = new AraObjectInstance<Ara2.Components.AraButton>();
       public Ara2.Components.AraButton bConfirmar
       {
          get { return _bConfirmar.Object; }
          set { _bConfirmar.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraButton> _bCancelar = new AraObjectInstance<Ara2.Components.AraButton>();
       public Ara2.Components.AraButton bCancelar
       {
          get { return _bCancelar.Object; }
          set { _bCancelar.Object = value; }
       }
       #endregion 
       #region Events
       public abstract IQueryable<System.Object> GridFiltroSimples_GetQuery();
       public abstract void GridFiltroSimples_ReturnSearch(Ara2.Components.AraGrid Object,Ara2.Components.AraGridRow Row);
       public abstract void bAdicionarAoFiltro_Click(System.Object sender,System.EventArgs e);
       public abstract void bConfirmar_Click(System.Object sender,System.EventArgs e);
       public abstract void bCancelar_Click(System.Object sender,System.EventArgs e);
       #endregion 
        public FrmAraGridSearchLinqFilterNumeroAraDesign(IAraContainerClient vConteiner)
            : base(vConteiner)
        {
            #region Instances
            #region Propertys Main
            this.ZIndexWindow  = 1016;
            this.LayoutsString  = @"AAEAAAD/////AQAAAAAAAAAMAgAAAEJBcmEyLCBWZXJzaW9uPTEuMi41NzY2LjE5OTcyLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGwFAQAAABpBcmEyLkNvbXBvbmVudHMuQXJhTGF5b3V0cwYAAAAOX0xheW91dEN1cnJlbnQGTm9TYXZlB19SZW5kZXINTGlzdGAxK19pdGVtcwxMaXN0YDErX3NpemUPTGlzdGAxK192ZXJzaW9uAQAABAAAAQEbQXJhMi5Db21wb25lbnRzLkFyYUxheW91dFtdAgAAAAgIAgAAAAYDAAAADU1lbm9yIHF1ZSA5NTAAAAkEAAAAAgAAAAIAAAAHBAAAAAABAAAABAAAAAQZQXJhMi5Db21wb25lbnRzLkFyYUxheW91dAIAAAAJBQAAAAkGAAAADQIFBQAAABlBcmEyLkNvbXBvbmVudHMuQXJhTGF5b3V0BQAAAAROYW1lFkxheW91dEN1cnJlbnRXaWR0aExlc3MXTGF5b3V0Q3VycmVudEhlaWdodExlc3MLRGV2aWNlVHlwZXMGQ2hpbGRzAQMDBANuU3lzdGVtLk51bGxhYmxlYDFbW1N5c3RlbS5JbnQzMiwgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XV1uU3lzdGVtLk51bGxhYmxlYDFbW1N5c3RlbS5JbnQzMiwgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XV0SQXJhMi5FRGV2aWNlVHlwZVtdAgAAAIgBU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuTGlzdGAxW1tBcmEyLkNvbXBvbmVudHMuQXJhTGF5b3V0T2JqZWN0LCBBcmEyLCBWZXJzaW9uPTEuMi41NzY2LjE5OTcyLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQIAAAAKCgoKCQcAAAABBgAAAAUAAAAJAwAAAAgItgMAAAoJCQAAAAkKAAAABAcAAACIAVN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLkxpc3RgMVtbQXJhMi5Db21wb25lbnRzLkFyYUxheW91dE9iamVjdCwgQXJhMiwgVmVyc2lvbj0xLjIuNTc2Ni4xOTk3MiwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0DAAAABl9pdGVtcwVfc2l6ZQhfdmVyc2lvbgQAACFBcmEyLkNvbXBvbmVudHMuQXJhTGF5b3V0T2JqZWN0W10CAAAACAgJCwAAAAAAAAAjBAAABwkAAAAAAQAAAAIAAAAEEEFyYTIuRURldmljZVR5cGUCAAAABfT///8QQXJhMi5FRGV2aWNlVHlwZQEAAAAHdmFsdWVfXwAIAgAAAAAAAAAB8/////T///8BAAAAAQoAAAAHAAAACQ4AAAANAAAAKQwAAAcLAAAAAAEAAAAgAAAABB9BcmEyLkNvbXBvbmVudHMuQXJhTGF5b3V0T2JqZWN0AgAAAA0gBw4AAAAAAQAAACAAAAAEH0FyYTIuQ29tcG9uZW50cy5BcmFMYXlvdXRPYmplY3QCAAAACQ8AAAAJEAAAAAkRAAAACRIAAAAJEwAAAAkUAAAACRUAAAAJFgAAAAkXAAAACRgAAAAJGQAAAAkaAAAACRsAAAANEwUPAAAAH0FyYTIuQ29tcG9uZW50cy5BcmFMYXlvdXRPYmplY3QDAAAABE5hbWUKTmFtZUZhdGhlcglQcm9wZXJ0eXMBAQPiAVN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLkRpY3Rpb25hcnlgMltbU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XSxbU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XV0CAAAABhwAAAAPVGFic1RpcG9zRmlsdHJvBh0AAAAACR4AAAABEAAAAA8AAAAGHwAAAApUYWJTaW1wbGVzCRwAAAAJIQAAAAERAAAADwAAAAYiAAAAEUdyaWRGaWx0cm9TaW1wbGVzCR8AAAAJJAAAAAESAAAADwAAAAYlAAAADFRhYk11bHRpcGxvcwkcAAAACScAAAABEwAAAA8AAAAGKAAAABJiQWRpY2lvbmFyQW9GaWx0cm8JJQAAAAkqAAAAARQAAAAPAAAABisAAAAHbEZpbHRybwklAAAACS0AAAABFQAAAA8AAAAGLgAAAAt0eHRDb25kaWNhbwklAAAACTAAAAABFgAAAA8AAAAGMQAAAAZBME8yNjQJJQAAAAkzAAAAARcAAAAPAAAABjQAAAAGQTBPMjk1CR0AAAAJNgAAAAEYAAAADwAAAAY3AAAABkEwTzMwMQk0AAAACTkAAAABGQAAAA8AAAAGOgAAAApiQ29uZmlybWFyCTcAAAAJPAAAAAEaAAAADwAAAAY9AAAABkEwTzMxMQk0AAAACT8AAAABGwAAAA8AAAAGQAAAAAliQ2FuY2VsYXIJPQAAAAlCAAAABB4AAADiAVN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLkRpY3Rpb25hcnlgMltbU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XSxbU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XV0EAAAAB1ZlcnNpb24IQ29tcGFyZXIISGFzaFNpemUNS2V5VmFsdWVQYWlycwADAAMIkgFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5HZW5lcmljRXF1YWxpdHlDb21wYXJlcmAxW1tTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldXQjmAVN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLktleVZhbHVlUGFpcmAyW1tTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldLFtTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldXVtdDwAAAAlDAAAAEQAAAAlEAAAAASEAAAAeAAAAEQAAAAlDAAAAEQAAAAlGAAAAASQAAAAeAAAAEAAAAAlDAAAAEQAAAAlIAAAAAScAAAAeAAAAEQAAAAlDAAAAEQAAAAlKAAAAASoAAAAeAAAAEgAAAAlDAAAAJQAAAAlMAAAAAS0AAAAeAAAAFAAAAAlDAAAAJQAAAAlOAAAAATAAAAAeAAAADwAAAAlDAAAAEQAAAAlQAAAAATMAAAAeAAAAFAAAAAlDAAAAJQAAAAlSAAAAATYAAAAeAAAADwAAAAlDAAAAEQAAAAlUAAAAATkAAAAeAAAAEQAAAAlDAAAAEQAAAAlWAAAAATwAAAAeAAAAEgAAAAlDAAAAJQAAAAlYAAAAAT8AAAAeAAAAEQAAAAlDAAAAEQAAAAlaAAAAAUIAAAAeAAAAEgAAAAlDAAAAJQAAAAlcAAAABEMAAACSAVN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLkdlbmVyaWNFcXVhbGl0eUNvbXBhcmVyYDFbW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dAAAAAAdEAAAAAAEAAAAPAAAAA+QBU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuS2V5VmFsdWVQYWlyYDJbW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV0sW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dBKP////kAVN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLktleVZhbHVlUGFpcmAyW1tTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldLFtTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldXQIAAAADa2V5BXZhbHVlAQEGXgAAAAtBbmNob3IuTGVmdAZfAAAAATUBoP///6P///8GYQAAAApBbmNob3IuVG9wBmIAAAABNQGd////o////wZkAAAADEFuY2hvci5SaWdodAZlAAAAATUBmv///6P///8GZwAAAA1BbmNob3IuQm90dG9tBmgAAAACNTABl////6P///8GagAAAA5BbmNob3IuQ2VudGVySAZrAAAABUZhbHNlAZT///+j////Bm0AAAAOQW5jaG9yLkNlbnRlclYJawAAAAGR////o////wZwAAAAB1Zpc2libGUGcQAAAARUcnVlAY7///+j////BnMAAAAETGVmdAZ0AAAAAzVweAGL////o////wZ2AAAAA1RvcAZ3AAAAAzVweAGI////o////wZ5AAAACE1pbldpZHRoBnoAAAAENDBweAGF////o////wZ8AAAACU1pbkhlaWdodAZ9AAAABDQwcHgBgv///6P///8GfwAAAAVXaWR0aAaAAAAABTYxM3B4AX////+j////BoIAAAAGSGVpZ2h0BoMAAAAFMzEycHgBfP///6P///8GhQAAAAxUeXBlUG9zaXRpb24GhgAAAAhBYnNvbHV0ZQF5////o////waIAAAABlpJbmRleAaJAAAAATUHRgAAAAABAAAAEQAAAAPkAVN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLktleVZhbHVlUGFpcmAyW1tTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldLFtTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldXQF2////o////waLAAAABFRleHQJHQAAAAFz////o////waOAAAADlN0eWxlQ29udGFpbmVyCWsAAAABcP///6P///8GkQAAAAtBbmNob3IuTGVmdAaSAAAAATEBbf///6P///8GlAAAAApBbmNob3IuVG9wBpUAAAACNDEBav///6P///8GlwAAAAxBbmNob3IuUmlnaHQGmAAAAAExAWf///+j////BpoAAAANQW5jaG9yLkJvdHRvbQabAAAAATEBZP///6P///8GnQAAAA5BbmNob3IuQ2VudGVySAlrAAAAAWH///+j////BqAAAAAOQW5jaG9yLkNlbnRlclYJawAAAAFe////o////wajAAAAB1Zpc2libGUJcQAAAAFb////o////wamAAAABExlZnQGpwAAAAMxcHgBWP///6P///8GqQAAAANUb3AGqgAAAAQ0MXB4AVX///+j////BqwAAAAITWluV2lkdGgGrQAAAAQxMHB4AVL///+j////Bq8AAAAJTWluSGVpZ2h0BrAAAAAEMTBweAFP////o////wayAAAABVdpZHRoBrMAAAAFNjExcHgBTP///6P///8GtQAAAAZIZWlnaHQGtgAAAAUyNzBweAFJ////o////wa4AAAADFR5cGVQb3NpdGlvbgmGAAAAAUb///+j////BrsAAAAGWkluZGV4BrwAAAABMQdIAAAAAAEAAAAQAAAAA+QBU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuS2V5VmFsdWVQYWlyYDJbW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV0sW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dAUP///+j////Br4AAAAOU3R5bGVDb250YWluZXIJawAAAAFA////o////wbBAAAAC0FuY2hvci5MZWZ0BsIAAAABNQE9////o////wbEAAAACkFuY2hvci5Ub3AGxQAAAAE1ATr///+j////BscAAAAMQW5jaG9yLlJpZ2h0BsgAAAABNQE3////o////wbKAAAADUFuY2hvci5Cb3R0b20GywAAAAE1ATT///+j////Bs0AAAAOQW5jaG9yLkNlbnRlckgJawAAAAEx////o////wbQAAAADkFuY2hvci5DZW50ZXJWCWsAAAABLv///6P///8G0wAAAAdWaXNpYmxlCXEAAAABK////6P///8G1gAAAARMZWZ0BtcAAAADNXB4ASj///+j////BtkAAAADVG9wBtoAAAADNXB4ASX///+j////BtwAAAAITWluV2lkdGgG3QAAAAQxMHB4ASL///+j////Bt8AAAAJTWluSGVpZ2h0BuAAAAAEMTBweAEf////o////wbiAAAABVdpZHRoBuMAAAAFNjAxcHgBHP///6P///8G5QAAAAZIZWlnaHQG5gAAAAUyNjBweAEZ////o////wboAAAADFR5cGVQb3NpdGlvbgmGAAAAARb///+j////BusAAAAGWkluZGV4BuwAAAABMQdKAAAAAAEAAAARAAAAA+QBU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuS2V5VmFsdWVQYWlyYDJbW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV0sW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dARP///+j////CYsAAAAJHQAAAAEQ////o////wmOAAAACWsAAAABDf///6P///8G9AAAAAtBbmNob3IuTGVmdAb1AAAAATEBCv///6P///8G9wAAAApBbmNob3IuVG9wBvgAAAACNDEBB////6P///8G+gAAAAxBbmNob3IuUmlnaHQG+wAAAAExAQT///+j////Bv0AAAANQW5jaG9yLkJvdHRvbQb+AAAAATEBAf///6P///8GAAEAAA5BbmNob3IuQ2VudGVySAlrAAAAAf7+//+j////BgMBAAAOQW5jaG9yLkNlbnRlclYJawAAAAH7/v//o////wmjAAAACXEAAAAB+P7//6P///8JpgAAAAYKAQAAAzFweAH1/v//o////wmpAAAABg0BAAAENDFweAHy/v//o////wmsAAAABhABAAAEMTBweAHv/v//o////wmvAAAABhMBAAAEMTBweAHs/v//o////wmyAAAABhYBAAAFNjExcHgB6f7//6P///8JtQAAAAYZAQAABTI3MHB4Aeb+//+j////CbgAAAAJhgAAAAHj/v//o////wm7AAAABh8BAAABMgdMAAAAAAEAAAASAAAAA+QBU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuS2V5VmFsdWVQYWlyYDJbW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV0sW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dAeD+//+j////BiEBAAAEVGV4dAYiAQAACkFkLiBGaWx0cm8B3f7//6P///8GJAEAAANJY28GJQEAAAROb25lAdr+//+j////BicBAAAMSWNvU2Vjb25kYXJ5CSUBAAAB1/7//6P///8GKgEAAAtBbmNob3IuTGVmdAoB1f7//6P///8GLAEAAApBbmNob3IuVG9wCgHT/v//o////wYuAQAADEFuY2hvci5SaWdodAYvAQAAATUB0P7//6P///8GMQEAAA1BbmNob3IuQm90dG9tCgHO/v//o////wYzAQAADkFuY2hvci5DZW50ZXJICWsAAAABy/7//6P///8GNgEAAA5BbmNob3IuQ2VudGVyVglrAAAAAcj+//+j////BjkBAAAHVmlzaWJsZQlxAAAAAcX+//+j////BjwBAAAETGVmdAY9AQAABTUwNHB4AcL+//+j////Bj8BAAADVG9wBkABAAAEMjVweAG//v//o////wZCAQAACE1pbldpZHRoBkMBAAAEMTBweAG8/v//o////wZFAQAACU1pbkhlaWdodAZGAQAABDI1cHgBuf7//6P///8GSAEAAAVXaWR0aAZJ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cmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldLFtTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldXQFG/v//o////wlUAQAABrwBAAA4PGI+Q29uZGnDp8Ojbzo8L2I+IDxzbWFsbD4gKEV4OiAiPSAxIiBvdSAiPiAxIiBvdSAiPCAxIikBQ/7//6P///8JVwEAAAlxAAAAAUD+//+j////CVoBAAAJWwEAAAE9/v//o////wldAQAACV4BAAABOv7//6P///8JYAEAAAlhAQAAATf+//+j////CWMBAAAKATX+//+j////BswBAAALQW5jaG9yLkxlZnQKATP+//+j////Bs4BAAAKQW5jaG9yLlRvcAoBMf7//6P///8G0AEAAAxBbmNob3IuUmlnaHQKAS/+//+j////BtIBAAANQW5jaG9yLkJvdHRvbQoBLf7//6P///8G1AEAAA5BbmNob3IuQ2VudGVySAlrAAAAASr+//+j////BtcBAAAOQW5jaG9yLkNlbnRlclYJawAAAAEn/v//o////wl4AQAABtsBAAADM3B4AST+//+j////CXsBAAAG3gEAAAM2cHgBIf7//6P///8JfgEAAAbhAQAABDEwcHgBHv7//6P///8JgQEAAAbkAQAABDE3cHgBG/7//6P///8JhAEAAAbnAQAABTI1NXB4ARj+//+j////CYcBAAAG6gEAAAQxN3B4ARX+//+j////CYoBAAAJhgAAAAES/v//o////wmNAQAABvABAAABNQdUAAAAAAEAAAAPAAAAA+QBU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuS2V5VmFsdWVQYWlyYDJbW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV0sW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dAQ/+//+j////BvIBAAALQW5jaG9yLkxlZnQG8wEAAAE1AQz+//+j////BvUBAAAKQW5jaG9yLlRvcAoBCv7//6P///8G9wEAAAxBbmNob3IuUmlnaHQG+AEAAAE1AQf+//+j////BvoBAAANQW5jaG9yLkJvdHRvbQb7AQAAATUBBP7//6P///8G/QEAAA5BbmNob3IuQ2VudGVySAlrAAAAAQH+//+j////BgACAAAOQW5jaG9yLkNlbnRlclYJawAAAAH+/f//o////wYDAgAAB1Zpc2libGUJcQAAAAH7/f//o////wYGAgAABExlZnQGBwIAAAM1cHgB+P3//6P///8GCQIAAANUb3AGCgIAAAUzMjdweAH1/f//o////wYMAgAACE1pbldpZHRoBg0CAAAEMjBweAHy/f//o////wYPAgAACU1pbkhlaWdodAYQAgAABDEwcHgB7/3//6P///8GEgIAAAVXaWR0aAYTAgAABTYxM3B4Aez9//+j////BhUCAAAGSGVpZ2h0BhYCAAAEMzVweAHp/f//o////wYYAgAADFR5cGVQb3NpdGlvbgmGAAAAAeb9//+j////BhsCAAAGWkluZGV4BhwCAAABNAdWAAAAAAEAAAARAAAAA+QBU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuS2V5VmFsdWVQYWlyYDJbW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV0sW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dAeP9//+j////Bh4CAAAEVGV4dAkdAAAAAeD9//+j////BiECAAAOU3R5bGVDb250YWluZXIJawAAAAHd/f//o////wYkAgAAC0FuY2hvci5MZWZ0CgHb/f//o////wYmAgAACkFuY2hvci5Ub3AKAdn9//+j////BigCAAAMQW5jaG9yLlJpZ2h0CgHX/f//o////wYqAgAADUFuY2hvci5Cb3R0b20KAdX9//+j////BiwCAAAOQW5jaG9yLkNlbnRlckgJawAAAAHS/f//o////wYvAgAADkFuY2hvci5DZW50ZXJWCWsAAAABz/3//6P///8GMgIAAAdWaXNpYmxlCXEAAAABzP3//6P///8GNQIAAARMZWZ0BjYCAAADMHB4Acn9//+j////BjgCAAADVG9wBjkCAAADMHB4Acb9//+j////BjsCAAAITWluV2lkdGgGPAIAAAQxMHB4AcP9//+j////Bj4CAAAJTWluSGVpZ2h0Bj8CAAAEMTBweAHA/f//o////wZBAgAABVdpZHRoBkICAAAFMzA2cHgBvf3//6P///8GRAIAAAZIZWlnaHQGRQIAAAQzNXB4Abr9//+j////BkcCAAAMVHlwZVBvc2l0aW9uCYYAAAABt/3//6P///8GSgIAAAZaSW5kZXgGSwIAAAExB1gAAAAAAQAAABIAAAAD5AFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5LZXlWYWx1ZVBhaXJgMltbU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XSxbU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XV0BtP3//6P///8JIQEAAAZOAgAAEENvbmZpcm1hciBGaWx0cm8Bsf3//6P///8JJAEAAAklAQAAAa79//+j////CScBAAAJJQEAAAGr/f//o////wZWAgAAC0FuY2hvci5MZWZ0BlcCAAABNQGo/f//o////wZZAgAACkFuY2hvci5Ub3AKAab9//+j////BlsCAAAMQW5jaG9yLlJpZ2h0BlwCAAABNQGj/f//o////wZeAgAADUFuY2hvci5Cb3R0b20GXwIAAAE1AaD9//+j////BmECAAAOQW5jaG9yLkNlbnRlckgJawAAAAGd/f//o////wZkAgAADkFuY2hvci5DZW50ZXJWCWsAAAABmv3//6P///8JOQEAAAlxAAAAAZf9//+j////CTwBAAAGawIAAAM1cHgBlP3//6P///8JPwEAAAZuAgAAAzVweAGR/f//o////wlCAQAABnECAAAEMTBweAGO/f//o////wlFAQAABnQCAAAEMjVweAGL/f//o////wlIAQAABncCAAAFMjk2cHgBiP3//6P///8JSwEAAAZ6AgAABDI1cHgBhf3//6P///8JTgEAAAmGAAAAAYL9//+j////CVEBAAAGgAIAAAExB1oAAAAAAQAAABEAAAAD5AFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5LZXlWYWx1ZVBhaXJgMltbU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XSxbU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XV0Bf/3//6P///8JHgIAAAkdAAAAAXz9//+j////CSECAAAJawAAAAF5/f//o////waIAgAAC0FuY2hvci5MZWZ0CgF3/f//o////waKAgAACkFuY2hvci5Ub3AKAXX9//+j////BowCAAAMQW5jaG9yLlJpZ2h0CgFz/f//o////waOAgAADUFuY2hvci5Cb3R0b20KAXH9//+j////BpACAAAOQW5jaG9yLkNlbnRlckgJawAAAAFu/f//o////waTAgAADkFuY2hvci5DZW50ZXJWCWsAAAABa/3//6P///8JMgIAAAlxAAAAAWj9//+j////CTUCAAAGmgIAAAczMDYuNXB4AWX9//+j////CTgCAAAGnQIAAAMwcHgBYv3//6P///8JOwIAAAagAgAABDEwcHgBX/3//6P///8JPgIAAAajAgAABDEwcHgBXP3//6P///8JQQIAAAamAgAABTMwNnB4AVn9//+j////CUQCAAAGqQIAAAQzNXB4AVb9//+j////CUcCAAAJhgAAAAFT/f//o////wlKAgAABq8CAAABMgdcAAAAAAEAAAASAAAAA+QBU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuS2V5VmFsdWVQYWlyYDJbW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV0sW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dAVD9//+j////CSEBAAAGsgIAAAhDYW5jZWxhcgFN/f//o////wkkAQAACSUBAAABSv3//6P///8JJwEAAAklAQAAAUf9//+j////BroCAAALQW5jaG9yLkxlZnQGuwIAAAE1AUT9//+j////Br0CAAAKQW5jaG9yLlRvcAoBQv3//6P///8GvwIAAAxBbmNob3IuUmlnaHQGwAIAAAE1AT/9//+j////BsICAAANQW5jaG9yLkJvdHRvbQbDAgAAATUBPP3//6P///8GxQIAAA5BbmNob3IuQ2VudGVySAlrAAAAATn9//+j////BsgCAAAOQW5jaG9yLkNlbnRlclYJawAAAAE2/f//o////wk5AQAACXEAAAABM/3//6P///8JPAEAAAbPAgAAAzVweAEw/f//o////wk/AQAABtICAAADNXB4AS39//+j////CUIBAAAG1QIAAAQxMHB4ASr9//+j////CUUBAAAG2AIAAAQyNXB4ASf9//+j////CUgBAAAG2wIAAAUyOTZweAEk/f//o////wlLAQAABt4CAAAEMjVweAEh/f//o////wlOAQAACYYAAAABHv3//6P///8JUQEAAAbkAgAAATELAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==";
            this.Layouts.NoSave = true;
            this.LayoutCurrent  = @"Menor que 950";
            this.Visible  = false;
            this.Left  =  new Ara2.Components.AraDistance(@"0px");
            this.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
            this.MinHeight  =  new Ara2.Components.AraDistance(@"10px");
            this.Width  =  new Ara2.Components.AraDistance(@"623px");
            this.Height  =  new Ara2.Components.AraDistance(@"367px");
            #endregion
            
            
            #region A0O295
            this.A0O295 = new Ara2.Components.AraSplitContainers(this);

            this.A0O295.Name = "A0O295";
            this.A0O295.Anchor.Left  = 5;
            this.A0O295.Anchor.Right  = 5;
            this.A0O295.Anchor.Bottom  = 5;
            this.A0O295.Left  =  new Ara2.Components.AraDistance(@"5px");
            this.A0O295.Top  =  new Ara2.Components.AraDistance(@"327px");
            this.A0O295.MinWidth  =  new Ara2.Components.AraDistance(@"20px");
            this.A0O295.MinHeight  =  new Ara2.Components.AraDistance(@"10px");
            this.A0O295.Width  =  new Ara2.Components.AraDistance(@"613px");
            this.A0O295.Height  =  new Ara2.Components.AraDistance(@"35px");
            this.A0O295.ZIndex  = 4;
            #endregion
            #region TabsTiposFiltro
            this.TabsTiposFiltro = new Ara2.Components.AraTabs(this);

            this.TabsTiposFiltro.Name = "TabsTiposFiltro";
            this.TabsTiposFiltro.Anchor.Left  = 5;
            this.TabsTiposFiltro.Anchor.Top  = 5;
            this.TabsTiposFiltro.Anchor.Right  = 5;
            this.TabsTiposFiltro.Anchor.Bottom  = 50;
            this.TabsTiposFiltro.Left  =  new Ara2.Components.AraDistance(@"5px");
            this.TabsTiposFiltro.Top  =  new Ara2.Components.AraDistance(@"5px");
            this.TabsTiposFiltro.MinWidth  =  new Ara2.Components.AraDistance(@"40px");
            this.TabsTiposFiltro.MinHeight  =  new Ara2.Components.AraDistance(@"40px");
            this.TabsTiposFiltro.Width  =  new Ara2.Components.AraDistance(@"613px");
            this.TabsTiposFiltro.Height  =  new Ara2.Components.AraDistance(@"312px");
            this.TabsTiposFiltro.ZIndex  = 5;
            #endregion
            #region TabSimples
            this.TabSimples = new Ara2.Components.AraTab(this.TabsTiposFiltro);

            this.TabSimples.Name = "TabSimples";
            this.TabSimples.Caption  = @"Filtro Simples";
            this.TabSimples.Pos  = 0;
            this.TabSimples.Anchor.Left  = 1;
            this.TabSimples.Anchor.Top  = 41;
            this.TabSimples.Anchor.Right  = 1;
            this.TabSimples.Anchor.Bottom  = 1;
            this.TabSimples.Left  =  new Ara2.Components.AraDistance(@"1px");
            this.TabSimples.Top  =  new Ara2.Components.AraDistance(@"41px");
            this.TabSimples.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
            this.TabSimples.MinHeight  =  new Ara2.Components.AraDistance(@"10px");
            this.TabSimples.Width  =  new Ara2.Components.AraDistance(@"611px");
            this.TabSimples.Height  =  new Ara2.Components.AraDistance(@"270px");
            this.TabSimples.ZIndex  = 1;
            #endregion
            #region TabMultiplos
            this.TabMultiplos = new Ara2.Components.AraTab(this.TabsTiposFiltro);

            this.TabMultiplos.Name = "TabMultiplos";
            this.TabMultiplos.Caption  = @"Filtro Múltiplo";
            this.TabMultiplos.Pos  = 1;
            this.TabMultiplos.TabActive  = true;
            this.TabMultiplos.Anchor.Left  = 1;
            this.TabMultiplos.Anchor.Top  = 41;
            this.TabMultiplos.Anchor.Right  = 1;
            this.TabMultiplos.Anchor.Bottom  = 1;
            this.TabMultiplos.Left  =  new Ara2.Components.AraDistance(@"1px");
            this.TabMultiplos.Top  =  new Ara2.Components.AraDistance(@"41px");
            this.TabMultiplos.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
            this.TabMultiplos.MinHeight  =  new Ara2.Components.AraDistance(@"10px");
            this.TabMultiplos.Width  =  new Ara2.Components.AraDistance(@"611px");
            this.TabMultiplos.Height  =  new Ara2.Components.AraDistance(@"270px");
            this.TabMultiplos.ZIndex  = 2;
            #endregion
            #region GridFiltroSimples
            this.GridFiltroSimples = new Ara2.Grid.AraGridSearchLinq(this.TabSimples);

            this.GridFiltroSimples.Name = "GridFiltroSimples";
            this.GridFiltroSimples.Anchor.Left  = 5;
            this.GridFiltroSimples.Anchor.Top  = 5;
            this.GridFiltroSimples.Anchor.Right  = 5;
            this.GridFiltroSimples.Anchor.Bottom  = 5;
            this.GridFiltroSimples.Left  =  new Ara2.Components.AraDistance(@"5px");
            this.GridFiltroSimples.Top  =  new Ara2.Components.AraDistance(@"5px");
            this.GridFiltroSimples.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
            this.GridFiltroSimples.MinHeight  =  new Ara2.Components.AraDistance(@"10px");
            this.GridFiltroSimples.Width  =  new Ara2.Components.AraDistance(@"601px");
            this.GridFiltroSimples.Height  =  new Ara2.Components.AraDistance(@"260px");
            this.GridFiltroSimples.ZIndex  = 1;
            this.GridFiltroSimples.GetQuery  += GridFiltroSimples_GetQuery;
            this.GridFiltroSimples.ReturnSearch  += GridFiltroSimples_ReturnSearch;
            #endregion
            #region bAdicionarAoFiltro
            this.bAdicionarAoFiltro = new Ara2.Components.AraButton(this.TabMultiplos);

            this.bAdicionarAoFiltro.Name = "bAdicionarAoFiltro";
            this.bAdicionarAoFiltro.Text  = @"Ad. Filtro";
            this.bAdicionarAoFiltro.Anchor.Right  = 5;
            this.bAdicionarAoFiltro.Left  =  new Ara2.Components.AraDistance(@"504px");
            this.bAdicionarAoFiltro.Top  =  new Ara2.Components.AraDistance(@"25px");
            this.bAdicionarAoFiltro.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
            this.bAdicionarAoFiltro.MinHeight  =  new Ara2.Components.AraDistance(@"25px");
            this.bAdicionarAoFiltro.Width  =  new Ara2.Components.AraDistance(@"100px");
            this.bAdicionarAoFiltro.Height  =  new Ara2.Components.AraDistance(@"25px");
            this.bAdicionarAoFiltro.ZIndex  = 1;
            this.bAdicionarAoFiltro.Click  += bAdicionarAoFiltro_Click;
            #endregion
            #region lFiltro
            this.lFiltro = new Ara2.Components.AraLabel(this.TabMultiplos);

            this.lFiltro.Name = "lFiltro";
            this.lFiltro.Text  = @"<b>Filtrando por:</b>";
            this.lFiltro.TextVAlignValue  =  new Ara2.Components.AraDistance(@"0px");
            this.lFiltro.Anchor.Left  = 5;
            this.lFiltro.Anchor.Top  = 60;
            this.lFiltro.Anchor.Right  = 5;
            this.lFiltro.Anchor.Bottom  = 5;
            this.lFiltro.Left  =  new Ara2.Components.AraDistance(@"5px");
            this.lFiltro.Top  =  new Ara2.Components.AraDistance(@"60px");
            this.lFiltro.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
            this.lFiltro.MinHeight  =  new Ara2.Components.AraDistance(@"17px");
            this.lFiltro.Width  =  new Ara2.Components.AraDistance(@"601px");
            this.lFiltro.Height  =  new Ara2.Components.AraDistance(@"205px");
            this.lFiltro.ZIndex  = 3;
            #endregion
            #region txtCondicao
            this.txtCondicao = new Ara2.Components.AraTextBox(this.TabMultiplos);

            this.txtCondicao.Name = "txtCondicao";
            this.txtCondicao.Anchor.Left  = 5;
            this.txtCondicao.Anchor.Right  = 120;
            this.txtCondicao.Left  =  new Ara2.Components.AraDistance(@"5px");
            this.txtCondicao.Top  =  new Ara2.Components.AraDistance(@"29px");
            this.txtCondicao.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
            this.txtCondicao.MinHeight  =  new Ara2.Components.AraDistance(@"17px");
            this.txtCondicao.Width  =  new Ara2.Components.AraDistance(@"486px");
            this.txtCondicao.Height  =  new Ara2.Components.AraDistance(@"17px");
            this.txtCondicao.ZIndex  = 4;
            #endregion
            #region A0O264
            this.A0O264 = new Ara2.Components.AraLabel(this.TabMultiplos);

            this.A0O264.Name = "A0O264";
            this.A0O264.Text  = @"<b>Condição:</b> <small> (Ex: ""= 1"" ou ""> 1"" ou ""< 1"")";
            this.A0O264.Left  =  new Ara2.Components.AraDistance(@"3px");
            this.A0O264.Top  =  new Ara2.Components.AraDistance(@"6px");
            this.A0O264.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
            this.A0O264.MinHeight  =  new Ara2.Components.AraDistance(@"17px");
            this.A0O264.Width  =  new Ara2.Components.AraDistance(@"255px");
            this.A0O264.Height  =  new Ara2.Components.AraDistance(@"17px");
            this.A0O264.ZIndex  = 5;
            #endregion
            #region A0O301
            this.A0O301 = new Ara2.Components.AraSplitContainer(this.A0O295);

            this.A0O301.Name = "A0O301";
            this.A0O301.Percent  = 50;
            this.A0O301.Left  =  new Ara2.Components.AraDistance(@"0px");
            this.A0O301.Top  =  new Ara2.Components.AraDistance(@"0px");
            this.A0O301.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
            this.A0O301.MinHeight  =  new Ara2.Components.AraDistance(@"10px");
            this.A0O301.Width  =  new Ara2.Components.AraDistance(@"306px");
            this.A0O301.Height  =  new Ara2.Components.AraDistance(@"35px");
            this.A0O301.ZIndex  = 1;
            #endregion
            #region A0O311
            this.A0O311 = new Ara2.Components.AraSplitContainer(this.A0O295);

            this.A0O311.Name = "A0O311";
            this.A0O311.Percent  = 50;
            this.A0O311.Left  =  new Ara2.Components.AraDistance(@"306.5px");
            this.A0O311.Top  =  new Ara2.Components.AraDistance(@"0px");
            this.A0O311.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
            this.A0O311.MinHeight  =  new Ara2.Components.AraDistance(@"10px");
            this.A0O311.Width  =  new Ara2.Components.AraDistance(@"306px");
            this.A0O311.Height  =  new Ara2.Components.AraDistance(@"35px");
            this.A0O311.ZIndex  = 2;
            #endregion
            #region bConfirmar
            this.bConfirmar = new Ara2.Components.AraButton(this.A0O301);

            this.bConfirmar.Name = "bConfirmar";
            this.bConfirmar.Text  = @"Confirmar Filtro";
            this.bConfirmar.Anchor.Left  = 5;
            this.bConfirmar.Anchor.Right  = 5;
            this.bConfirmar.Anchor.Bottom  = 5;
            this.bConfirmar.Left  =  new Ara2.Components.AraDistance(@"5px");
            this.bConfirmar.Top  =  new Ara2.Components.AraDistance(@"5px");
            this.bConfirmar.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
            this.bConfirmar.MinHeight  =  new Ara2.Components.AraDistance(@"25px");
            this.bConfirmar.Width  =  new Ara2.Components.AraDistance(@"296px");
            this.bConfirmar.Height  =  new Ara2.Components.AraDistance(@"25px");
            this.bConfirmar.ZIndex  = 1;
            this.bConfirmar.Click  += bConfirmar_Click;
            #endregion
            #region bCancelar
            this.bCancelar = new Ara2.Components.AraButton(this.A0O311);

            this.bCancelar.Name = "bCancelar";
            this.bCancelar.Text  = @"Cancelar";
            this.bCancelar.Anchor.Left  = 5;
            this.bCancelar.Anchor.Right  = 5;
            this.bCancelar.Anchor.Bottom  = 5;
            this.bCancelar.Left  =  new Ara2.Components.AraDistance(@"5px");
            this.bCancelar.Top  =  new Ara2.Components.AraDistance(@"5px");
            this.bCancelar.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
            this.bCancelar.MinHeight  =  new Ara2.Components.AraDistance(@"25px");
            this.bCancelar.Width  =  new Ara2.Components.AraDistance(@"296px");
            this.bCancelar.Height  =  new Ara2.Components.AraDistance(@"25px");
            this.bCancelar.ZIndex  = 1;
            this.bCancelar.Click  += bCancelar_Click;
            #endregion
            
            
            #region Layouts Reander
            this.Layouts.Render();
            #endregion
            #endregion
        } 
    } 
} 
