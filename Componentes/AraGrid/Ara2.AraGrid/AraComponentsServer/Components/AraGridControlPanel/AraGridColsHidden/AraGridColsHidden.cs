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

namespace Ara2.Components.Grid.ColsHidden
{
    [Serializable]
    [AraDevComponent(vDisplayToolBar: false)]
    public class AraGridColsHidden : Ara2.Components.AraWindow
    {


        AraGrid GridColunas;

        AraButton bConfirmar;
        AraButton bCancelar;

        private AraGrid _Grid;

        public AraGridColsHidden(AraGrid vGrid):
            base((IAraContainerClient)vGrid.ConteinerFather)
        {
            _Grid = vGrid;

            Assembly asm = Assembly.GetExecutingAssembly();
            Stream stream = asm.GetManifestResourceStream("Ara.Component.AraGrid.AraGridColsHidden.AraGridColsHidden.htm");

            this.Active += AraGridTreeGroupFrmSelection_Active;
        }


        public void AraGridTreeGroupFrmSelection_Active()
        {
            this.Width = 500;
            this.Height = 300;
            this.Title = "Selecione as Colunas";

            GridColunas = new AraGrid(this);
            GridColunas.MultiSelect = true;

            GridColunas.Cols.Add(new AraGridCol(GridColunas,"name","name",vHidden:true));
            GridColunas.Cols.Add(new AraGridCol(GridColunas, "Coluna", "Coluna"));
            GridColunas.Commit();
            //GridColunas.Anchor = new Anchor.AraAnchor(GridColunas, true, true, true, true);
                      
            foreach(AraGridCol Col in _Grid.Cols.ToArray())
            {
                bool ColunaInterna = false;

                if (_Grid.Tree != null)
                {
                    if (
                        _Grid.Tree.ColCaption.Name == Col.Name ||
                        _Grid.Tree.ColFather.Name == Col.Name ||
                        _Grid.Tree.ColId.Name == Col.Name
                    ) ColunaInterna = true;
                }

                if (_Grid.TreeGroup != null)
                {
                    if (
                        _Grid.TreeGroup.NameColCod == Col.Name ||
                        _Grid.TreeGroup.NameColCodCaption == Col.Name ||
                        _Grid.TreeGroup.NameColCodFather == Col.Name ||
                        _Grid.TreeGroup.NameColCodIdentification == Col.Name
                    ) ColunaInterna = true;
                }

                if (!ColunaInterna)
                {
                    GridColunas.Rows.Add(Col.NameReal, new Dictionary<string, string>
                    {
                        {"name",Col.NameReal},
                        {"Coluna",Col.Caption},
                    });

                    GridColunas.Rows[Col.NameReal].Select = !Col.hidden;
                }
            }

            bConfirmar = new AraButton(this);
            //bConfirmar.Anchor.Bottom = true;
            bConfirmar.Click += bConfirmar_Click;
			
            bCancelar = new AraButton(this);
			//bCancelar.Anchor.Bottom = true;
            //bCancelar.Anchor.Right = true;
            bCancelar.Click += bCancelar_Click;

        }

        private void bConfirmar_Click(object sender, EventArgs e)
        {
            foreach (AraGridRow Row in GridColunas.Rows.ToArray())
            {
                _Grid.Cols[Row["name"].Text].hidden = !Row.Select;
            }

            this.Close();
        }

        private void bCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}