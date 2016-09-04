
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
    public abstract class FrmAraGridSearchLinqFilterStringAraDesign : Ara2.Components.AraWindow
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
       private AraObjectInstance<Ara2.Components.AraForeignKeyLinq> _cePesquisa = new AraObjectInstance<Ara2.Components.AraForeignKeyLinq>();
       public Ara2.Components.AraForeignKeyLinq cePesquisa
       {
          get { return _cePesquisa.Object; }
          set { _cePesquisa.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraLabel> _lFiltro = new AraObjectInstance<Ara2.Components.AraLabel>();
       public Ara2.Components.AraLabel lFiltro
       {
          get { return _lFiltro.Object; }
          set { _lFiltro.Object = value; }
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
       public abstract IQueryable<System.Object> cePesquisa_GetQuery();
       public abstract void bConfirmar_Click(System.Object sender,System.EventArgs e);
       public abstract void bCancelar_Click(System.Object sender,System.EventArgs e);
       #endregion 
        public FrmAraGridSearchLinqFilterStringAraDesign(IAraContainerClient vConteiner)
            : base(vConteiner)
        {
            #region Instances
            #region Propertys Main
            this.ZIndexWindow  = 1016;
            this.LayoutsString  = @"AAEAAAD/////AQAAAAAAAAAMAgAAAEJBcmEyLCBWZXJzaW9uPTEuMi41NzY2LjE5OTcyLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGwFAQAAABpBcmEyLkNvbXBvbmVudHMuQXJhTGF5b3V0cwYAAAAOX0xheW91dEN1cnJlbnQGTm9TYXZlB19SZW5kZXINTGlzdGAxK19pdGVtcwxMaXN0YDErX3NpemUPTGlzdGAxK192ZXJzaW9uAQAABAAAAQEbQXJhMi5Db21wb25lbnRzLkFyYUxheW91dFtdAgAAAAgIAgAAAAYDAAAADU1lbm9yIHF1ZSA5NTAAAAkEAAAAAgAAAAIAAAAHBAAAAAABAAAABAAAAAQZQXJhMi5Db21wb25lbnRzLkFyYUxheW91dAIAAAAJBQAAAAkGAAAADQIFBQAAABlBcmEyLkNvbXBvbmVudHMuQXJhTGF5b3V0BQAAAAROYW1lFkxheW91dEN1cnJlbnRXaWR0aExlc3MXTGF5b3V0Q3VycmVudEhlaWdodExlc3MLRGV2aWNlVHlwZXMGQ2hpbGRzAQMDBANuU3lzdGVtLk51bGxhYmxlYDFbW1N5c3RlbS5JbnQzMiwgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XV1uU3lzdGVtLk51bGxhYmxlYDFbW1N5c3RlbS5JbnQzMiwgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XV0SQXJhMi5FRGV2aWNlVHlwZVtdAgAAAIgBU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuTGlzdGAxW1tBcmEyLkNvbXBvbmVudHMuQXJhTGF5b3V0T2JqZWN0LCBBcmEyLCBWZXJzaW9uPTEuMi41NzY2LjE5OTcyLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGxdXQIAAAAKCgoKCQcAAAABBgAAAAUAAAAJAwAAAAgItgMAAAoJCQAAAAkKAAAABAcAAACIAVN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLkxpc3RgMVtbQXJhMi5Db21wb25lbnRzLkFyYUxheW91dE9iamVjdCwgQXJhMiwgVmVyc2lvbj0xLjIuNTc2Ni4xOTk3MiwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxsXV0DAAAABl9pdGVtcwVfc2l6ZQhfdmVyc2lvbgQAACFBcmEyLkNvbXBvbmVudHMuQXJhTGF5b3V0T2JqZWN0W10CAAAACAgJCwAAAAAAAAAjBAAABwkAAAAAAQAAAAIAAAAEEEFyYTIuRURldmljZVR5cGUCAAAABfT///8QQXJhMi5FRGV2aWNlVHlwZQEAAAAHdmFsdWVfXwAIAgAAAAAAAAAB8/////T///8BAAAAAQoAAAAHAAAACQ4AAAAMAAAA7QoAAAcLAAAAAAEAAAAgAAAABB9BcmEyLkNvbXBvbmVudHMuQXJhTGF5b3V0T2JqZWN0AgAAAA0gBw4AAAAAAQAAACAAAAAEH0FyYTIuQ29tcG9uZW50cy5BcmFMYXlvdXRPYmplY3QCAAAACQ8AAAAJEAAAAAkRAAAACRIAAAAJEwAAAAkUAAAACRUAAAAJFgAAAAkXAAAACRgAAAAJGQAAAAkaAAAADRQFDwAAAB9BcmEyLkNvbXBvbmVudHMuQXJhTGF5b3V0T2JqZWN0AwAAAAROYW1lCk5hbWVGYXRoZXIJUHJvcGVydHlzAQED4gFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5EaWN0aW9uYXJ5YDJbW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV0sW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dAgAAAAYbAAAAD1RhYnNUaXBvc0ZpbHRybwYcAAAAAAkdAAAAARAAAAAPAAAABh4AAAAKVGFiU2ltcGxlcwkbAAAACSAAAAABEQAAAA8AAAAGIQAAABFHcmlkRmlsdHJvU2ltcGxlcwkeAAAACSMAAAABEgAAAA8AAAAGJAAAAAxUYWJNdWx0aXBsb3MJGwAAAAkmAAAAARMAAAAPAAAABicAAAASYkFkaWNpb25hckFvRmlsdHJvCSQAAAAJKQAAAAEUAAAADwAAAAYqAAAACmNlUGVzcXVpc2EJJAAAAAksAAAAARUAAAAPAAAABi0AAAAHbEZpbHRybwkkAAAACS8AAAABFgAAAA8AAAAGMAAAAAZBME8yOTUJHAAAAAkyAAAAARcAAAAPAAAABjMAAAAGQTBPMzAxCTAAAAAJNQAAAAEYAAAADwAAAAY2AAAACmJDb25maXJtYXIJMwAAAAk4AAAAARkAAAAPAAAABjkAAAAGQTBPMzExCTAAAAAJOwAAAAEaAAAADwAAAAY8AAAACWJDYW5jZWxhcgk5AAAACT4AAAAEHQAAAOIBU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuRGljdGlvbmFyeWAyW1tTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldLFtTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldXQQAAAAHVmVyc2lvbghDb21wYXJlcghIYXNoU2l6ZQ1LZXlWYWx1ZVBhaXJzAAMAAwiSAVN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLkdlbmVyaWNFcXVhbGl0eUNvbXBhcmVyYDFbW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dCOYBU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuS2V5VmFsdWVQYWlyYDJbW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV0sW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9u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U3R5bGVDb250YWluZXIJZQAAAAF2////qf///waLAAAAC0FuY2hvci5MZWZ0BowAAAABMQFz////qf///waOAAAACkFuY2hvci5Ub3AGjwAAAAI0MQFw////qf///waRAAAADEFuY2hvci5SaWdodAaSAAAAATEBbf///6n///8GlAAAAA1BbmNob3IuQm90dG9tBpUAAAABMQFq////qf///waXAAAADkFuY2hvci5DZW50ZXJICWUAAAABZ////6n///8GmgAAAA5BbmNob3IuQ2VudGVyVgllAAAAAWT///+p////Bp0AAAAHVmlzaWJsZQlrAAAAAWH///+p////BqAAAAAETGVmdAahAAAAAzFweAFe////qf///wajAAAAA1RvcAakAAAABDQxcHgBW////6n///8GpgAAAAhNaW5XaWR0aAanAAAABDEwcHgBWP///6n///8GqQAAAAlNaW5IZWlnaHQGqgAAAAQxMHB4AVX///+p////BqwAAAAFV2lkdGgGrQAAAAU2MTFweAFS////qf///wavAAAABkhlaWdodAawAAAABTI3MHB4AU////+p////BrIAAAAMVHlwZVBvc2l0aW9uCYAAAAABTP///6n///8GtQAAAAZaSW5kZXgGtgAAAAExB0QAAAAAAQAAABAAAAAD5AFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5LZXlWYWx1ZVBhaXJgMltbU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XSxbU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XV0BSf///6n///8GuAAAAA5TdHlsZUNvbnRhaW5lcgllAAAAAUb///+p////BrsAAAALQW5jaG9yLkxlZnQGvAAAAAE1AUP///+p////Br4AAAAKQW5jaG9yLlRvcAa/AAAAATUBQP///6n///8GwQAAAAxBbmNob3IuUmlnaHQGwgAAAAE1AT3///+p////BsQAAAANQW5jaG9yLkJvdHRvbQbFAAAAATUBOv///6n///8GxwAAAA5BbmNob3IuQ2VudGVySAllAAAAATf///+p////BsoAAAAOQW5jaG9yLkNlbnRlclYJZQAAAAE0////qf///wbNAAAAB1Zpc2libGUJawAAAAEx////qf///wbQAAAABExlZnQG0QAAAAM1cHgBLv///6n///8G0wAAAANUb3AG1AAAAAM1cHgBK////6n///8G1gAAAAhNaW5XaWR0aAbXAAAABDEwcHgBKP///6n///8G2QAAAAlNaW5IZWlnaHQG2gAAAAQxMHB4ASX///+p////BtwAAAAFV2lkdGgG3QAAAAU2MDFweAEi////qf///wbfAAAABkhlaWdodAbgAAAABTI2MHB4AR////+p////BuIAAAAMVHlwZVBvc2l0aW9uCYAAAAABHP///6n///8G5QAAAAZaSW5kZXgG5gAAAAExB0YAAAAAAQAAABEAAAAD5AFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5LZXlWYWx1ZVBhaXJgMltbU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XSxbU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XV0BGf///6n///8JhQAAAAkcAAAAARb///+p////CYgAAAAJZQAAAAET////qf///wbuAAAAC0FuY2hvci5MZWZ0Bu8AAAABMQEQ////qf///wbxAAAACkFuY2hvci5Ub3AG8gAAAAI0MQEN////qf///wb0AAAADEFuY2hvci5SaWdodAb1AAAAATEBCv///6n///8G9wAAAA1BbmNob3IuQm90dG9tBvgAAAABMQEH////qf///wb6AAAADkFuY2hvci5DZW50ZXJICWUAAAABBP///6n///8G/QAAAA5BbmNob3IuQ2VudGVyVgllAAAAAQH///+p////CZ0AAAAJawAAAAH+/v//qf///wmgAAAABgQBAAADMXB4Afv+//+p////CaMAAAAGBwEAAAQ0MXB4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AAxUeXBlUG9zaXRpb24JgAAAAAF5/v//qf///waIAQAABlpJbmRleAaJAQAAATIHTAAAAAABAAAAFAAAAAPkAVN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLktleVZhbHVlUGFpcmAyW1tTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldLFtTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldXQF2/v//qf///waLAQAABFRleHQGjAEAABU8Yj5GaWx0cmFuZG8gcG9yOjwvYj4Bc/7//6n///8GjgEAAAdWaXNpYmxlCWsAAAABcP7//6n///8GkQEAAAxTeWxlT3ZlcmZsb3cGkgEAAARub25lAW3+//+p////BpQBAAAJVGV4dEFsaWduBpUBAAAEbGVmdAFq/v//qf///waXAQAAClRleHRWQWxpZ24GmAEAAARub25lAWf+//+p////BpoBAAAPVGV4dFZBbGlnblZhbHVlBpsBAAADMHB4AWT+//+p////Bp0BAAALQW5jaG9yLkxlZnQGngEAAAE1AWH+//+p////BqABAAAKQW5jaG9yLlRvcAahAQAAAjQ1AV7+//+p////BqMBAAAMQW5jaG9yLlJpZ2h0BqQBAAABNQFb/v//qf///wamAQAADUFuY2hvci5Cb3R0b20GpwEAAAE1AVj+//+p////BqkBAAAOQW5jaG9yLkNlbnRlckgJZQAAAAFV/v//qf///wasAQAADkFuY2hvci5DZW50ZXJWCWUAAAABUv7//6n///8GrwEAAARMZWZ0BrABAAADNXB4AU/+//+p////BrIBAAADVG9wBrMBAAAENDVweAFM/v//qf///wa1AQAACE1pbldpZHRoBrYBAAAEMTBweAFJ/v//qf///wa4AQAACU1pbkhlaWdodAa5AQAABDE3cHgBRv7//6n///8GuwEAAAVXaWR0aAa8AQAABTYwMXB4AUP+//+p////Br4BAAAGSGVpZ2h0Br8BAAAFMjIwcHgB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RmlsdHJvAdz9//+p////CR4BAAAJHwEAAAHZ/f//qf///wkhAQAACR8BAAAB1v3//6n///8GKwIAAAtBbmNob3IuTGVmdAYsAgAAATUB0/3//6n///8GLgIAAApBbmNob3IuVG9wCgHR/f//qf///wYwAgAADEFuY2hvci5SaWdodAYxAgAAATUBzv3//6n///8GMwIAAA1BbmNob3IuQm90dG9tBjQCAAABNQHL/f//qf///wY2AgAADkFuY2hvci5DZW50ZXJICWUAAAAByP3//6n///8GOQIAAA5BbmNob3IuQ2VudGVyVgllAAAAAcX9//+p////CTMBAAAJawAAAAHC/f//qf///wk2AQAABkACAAADNXB4Ab/9//+p////CTkBAAAGQwIAAAM1cHgBvP3//6n///8JPAEAAAZGAgAABDEwcHgBuf3//6n///8JPwEAAAZJAgAABDI1cHgBtv3//6n///8JQgEAAAZMAgAABTI5NnB4AbP9//+p////CUUBAAAGTwIAAAQyNXB4AbD9//+p////CUgBAAAJgAAAAAGt/f//qf///wlLAQAABlUCAAABMQdUAAAAAAEAAAARAAAAA+QBU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuS2V5VmFsdWVQYWlyYDJbW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV0sW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dAar9//+p////CfMBAAAJHAAAAAGn/f//qf///wn2AQAACWUAAAABpP3//6n///8GXQIAAAtBbmNob3IuTGVmdAoBov3//6n///8GXwIAAApBbmNob3IuVG9wCgGg/f//qf///wZhAgAADEFuY2hvci5SaWdodAoBnv3//6n///8GYwIAAA1BbmNob3IuQm90dG9tCgGc/f//qf///wZlAgAADkFuY2hvci5DZW50ZXJICWUAAAABmf3//6n///8GaAIAAA5BbmNob3IuQ2VudGVyVgllAAAAAZb9//+p////CQcCAAAJawAAAAGT/f//qf///wkKAgAABm8CAAAHMzA2LjVweAGQ/f//qf///wkNAgAABnICAAADMHB4AY39//+p////CRACAAAGdQIAAAQxMHB4AYr9//+p////CRMCAAAGeAIAAAQxMHB4AYf9//+p////CRYCAAAGewIAAAUzMDZweAGE/f//qf///wkZAgAABn4CAAAEMzVweAGB/f//qf///wkcAgAACYAAAAABfv3//6n///8JHwIAAAaEAgAAATIHVgAAAAABAAAAEgAAAAPkAVN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLktleVZhbHVlUGFpcmAyW1tTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldLFtTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldXQF7/f//qf///wkbAQAABocCAAAIQ2FuY2VsYXIBeP3//6n///8JHgEAAAkfAQAAAXX9//+p////CSEBAAAJHwEAAAFy/f//qf///waPAgAAC0FuY2hvci5MZWZ0BpACAAABNQFv/f//qf///waSAgAACkFuY2hvci5Ub3AKAW39//+p////BpQCAAAMQW5jaG9yLlJpZ2h0BpUCAAABNQFq/f//qf///waXAgAADUFuY2hvci5Cb3R0b20GmAIAAAE1AWf9//+p////BpoCAAAOQW5jaG9yLkNlbnRlckgJZQAAAAFk/f//qf///wadAgAADkFuY2hvci5DZW50ZXJWCWUAAAABYf3//6n///8JMwEAAAlrAAAAAV79//+p////CTYBAAAGpAIAAAM1cHgBW/3//6n///8JOQEAAAanAgAAAzVweAFY/f//qf///wk8AQAABqoCAAAEMTBweAFV/f//qf///wk/AQAABq0CAAAEMjVweAFS/f//qf///wlCAQAABrACAAAFMjk2cHgBT/3//6n///8JRQEAAAazAgAABDI1cHgBTP3//6n///8JSAEAAAmAAAAAAUn9//+p////CUsBAAAGuQIAAAExCwAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==";
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
            this.TabSimples.TabActive  = true;
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
            this.bAdicionarAoFiltro.Left  =  new Ara2.Components.AraDistance(@"506px");
            this.bAdicionarAoFiltro.Top  =  new Ara2.Components.AraDistance(@"11px");
            this.bAdicionarAoFiltro.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
            this.bAdicionarAoFiltro.MinHeight  =  new Ara2.Components.AraDistance(@"25px");
            this.bAdicionarAoFiltro.Width  =  new Ara2.Components.AraDistance(@"100px");
            this.bAdicionarAoFiltro.Height  =  new Ara2.Components.AraDistance(@"25px");
            this.bAdicionarAoFiltro.ZIndex  = 1;
            this.bAdicionarAoFiltro.Click  += bAdicionarAoFiltro_Click;
            #endregion
            #region cePesquisa
            this.cePesquisa = new Ara2.Components.AraForeignKeyLinq(this.TabMultiplos);

            this.cePesquisa.Name = "cePesquisa";
            this.cePesquisa.ColunaCodigo  = @"NOME";
            this.cePesquisa.ColunaNome  = @"NOME";
            this.cePesquisa.VisibleCode  = false;
            this.cePesquisa.VisibleSelectButton  = true;
            this.cePesquisa.Anchor.Left  = 5;
            this.cePesquisa.Anchor.Right  = 120;
            this.cePesquisa.Left  =  new Ara2.Components.AraDistance(@"5px");
            this.cePesquisa.Top  =  new Ara2.Components.AraDistance(@"8px");
            this.cePesquisa.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
            this.cePesquisa.MinHeight  =  new Ara2.Components.AraDistance(@"10px");
            this.cePesquisa.Width  =  new Ara2.Components.AraDistance(@"486px");
            this.cePesquisa.Height  =  new Ara2.Components.AraDistance(@"25px");
            this.cePesquisa.ZIndex  = 2;
            this.cePesquisa.GetQuery  += cePesquisa_GetQuery;
            #endregion
            #region lFiltro
            this.lFiltro = new Ara2.Components.AraLabel(this.TabMultiplos);

            this.lFiltro.Name = "lFiltro";
            this.lFiltro.Text  = @"<b>Filtrando por:</b>";
            this.lFiltro.TextVAlignValue  =  new Ara2.Components.AraDistance(@"0px");
            this.lFiltro.Anchor.Left  = 5;
            this.lFiltro.Anchor.Top  = 45;
            this.lFiltro.Anchor.Right  = 5;
            this.lFiltro.Anchor.Bottom  = 5;
            this.lFiltro.Left  =  new Ara2.Components.AraDistance(@"5px");
            this.lFiltro.Top  =  new Ara2.Components.AraDistance(@"45px");
            this.lFiltro.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
            this.lFiltro.MinHeight  =  new Ara2.Components.AraDistance(@"17px");
            this.lFiltro.Width  =  new Ara2.Components.AraDistance(@"601px");
            this.lFiltro.Height  =  new Ara2.Components.AraDistance(@"220px");
            this.lFiltro.ZIndex  = 3;
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
