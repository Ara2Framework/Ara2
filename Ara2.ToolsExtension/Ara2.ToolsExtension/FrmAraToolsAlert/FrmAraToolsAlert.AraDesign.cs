// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14

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
  public abstract class FrmAraToolsAlertAraDesign : Ara2.Components.AraWindow
  {
  
       #region Objects
       private AraObjectInstance<Ara2.Components.AraLabel> _lMsg = new AraObjectInstance<Ara2.Components.AraLabel>();
       public Ara2.Components.AraLabel lMsg
       {
          get { return _lMsg.Object; }
          set { _lMsg.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraButton> _bOk = new AraObjectInstance<Ara2.Components.AraButton>();
       public Ara2.Components.AraButton bOk
       {
          get { return _bOk.Object; }
          set { _bOk.Object = value; }
       }
       #endregion 
       #region Events
       public abstract void bOk_Click(System.Object sender,System.EventArgs e);
       #endregion 
       public FrmAraToolsAlertAraDesign(IAraContainerClient vConteiner) : base(vConteiner)
       {
           #region Instances
           #region Propertys Main
           this.Title  = @"Alerta";
           this.ZIndexWindow  = 1000004;
           this.Visible  = false;
           this.Width  =  new Ara2.Components.AraDistance(@"473px");
           this.Height  =  new Ara2.Components.AraDistance(@"217px");
           #endregion


           #region lMsg
           this.lMsg = new Ara2.Components.AraLabel(this);

           this.lMsg.Anchor.Left  = 10;
           this.lMsg.Anchor.Top  = 10;
           this.lMsg.Anchor.Right  = 10;
           this.lMsg.Anchor.Bottom  = 50;
           this.lMsg.Left  =  new Ara2.Components.AraDistance(@"10px");
           this.lMsg.Top  =  new Ara2.Components.AraDistance(@"10px");
           this.lMsg.Width  =  new Ara2.Components.AraDistance(@"453px");
           this.lMsg.Height  =  new Ara2.Components.AraDistance(@"157px");
           this.lMsg.ZIndex  = 3;
           #endregion
           #region bOk
           this.bOk = new Ara2.Components.AraButton(this);

           this.bOk.Text  = @"<font size=4>OK</font>";
           this.bOk.Anchor.Bottom  = 10;
           this.bOk.Anchor.CenterH  = true;
           this.bOk.Left  =  new Ara2.Components.AraDistance(@"186.5px");
           this.bOk.Top  =  new Ara2.Components.AraDistance(@"172px");
           this.bOk.Width  =  new Ara2.Components.AraDistance(@"100px");
           this.bOk.Height  =  new Ara2.Components.AraDistance(@"35px");
           this.bOk.ZIndex  = 8;
           this.bOk.Click  += bOk_Click;
           #endregion
           #endregion
       } 
   } 
} 
