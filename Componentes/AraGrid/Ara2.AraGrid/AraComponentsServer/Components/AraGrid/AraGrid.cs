// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Ara2.Components.AraGridPrivate;
using Ara2.Components.Grid;
using System.Reflection;
using Ara2;
using Ara2.Dev;
// SUB GRID -> http://www.trirand.com/jqgridwiki/doku.php?id=wiki:subgrid_as_grid

namespace Ara2.Components
{
    [Serializable]
    [AraDevComponent(false)]
    public class AraGrid : AraComponentVisualAnchorConteiner,IAraDev
    {

        public AraGrid(IAraContainerClient ConteinerFather)
            : base(AraObjectClienteServer.Create(ConteinerFather, "table"), ConteinerFather, "AraGrid")
        {

            OnCommitAfter = new AraEvent<dOnCommitBefore>();
            OnCommitBefore = new AraEvent<dOnCommitBefore>();
            Click = new AraComponentEventKey<EventHandler>(this, "Click");

            //Bottom = new AraGridBottom(this);
            Cols = new AraGridCols(this);
            Buttons = new AraGridButton(this);
            //Nav = new AraGridBottomNav(this);

            //CreateButtons();

            KeyDown = new AraComponentEventKey<Key_delegate>(this, "KeyDown", EAraComponentEventTypeThread.ThreadMulti);
            KeyUp = new AraComponentEventKey<Key_delegate>(this, "KeyUp", EAraComponentEventTypeThread.ThreadMulti);
            KeyPress = new AraComponentEventKey<Key_delegate>(this, "KeyPress", EAraComponentEventTypeThread.ThreadMulti);

            ClickCell = new AraComponentEventKey<dClickCell>(this, "ClickCell", EAraComponentEventTypeThread.ThreadClick);
            ClickDblCell = new AraComponentEventKey<dClickCell>(this, "ClickDblCell", EAraComponentEventTypeThread.ThreadClick);

            SelectCell = new AraComponentEventKey<EventHandler>(this, "SelectCell");
            SelectRow = new AraComponentEventKey<SelectRow_delegate>(this, "SelectRow");

            //PageChange = new AraComponentEventKey<PageChange_delegate>(this, "PageChange", EAraComponentEventTypeThread.ThreadMulti);
            //PageChange.ChangeEnabled += () =>
            //{
            //    this.Nav.Visible = PageChange.Enabled;
            //};

            ChangeCell = new AraComponentEventKey<ChangeCell_delegate>(this, "ChangeCell", EAraComponentEventTypeThread.ThreadMulti);

            this.EventInternal += AraGrid_EventInternal;
            this.SetProperty += AraGrid_SetProperty;
            this.WidthChangeBefore += AraGrid_WHChangeBefore;
            this.HeightChangeBefore += AraGrid_WHChangeBefore;


            //AraTools.AsynchronousFunction(CarregaPrimeraVez);
            this._Width = 400;
            this._Height = 400;
        }

        private AraScrollBar _ScrollGrid=null;
        public AraScrollBar ScrollGrid
        {
            get { return _ScrollGrid; }
            set {
                value.ObjectNameHtml = this.InstanceID + "_gridscroll";
                _ScrollGrid = value;
                _ScrollGrid.Commit();
            }

        }

        //private void CarregaPrimeraVez()
        //{
        //    if (!this.IsCommit)
        //    {
        //        this.Cols.Add(new AraGridCol(this, "tmp", "tmp"));
        //        this.Commit();

        //        this.Rows.Add("1", new Dictionary<string, string> { { "tmp", "" } });


        //        this.Width = 100;
        //        this.Height = 100;
        //    }
        //}

        void AraGrid_WHChangeBefore(AraDistance ToDistance)
        {
            if (ToDistance.Unity != AraDistance.EUnity.px)
                throw ExceptionOnlyPx;
        }

       
        //public AraGridBottom Bottom;
        //public AraGridBottomNav Nav;
       

        //public AraMenu MenuConfiguracoes;
        //AraButton bConfiguracoes;


        //private void CreateButtons()
        //{

        //    string Name;


        //    Name = this.Name + "_bConfiguracoes";
        //    Bottom.Add(Name, "<div id='" + Name + "'></div> <div id='" + Name + "_menu'></div>");
        //    bConfiguracoes = new AraButton(Name, this.Form);
        //    MenuConfiguracoes = new AraMenu(Name + "_menu", this.Form);
        //    Bottom.SetObject(Name, bConfiguracoes);

