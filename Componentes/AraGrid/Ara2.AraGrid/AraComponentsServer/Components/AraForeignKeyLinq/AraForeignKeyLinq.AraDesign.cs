
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
using System.ComponentModel;

namespace AraDesign
{
  [Serializable]
  [Browsable(false)]
  public abstract class AraForeignKeyLinqAraDesign : Ara2.Components.AraDiv
  {
  
       #region Objects
       private AraObjectInstance<Ara2.Grid.AraFormSearchLinqButton> _bPesquisa = new AraObjectInstance<Ara2.Grid.AraFormSearchLinqButton>();
       public Ara2.Grid.AraFormSearchLinqButton bPesquisa
       {
          get { return _bPesquisa.Object; }
          set { _bPesquisa.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraButton> _bNew = new AraObjectInstance<Ara2.Components.AraButton>();
       public Ara2.Components.AraButton bNew
       {
          get { return _bNew.Object; }
          set { _bNew.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraTextBox> _txtCodigo = new AraObjectInstance<Ara2.Components.AraTextBox>();
       public Ara2.Components.AraTextBox txtCodigo
       {
          get { return _txtCodigo.Object; }
          set { _txtCodigo.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraTextBox> _txtNome = new AraObjectInstance<Ara2.Components.AraTextBox>();
       public Ara2.Components.AraTextBox txtNome
       {
          get { return _txtNome.Object; }
          set { _txtNome.Object = value; }
       }
       private AraObjectInstance<Ara2.Components.AraButton> _bSelect = new AraObjectInstance<Ara2.Components.AraButton>();
       public Ara2.Components.AraButton bSelect
       {
          get { return _bSelect.Object; }
          set { _bSelect.Object = value; }
       }
       #endregion 
       #region Events
       public abstract void bPesquisa_Return(System.Object vObjReturn);
       public abstract void bNew_Click(System.Object sender,System.EventArgs e);
       public abstract void txtCodigo_LostFocus(System.Object sender,System.EventArgs e);
       public abstract void txtCodigo_KeyDown(Ara2.Components.AraTextBox Object,System.Int32 vKey);
       public abstract void txtNome_LostFocus(System.Object sender,System.EventArgs e);
       public abstract void bSelect_Click(System.Object sender,System.EventArgs e);
       #endregion 
       public AraForeignKeyLinqAraDesign(IAraContainerClient vConteiner) : base(vConteiner)
       {
           #region Instances
           #region Propertys Main
           this.Left  =  new Ara2.Components.AraDistance(@"0px");
           this.Top  =  new Ara2.Components.AraDistance(@"0px");
           this.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
           this.MinHeight  =  new Ara2.Components.AraDistance(@"10px");
           this.Width  =  new Ara2.Components.AraDistance(@"600px");
           this.Height  =  new Ara2.Components.AraDistance(@"25px");
           #endregion


           #region bPesquisa
           this.bPesquisa = new Ara2.Grid.AraFormSearchLinqButton(this);

           this.bPesquisa.Text  = @"";
           this.bPesquisa.Ico  = Ara2.Components.AraButton.ButtonIco.search;
           this.bPesquisa.Left  =  new Ara2.Components.AraDistance(@"99px");
           this.bPesquisa.Top  =  new Ara2.Components.AraDistance(@"2px");
           this.bPesquisa.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
           this.bPesquisa.MinHeight  =  new Ara2.Components.AraDistance(@"20px");
           this.bPesquisa.Width  =  new Ara2.Components.AraDistance(@"25px");
           this.bPesquisa.Height  =  new Ara2.Components.AraDistance(@"20px");
           this.bPesquisa.ZIndex  = 1;
           this.bPesquisa.Return  += bPesquisa_Return;
           #endregion
           #region bNew
           this.bNew = new Ara2.Components.AraButton(this);

           this.bNew.Ico  = Ara2.Components.AraButton.ButtonIco.plus;
           this.bNew.Left  =  new Ara2.Components.AraDistance(@"128px");
           this.bNew.Top  =  new Ara2.Components.AraDistance(@"2px");
           this.bNew.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
           this.bNew.MinHeight  =  new Ara2.Components.AraDistance(@"2px");
           this.bNew.Width  =  new Ara2.Components.AraDistance(@"25px");
           this.bNew.Height  =  new Ara2.Components.AraDistance(@"20px");
           this.bNew.ZIndex  = 2;
           this.bNew.Click  += bNew_Click;
           #endregion
           #region txtCodigo
           this.txtCodigo = new Ara2.Components.AraTextBox(this);

           this.txtCodigo.MaskType  = @"";
           this.txtCodigo.MaskDefaultValue  = @"";
           this.txtCodigo.Left  =  new Ara2.Components.AraDistance(@"0px");
           this.txtCodigo.Top  =  new Ara2.Components.AraDistance(@"3px");
           this.txtCodigo.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
           this.txtCodigo.MinHeight  =  new Ara2.Components.AraDistance(@"17px");
           this.txtCodigo.Width  =  new Ara2.Components.AraDistance(@"90px");
           this.txtCodigo.Height  =  new Ara2.Components.AraDistance(@"17px");
           this.txtCodigo.ZIndex  = 3;
           this.txtCodigo.LostFocus  += txtCodigo_LostFocus;
           this.txtCodigo.KeyDown  += txtCodigo_KeyDown;
           #endregion
           #region txtNome
           this.txtNome = new Ara2.Components.AraTextBox(this);

           this.txtNome.MaskType  = @"";
           this.txtNome.MaskDefaultValue  = @"";
           this.txtNome.Anchor.Left  = 160;
           this.txtNome.Anchor.Right  = 40;
           this.txtNome.Left  =  new Ara2.Components.AraDistance(@"160px");
           this.txtNome.Top  =  new Ara2.Components.AraDistance(@"3px");
           this.txtNome.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
           this.txtNome.MinHeight  =  new Ara2.Components.AraDistance(@"17px");
           this.txtNome.Width  =  new Ara2.Components.AraDistance(@"400px");
           this.txtNome.Height  =  new Ara2.Components.AraDistance(@"17px");
           this.txtNome.ZIndex  = 4;
           this.txtNome.LostFocus  += txtNome_LostFocus;
           #endregion
           #region bSelect
           this.bSelect = new Ara2.Components.AraButton(this);

           this.bSelect.Ico  = Ara2.Components.AraButton.ButtonIco.triangle_1_s;
           this.bSelect.Anchor.Right  = 3;
           this.bSelect.Left  =  new Ara2.Components.AraDistance(@"567px");
           this.bSelect.Top  =  new Ara2.Components.AraDistance(@"3px");
           this.bSelect.MinWidth  =  new Ara2.Components.AraDistance(@"10px");
           this.bSelect.MinHeight  =  new Ara2.Components.AraDistance(@"20px");
           this.bSelect.Width  =  new Ara2.Components.AraDistance(@"30px");
           this.bSelect.Height  =  new Ara2.Components.AraDistance(@"20px");
           this.bSelect.ZIndex  = 5;
           this.bSelect.Click  += bSelect_Click;
           #endregion
           #endregion
       } 
   } 
} 
