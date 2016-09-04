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
    public class FrmAraGridSearchLinqFilterDateTime : AraDesign.FrmAraGridSearchLinqFilterDateTimeAraDesign
    {
        private AraGridSearchLinq _SearchLinq = null;
        private AraGridSearchLinq.SColuna _Coluna = null;
        string vFormatacaoParaString=null;

        public FrmAraGridSearchLinqFilterDateTime(IAraContainerClient ConteinerFather, AraGridSearchLinq vSearchLinq, AraGridSearchLinq.SColuna vColuna, string vFiltroAtual)
            : base(ConteinerFather)
        {
            _SearchLinq = vSearchLinq;
            _Coluna = vColuna;

            
            switch (_Coluna.Tipo)
            {
                case AraGridSearchLinq.enum_TipoColuna.Data:
                    {
                        vFormatacaoParaString = "dd/MM/yyyy";
                        txtDataIni.Mask = AraTextBox.AraTextBoxMaskTypes.date;
                        txtDataFim.Mask = txtDataIni.Mask;
                    }
                    break;
                case AraGridSearchLinq.enum_TipoColuna.DataHora:
                    {
                        vFormatacaoParaString = "dd/MM/yyyy HH:mm:ss";
                        txtDataIni.Mask = AraTextBox.AraTextBoxMaskTypes.datetime;
                        txtDataFim.Mask = txtDataIni.Mask;
                    }
                    break;
                case AraGridSearchLinq.enum_TipoColuna.Hora:
                    {
                        vFormatacaoParaString = "HH:mm:ss";
                        txtDataIni.Mask = AraTextBox.AraTextBoxMaskTypes.time;
                        txtDataFim.Mask = txtDataIni.Mask;
                    }
                    break;
                default:
                    throw new Exception("Tipo da coluna não previsto.");
                    break;
            }

            GridFiltroSimples.PageReload();

            if (vFiltroAtual.Length > 1 && vFiltroAtual.Substring(0, 1) == "=")
            {
                try
                {
                    var vTmpArray = Json.DynamicJson.Parse(vFiltroAtual.Substring(1, vFiltroAtual.Length - 1));

                    FiltrosMultiplos.AddRange((string[])vTmpArray);

                    TabsTiposFiltro.TabActive = TabMultiplos;
                    txtCondicao.SetFocus();
                }
                catch { }
            }
            else if (vFiltroAtual.Length > 1 && _Coluna.Tipo == AraGridSearchLinq.enum_TipoColuna.Hora && AraGridSearchLinq.ClassTimeSpanIntervalo.TryParse(vFiltroAtual.Substring(1, vFiltroAtual.Length - 1)) != null)
            {
                var Intervalo = AraGridSearchLinq.ClassTimeSpanIntervalo.TryParse(vFiltroAtual.Substring(1, vFiltroAtual.Length - 1));
                txtDataIni.Text = Intervalo.Inicio.ToString();
                txtDataFim.Text = Intervalo.Fim.ToString();

                TabsTiposFiltro.TabActive = tabIntervaloDatas;
                txtDataIni.SetFocus();
            }
            else if (vFiltroAtual.Length > 1 
                && (_Coluna.Tipo == AraGridSearchLinq.enum_TipoColuna.DataHora || _Coluna.Tipo == AraGridSearchLinq.enum_TipoColuna.Data)
                && AraGridSearchLinq.ClassDateTimeIntervalo.TryParse(vFiltroAtual.Substring(1, vFiltroAtual.Length - 1)) != null)
            {
                var Intervalo = AraGridSearchLinq.ClassDateTimeIntervalo.TryParse(vFiltroAtual.Substring(1, vFiltroAtual.Length - 1));
                txtDataIni.Text = Intervalo.DataIni.ToString();
                txtDataFim.Text = Intervalo.DataFim.ToString();

                TabsTiposFiltro.TabActive = tabIntervaloDatas;
                txtDataIni.SetFocus();
            }
            else
                TabsTiposFiltro.TabActive = TabSimples;

            AtualizalFiltro();
        }

        public IQueryable<QueryCePesquisaDateTime> GetQuery()
        {
            if (_SearchLinq != null && _Coluna != null && vFormatacaoParaString != null)
            {
                switch (_Coluna.Tipo)
                {
                    case AraGridSearchLinq.enum_TipoColuna.Data:
                    case AraGridSearchLinq.enum_TipoColuna.DataHora:
                        {
                            var vQuery = from tmp in _SearchLinq
                                    .SqlTratado()
                                    .Where(_Coluna.Name + " != null")
                                    .Select(_Coluna.Name)
                                    .Cast<DateTime>()
                                   group tmp by tmp into g
                                   select new QueryCePesquisaDateTime
                                   {
                                       NOME = g.Key.ToString(vFormatacaoParaString),
                                       Data = g.Key
                                   };

                            var queryDelegate = Expression.Lambda<Func<IQueryable<QueryCePesquisaDateTime>>>(new OrderByRemover().Visit(vQuery.Expression)).Compile();

                            return queryDelegate().OrderBy(a=>a.Data);
                        }
                        break;
                    case AraGridSearchLinq.enum_TipoColuna.Hora:
                        {
                            var vQuery = from tmp in _SearchLinq
                                    .SqlTratado()
                                    .Where(_Coluna.Name + " != null")
                                    .Select(_Coluna.Name)
                                    .Cast<TimeSpan?>()
                                   group tmp by tmp into g
                                   select new QueryCePesquisaDateTime
                                   {
                                       NOME = g.Key.ToString() //,).ToString(vFormatacaoParaString),
                                       //Hora = g.Key
                                   };

                            var queryDelegate = Expression.Lambda<Func<IQueryable<QueryCePesquisaDateTime>>>(new OrderByRemover().Visit(vQuery.Expression)).Compile();

                            return queryDelegate().OrderBy(a => a.NOME);
                        }
                        break;
                    default:
                        throw new Exception("Tipo da coluna não previsto.");
                        break;
                }
            }
            else
                return null;
        }

        

        public override IQueryable<object> GridFiltroSimples_GetQuery()
        {
            return GetQuery();
        }

        public class QueryCePesquisaDateTime 
        {
            [AraFieldAlias(" ")]
            public string NOME;

            [AraFieldHide]
            public DateTime? Data { get; set; }
            [AraFieldHide]
            public TimeSpan? Hora { get; set; }
        }

        public override void GridFiltroSimples_ReturnSearch(AraGrid Object, AraGridRow Row)
        {
            this.Close(Row[0].Text);
        }

        List<string> FiltrosMultiplos = new List<string>();

        public override void bAdicionarAoFiltro_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCondicao.Text))
            {
                txtCondicao.SetFocus();
                return;
            }

            if (AraGridSearchLinq.ClassCondicaoDateTime.TryParse(txtCondicao.Text) == null)
            {
                AraTools.Alert("Condição invalida ! Vide exemplos.");
                txtCondicao.SetFocus();
                return;
            }

            if (!FiltrosMultiplos.Where(a => a == txtCondicao.Text).Any())
                FiltrosMultiplos.Add(txtCondicao.Text);

            txtCondicao.Text = "";
            txtCondicao.SetFocus();

            AtualizalFiltro();
        }

        public override void bConfirmar_Click(object sender, EventArgs e)
        {
            if (TabsTiposFiltro.TabActive == TabSimples)
            {
                AraTools.Alert("Clique na linha da lista desejada para confirmar");
            }
            else if (TabsTiposFiltro.TabActive == tabIntervaloDatas)
            {
                if (!AraTools.IsDate(txtDataIni.Text))
                {
                    AraTools.Alert("Data Inicial invalida.");
                    txtDataIni.SetFocus();
                    return;
                }

                if (!AraTools.IsDate(txtDataFim.Text))
                {
                    AraTools.Alert("Data Final invalida.");
                    txtDataFim.SetFocus();
                    return;
                }

                if (Convert.ToDateTime(txtDataFim.Text) <= Convert.ToDateTime(txtDataIni.Text))
                {
                    AraTools.Alert("Data Final menor ou igual a data inicial.");
                    txtDataFim.SetFocus();
                    return;
                }

                
                this.Close(Convert.ToDateTime(txtDataIni.Text).ToString(vFormatacaoParaString) + " até " + Convert.ToDateTime(txtDataFim.Text).ToString(vFormatacaoParaString));
            }
            else if (TabsTiposFiltro.TabActive == TabMultiplos)
            {
                if (!FiltrosMultiplos.Any())
                {
                    AraTools.Alert("Adicione pelomenos um item na lista do filtro");
                    txtCondicao.SetFocus();
                    return;
                }

                this.Close("=" + Json.DynamicJson.Serialize(FiltrosMultiplos));
            }

        }


        private void AtualizalFiltro()
        {
            StringBuilder vTmp = new StringBuilder();

            if (FiltrosMultiplos.Any())
            {
                vTmp.Append("<b>Filtrando por:</b><br>");
                vTmp.Append(string.Join(",", FiltrosMultiplos.Select(a => a + " " + lFiltro.Buttons.Add<Action<string>>(AraLabelButton.ButtonIco.trash, TirarDoFiltro, a))));
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