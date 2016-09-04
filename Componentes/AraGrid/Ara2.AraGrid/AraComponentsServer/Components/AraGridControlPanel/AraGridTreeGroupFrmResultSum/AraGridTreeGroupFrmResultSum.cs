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
    [AraDevComponent(vDisplayToolBar: false)]
    public class AraGridTreeGroupFrmResultSum : Ara2.Components.AraWindow
    {
        AraButton bSair;
        AraGrid GridResultado;

        AraGridTreeGroup _TreeGroup;
        AraLabel lColunaGrup;
        AraGridRow RowStart;
        AraGridCol ColGrup;
        AraGridCol[] ColGrups;

        public AraGridTreeGroupFrmResultSum(AraGridTreeGroup vTreeGroup,AraGridRow vRowStart,AraGridCol vColGrup):
            base((IAraContainerClient)vTreeGroup.AraGrid.ConteinerFather)
        {
            _TreeGroup = vTreeGroup;
            RowStart = vRowStart;
            ColGrup = vColGrup;

            List<AraGridCol> TmpColGrups = new List<AraGridCol>();
            foreach (AraGridCol TmpCol in _TreeGroup.ColsGrup)
            {
                TmpColGrups.Add(TmpCol);
                if (TmpCol.Name == ColGrup.Name)
                    break;
            }

            ColGrups = TmpColGrups.ToArray();

            Assembly asm = Assembly.GetExecutingAssembly();
            Stream stream = asm.GetManifestResourceStream("Ara.Component.AraGrid.AraGridTreeGroupFrmResultSum.AraGridTreeGroupFrmResultSum.htm");

            this.Active += AraGridTreeGroupFrmSelection_Active;
        }


        private void AraGridTreeGroupFrmSelection_Active()
        {
            this.Width = 500;
            this.Height = 200;
            this.Title = "Resultado da Soma das colunas desejadas";

            bSair = new AraButton(this);
            bSair.Click += bSair_Click;
            //bSair.Anchor.Right = true;

            lColunaGrup = new AraLabel(this);
            //lColunaGrup.Anchor = new Anchor.AraAnchor(lColunaGrup, true, true);

            Dictionary<AraGridCol, decimal> TmpSum = new Dictionary<AraGridCol, decimal>();
            List<AraGridCol> TmpColSum = _TreeGroup.GetColsGrupSum(ColGrup.Name);

            bool AchoIni = false;
            string vVariavelQuebra = "";
            foreach (AraGridRow Row in _TreeGroup.AraGrid.Rows.ToArray())
            {
                if (!AchoIni)
                {
                    if (Row.ID == RowStart.ID)
                        AchoIni = true;

                    vVariavelQuebra = "";
                    foreach (AraGridCol TmpCol in ColGrups)
                    {
                        vVariavelQuebra += Row[TmpCol].Text + ",";
                    }
                    lColunaGrup.Text = "<b>" + ColGrup.Caption + "</b> : " + Row[ColGrup].Text;
                }

                if (AchoIni)
                {

                    string vTmpVariavelQuebra = "";
                    foreach (AraGridCol TmpCol in ColGrups)
                    {
                        vTmpVariavelQuebra += Row[TmpCol].Text + ",";
                    }

                    if (vVariavelQuebra == vTmpVariavelQuebra)
                    {
                        foreach (AraGridCol TmpCol in TmpColSum)
                        {
                            decimal TmpValue = 0;
                            try
                            {
                                TmpValue = Convert.ToDecimal(Row[TmpCol].Text);
                            }
                            catch { }

                            if (!TmpSum.ContainsKey(TmpCol))
                                TmpSum.Add(TmpCol, TmpValue);
                            else
                                TmpSum[TmpCol] += TmpValue;
                        }
                    }
                }


            }

            GridResultado = new AraGrid(this);
            GridResultado.Cols.Add(new AraGridCol(GridResultado, "Coluna", "caption"));
            GridResultado.Cols.Add(new AraGridCol(GridResultado, "Valor", "valor"));
            GridResultado.Commit();
            //GridResultado.Anchor = new Anchor.AraAnchor(GridResultado, true, true, true, true);

            foreach (AraGridCol Col in TmpSum.Keys)
            {
                GridResultado.Rows.Add(Col.Name, new Dictionary<string, string>
                {
                    {"caption",Col.Caption},
                    {"valor",string.Format("{0:n}",TmpSum[Col])},
                });
            }


            GridResultado.AutoAdjustColumnWidth();

        }

        private void bSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}