        //    bConfiguracoes.Ico = AraButton.ButtonIco.gear;
        //    bConfiguracoes.Click += bConfiguracoes_Click;


        //    MenuConfiguracoes.Itens.Add(new AraMenuItens("bAgruparPorPasta", "Agrupar por Pasta", bAgruparPorPasta_Click));
        //    MenuConfiguracoes.Itens.Add(new AraMenuItens("bOcultarColunas", "Ocultar Colunas", bOcultarColunas_Click));
        //    MenuConfiguracoes.Commit();

          

        //}

        //private void bConfiguracoes_Click(object sender, EventArgs e)
        //{
        //    MenuConfiguracoes.Show();
        //}



        //private void bAgruparPorPasta_Click(AraMenuItens Object)
        //{
        //    this.TreeGroup.ShowFormSelection();
        //}

        private void bOcultarColunas_Click(AraMenuItens Object)
        {
            this.Cols.ShowFormColsHidden();
        }

        public override void LoadJS()
        {
            Tick vTick = Tick.GetTick();
            vTick.Session.AddJs("Ara2/Components/AraGrid/files/grid_locale-pt-br.js");
            vTick.Session.AddJs("Ara2/Components/AraGrid/files/jquery_jqGrid_src.js");
            vTick.Session.AddCss("Ara2/Components/AraGrid/files/ui_jqgrid.css");
            vTick.Session.AddCss("Ara2/Components/AraGrid/files/ui_jqgrid_Ara2.css");
            vTick.Session.AddJs("Ara2/Components/AraGrid/AraGrid.js");
        }



        public AraGridRows Rows = null;
        public AraGridCols Cols;

        private AraGridTree _Tree = null;
        public AraGridTree Tree
        {
            get { return _Tree; }
            set
            {
                if (value.AraGrid != this)
                    throw new Exception("AraGridTree AraGrid in the parameter is incorrect");
                _Tree = value;

                this.Cols.AutoOrder = false;
            }
        }

        private AraGridTreeGroup _TreeGroup = null;
        public AraGridTreeGroup TreeGroup
        {
            get { return _TreeGroup; }
            set
            {
                if (value.AraGrid != this)
                    throw new Exception("AraGridTree AraGrid in the parameter is incorrect");
                _TreeGroup = value;
            }
        }

        public AraGridButton Buttons;

        private string _SelRow = "";
        private string _SelCol = "";
        private int _SelNRow = -1;
        private int _SelNCol = -1;

        public string SelRow
        {
            get { return _SelRow; }
            set { }
        }

        public string SelCol
        {
            get { return _SelCol; }
            set { }
        }

        public int SelNRow
        {
            get { return _SelNRow; }
            set { }
        }

        public int SelNCol
        {
            get { return _SelNCol; }
            set { }
        }

