// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ara2.Components
{
    [Serializable]
    public class AraGridRows
    {
        private AraGrid AraGrid;
        public delegate void DOnAddRow(AraGrid vGrid,AraGridRow Row);
        public event DOnAddRow OnAdd;

        public AraGridRows(AraGrid vAraGrid)
        {
            AraGrid = vAraGrid;
        }

        private List<AraGridRow> Rows = new List<AraGridRow>();


        public AraGridRow Add(string Id, Dictionary<string, string> Cells, AraGridRow.eOnLoad OnLoad)
        {
            return Add(Id, Cells, false, OnLoad);
        }

        public AraGridRow Add(string Id, Dictionary<string, string> Cells, bool vPermissionsEditingRow)
        {
            return Add(Id, Cells, vPermissionsEditingRow, null);
        }

        public AraGridRow Add(string Id, Dictionary<string, string> Cells, bool vPermissionsEditingRow=false, AraGridRow.eOnLoad OnLoad=null)
        {
            Dictionary<string, string> CellsUpper=new Dictionary<string,string>();;

            foreach (string TmpKey in Cells.Keys)
            {
                try
                {
                    CellsUpper.Add(TmpKey.ToUpper(), Cells[TmpKey]);
                }
                catch (Exception err)
                {
                    throw new Exception("Erro on add row by duplicate cell '" + TmpKey + "'.\n" + err.Message);
                }
            }

            List<string> TmpCell = new List<string>();
            for (int n = 0; n < AraGrid.Cols.Count; n++)
            {
                if (CellsUpper.ContainsKey(AraGrid.Cols[n].Name.ToUpper()))
                    TmpCell.Add(CellsUpper[AraGrid.Cols[n].Name.ToUpper()]);
                else
                    TmpCell.Add("");
            }

            return Add(Id, TmpCell.ToArray(), vPermissionsEditingRow, OnLoad);
        }

       

        public AraGridRow Add(string Id, string[] Cells, bool vPermissionsEditingRow=false,AraGridRow.eOnLoad OnLoad=null)
        {
            try
            {
                if (this[Id] != null)
                    throw new Exception("Error adding the line. duplicate key.");

                if (AraGrid.TreeGroup != null)
                {

                    if (Cells[AraGrid.Cols[AraGrid.TreeGroup.NameColCodIdentification].Pos] != "1")
                    {

                        string vPathGrup = "AraGridTreeGroup_";
                        string vPathGrupFather = "";
                        foreach (AraGridCol Col in AraGrid.TreeGroup.ColsGrup)
                        {
                            string vText = AraGridCell.FormatText(Col.FormatterServerType,Col.FormatterServerText, Cells[Col.Pos]);
                            vPathGrup += AraTools.StringToIDJS(vText) + "_";
                            if (this[vPathGrup.Substring(0, vPathGrup.Length - 1)] == null)
                            {
                                string vTmpCaption ="<b>" + Col.Caption + "</b>: " + vText ;
                                if (AraGrid.TreeGroup.GetColsGrupSum(Col.Name) != null)
                                    vTmpCaption += AraGrid.Buttons.Add(AraGridButton.ButtonIco.plus, new System.Collections.Hashtable { { "RowId", Id }, { "ColGrup", Col.Name } }, AraGrid.TreeGroup.ButtonSum_Click);
                                GroupCria(vTmpCaption, vPathGrupFather, vPathGrup.Substring(0, vPathGrup.Length - 1));
                            }

                            vPathGrupFather = vPathGrup.Substring(0, vPathGrup.Length - 1);
                        }
                        //vPathGrup = vPathGrup.Substring(0, vPathGrup.Length - 1);
                        vPathGrup += Id;

                        Cells[AraGrid.Cols[AraGrid.TreeGroup.NameColCod].Pos] = vPathGrup;
                        Cells[AraGrid.Cols[AraGrid.TreeGroup.NameColCodFather].Pos] = vPathGrupFather;
                    }
                }

                AraGridRow TmpR = new AraGridRow(AraGrid, Id, Cells, vPermissionsEditingRow, OnLoad);
                return Add(TmpR);
            }
            catch(Exception err)
            {
                throw new Exception("Error add row.\n Erro:" + err.Message);
            }
        }

        public AraGridRow Add(AraGridRow vRow)
        {
            Rows.Add(vRow);
            vRow.Pos = Rows.Count - 1;
            vRow.Commit();

            if (OnAdd != null)
            {
                try
                {
                    OnAdd(AraGrid, vRow);
                }
                catch (Exception err)
                {
                    throw new Exception("AraGridRows erro in event OnAdd.\n" + err.Message);
                }
            }
            return vRow;
        }

        #region Group

        private void GroupCria(string vCaption, string vFather, string vCod)
        {
            this.Add(vCod, new Dictionary<string, string>
            {
                {AraGrid.TreeGroup.NameColCod,vCod},
                {AraGrid.TreeGroup.NameColCodFather,vFather},
                {AraGrid.TreeGroup.NameColCodCaption,vCaption},
                {AraGrid.TreeGroup.NameColCodIdentification,"1"}
            });

            int vNColVN = 1;
            foreach (AraGridCell Cell in this[vCod].Cells)
            {
                if (Cell.Col.hidden == false)
                {
                    if (Cell.Col.Name != AraGrid.TreeGroup.NameColCodCaption)
                    {
                        Cell.Visible = false;
                        vNColVN++;
                    }
                }
            }

            this[vCod][AraGrid.TreeGroup.NameColCodCaption].ColSpan = vNColVN;
        }

       
        #endregion

        public void Del(string vIdRow)
        {
            this.Del(this[vIdRow]);
        }

        public void Del(AraGridRow vRow)
        {
            if (AraGrid.Tree != null)
            {
                foreach (AraGridRow vTmpRowC in vRow.Children)
                {
                    this.Del(vTmpRowC);
                }

            }

            AraGrid.TickScriptCall();
            Tick.GetTick().Script.Send(" vObj.DelRow('" + AraTools.StringToStringJS(vRow.ID) + "'); \n");
            Rows.Remove(vRow);

            if (AraGrid.Tree != null)
            {
                if (vRow.TreeRowFather != null)
                {
                    foreach (AraGridRow vTmpC in vRow.TreeRowFather.Children)
                    {
                        if (vTmpC != vRow)
                            vTmpC.CheckTreeTypeLevesChange();
                    }

                    vRow.TreeRowFather.CheckTreeTypeLevesChange();
                    if (vRow.TreeRowFather.Children.Length == 0)
                    {
                        vRow.TreeRowFather.TreeContainer = false;
                        //vRow.TreeRowFather.Commit();
                    }
                }
            }

        }

        public AraGridRow[] ToArray()
        {
            return Rows.ToArray();
        }

        public void Clear()
        {
            AraGrid.TickScriptCall();
            Tick.GetTick().Script.Send(" vObj.ClearData(); \n");
            Rows = new List<AraGridRow>();
        }

        public void ResetSelectInternal()
        {
            foreach (AraGridRow vRow in Rows)
                vRow.SetSelectInternal(false);
        }


        public AraGridRow this[string vRowId]
        {
            get
            {
                return Rows.Find(a=> a.ID == vRowId);
            }
        }

        public AraGridRow[] this[string vColName,string vText]
        {
            get
            {
                vText=vText.ToUpper();
                return Rows.FindAll(a => a[vColName].Text.ToUpper() == vText).ToArray();
            }
        }

        public AraGridRow this[int vIndex]
        {
            get
            {
                return Rows[vIndex];
            }
        }

        public int Count
        {
            get
            {
                return Rows.Count;
            }
        }
        //public 

        public void GroupingByCol(AraGridCol vCol)
        {
            this.AraGrid.TickScriptCall();
            Tick.GetTick().Script.Send(" vObj.ObjJQuery.groupingGroupBy(['" + vCol.NameReal + "']); ");
        }

        public void ChangePosition(AraGridRow SourceRow, AraGridRow DestinationRow)
        {

            SourceRow._AddPositionP = AraGridRow.EAddPosition.Before;
            SourceRow._AddPositionRowID = DestinationRow.ID;
            this.Del(SourceRow);
            Add(SourceRow);

        }


        public AraGridRow Find(Predicate<AraGridRow> match)
        {
            return Rows.Find(match);
        }

        public List<AraGridRow> FindAll(Predicate<AraGridRow> match)
        {
            return Rows.FindAll(match);
        }

    }

}
