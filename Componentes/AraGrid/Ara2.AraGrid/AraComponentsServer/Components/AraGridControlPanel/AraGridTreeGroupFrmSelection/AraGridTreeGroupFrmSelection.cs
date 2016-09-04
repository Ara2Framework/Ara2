// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Reflection;
using Ara2;
using Ara2.Components;
using Ara2.Dev;

namespace Ara2.Components.Grid.Group
{
    [Serializable]
    [AraDevComponent(vDisplayToolBar:false)]
    public class AraGridTreeGroupFrmSelection : Ara2.Components.AraWindow
    {
        AraSelect TxtColunasGroup;
        AraSelect TxtColunasGroupSum;
        AraSelect TxtColunasGroupSumCols;
        AraButton bAdd;
        AraButton bEx;

        AraButton bAddSum;
        AraButton bAddSumR;

        AraSelect TxtColunas;

        AraButton bConfirmar;
        AraButton bCancelar;

        AraGridCol[] Colunas;
        AraGridCol[] ColunasGroup;

        AraGridTreeGroup _TreeGroup;
        Dictionary<AraGridCol, List<AraGridCol>> _ColsGrupSum;

        public AraGridTreeGroupFrmSelection(AraGridTreeGroup vTreeGroup):
            base((IAraContainerClient)vTreeGroup.AraGrid.ConteinerFather)
        {
            _TreeGroup = vTreeGroup;
            _ColsGrupSum = _TreeGroup.ColsGrupSum;
            ColunasGroup = _TreeGroup.ColsGrup;


            this.Active += AraGridTreeGroupFrmSelection_Active;
        }


        private void AraGridTreeGroupFrmSelection_Active()
        {
            this.Width = 500;
            this.Height = 430;
            this.Title = "Selecione as Colunas";


            List<AraGridCol> TmpCols = new List<AraGridCol>();

            foreach (AraGridCol Col in _TreeGroup.AraGrid.Cols.ToArray())
            {
                if (Col.Name != _TreeGroup.NameColCod &&
                    Col.Name != _TreeGroup.NameColCodCaption &&
                    Col.Name != _TreeGroup.NameColCodFather &&
                    Col.Name != _TreeGroup.NameColCodIdentification
                )
                    TmpCols.Add(Col);
            }
            Colunas = TmpCols.ToArray();

            

            TxtColunasGroup = new AraSelect( this);
            TxtColunasGroup.Change += TxtColunasGroup_Change;
            foreach(AraGridCol Col in ColunasGroup)
            {
                TxtColunasGroup.List.Add(Col.Name, Col.Caption);
            }

            TxtColunas = new AraSelect( this);
            foreach (AraGridCol Col in Colunas)
            {
                if (!TxtColunasGroup.List.Contains(Col.Name))
                    TxtColunas.List.Add(Col.Name, Col.Caption);
            }


            bAdd = new AraButton( this);
            bAdd.Ico = AraButton.ButtonIco.arrow_1_w;
            
            bAdd.Click += bAdd_Click;

            bEx = new AraButton(this);
            bEx.Ico = AraButton.ButtonIco.arrow_1_e;
            
            bEx.Click += bEx_Click;

            bConfirmar = new AraButton(this);
            //bConfirmar.Anchor.Bottom = true;
            bConfirmar.Click += bConfirmar_Click;
			
            bCancelar = new AraButton(this);
			//bCancelar.Anchor.Bottom = true;
            bCancelar.Click += bCancelar_Click;

            TxtColunasGroupSum = new AraSelect( this);

            TxtColunasGroupSumCols = new AraSelect(this);

            foreach (AraGridCol Col in _TreeGroup.AraGrid.Cols.ToArray())
            {
                if (Col.Name != _TreeGroup.NameColCod &&
                    Col.Name != _TreeGroup.NameColCodCaption &&
                    Col.Name != _TreeGroup.NameColCodFather &&
                    Col.Name != _TreeGroup.NameColCodIdentification
                )
                    TxtColunasGroupSumCols.List.Add(Col.Name,Col.Caption);
            }


            bAddSum = new AraButton( this);
            bAddSum.Ico = AraButton.ButtonIco.arrow_1_w;
            
            bAddSum.Click += bAddSum_Click;

            bAddSumR = new AraButton(this);
            bAddSumR.Ico = AraButton.ButtonIco.arrow_1_e;
            bAddSumR.Click += bAddSumR_Click;


        }