        private void AraGrid_EventInternal(String vFunction)
		{
            Tick vTick = Tick.GetTick();
            switch (vFunction.ToUpper())
            {
                case "SAVEROW":
                    /*
                    string vOper = Client.Page.Request["oper"];
                    string vid = Client.Page.Request["id"];

                    AraGridRow vRow = Row.GetRow(vid);

                    foreach (AraGridCell vCell in vRow.Cells)
                    {
                        string vTmpValue = Client.Page.Request[vCell.Col.Name];
                        vCell.SetTextinternal(vTmpValue);
                    }
                    

                    Client.Page.Response.Clear();
                    Client.Script.Send("[true,'']");
                        * */
                    break;
                case "KEYDOWN":
                    int vKey = Convert.ToInt16(vTick.Page.Request["KeyDown"]);
                    KeyDown.InvokeEvent(this, vKey);
                    break;
                case "KEYUP":
                    int vKey2 = Convert.ToInt16(vTick.Page.Request["KeyUp"]);
                    KeyDown.InvokeEvent(this, vKey2);
                    break;
                case "KEYPRESS":
                    int vKey3 = Convert.ToInt16(vTick.Page.Request["KeyPress"]);
                    KeyDown.InvokeEvent(this, vKey3);
                    break;
                case "CLICK":
                    Click.InvokeEvent(this, new EventArgs());
                    break;
                case "SELECTCELL":
                    SelectCell.InvokeEvent(this, new EventArgs());
                    break;
                case "CLICKCELL":
                    ClickCell.InvokeEvent(this.Cols[this.SelCol], this.Rows[this.SelRow], new AraEventMouse());
                    break;
                case "CLICKDBLCELL":
                    ClickDblCell.InvokeEvent(this.Cols[this.SelCol], this.Rows[this.SelRow], new AraEventMouse());
                    break;
                case "SELECTROW":
                    AraGridRow vTmpRowID = Rows[vTick.Page.Request["RowID"]];
                    SelectRow.InvokeEvent(this, vTmpRowID);
                    break;
                //case "GETPAGE":
                //    _PageMaxRecords = Convert.ToInt32(Tick.Page.Request["len"]);
                //    _Page = Convert.ToInt32(Tick.Page.Request["page"]);
                //    if (_Page > _PageTotal) _Page = _PageTotal;
                //    GETPAGE(_Page, _PageMaxRecords);

                //    Page = _Page;
                //    break;
                case "CHANGECELL":
                    lock (this)
                    {
                        AraGridRow vTmpRow;
                        AraGridCol vTmpCol;
                        try
                        {
                            vTmpRow = Rows[vTick.Page.Request["row"]];
                            vTmpCol = Cols[vTick.Page.Request["col"]];
                        }
                        catch (Exception err)
                        {
                            throw new Exception("On error ChangeCell in Get Col('" + vTick.Page.Request["col"] + "') and Row('" + vTick.Page.Request["row"] + "').\n" + err.Message);
                        }

                        if (vTmpRow == null)
                            throw new Exception("On error ChangeCell in Get Row('" + vTick.Page.Request["row"] + "').");
                        if (vTmpCol == null)
                            throw new Exception("On error ChangeCell in Get Col('" + vTick.Page.Request["col"] + "').");

                        try
                        {
                            vTmpRow[vTmpCol].Text = vTick.Page.Request["value"].ToString();
                            this.ChangeCell.InvokeEvent(this, vTmpRow, vTmpCol, vTick.Page.Request["value"].ToString());
                        }
                        catch (Exception err)
                        {
                            throw new Exception("On error ChangeCell in Get Col('" + vTick.Page.Request["col"] + "') and Row('" + vTick.Page.Request["row"] + "') text('" + vTick.Page.Request["value"].ToString() + "').\n" + err.Message);
                        }
                    }
                    break;
                case "TREELOADROW":
                    AraGridRow vTmpRow2 = Rows[vTick.Page.Request["row"]];
                    vTmpRow2.RumOnLoad();
                    break;
                case "ACTIONLOAD":
                    lock (this)
                    {
                        RumActionLoad(Convert.ToInt32(vTick.Page.Request["codigo"]));
                    }
                    break;
                case "COLORDERBY":
                    Cols.Orderby.InvokeEvent(Cols[vTick.Page.Request["col"]], (AraGridCols.OrderBySense)Convert.ToInt32(vTick.Page.Request["Sense"]));
                    break;
                case "CLICKBUTTON":
                    Buttons.RunEventClick(Convert.ToInt32(vTick.Page.Request["codigo"]));
                    break;
            }
        }

        private void AraGrid_SetProperty(String vNome, dynamic vValor)
        {

            switch (vNome)
            {
                case "IsDestroyed()":
                    if (Convert.ToBoolean(vValor) == false)
                    {
                        this.Dispose();
                        return; // Cuidado
                    }
                    break;
                case "GetSelRow()":
                    _SelRow = vValor;
                    break;
                case "GetSelCol()":
                    _SelCol = vValor;
                    break;
                case "GetSelNRow()":
                    _SelNRow = Convert.ToInt32(vValor);
                    break;
                case "GetSelNCol()":
                    _SelNCol = Convert.ToInt32(vValor);
                    break;
                case "GetChangesInCell()":

                    if (vValor != "")
                    {
                        foreach (dynamic vObj in Json.DynamicJson.Parse(vValor))
                        {
                            string rowid = vObj.rowid;
                            string colid = vObj.colid;
                            string value = vObj.value;

                                Rows[rowid][colid].Text = value;

                                this.TickScriptCall();
                                Tick.GetTick().Script.Send(" vObj.WarningOfReceiptOfCell('" + AraTools.StringToStringJS(rowid) + "','" + AraTools.StringToStringJS(colid) + "','" + AraTools.StringToStringJS(value) + "'); \n");
                        }
                    }
                    break;
                case "GetRowSelect()":
                    if (vValor != "")
                    {
                        Rows.ResetSelectInternal();

                        foreach (string TmpRowID in Json.DynamicJson.Parse(vValor))
                        {
                            Rows[TmpRowID].SetSelectInternal(true);
                        }
                    }
                    break;
                case "GetTreeRowsExpand()":
                    if (vValor != "")
                    {
                        foreach (dynamic vObj in Json.DynamicJson.Parse(vValor))
                            Rows[vObj.rowid].TreeExpand = (vObj.value == "true"?true:false);
                    }
                    break;
            }

        }

