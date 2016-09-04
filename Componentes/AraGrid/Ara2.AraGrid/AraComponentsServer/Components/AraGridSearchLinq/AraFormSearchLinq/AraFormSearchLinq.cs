using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Reflection;
using System.Data.OleDb;
using Ara2;
using Ara2.Components;

namespace Ara2.Grid
{
    [Serializable]
    public class AraFormSearchLinq : AraWindow
    {
        private AraEvent<Func<IQueryable<object>>> _GetQuery = new AraEvent<Func<IQueryable<object>>>();
        public AraEvent<Func<IQueryable<object>>> GetQuery
        {
            get { return _GetQuery; }
            set { 
                _GetQuery = value;

                if (Grid != null)
                    Grid.GetQuery = _GetQuery;
            }
        }
     


        public AraFormSearchLinq(IAraObject vConteiner, Func<IQueryable<object>> vGetQuery) :
            this(vConteiner, vGetQuery, null)
        {

        }

        

        private AraEvent<AraGridSearchLinq.dOnCommitBefore> _OnCommitBefore = new AraEvent<AraGridSearchLinq.dOnCommitBefore>();
        public AraEvent<AraGridSearchLinq.dOnCommitBefore> OnCommitBefore
        {
            get { return _OnCommitBefore; }
            set { _OnCommitBefore = value; }
        }

        public AraFormSearchLinq(IAraObject vConteiner, Func<IQueryable<object>> vGetQuery,AraGridSearchLinq.dOnCommitBefore vOnCommitBefore) :
            base(vConteiner)
        {
            GetQuery += vGetQuery;
            if(vOnCommitBefore != null)
                OnCommitBefore += vOnCommitBefore;

            
            this.Active += FrmGrupo_Active;
        }

        //public AraFormSearchFB(FbConnection fbConnection, string p)
        //{
        //    // TODO: Complete member initialization
        //    this.fbConnection = fbConnection;
        //    this.p = p;
        //}

        
        AraButton bCancelar;
        AraGridSearchLinq Grid;
        public void FrmGrupo_Active()
        {
            this.Width = 475;
            this.Height = 400;
            this.Maximize = true;

            bCancelar = new AraButton(this);
            bCancelar.Click += bCancelar_Click;
            bCancelar.Text = "Cancelar";
            bCancelar.Width = 100;
            bCancelar.Height = 25;
            bCancelar.Anchor.Bottom = 10;
            bCancelar.Anchor.Right = 10;


            Grid = new AraGridSearchLinq(this, GetQuery.InvokeEvent, Grid_OnCommitBefore);
            Grid.Anchor.Top = 10;
            Grid.Anchor.Left  = 10;
            Grid.Anchor.Right = 10;
            Grid.Anchor.Bottom = 40;
            Grid.ReturnSearch += Grid_ReturnSearch;
            Grid.OnLoadData += Grid_OnLoadData;
            Grid.Grid.KeyDown += Grid_KeyDown;
        }

        void Grid_ReturnSearch(AraGrid Object, AraGridRow Row)
        {
            this.Close(Row);
        }

        void Grid_OnCommitBefore(AraGridSearchLinq vGrid)
        {
            if (OnCommitBefore.InvokeEvent!=null)
                OnCommitBefore.InvokeEvent(vGrid);
            
        }
        bool PrimeiroFoto = true;
        void GridSetFocus()
        {
            if (PrimeiroFoto)
            {
                try
                {
                    if (Grid.Grid.Rows.Count > 0)
                    {
                        string ColName = Grid.Grid.Cols.ToArray().Where(a => a.hidden == false).FirstOrDefault().Index;
                        Grid.Grid.Rows[0][ColName].SetFocus();
                        PrimeiroFoto = false;
                    }
                }
                catch { }
            }
        }

        private void Grid_OnLoadData(AraGridSearchLinq vGrid)
        {
            AraTools.AsynchronousFunction(GridSetFocus);
        }

        private void Grid_KeyDown(AraGrid Object, int vKey)
        {
            if (vKey == 13 && !string.IsNullOrEmpty(Grid.Grid.SelRow) && Grid.Grid.SelRow != "Search")
            {
                Grid_ReturnSearch(Grid.Grid, Grid.Grid.Rows[Grid.Grid.SelRow]);
            }
        }

        public void bCancelar_Click(object vObj, EventArgs e)
        {
            this.Close();
        }


    }
}