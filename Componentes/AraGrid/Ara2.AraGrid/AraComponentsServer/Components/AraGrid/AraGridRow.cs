// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ara2.Components
{
    [Serializable]
    public class AraGridRow
    {
        private string _ID = "";

        public string ID {
            get {return _ID;}
        }

        public int Pos = -1;
        public List<AraGridCell> Cells = new List<AraGridCell>();
        private AraGrid _AraGrid;
        public AraGrid AraGrid
        {
            get { return _AraGrid; }
        }

        private bool _Select = false;

        private AraGridRow _RowFather = null;
        public AraGridRow RowFather
        {
            get { return _RowFather;}
        }

        public delegate void eOnLoad(AraGridRow vRow);
        public AraEvent<eOnLoad> OnLoad = null;

        //public AraGridRow(AraGrid vAraGrid, string vID, string[] vCells, bool vPermissionsEditingRow)
        //{
        //    _ID = vID;
        //    _AraGrid = vAraGrid;
        //    _PermissionsEditingRow = vPermissionsEditingRow;
        //    Construct(vCells);
        //}

        public enum EAddPosition
        {
            /// <summary>
            /// Antes
            /// </summary>
            Before, 

            /// <summary>
            /// Depois
            /// </summary>
            After
        }

        public EAddPosition? _AddPositionP = null;
        public string _AddPositionRowID = null;

        public AraGridRow(AraGrid vAraGrid, string vID, string[] vCells, bool vPermissionsEditingRow = false, eOnLoad vOnLoad = null,EAddPosition? vAddPositionP = null,string vAddPositionRowID = null) 
        {
            _ID = vID;
            _AraGrid = vAraGrid;
            if (vOnLoad != null)
            {
                OnLoad = new AraEvent<eOnLoad>();
                OnLoad += vOnLoad;
            }
            _PermissionsEditingRow = vPermissionsEditingRow;

            _AddPositionP =vAddPositionP ;
            _AddPositionRowID = vAddPositionRowID;

            Construct(vCells);
        }




        private void Construct(string[] vCells)
        {
            for (int n = 0; n < AraGrid.Cols.Count; n++)
            {
                Cells.Add(new AraGridCell(_AraGrid,AraGrid.Cols[n], this, (vCells[n] == null ? "" : vCells[n])));
            }

            if (_AraGrid.Tree!=null)
                if (this[_AraGrid.Tree.ColFather.Name].Text != "")
                    if (this.TreeRowFather == null)
                        throw new Exception("AraGrid AddRow error. CodFather not Found");
        }

        
        private bool PrimeiroCommit=true;
        //private bool PrimeiroCommit_Emprocesso = false;
        public void Commit()
        {

            Tick vTick = Tick.GetTick();

            AraGrid.TickScriptCall();
            if (PrimeiroCommit)
            {
                //PrimeiroCommit_Emprocesso = true;
                PrimeiroCommit = false;

                foreach (AraGridCell Cell in Cells)
                {
                    try
                    {
                        Cell.RumColFormatText();
                    }
                    catch (Exception err)
                    {
                        AraTools.Alert("Error on format cell. RowID = " + this.ID + " ColCaption = '" + Cell.Col.Caption + "'.\n" + err.Message);
                    }
                }

                bool TmpRowLoad = false;
                if (_AraGrid.Tree != null)
                {
                    if (OnLoad !=null && OnLoad.InvokeEvent != null)
                        TmpRowLoad = OnLoad.InvokeEvent.GetInvocationList().Length > 0 ? false : true;
                    else
                        TmpRowLoad = true;

                    if (TmpRowLoad == false)
                        _TreeContainer = true;

                    CheckTreeTypeLevesChange(false);
                }

                AraGrid.TickScriptCall();
                if (_AraGrid.Tree == null)
                    vTick.Script.Send(" vObj.AddRow('" + AraTools.StringToStringJS(ID) + "'," + GetScript() + ",'" + (_AddPositionP == null ? "" : (_AddPositionP == EAddPosition.After ? "after" : "before")) + "','" + (_AddPositionRowID == null ? "" : _AddPositionRowID) + "'," + (_PermissionsEditingRow == true ? "true" : "false") + "); \n");
                else
                {
                    if (this.TreeRowFather == null)
                        vTick.Script.Send(" vObj.AddRow('" + AraTools.StringToStringJS(ID) + "'," + GetScript() + ",'" + (_AddPositionP == null ? "" : (_AddPositionP == EAddPosition.After ? "after" : "before")) + "','" + (_AddPositionRowID == null ? "" : _AddPositionRowID) + "'," + (_PermissionsEditingRow == true ? "true" : "false") + "); \n");
                    else
                    {
                        AraGridRow RowAdd = this.TreeRowFather;
                        if (RowAdd.TreeContainer == false)
                        {
                            RowAdd.TreeContainer = true;
                            //RowAdd.Commit();
                        }

                        List<AraGridRow> RowAddC = new List<AraGridRow>(RowAdd.Children);

                        if (RowAddC.Count > 1)
                        {
                            RowAdd = RowAddC[RowAddC.Count - 2];
                            RowAddC = new List<AraGridRow>(RowAdd.Children);

                            while (RowAddC.Count > 0)
                            {
                                RowAdd = RowAddC[RowAddC.Count - 1];
                                RowAddC = new List<AraGridRow>(RowAdd.Children);
                            }
                        }

                        

                        AraGrid.TickScriptCall();
                        vTick.Script.Send(" vObj.AddRow('" + AraTools.StringToStringJS(ID) + "'," + GetScript() + ",'after','" + AraTools.StringToStringJS(RowAdd.ID) + "'," + (_PermissionsEditingRow == true ? "true" : "false") + "); \n");
                    }
                }




                _PermissionsEditingRow = AraGrid.CellEdit;

                if (_AraGrid.Tree != null)
                {
                    AraGrid.TickScriptCall();
                    vTick.Script.Send(" vObj.SetTreePa('" + AraTools.StringToStringJS(ID) + "','" + AraTools.StringToStringJS(this[_AraGrid.Tree.ColFather.Name].Text) + "','" + AraTools.StringToStringJS(this[_AraGrid.Tree.ColId.Name].Text) + "'," + (TmpRowLoad ? "true" : "false") + "," + (_TreeExpand ? "true" : "false") + "); \n");


                    if (this.TreeRowFather != null)
                    {
                        if (this.TreeRowFather.TreeExpand == false)
                            this.Visible = false;
                    }
                }
                //PrimeiroCommit_Emprocesso = false;
            }
                /*
            else
            {
                if (!PrimeiroCommit_Emprocesso)
                {
                    AraGrid.Call();
                    Client.Script.Send(" vObj.SetRowText('" + AraTools.StringToStringJS(ID) + "'," + GetScript() + "); \n");
                }
            }
                 */
        }
        
        public int TreeLevel
        {
            get
            {
                try
                {
                    if (_AraGrid.Tree == null)
                        return -1;
                    else
                    {
                        if (this[_AraGrid.Tree.ColFather.Name].Text == null || this[_AraGrid.Tree.ColFather.Name].Text=="")
                            return 0;
                        else
                        {
                            int vLevel = 0;
                            AraGridRow TmpRow = TreeRowFather;
                            while (TmpRow != null)
                            {
                                vLevel++;
                                TmpRow = TmpRow.TreeRowFather;
                            }
                            return vLevel;
                        }
                    }
                }
                catch (Exception err)
                {
                    throw new Exception("Error get level.\n"+ err.Message);
                }
            }
        }

        public AraGridRow TreeRowFather
        {
            get
            {
                if (_AraGrid.Tree == null)
                    return null;
                else
                {
                    if (this[_AraGrid.Tree.ColFather.Name].Text == null)
                        return null;
                    else
                    {
                        return _AraGrid.Tree.GetRowByCodId(this[_AraGrid.Tree.ColFather.Name].Text);
                    }
                }
            }
        }

        public AraGridRow TreeRowID
        {
            get
            {
                if (_AraGrid.Tree == null)
                    return null;
                else
                {
                    return _AraGrid.Tree.GetRowByCodId(this[_AraGrid.Tree.ColId.Name].Text);
                }
            }
        }

        public AraGridRow TreeRowCaption
        {
            get
            {
                if (_AraGrid.Tree == null)
                    return null;
                else
                {
                    return _AraGrid.Tree.GetRowByCodId(this[_AraGrid.Tree.ColCaption.Name].Text);
                }
            }
        }

        private bool _TreeExpand = false;
        public bool TreeExpand
        {
            get
            {
                return _TreeExpand;
            }
            set
            {
                if (this.TreeContainer != true && value == true)
                    throw new Exception("Tree Grid -> You can not expand an object that has no children");

                if (_TreeExpand != value)
                {
                    _TreeExpand = value;

                    AraGrid.TickScriptCall();
                    Tick.GetTick().Script.Send(" vObj.TreeExpand('" + AraTools.StringToStringJS(ID) + "'," + (_TreeExpand == true ? "true" : "false") + "); \n");
                }
            }
        }

        private bool _TreeContainer = false;
        public bool TreeContainer
        {
            get
            {
                return _TreeContainer;
            }
            set
            {
                _TreeContainer = value;

                AraGrid.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.TreeContainer['" + AraTools.StringToStringJS(ID) + "'] = " + (_TreeContainer == true ? "true" : "false") + "; \n");

                this[AraGrid.Tree.ColCaption].Text = this[AraGrid.Tree.ColCaption].Text;

                if (!_TreeContainer)
                    if(TreeExpand)
                        TreeExpand = false;
            }
        }


        private bool _edit = false;

        public bool Edit
        {
            get { return _edit; }
            set
            {
                _edit = value;

                if (_edit == true)
                {
                    AraGrid.TickScriptCall();
                    Tick.GetTick().Script.Send(" vObj.SetEdit('" + AraTools.StringToStringJS(ID) + "'," + (_edit == true ? "true" : "false") + "); \n");
                }
            }
        }

        private bool _Visible = true;

        public bool Visible
        {
            get { return _Visible; }
            set
            {
                _Visible = value;

                Tick vTick = Tick.GetTick();
                AraGrid.TickScriptCall();
                if (_Visible == true)
                    vTick.Script.Send(" vObj.ShowRow('" + AraTools.StringToStringJS(ID) + "'); \n");
                else
                    vTick.Script.Send(" vObj.HideRow('" + AraTools.StringToStringJS(ID) + "'); \n");
                
            }
        }

        private bool _PermissionsEditingRow = true;

        public bool PermissionsEditingRow
        {
            get { return _PermissionsEditingRow; }
            set
            {
                _PermissionsEditingRow = value;

                Tick vTick = Tick.GetTick();
                AraGrid.TickScriptCall();
                vTick.Script.Send(" vObj.PermissionsEditingRow['" + AraTools.StringToStringJS(ID) + "'] = " + (_PermissionsEditingRow == true ? "true" : "false") + "; \n");
            }
        }

        public string GetScript()
        {
            string vTmp = "{";
            foreach (AraGridCell Cell in Cells)
                vTmp += Cell.GetScript() + ",";
            /*
            vTmp += "expanded:false,";
            vTmp += "loaded:false,";
            vTmp += "level:" + _Level + ",";
            vTmp += "isLeaf:false,";
            vTmp += "parent:" + (_RowFather==null?"null":"'" + _RowFather.ID + "'") + ",";
            */

            vTmp = vTmp.Substring(0, vTmp.Length - 1);
            vTmp += "}";
            return vTmp;
        }


        public AraGridCell Cell(string ColId)
        {
            foreach (AraGridCell Cell in Cells)
            {
                if (Cell.Col.Name.ToUpper() == ColId.ToUpper())
                    return Cell;
            }

            foreach (AraGridCell Cell in Cells)
            {
                if (Cell.Col.NameReal == ColId)
                    return Cell;
            }
            return null;
        }

        public bool Select
        {
            get { return _Select; }
            set
            {
                if (_Select != value)
                {
                    _Select = value;

                    Tick vTick = Tick.GetTick();
                    AraGrid.TickScriptCall();
                    vTick.Script.Send(" vObj.SetSelectRow('" + AraTools.StringToStringJS(ID) + "'); \n");
                }
            }
        }

        public void SetSelectInternal(bool vTmp)
        {
            _Select = vTmp;
        }

        public AraGridCell this[AraGridCol vCol]
        {
            get
            {
                return Cell(vCol.Name);
            }
        }

        public AraGridCell this[string ColId]
        {
            get
            {
                return Cell(ColId);
            }
        }

        public AraGridCell this[int index]
        {
            get
            {
                return Cells[index];
            }
        }

        public int Count
        {
            get
            {
                return Cells.Count;
            }
        }

        public string[] GetStringArray()
        {
            string[] Tmp = new String[Cells.Count];

            for (int c = 0; c < Cells.Count; c++)
            {
                Tmp[c] = this[c].Text;
            }

            return Tmp;
        }

        public AraGridRow[] Children
        {
            get
            {
                if (_AraGrid.Tree!=null)
                    return _AraGrid.Tree.GetRowInColFatherByCodId(this[_AraGrid.Tree.ColId].Text); // ID
                else
                    return (new List<AraGridRow>()).ToArray();

            }
        }

        public AraGridRow ChildrenLast
        {
            get
            {
                AraGridRow[] Tmp = _AraGrid.Tree.GetRowInColFatherByCodId(this[_AraGrid.Tree.ColId].Text); 

                if (Tmp.Length >0)
                    return Tmp[Tmp.Length-1];
                else
                    return null;
            }
        }
        


        public void RumOnLoad()
        {
            try
            {
                AraGrid.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.TreeRowLoad['" + AraTools.StringToStringJS(ID) + "'] = true ; \n");

                this.ChildrenClear();
                this.OnLoad.InvokeEvent(this);
                this.OnLoad = null;

                if (this.Children.Length > 0)
                {
                    TreeExpand = true;
                }
                else
                {
                    this.TreeContainer = false;
                    this[AraGrid.Tree.ColCaption].Text = this[AraGrid.Tree.ColCaption].Text;
                    //this.Commit();
                }           
            } 
            catch(Exception err)
            {
                throw new Exception("Error on AraGridRow.RumOnLoad.\n " + err.Message);
            }
            finally
            {
                AraGrid.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.WarningLoading(-1,false); \n");
            }
            
        }

        public void ChildrenClear()
        {
            if (_AraGrid.Tree != null)
            {
                foreach (AraGridRow vTmpR in _AraGrid.Tree.GetRowInColFatherByCodId(ID))
                    _AraGrid.Rows.Del(vTmpR);
            }
        }

        public Hashtable ToHashtable()
        {
            Hashtable Tmp = new Hashtable();
            foreach (AraGridCell vCell in Cells)
            {
                Tmp.Add(vCell.Col.Name, vCell.Text);
            }
            return Tmp;
        }

        public Dictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> Tmp = new Dictionary<string, string>();
            foreach (AraGridCell vCell in Cells)
            {
                Tmp.Add(vCell.Col.Name, vCell.Text);
            }
            return Tmp;
        }


        public enum ETypeLineByLevel
        {
            PassLine=1,
            LastLine=2, 
            LineCrusade=3, 
            nd=4
        }

        private ETypeLineByLevel[] _TypeLineByLevel = new ETypeLineByLevel[0];
        public ETypeLineByLevel[] TypeLineByLevel
        {
            get
            {
                return _TypeLineByLevel;
            }
        }

        private ETypeLineByLevel[] GetTypeLineByLevel()
        {

            ETypeLineByLevel[] TmpTypeLineByLevel = new ETypeLineByLevel[TreeLevel];


            for (int vLevel = 0; vLevel < TreeLevel; vLevel++)
            {
                if (vLevel == TreeLevel - 1)
                {
                    if (TreeRowFather.ChildrenLast == this)
                    {
                        //////AraGridRow[] vIrmas = TreeRowFather.Children;
                        //////if (vIrmas.Length > 1)
                        //////{
                        //////    vIrmas[vIrmas.Length - 2].CheckTreeTypeLevesChange();

                        //////    //vIrmas[vIrmas.Length - 2].Commit();
                        //////}

                        //LastLine
                        TmpTypeLineByLevel[vLevel] = ETypeLineByLevel.LastLine;
                    }
                    else
                    {
                        //LineCrusade
                        TmpTypeLineByLevel[vLevel] = ETypeLineByLevel.LineCrusade;
                    }
                }
                else
                {
                    AraGridRow RowFather = TreeRowFather;
                    AraGridRow RowFather2 = RowFather;

                    while (RowFather.TreeLevel != vLevel)
                    {
                        RowFather2 = RowFather;
                        RowFather = RowFather.TreeRowFather;
                    }

                    if (RowFather.ChildrenLast == RowFather2)
                    {
                        TmpTypeLineByLevel[vLevel] = ETypeLineByLevel.nd;
                    }
                    else
                    {
                        //PassLine
                        TmpTypeLineByLevel[vLevel] = ETypeLineByLevel.PassLine;
                    }
                }
            }

            return TmpTypeLineByLevel;
        }

        private bool EnumEqual<T>(T[] E1, T[] E2)
        {
            if (E1.Length != E2.Length)
                return false;
            else
            {
                for (int i = 0; i < E1.Length; i++)
                {
                    if (E1[i].ToString() != E2[i].ToString())
                        return false;
                }
                return true;
            }
        }

        private bool FistLoadTypeLineByLevel = true;
        private bool LoadTypeLineByLevel()
        {
            ETypeLineByLevel[] TmpTypeLineByLevel = GetTypeLineByLevel();
            if (!EnumEqual<ETypeLineByLevel>(TmpTypeLineByLevel,_TypeLineByLevel))
            {
                _TypeLineByLevel = TmpTypeLineByLevel;
                return true;
            }

            return false;
        }

        public bool CheckTreeTypeLevesChange()
        {
            return CheckTreeTypeLevesChange(true);
        }
        public bool CheckTreeTypeLevesChange(bool ChangeCaption)
        {
            if (LoadTypeLineByLevel())
            {
                //foreach (AraGridRow vTmpC in TreeRowFather.Children)
                //{
                //    if (vTmpC != this)
                //        vTmpC.CheckTreeTypeLevesChange();
                //}

                //foreach (AraGridRow vTmpC in Children)
                //{
                //    if (vTmpC != this)
                //        vTmpC.CheckTreeTypeLevesChange();
                //}

                //if (ChangeCaption)
                //    this[AraGrid.Tree.ColCaption].Text = this[AraGrid.Tree.ColCaption].Text;

                AraGrid.Tree.OnClientUnloadUpdateCaptions();

                

                return true;
            }
            else
                return false;
        }

        


    }

}
