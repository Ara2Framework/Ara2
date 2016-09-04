using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ara2.Components;
using Ara2;
using System.Text;

using System.Linq.Dynamic;
using System.Linq.Expressions;


namespace Ara2.Grid.Filters.Forms
{
    public class FrmAraGridSearchLinqFilterString : AraDesign.FrmAraGridSearchLinqFilterStringAraDesign
    {
        private AraGridSearchLinq _SearchLinq=null;
        private AraGridSearchLinq.SColuna _Coluna = null;

        public FrmAraGridSearchLinqFilterString(IAraContainerClient ConteinerFather, AraGridSearchLinq vSearchLinq, AraGridSearchLinq.SColuna vColuna,string vFiltroAtual)
            : base(ConteinerFather)
        {
            _SearchLinq = vSearchLinq;
            _Coluna = vColuna;

            GridFiltroSimples.PageReload();

            if (vFiltroAtual.Length > 1 && vFiltroAtual.Substring(0,1)=="=")
            {
                try
                {
                    var vTmpArray = Json.DynamicJson.Parse(vFiltroAtual.Substring(1, vFiltroAtual.Length - 1));

                    FiltrosMultiplos.AddRange(((string[])vTmpArray).Select(a=> (a.Length >0 && a.Substring(0,1) == "="?a.Substring(1,a.Length-1):a)));

                    TabsTiposFiltro.TabActive = TabMultiplos;
                    cePesquisa.txtNome.SetFocus();
                }
                catch { }
            }

            AtualizalFiltro();
        }

        public IQueryable<QueryCePesquisa> GetQuery()
        {
            if (_SearchLinq != null && _Coluna != null)
            {
                return from tmp in ((IQueryable<string>)_SearchLinq.SqlTratado().Select(_Coluna.Name)).Distinct()
                       select new QueryCePesquisa { NOME = tmp };
            }
            else
                return null;
        }

        public override IQueryable<object> cePesquisa_GetQuery()
        {
            return GetQuery();
        }

        public override IQueryable<object> GridFiltroSimples_GetQuery()
        {
            return GetQuery();
        }

        public class QueryCePesquisa
        {
            [AraFieldAlias(" ")]
            public string NOME;
        }

        public override void GridFiltroSimples_ReturnSearch(AraGrid Object, AraGridRow Row)
        {
            this.Close("=" + Row[0].Text);
        }

        List<string> FiltrosMultiplos = new List<string>();

        public override void bAdicionarAoFiltro_Click(object sender, EventArgs e)
        {
            if (cePesquisa.Codigo == null)
            {
                cePesquisa.txtNome.SetFocus();
                return;
            }

            if (!FiltrosMultiplos.Where(a => a == cePesquisa.Codigo).Any())
                FiltrosMultiplos.Add(cePesquisa.Codigo);

            cePesquisa.Codigo = null;

            AtualizalFiltro();
        }

        public override void bConfirmar_Click(object sender, EventArgs e)
        {
            if (TabsTiposFiltro.TabActive == TabSimples)
            {
                AraTools.Alert("Clique na linha da lista desejada para confirmar");
            }
            else if (TabsTiposFiltro.TabActive == TabMultiplos)
            {
                if (!FiltrosMultiplos.Any())
                {
                    AraTools.Alert("Adicione pelomenos um item na lista do filtro");
                    cePesquisa.txtNome.SetFocus();
                    return;
                }

                this.Close("=" + Json.DynamicJson.Serialize(FiltrosMultiplos.Select(a=>"=" + a)));
            }

        }


        private void AtualizalFiltro()
        {
            StringBuilder vTmp = new StringBuilder();

            if (FiltrosMultiplos.Any())
            {
                vTmp.Append("<b>Filtrando por:</b><br>");
                vTmp.Append(string.Join(",", FiltrosMultiplos.Select(a => a + " " + lFiltro.Buttons.Add<Action<string>>(AraLabelButton.ButtonIco.trash, TirarDoFiltro,a))));
                vTmp.Append("<br>");
                vTmp.Append(lFiltro.Buttons.Add<Action>("Remover todos os filtros", AraLabelButton.ButtonIco.trash, TirarTodosFiltros));
            }
            else
                vTmp.Append("<b>Selecione e adicione um ou mais elementos no filtro</b>");

            lFiltro.Text = vTmp.ToString(); ;
        }

        private void TirarDoFiltro(string vTexto)
        {
            AraTools.AlertYesOrNo<Action<bool, string>>("Deseja retirar este item '" + vTexto + "' do filtro?", MsgTirarDoFiltro, vTexto);
            
        }

        private void MsgTirarDoFiltro(bool vResu, string vTexto)
        {
            if (vResu)
            {
                FiltrosMultiplos.RemoveAll(a => a == vTexto);
                AtualizalFiltro();
            }
        }

        private void TirarTodosFiltros()
        {
            AraTools.AlertYesOrNo<Action<bool>>("Deseja limpar este filtro?", MsgTirarTodosFiltros);
        }

        private void MsgTirarTodosFiltros(bool vResu)
        {
            if (vResu)
            {
                FiltrosMultiplos.Clear();
                AtualizalFiltro();
            }
        }

        public override void bCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}