        // Fim Padão 


        #region Eventos
        [AraDevEvent]
        public AraComponentEventKey<EventHandler> Click { get; set; }

        public delegate void Key_delegate(AraGrid Object, int vKey);

        [AraDevEvent]
        public AraComponentEventKey<Key_delegate> KeyDown { get; set; }
        [AraDevEvent]
        public AraComponentEventKey<Key_delegate> KeyUp { get; set; }
        [AraDevEvent]
        public AraComponentEventKey<Key_delegate> KeyPress { get; set; }

        public delegate void dClickCell(AraGridCol vCol, AraGridRow vRow, AraEventMouse vMouse);

        [AraDevEvent]
        public AraComponentEventKey<dClickCell> ClickCell { get; set; }
        [AraDevEvent]
        public AraComponentEventKey<dClickCell> ClickDblCell { get; set; }

        [AraDevEvent]
        public AraComponentEventKey<EventHandler> SelectCell { get; set; }

        public delegate void SelectRow_delegate(AraGrid Object, AraGridRow Row);

        [AraDevEvent]
        public AraComponentEventKey<SelectRow_delegate> SelectRow { get; set; }

        //public delegate void PageChange_delegate(AraGrid Object, int Page, int PageMaxRecords);
        //public AraComponentEventKey<PageChange_delegate> PageChange;

        public delegate void ChangeCell_delegate(AraGrid Object, AraGridRow Row, AraGridCol Col, string Value);

        [AraDevEvent]
        public AraComponentEventKey<ChangeCell_delegate> ChangeCell { get; set; }
        #endregion

        private bool _Visible = true;

        [AraDevProperty(true)]
        [PropertySupportLayout]
        public bool Visible
        {
            set
            {
                _Visible = value;
                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetVisible(" + (_Visible == true ? "true" : "false") + "); \n");
            }
            get { return _Visible; }
        }

        private bool _Enabled = true;

        [AraDevProperty(true)]
        public bool Enabled
        {
            set
            {
                _Enabled = value;
                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetEnabled(" + (_Enabled == true ? "true" : "false") + "); \n");
            }
            get { return _Enabled; }
        }



        public void SetColNames(List<string> vColNames)
        {
            string TmpArray = "[";
            for (int n = 0; n < vColNames.Count; n++)
            {
                TmpArray += "'" + vColNames[n] + "'" + (n < vColNames.Count - 1 ? "," : "");
            }
            TmpArray += "]";

            this.TickScriptCall();
            Tick.GetTick().Script.Send(" vObj.SetColNames(" + TmpArray + "); \n");
        }

        bool _IsCommit = false;
        public bool IsCommit
        {
            get
            {
                return _IsCommit;
            }
        }

        [AraDevProperty("")]
        public string Caption = "";
        [AraDevProperty(false)]
        public bool MultiSelect = false;
        [AraDevProperty(true)]
        public bool CellEdit = true;
        [AraDevProperty(false)]
        public bool ShrinkToFit = false;

        public delegate void dOnCommitBefore(AraGrid AraGrid);
        [AraDevEvent]
        public AraEvent<dOnCommitBefore> OnCommitBefore { get; set; }

        [AraDevEvent]
        public AraEvent<dOnCommitBefore> OnCommitAfter { get; set; }
        
        [AraDevProperty(true)]
        public bool Sortable = true;

