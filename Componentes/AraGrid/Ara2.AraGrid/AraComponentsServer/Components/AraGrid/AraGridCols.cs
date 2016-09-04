// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ara2.Components.Grid.ColsHidden;

namespace Ara2.Components
{
    [Serializable]
    public class AraGridCols
    {
        public enum OrderBySense
        {
            Ascending=0,
            Descending=1
        }

        public delegate void DOnOrderby(AraGridCol Col,OrderBySense Sense);
        public AraComponentEventKey<DOnOrderby> Orderby;
        

        private List<AraGridCol> Cols = new List<AraGridCol>();
        private AraGrid Grid;

        public AraGridCols(AraGrid vGrid)
        {
            Grid = vGrid;
            Orderby = new AraComponentEventKey<DOnOrderby>(Grid, "ColOrderby", EAraComponentEventTypeThread.ThreadSingle);
        }


        public void Add(AraGridCol vCol)
        {
            Cols.Add(vCol);
        }

        public void Add(AraGridCol vCol,int vPos)
        {
            Cols.Insert(vPos, vCol);
        }

        public void Del(AraGridCol vCol)
        {
            Cols.Remove(vCol);
        }

        public void Clear()
        {
            Cols.Clear();
        }

        public int Count
        {
            get
            {
                return Cols.Count;
            }
        }

        public AraGridCol this[string vName]
        {
            get
            {
                foreach (AraGridCol vCol in Cols)
                {
                    if (vCol.Name.ToUpper() == vName.ToUpper())
                        return vCol;
                }

                foreach (AraGridCol vCol in Cols)
                {
                    if (vCol.NameReal.ToUpper() == vName.ToUpper())
                        return vCol;
                }
                return null;
            }
        }

        public AraGridCol this[int vIndex]
        {
            get
            {
                try
                {
                    return Cols[vIndex];
                }
                catch
                {
                    return null;
                }
            }
        }

        public string GetScript()
        {
            string vTmp = "";
            if (Count > 0)
            {
                vTmp += " colNames: [";
                for (int n = 0; n < Count; n++)
                {
                    vTmp += "'" + AraTools.StringToStringJS( this[n].Caption) + "',";
                }
                vTmp = vTmp.Substring(0, vTmp.Length - 1);
                vTmp += "],";

                vTmp += " colModel: [";
                for (int n = 0; n < Count; n++)
                {
                    vTmp += this[n].GetScript() + ",";
                }
                vTmp = vTmp.Substring(0, vTmp.Length - 1);
                vTmp += "],";


                string vTmpGroupingView = " grouping:true, groupingView : {";

                bool vGroupField = false;

                vTmpGroupingView += "groupField : [";
                foreach (AraGridCol Col in Cols)
                {
                    if (Col.Group)
                    {
                        vGroupField = true;
                        vTmpGroupingView += "'" + Col.NameReal + "',";
                    }
                }
                vTmpGroupingView = vTmpGroupingView.Substring(0, vTmpGroupingView.Length - 1);
                vTmpGroupingView += "],";

                vTmpGroupingView += "groupColumnShow : [";
                foreach (AraGridCol Col in Cols)
                {
                    if (Col.Group)
                    {
                        
                        vTmpGroupingView += "true,";
                    }
                }
                vTmpGroupingView = vTmpGroupingView.Substring(0, vTmpGroupingView.Length - 1);
                vTmpGroupingView += "],";

                vTmpGroupingView += "groupText  : [";
                foreach (AraGridCol Col in Cols)
                {
                    if (Col.Group)
                    {

                        vTmpGroupingView += "'<b>{0}</b>',";
                    }
                }
                vTmpGroupingView = vTmpGroupingView.Substring(0, vTmpGroupingView.Length - 1);
                vTmpGroupingView += "]";

                vTmpGroupingView += "},";

                if (vGroupField)
                    vTmp += vTmpGroupingView;



                vTmp = vTmp.Substring(0, vTmp.Length - 1);
            }
            return vTmp;
        }

        public int GetPosCol(string vCol)
        {
            return GetPosCol(this[vCol]);
        }

        public int GetPosCol(AraGridCol vCol)
        {
            int vpos = 0;
            foreach (AraGridCol TmpCol in Cols)
            {
                if (TmpCol == vCol)
                    return vpos;
                vpos++;
            }
            return -1;
        }

        public string[] Keys
        {
            get
            {
                List<String> TmpKeys = new List<String>();

                for (int n = 0; n < Count; n++)
                    TmpKeys.Add(this[n].Name);
                return TmpKeys.ToArray();
            }
        }

        private bool _AutoOrder=true;
        public bool AutoOrder
        {
            get { return _AutoOrder; }
            set
            {
                _AutoOrder = value;

                Grid.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetAutoOrderCols(" + (_AutoOrder ? "true" : "false") + "); \n");
            }
        }

        public AraGridCol[] ToArray()
        {
            return Cols.ToArray();
        }

        public void ShowFormColsHidden()
        {
            AraGridColsHidden FrmSelection = new AraGridColsHidden(this.Grid);
            FrmSelection.Show((object vObj) =>
            {
                

            });
        }
    }

}
