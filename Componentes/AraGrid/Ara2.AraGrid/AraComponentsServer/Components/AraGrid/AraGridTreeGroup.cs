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
using Ara2.Components.Grid.Group;


namespace Ara2.Components
{
    [Serializable]
    public class AraGridTreeGroup
    {
        AraGrid _AraGrid;
        public AraGrid AraGrid
        {
            get
            {
                return _AraGrid;
            }
        }

        private string _NameColCod = "AraGridTreeGroup_Cod";

        public string NameColCod
        {
            get { return _NameColCod; }
            // set { _NameColCod = value; }
        }
        private string _NameColCodFather = "AraGridTreeGroup_CodFather";

        public string NameColCodFather
        {
            get { return _NameColCodFather; }
            // set { _NameColCodFather = value; }
        }

        private string _NameColCodCaption = "AraGridTreeGroup_Caption";
        public string NameColCodCaption
        {
            get { return _NameColCodCaption; }
            // set { _NameColCodCaption = value; }
        }

        private string _NameColCodIdentification = "AraGridTreeGroup_Identification";
        public string NameColCodIdentification
        {
            get { return _NameColCodIdentification; }
            // set { _NameColCodCaption = value; }
        }

        public Action OnSelColsGrop = null;

        private AraGridCol[] _ColsGrup;
        public AraGridCol[] ColsGrup
        {
            get { return _ColsGrup; }
            set {
                _ColsGrup = value;

                if (OnSelColsGrop != null)
                    OnSelColsGrop();
            }
        }

        private Dictionary<AraGridCol, List<AraGridCol>> _ColsGrupSum = new Dictionary<AraGridCol, List<AraGridCol>>();
        public Dictionary<AraGridCol, List<AraGridCol>> ColsGrupSum
        {
            get { return _ColsGrupSum; }
            set { _ColsGrupSum = value; }
        }

        public  List<AraGridCol> GetColsGrupSum(string vNameCol)
        {
            foreach (AraGridCol Col in _ColsGrupSum.Keys)
            {
                if (Col.Name == vNameCol)
                    return _ColsGrupSum[Col];
            }
            return null;
        }

        public AraGridTreeGroup(AraGrid vAraGrid, AraGridCol[] vColGrup, Dictionary<AraGridCol, List<AraGridCol>> vColsGrupSum):
            this(vAraGrid,vColGrup)
        {
            _ColsGrupSum = vColsGrupSum;
        }

        public AraGridTreeGroup(AraGrid vAraGrid, AraGridCol[] vColGrup)
        {
            _AraGrid = vAraGrid;
            _ColsGrup = vColGrup;

            if (_AraGrid.TreeGroup == null)
                if (_AraGrid.Tree !=null)
                    throw new Exception("The use of AraGridTreeGroup are incompatible with AraGridTree.");

            //foreach (AraGridCol Col in vColGrup)
            //{
            //    Col.hidden = true;
            //}

            if (_AraGrid.TreeGroup == null)
            {
                _AraGrid.Cols.Add(new AraGridCol(_AraGrid, _NameColCod, _NameColCod, vHidden: true), 0);
                _AraGrid.Cols.Add(new AraGridCol(_AraGrid, _NameColCodFather, _NameColCodFather, vHidden: true), 1);
                _AraGrid.Cols.Add(new AraGridCol(_AraGrid, "", _NameColCodCaption), 2);
                _AraGrid.Cols.Add(new AraGridCol(_AraGrid, _NameColCodIdentification, _NameColCodIdentification, vHidden: true), 3);
            }

            _AraGrid.Tree = new AraGridTree(_AraGrid, _AraGrid.Cols[_NameColCod], _AraGrid.Cols[_NameColCodFather], _AraGrid.Cols[_NameColCodCaption]);
        }

        //public void ShowFormSelection()
        //{
        //    AraGridTreeGroupFrmSelection FrmSelection = new AraGridTreeGroupFrmSelection( this);
        //    FrmSelection.Show((object vObj)=>
        //    {
        //        if (vObj != null)
        //            _AraGrid.PageReload();
        //    });
        //}


        public void ButtonSum_Click(AraGrid Object, Hashtable Parans)
        {
            AraGridRow RowIni = AraGrid.Rows[Parans["RowId"].ToString()];
            AraGridCol ColGrup = AraGrid.Cols[Parans["ColGrup"].ToString()];

            AraGridTreeGroupFrmResultSum AraGridTreeGroupFrmResultSum = new Grid.Group.AraGridTreeGroupFrmResultSum(this, RowIni, ColGrup);
            AraGridTreeGroupFrmResultSum.Show((object vObj) =>
                {
                    
                });

             
        }
    }

}

