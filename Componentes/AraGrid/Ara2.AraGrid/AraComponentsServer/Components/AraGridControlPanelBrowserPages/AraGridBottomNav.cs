using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ara2.Dev;

namespace Ara2.Components
{
    [Serializable]
    [AraDevComponent(vConteiner:false,vDisplayToolBar:false)]
    public class AraGridBottomNav : AraGridControlPanel
    {
        public AraGridBottomNav(IAraObjectClienteServer Container):
            base(Container)
        {
            OnPageReload = new AraEvent<Action>();
            Bottom_CreateButtons();
        }

        private bool _VisibleNav = true;
        public bool VisibleNav
        {
            get { return _VisibleNav; }
            set
            {
                if (_VisibleNav != value)
                {
                    _VisibleNav = value;

                    bVoltarPrimeiro.Visible = _VisibleNav;
                    bVoltar.Visible = _VisibleNav;
                    lRegistro.Visible = _VisibleNav;
                    lIr.Visible = _VisibleNav;
                    lIrUltimo.Visible = _VisibleNav;
                    TxtRegistrosPorPagina.Visible = _VisibleNav;
                }
            }
        }

        AraObjectInstance<AraButton> _bVoltarPrimeiro = new AraObjectInstance<AraButton>();
        AraButton bVoltarPrimeiro
        {
            get { return _bVoltarPrimeiro.Object; }
            set { _bVoltarPrimeiro.Object = value; }
        }

        AraObjectInstance<AraButton> _bVoltar = new AraObjectInstance<AraButton>();
        AraButton bVoltar
        {
            get { return _bVoltar.Object; }
            set { _bVoltar.Object = value; }
        }

        AraObjectInstance<AraLabel> _lRegistro = new AraObjectInstance<AraLabel>();
        AraLabel lRegistro
        {
            get { return _lRegistro.Object; }
            set { _lRegistro.Object = value; }
        }

        AraObjectInstance<AraButton> _lIr = new AraObjectInstance<AraButton>();
        AraButton lIr
        {
            get { return _lIr.Object; }
            set { _lIr.Object = value; }
        }

        AraObjectInstance<AraButton> _lIrUltimo = new AraObjectInstance<AraButton>();
        AraButton lIrUltimo
        {
            get { return _lIrUltimo.Object; }
            set { _lIrUltimo.Object = value; }
        }

        AraObjectInstance<AraSelect> _TxtRegistrosPorPagina = new AraObjectInstance<AraSelect>();
        AraSelect TxtRegistrosPorPagina
        {
            get { return _TxtRegistrosPorPagina.Object; }
            set { _TxtRegistrosPorPagina.Object = value; }
        }

        private int _Page = 1;

        public int Page
        {
            get { return _Page; }
            set { 
                _Page = value;
                PageReload();
            }
        }
        private int _PageMax = 1;
        public int PageMax
        {
            get { return _PageMax; }
            set { 
                _PageMax = value;
                ReloadCaption();
            }
        }

        private int _PageMaxRecords = 0;
        public int PageMaxRecords
        {
            get { return _PageMaxRecords; }
            set { 
                

                if (TxtRegistrosPorPagina.List.ToArray().First(a => a.Key == value.ToString()) == null)
                    TxtRegistrosPorPagina.List.Add(value.ToString(), value.ToString());

                TxtRegistrosPorPagina.Text = value.ToString();
                _PageMaxRecords = value;
                PageReload();
            }
        }

        

        private void Bottom_CreateButtons()
        {
            bVoltarPrimeiro = new AraButton(this.NewConteiner());
            bVoltarPrimeiro.Ico = AraButton.ButtonIco.seek_first;
            bVoltarPrimeiro.Click += bVoltarPrimeiro_Click;
            bVoltarPrimeiro.Visible = _VisibleNav;
            bVoltarPrimeiro.Height = new AraDistance("100%");
            bVoltarPrimeiro.Width = new AraDistance("26px");

            bVoltar = new AraButton(this.NewConteiner());
            bVoltar.Ico = AraButton.ButtonIco.seek_prev;
            bVoltar.Click += bVoltar_Click;
            bVoltar.Visible = _VisibleNav;
            bVoltar.Height = new AraDistance("100%");
            bVoltar.Width = new AraDistance("26px");

            lRegistro = new AraLabel(this.NewConteiner());
            lRegistro.Visible = _VisibleNav;
            lRegistro.Width = null;
            lRegistro.Height = null;

            lIr = new AraButton(this.NewConteiner());
            lIr.Ico = AraButton.ButtonIco.seek_next;
            lIr.Click += lIr_Click;
            lIr.Visible = _VisibleNav;
            lIr.Height = new AraDistance("100%");
            lIr.Width = new AraDistance("26px");

            lIrUltimo = new AraButton(this.NewConteiner());
            lIrUltimo.Ico = AraButton.ButtonIco.seek_end;
            lIrUltimo.Click += lIrUltimo_Click;
            lIrUltimo.Visible = _VisibleNav;
            lIrUltimo.Height = new AraDistance("100%");
            lIrUltimo.Width = new AraDistance("26px");

            TxtRegistrosPorPagina = new AraSelect(this.NewConteiner());
            TxtRegistrosPorPagina.List.Add("30", "30");
            TxtRegistrosPorPagina.List.Add("50", "50");
            TxtRegistrosPorPagina.List.Add("100", "100");
            TxtRegistrosPorPagina.Text = "30";
            _PageMaxRecords = 30;
            TxtRegistrosPorPagina.Change += TxtRegistrosPorPagina_Click;
            TxtRegistrosPorPagina.Visible = _VisibleNav;
            TxtRegistrosPorPagina.Width = null;

            ReloadCaption();
        }

        public void bVoltarPrimeiro_Click(object sender, EventArgs e)
        {
            this.Page = 1;
        }

        public void bVoltar_Click(object sender, EventArgs e)
        {
            if (Page >1) 
                Page -= 1;
        }

        public void lIr_Click(object sender, EventArgs e)
        {
            if (Page + 1 <= PageMax)
                Page += 1;
            else
            {
                // Recarrega para ver se tem mais paginas
                AraTools.AsynchronousFunction(PageReload);
            }
        }

        public void lIrUltimo_Click(object sender, EventArgs e)
        {
            Page = PageMax;
        }

        public void TxtRegistrosPorPagina_Click(object sender, EventArgs e)
        {
            _PageMaxRecords = Convert.ToInt32(TxtRegistrosPorPagina.Text);
            PageReload();
        }

        public void ReloadCaption()
        {
            lRegistro.Text = Page + "/" + PageMax;
        }

        public AraEvent<Action> OnPageReload { get; set; }
        public void PageReload()
        {
            try
            {
                if (OnPageReload.InvokeEvent != null)
                    OnPageReload.InvokeEvent();
            }
            catch (Exception err)
            {
                throw new Exception("Error in PageReload.\n" + err.Message);
            }
            finally
            {
                ReloadCaption();
            }
        }

    }
}
