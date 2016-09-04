
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
    public abstract class FrmAraGridSearchLinqExportXLSxAraDesign : Ara2.Components.AraWindow
    {
    
       #region Objects
       private AraObjectInstance<Ara2.Components.AraButton> _bCancelar = new AraObjectInstance<Ara2.Components.AraButton>();
       public Ara2.Components.AraButton bCancelar
       {
          get { return _bCancelar.Object; }
          set { _bCancelar.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraLabel> _lEstadoExportacao = new AraObjectInstance<Ara2.Components.AraLabel>();
       public Ara2.Components.AraLabel lEstadoExportacao
       {
          get { return _lEstadoExportacao.Object; }
          set { _lEstadoExportacao.Object = value; }
       }
       #endregion 
       #region Events
       public abstract void bCancelar_Click(System.Object sender,System.EventArgs e);
       #endregion 
        public FrmAraGridSearchLinqExportXLSxAraDesign(IAraContainerClient vConteiner)
            : base(vConteiner)
        {
            #region Instances
            #region Propertys Main
            this.Title  = @"Exportação para XLSX";
            this.ZIndexWindow  = 1020;
            this.Visible  = false;
            this.Left  =  new Ara2.Components.AraDistance(@"0px");
            this.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
            this.MinHeight  =  new Ara2.Components.AraDistance(@"10px");
            this.Width  =  new Ara2.Components.AraDistance(@"627px");
            this.Height  =  new Ara2.Components.AraDistance(@"142px");
            #endregion
            
            
            #region bCancelar
            this.bCancelar = new Ara2.Components.AraButton(this);

            this.bCancelar.Name = "bCancelar";
            this.bCancelar.Text  = @"Cancelar";
            this.bCancelar.Anchor.Right  = 5;
            this.bCancelar.Anchor.Bottom  = 5;
            this.bCancelar.Left  =  new Ara2.Components.AraDistance(@"482px");
            this.bCancelar.Top  =  new Ara2.Components.AraDistance(@"112px");
            this.bCancelar.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
            this.bCancelar.MinHeight  =  new Ara2.Components.AraDistance(@"25px");
            this.bCancelar.Width  =  new Ara2.Components.AraDistance(@"140px");
            this.bCancelar.Height  =  new Ara2.Components.AraDistance(@"25px");
            this.bCancelar.ZIndex  = 10;
            this.bCancelar.Click  += bCancelar_Click;
            #endregion
            #region lEstadoExportacao
            this.lEstadoExportacao = new Ara2.Components.AraLabel(this);

            this.lEstadoExportacao.Name = "lEstadoExportacao";
            this.lEstadoExportacao.Anchor.Left  = 5;
            this.lEstadoExportacao.Anchor.Top  = 5;
            this.lEstadoExportacao.Anchor.Right  = 5;
            this.lEstadoExportacao.Anchor.Bottom  = 40;
            this.lEstadoExportacao.Left  =  new Ara2.Components.AraDistance(@"5px");
            this.lEstadoExportacao.Top  =  new Ara2.Components.AraDistance(@"5px");
            this.lEstadoExportacao.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
            this.lEstadoExportacao.MinHeight  =  new Ara2.Components.AraDistance(@"17px");
            this.lEstadoExportacao.Width  =  new Ara2.Components.AraDistance(@"617px");
            this.lEstadoExportacao.Height  =  new Ara2.Components.AraDistance(@"97px");
            this.lEstadoExportacao.ZIndex  = 11;
            #endregion
            #endregion
        } 
    } 
} 