        public void Commit()
        {
            _IsCommit = false;
            
            if (this.OnCommitBefore.InvokeEvent!=null)
                this.OnCommitBefore.InvokeEvent(this);
            

            if (Rows != null)
                Rows.Clear();

            Tick vTick = Tick.GetTick();

            string vTmp = "{ \n";

            vTmp += " datatype: 'local',";
            if (MultiSelect)
                vTmp += " multiselect:true,";
            if (Caption != "")
                vTmp += " caption: '" + Caption + @"',";

            vTmp += "cellEdit:" + (CellEdit == true ? "true" : "false") + ",";
            vTmp += "shrinkToFit:" + (ShrinkToFit == true ? "true" : "false") + ",";

            vTmp += "url:'" + AraTools.UrlServer + "?ARA2=1&SessionID=" + vTick.Session.Id + "&ObjId=" + this.InstanceID + "&Event=getxml&evgrid=url',";
            vTmp += "editurl:'" + AraTools.UrlServer + "?ARA2=1&SessionID=" + vTick.Session.Id + "&ObjId=" + this.InstanceID + "&Event=getxml&evgrid=edit',";
            vTmp += "cellurl:'" + AraTools.UrlServer + "?ARA2=1&SessionID=" + vTick.Session.Id + "&ObjId=" + this.InstanceID + "&Event=getxml&evgrid=cell',";

            vTmp += "\n";
            if (Cols.Count > 0)
                vTmp += Cols.GetScript() + ",\n";
           
            vTmp += "sortable:" + (Sortable == true ? "true" : "false") + ",";


            vTmp = vTmp.Substring(0, vTmp.Length - 1);
            vTmp += "\n } \n";

            this.TickScriptCall();
            vTick.Script.Send(" vObj.Create(" + vTmp + "); \n");


            Rows = new AraGridRows(this);

            _IsCommit = true;
            //MaxGetPagens = _MaxGetPagens;

            // Ativa ScrollBar
            if (ScrollGrid == null)
                ScrollGrid = new AraScrollBar();
            else
                ScrollGrid.Commit();

            try
            {
                if (this.OnCommitAfter != null)
                    if (this.OnCommitAfter.InvokeEvent != null)
                        this.OnCommitAfter.InvokeEvent(this);
            }
            catch (Exception err)
            {
                throw new Exception("Error on OnCommitAfter.\n " + err.Message);
            }

            //this.Width = this.Width;
            //this.Height = this.Height;
        }

        public void AutoAdjustColumnWidth()
        {
            this.TickScriptCall();
            Tick.GetTick().Script.Send(" vObj.AutoAdjustColumnWidth(); \n");

            if (this.TreeGroup != null)
            {
                this.Cols[this.TreeGroup.NameColCodCaption].Width = (this.TreeGroup.ColsGrup.Length * this.Tree.IcoWidth);
            }
        }

        private Dictionary<int, AraEvent<Action>> _ActionLoad = new Dictionary<int, AraEvent<Action>>();
        //private int _ActionLoad_ID = 1;
        public void WaitingLoading(string vMessage, Action ActionLoad)
        {
            lock (_ActionLoad)
            {
                int TmpId = (_ActionLoad.Keys.Any() ? _ActionLoad.Keys.Max() + 1 : 1);

                Tick Tick = Tick.GetTick();
                AraEvent<Action> Event = new AraEvent<Action>();
                Event += ActionLoad;
                _ActionLoad.Add(TmpId, Event);

                this.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.WarningLoading(" + TmpId + ",true,'" + AraTools.StringToStringJS(vMessage) + "'); \n");
            }
        }

        private void RumActionLoad(int vId)
        {
            lock (_ActionLoad)
            {
                try
                {
                    _ActionLoad[vId].InvokeEvent();
                }
                catch (Exception err)
                {
                    throw new Exception("Error on grid.WarningLoading.\n Erro: " + err.Message);
                }
                finally
                {
                    _ActionLoad.Remove(vId);
                    this.TickScriptCall();
                    Tick.GetTick().Script.Send(" vObj.WarningLoading(" + vId + ",false); \n");
                }
            }
        }

        public void Dispose()
        {
            if (_ScrollGrid != null)
            {
                _ScrollGrid.Dispose();
                _ScrollGrid = null;
            }

            base.Dispose();
        }

        #region Ara2Dev

        private string _Name = "";
        [AraDevProperty("")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private AraEvent<DStartEditPropertys> _StartEditPropertys = null;
        public AraEvent<DStartEditPropertys> StartEditPropertys
        {
            get
            {
                if (_StartEditPropertys == null)
                {
                    _StartEditPropertys = new AraEvent<DStartEditPropertys>();
                    this.Click += this_ClickEdit;
                }

                return _StartEditPropertys;
            }
            set
            {
                _StartEditPropertys = value;
            }
        }
        private void this_ClickEdit(object sender, EventArgs e)
        {
            if (_StartEditPropertys.InvokeEvent != null)
                _StartEditPropertys.InvokeEvent(this);
        }

        private AraEvent<DStartEditPropertys> _ChangeProperty = new AraEvent<DStartEditPropertys>();
        public AraEvent<DStartEditPropertys> ChangeProperty
        {
            get
            {
                return _ChangeProperty;
            }
            set
            {
                _ChangeProperty = value;
            }
        }

        #endregion

    }

}
