// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ara2.Json;


// SUB GRID -> http://www.trirand.com/jqgridwiki/doku.php?id=wiki:subgrid_as_grid

namespace Ara2.Components
{
    [Serializable]
    public class AraGridTree 
    {
        AraGrid _AraGrid;
        public AraGrid AraGrid
        {
            get
            {
                return _AraGrid;
            }
        }

        public AraGridTree(AraGrid vAraGrid, AraGridCol vColId, AraGridCol vColFather, AraGridCol vColCaption)
        {
            _AraGrid = vAraGrid;
            _ColId = vColId;
            _ColFather = vColFather;
            _ColCaption = vColCaption;

            _ColId.hidden = true;
            _ColFather.hidden = true;

            AraGrid.Sortable = false;


            Tick vTick = Tick.GetTick();
            AraGrid.TickScriptCall();

            vTick.Script.Send(" vObj.Tree = true; \n");
            vTick.Script.Send(" vObj.TreeColID = '" + vColId.NameReal + "'; \n");
            vTick.Script.Send(" vObj.TreeColFather = '" + vColFather.NameReal + "'; \n");
            vTick.Script.Send(" vObj.TreeColCaption = '" + vColCaption.NameReal + "'; \n");

            

            IcoExpand = "ui-icon-folder-collapsed";
            IcoContract = "ui-icon-folder-open";
            IcoLoad = "ui-icon-refresh";
            IcoWidth = 18;
            
        }

        private AraGridCol _ColId = null;
        private AraGridCol _ColFather = null;
        private AraGridCol _ColCaption = null;

        public AraGridCol ColId
        {
            get { return _ColId; }
        }
        public AraGridCol ColFather
        {
            get { return _ColFather; }
        }
        public AraGridCol ColCaption
        {
            get { return _ColCaption; }
        }


        public AraGridRow GetRowByCodId(string vCod)
        {
            foreach (AraGridRow vTmpR in AraGrid.Rows.ToArray())
            {
                if (vTmpR[_ColId.Name].Text  == vCod)
                    return vTmpR;
            }

            return null;
        }

        public AraGridRow[] GetRowInColFatherByCodId(string vCod)
        {
            List<AraGridRow> Tmp = new List<AraGridRow>();
            foreach (AraGridRow vTmpR in AraGrid.Rows.ToArray())
            {
                if (vTmpR[_ColFather.Name].Text == vCod)
                    Tmp.Add(vTmpR);
            }

            return Tmp.ToArray();
        }

        private string _IcoExpand; // "ui-icon-triangle-1-e";

        public string IcoExpand
        {
            get { return _IcoExpand; }
            set { 
                _IcoExpand = value;
                Tick.GetTick().Script.Send(" vObj.TreeIcoExpand = '" + _IcoExpand.Replace("'", "''") + "'; \n");
            }
        }
        private string _IcoContract ;//"ui-icon-triangle-1-s";

        public string IcoContract
        {
            get { return _IcoContract; }
            set { 
                _IcoContract = value;
                Tick.GetTick().Script.Send(" vObj.TreeIcoContract = '" + _IcoContract.Replace("'", "''") + "'; \n");
            }
        }
        private string _IcoLoad;

        public string IcoLoad
        {
            get { return _IcoLoad; }
            set { 
                _IcoLoad = value;
                Tick.GetTick().Script.Send(" vObj.TreeIcoLoad = '" + _IcoLoad.Replace("'", "''") + "'; \n");
            }
        }


        public void ExpandAllRows()
        {
            foreach (AraGridRow vTmpR in AraGrid.Rows.ToArray())
            {
                if (vTmpR.TreeContainer)
                    vTmpR.TreeExpand = true;
            }
        }

        public void RetraiAllRows()
        {
            foreach (AraGridRow vTmpR in AraGrid.Rows.ToArray())
            {
                if (vTmpR.TreeContainer)
                    vTmpR.TreeExpand = false;
            }
        }

        private int _IcoWidth;

        public int IcoWidth
        {
            get { return _IcoWidth; }
            set
            {
                _IcoWidth = value;
                Tick.GetTick().Script.Send(" vObj.TreeIcoWidth = " + _IcoWidth + "; \n");
            }
        }

        List<int> OnClientUnloadUpdateCaptions_ClientId = new List<int>();
        public void OnClientUnloadUpdateCaptions()
        {
            Tick vTick = Tick.GetTick();
            if (!OnClientUnloadUpdateCaptions_ClientId.Contains(vTick.Id))
            {
                OnClientUnloadUpdateCaptions_ClientId.Add(vTick.Id);
                vTick.Script.UnLoad += Script_UnLoad;
            }
        }

        void Script_UnLoad()
        {
            
            foreach (AraGridRow Row in AraGrid.Rows.ToArray())
            {
                Row.CheckTreeTypeLevesChange();
                Row[ColCaption].Text = Row[ColCaption].Text;
            }

            OnClientUnloadUpdateCaptions_ClientId.Remove(Tick.GetTick().Id);
        }

    }

}