        //private void bCima_Click(object sender, EventArgs e)
        //{
            
        //}

        //private void bBaixo_Click(object sender, EventArgs e)
        //{

        //}

        private void bAdd_Click(object sender, EventArgs e)
        {
            if (TxtColunas.Text != "")
            {
                TxtColunasGroup.List.Add(TxtColunas.Text, TxtColunas.List[TxtColunas.Text].Caption);
                TxtColunas.List.Remove(TxtColunas.Text);
            }
        }

        private void bEx_Click(object sender, EventArgs e)
        {
            if (TxtColunasGroup.Text != "")
            {
                TxtColunas.List.Add(TxtColunasGroup.Text, TxtColunasGroup.List[TxtColunasGroup.Text].Caption);
                TxtColunasGroup.List.Remove(TxtColunasGroup.Text);
            }
        }

        private void bConfirmar_Click(object sender, EventArgs e)
        {
            //if (!(TxtColunasGroup.List.Count>0))
            //{
            //    AraTools.Alert("Selecione uma coluna");
            //    return;
            //}

            List<AraGridCol> Cols = new List<AraGridCol>();

            foreach (Ara2.Components.Select.AraSelectListItem Item in TxtColunasGroup.List.ToArray())
            {
                Cols.Add(GetCol(Item.Key));
            }

            _TreeGroup.ColsGrup = Cols.ToArray();
            _TreeGroup.ColsGrupSum=_ColsGrupSum;

            this.Close(true);
        }

        private void bCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private AraGridCol GetCol(string vName)
        {
            foreach (AraGridCol Col in Colunas)
            {
                if (Col.Name == vName)
                    return Col;
            }

            return null;
        }


        private void TxtColunasGroup_Change(object sender, EventArgs e)
        {
            TxtColunasGroupSum_Carrega();
        }

        private void TxtColunasGroupSum_Carrega()
        {
            TxtColunasGroupSum.Clear();
            if (TxtColunasGroup.Text != "")
            {
                
                AraGridCol ColGrup = _TreeGroup.AraGrid.Cols[TxtColunasGroup.Text];

                foreach (AraGridCol Col in _ColsGrupSum.Keys)
                {
                    if (Col.Name == ColGrup.Name)
                    {
                        foreach (AraGridCol ColS in _ColsGrupSum[Col])
                        {
                            TxtColunasGroupSum.List.Add(ColS.Name, ColS.Caption);
                        }

                        return;
                    }
                }
            }
        }
        
        private void bAddSum_Click(object sender, EventArgs e)
        {

            if (TxtColunasGroup.Text != "" && TxtColunasGroupSumCols.Text !="")
            {
                AraGridCol ColGrup = _TreeGroup.AraGrid.Cols[TxtColunasGroup.Text];
                AraGridCol ColAdd = _TreeGroup.AraGrid.Cols[TxtColunasGroupSumCols.Text];

                foreach (AraGridCol Col in _ColsGrupSum.Keys)
                {
                    if (Col.Name == ColGrup.Name)
                    {
                        _ColsGrupSum[Col].Add(ColAdd);
                        TxtColunasGroupSum_Carrega();
                        return;
                    }
                }

                _ColsGrupSum.Add(ColGrup,new List<AraGridCol>{ ColAdd});
                TxtColunasGroupSum_Carrega();
            }
        }

        private void bAddSumR_Click(object sender, EventArgs e)
        {
            if (TxtColunasGroup.Text != "" )
            {
                AraGridCol ColGrup = _TreeGroup.AraGrid.Cols[TxtColunasGroup.Text];
                AraGridCol ColR = _TreeGroup.AraGrid.Cols[TxtColunasGroupSum.Text];

                foreach (AraGridCol Col in _ColsGrupSum.Keys)
                {
                    if (Col.Name == ColGrup.Name)
                    {
                        foreach (AraGridCol ColTmpR in _ColsGrupSum[Col])
                        {
                            if (ColTmpR.Name == ColR.Name)
                            {
                                _ColsGrupSum[Col].Remove(ColTmpR);
                                TxtColunasGroupSum_Carrega();
                                return;
                            }
                        }
                    }
                }
            }

        }

    }
}