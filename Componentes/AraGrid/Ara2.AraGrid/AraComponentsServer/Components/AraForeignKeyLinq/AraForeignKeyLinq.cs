using System;
using System.Collections.Generic;
using Ara2.Components;
using System.Data.OleDb;
using Ara2.Dev;
using System.Linq;
using System.Dynamic;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using Ara2.Grid;

namespace Ara2.Components
{
    [Serializable]
    [AraDevComponent(vConteiner:false)]
    public class AraForeignKeyLinq : AraDesign.AraForeignKeyLinqAraDesign, IAraDev
    {
        public AraForeignKeyLinq(IAraContainerClient ConteinerFather)
            : base(ConteinerFather)
        {
            New.ChangeEnabled += New_ChangeEnabled;

            txtNome.AutoCompleteSearch += txtNome_AutoCompleteSearch;
            txtNome.LostFocus += txtNome_LostFocus;
            txtNome.AutoCompleteSearch_minLength = 0;

            txtCodigo.KeyDown.Only = new int[] { 13, 114 };
            txtCodigo.KeyDown.ReturnFalse = new int[] { 13, 114 };
            txtCodigo.AutoCompleteSearch += txtCodigo_AutoCompleteSearch;
            txtCodigo.AutoCompleteSearch_minLength = 0;

            bPesquisa.OnCommitBefore += bPesquisa_OnCommitBefore;
            bPesquisa.GetQuery += GetQuerybPesquisa;

            ReformulaInterface();
        }

        private void ReformulaInterface()
        {
            txtCodigo.Visible = VisibleCode;
            bPesquisa.Visible = VisibleSearchButton;
            bNew.Visible = VisibleNewButton;
            bSelect.Visible = VisibleSelectButton;

            if (VisibleCode)
            {
                if (VisibleSearchButton)
                {
                    bPesquisa.Left = 99;
                    if (VisibleNewButton)
                        bNew.Left = 128;
                }
                else if (VisibleNewButton)
                    bNew.Left = 99;

                if (VisibleSearchButton && VisibleNewButton)
                    txtNome.Anchor.Left = 160;
                else if (VisibleSearchButton == true || VisibleNewButton == true)
                    txtNome.Anchor.Left = 130;
                else
                    txtNome.Anchor.Left = 100;
            }
            else
            {
                if (VisibleSearchButton)
                {
                    bPesquisa.Left = 1;

                    if (VisibleNewButton)
                        bNew.Left = 30;
                }
                else
                {
                    if (VisibleNewButton)
                        bNew.Left = 1;
                }

                if (VisibleSearchButton && VisibleNewButton)
                    txtNome.Anchor.Left = 60;
                else if (VisibleSearchButton ==true || VisibleNewButton==true)
                    txtNome.Anchor.Left = 30;
                else
                    txtNome.Anchor.Left = 1;
            }

            if (VisibleSelectButton)
                txtNome.Anchor.Right = 40;
            else
                txtNome.Anchor.Right = 2;
        }

        private string _codigo = null;
        private Type _codigoType = null;
        private string _codexterno = null;
        private Type _codexternoType = null;
        private string _nome = null;

        private AraEvent<OnChange> _Change = new AraEvent<OnChange>();
        [AraDevEvent]
        public AraEvent<OnChange> Change
        {
            get { return _Change; }
            set { _Change = value; }
        }

        public delegate void OnChange();

        private AraEvent<OnNew> _New = new AraEvent<OnNew>();
        [AraDevEvent]
        public AraEvent<OnNew> New
        {
            get { return _New; }
            set { _New = value; }
        }
        public delegate void OnNew();

        private void New_ChangeEnabled()
        {
            VisibleNewButton = New.Enabled;
        }

        #region Propriedades

        public string Codigo
        {
            get
            {
                return _codigo;
            }
            set
            {
                if (value == null)
                    Limpar();
                else
                    Pesquisar(value, null, null);
            }
        }

        public string CodExterno
        {
            get
            {
                return _codexterno;
            }
        }

        private AraComponentVisual _ProximoCampo = null;
        public AraComponentVisual ProximoCampo
        {
            get
            {
                return _ProximoCampo;
            }
            set
            {
                _ProximoCampo = value;
            }
        }

        private string _ColunaCodigo = "CODIGO";
        [AraDevProperty("")]
        public string ColunaCodigo
        {
            get
            {
                return _ColunaCodigo;
            }
            set
            {
                _ColunaCodigo = value;
            }
        }

        private string _ColunaCodExterno = null;
        [AraDevProperty(null)]
        public string ColunaCodExterno
        {
            get
            {
                return _ColunaCodExterno;
            }
            set
            {
                _ColunaCodExterno = value;
            }
        }

        private string _ColunaNome = "NOME";
        [AraDevProperty("")]
        public string ColunaNome
        {
            get
            {
                return _ColunaNome;
            }
            set
            {
                _ColunaNome = value;
            }
        }

        private string _ColunaExcluido = "";
        [AraDevProperty("")]
        public string ColunaExcluido
        {
            get
            {
                return _ColunaExcluido;
            }
            set
            {
                _ColunaExcluido = value;
            }
        }

        public enum ECampoCodigoVisual
        {
            Codigo,
            CodExterno
        }

        private ECampoCodigoVisual _CampoCodigoVisual = ECampoCodigoVisual.Codigo;
        [AraDevProperty(ECampoCodigoVisual.Codigo)]
        public ECampoCodigoVisual CampoCodigoVisual
        {
            get
            {
                return _CampoCodigoVisual;
            }
            set
            {
                _CampoCodigoVisual = value;
            }
        }

        private bool _Enabled = true;
        [AraDevProperty(true)]
        public bool Enabled
        {
            get
            {
                return _Enabled;
            }
            set
            {
                _Enabled = value;
                txtCodigo.Enabled = value;
                bPesquisa.Enabled = value;
                txtNome.Enabled = value;
                bSelect.Enabled = value;
            }
        }


        public enum ECampoOrderBy
        {
            Codigo,
            CodExterno,
            Nome
        }

        private ECampoOrderBy _OrderByAutoComplite = ECampoOrderBy.Nome;
        [AraDevProperty(ECampoOrderBy.Nome)]
        public ECampoOrderBy OrderByAutoComplite
        {
            get
            {
                return _OrderByAutoComplite;
            }
            set
            {
                _OrderByAutoComplite = value;
            }
        }

        private AraEvent<Func<IQueryable<object>>> _GetQuery = new AraEvent<Func<IQueryable<object>>>();
        [AraDevEvent]
        public AraEvent<Func<IQueryable<object>>> GetQuery
        {
            get { return _GetQuery; }
            set
            {
                _GetQuery = value;
            }
        }

        private IQueryable<object> GetQuerybPesquisa()
        {
            if (_GetQuery.InvokeEvent != null)
            {
                IQueryable<object> TmpQuery = _GetQuery.InvokeEvent();
                if (!string.IsNullOrWhiteSpace(ColunaExcluido))
                {
                    bool AliasExcluido = (ColunaExcluido != ColunaExcluido.ToUpper());
                    TmpQuery = TmpQuery.Where(ColunaExcluido + " == null");
                }

                return TmpQuery;
            }
            else
                return null;
        }

        private bool _AutoComplete = true;
        [AraDevProperty(true)]
        public bool AutoComplete
        {
            get
            {
                return _AutoComplete;
            }
            set
            {
                if (_AutoComplete != value)
                {
                    _AutoComplete = value;
                    if (value)
                    {
                        txtNome.AutoCompleteSearch += txtNome_AutoCompleteSearch;
                    }
                    else
                    {
                        txtNome.AutoCompleteSearch -= txtNome_AutoCompleteSearch;
                    }
                    txtNome.Enabled = value;
                }
            }
        }

        private bool _VisibleCode = true;
        [AraDevProperty(true)]
        [PropertySupportLayout]
        public bool VisibleCode
        {
            get
            {
                return _VisibleCode;
            }
            set
            {
                if (_VisibleCode != value)
                {
                    _VisibleCode = value;
                    ReformulaInterface();
                }
            }
        }

        private bool _VisibleSearchButton = true;
        [AraDevProperty(true)]
        [PropertySupportLayout]
        public bool VisibleSearchButton
        {
            get
            {
                return _VisibleSearchButton;
            }
            set
            {
                if (_VisibleSearchButton != value)
                {
                    _VisibleSearchButton = value;
                    ReformulaInterface();
                }
            }
        }

        private bool _VisibleSelectButton = false;
        [AraDevProperty(false)]
        [PropertySupportLayout]
        public bool VisibleSelectButton
        {
            get
            {
                return _VisibleSelectButton;
            }
            set
            {
                if (_VisibleSelectButton != value)
                {
                    _VisibleSelectButton = value;
                    ReformulaInterface();
                }
            }
        }

        private bool _VisibleNewButton = false;
        [AraDevProperty(false)]
        [PropertySupportLayout]
        public bool VisibleNewButton
        {
            get
            {
                return _VisibleNewButton;
            }
            set
            {
                if (_VisibleNewButton != value)
                {
                    _VisibleNewButton = value;
                    ReformulaInterface();
                }
            }
        }
        
        #endregion

        private void bPesquisa_OnCommitBefore(AraGridSearchLinq  vGrid)
        {
            if (!string.IsNullOrWhiteSpace(ColunaExcluido) && vGrid.Grid.Cols[ColunaExcluido] != null)
            {
                vGrid.Grid.Cols[ColunaExcluido].hidden = true;
            }
        }

        private void SelecionarProximoCampo()
        {
            if (ProximoCampo != null)
            {
                if (ProximoCampo is AraTextBox)
                    ((AraTextBox)ProximoCampo).SetFocus();
                else if (ProximoCampo is AraSelect)
                    ((AraSelect)ProximoCampo).SetFocus();
            }
        }

        public void Limpar()
        {
            string CodAnterior = Codigo;

            _codigo = null;
            _codexterno = null;
            _nome = null;
            txtCodigo.Text = "";
            txtNome.Text = "";

            if (Change.InvokeEvent != null && CodAnterior != null)
                Change.InvokeEvent();
        }

        private void Pesquisar(string p_Codigo, string p_CodExterno, string p_Nome)
        {
            string CodAnterior = Codigo;

            VerificaTipoCampos();

            IQueryable<object> vQuery = GetQuery.InvokeEvent();
            

            if (p_Codigo != null)
                vQuery = vQuery.Where((_codigoType.Equals(typeof(string)) ? ColunaCodigo : ColunaCodigo + ".ToString()") + ".Trim() == @0 ", p_Codigo.Trim());
            else if (p_CodExterno != null)
                vQuery = GetQuerybPesquisa().Where((_codexternoType.Equals(typeof(string)) ? ColunaCodExterno : ColunaCodExterno + ".ToString()") + ".Trim() == @0 ", p_CodExterno.Trim());
            else if (p_Nome != null)
                vQuery = GetQuerybPesquisa().Where(ColunaNome + ".Trim() == @0 ", p_Nome.Trim());
            else
                return;

            object vObj = vQuery.Take(1).FirstOrDefault();

            if (vObj != null)
            {
                _codigo = (vObj.GetType().GetProperty(ColunaCodigo)!=null? vObj.GetType().GetProperty(ColunaCodigo).GetValue(vObj, null):vObj.GetType().GetField(ColunaCodigo).GetValue(vObj)).ToString();
                if (ColunaCodExterno != null)
                    _codexterno = (vObj.GetType().GetProperty(ColunaCodExterno) != null ? vObj.GetType().GetProperty(ColunaCodExterno).GetValue(vObj, null) : vObj.GetType().GetField(ColunaCodExterno).GetValue(vObj)).ToString();
                else
                    _codexterno = null;
                _nome = (vObj.GetType().GetProperty(ColunaNome) != null ? vObj.GetType().GetProperty(ColunaNome).GetValue(vObj, null) : vObj.GetType().GetField(ColunaNome).GetValue(vObj)).ToString();
                if (CampoCodigoVisual == ECampoCodigoVisual.Codigo)
                {
                    txtCodigo.Text = _codigo;
                }
                else if (CampoCodigoVisual == ECampoCodigoVisual.CodExterno)
                {
                    txtCodigo.Text = _codexterno;
                }
                txtNome.Text = _nome;
            }
            else
                Limpar();


            if (Change.InvokeEvent != null && CodAnterior != Codigo)
                Change.InvokeEvent();
        }

        public override void txtCodigo_KeyDown(AraTextBox Object, int vKey)
        {
            try
            {
                if (vKey == 13)
                {
                    if (!string.IsNullOrWhiteSpace(txtCodigo.Text))
                    {
                        if (CampoCodigoVisual == ECampoCodigoVisual.Codigo)
                        {
                            if (txtCodigo.Text.Trim() != "" && txtCodigo.Text.Trim() != (_codigo==null?"":_codigo))
                            {
                                Pesquisar(txtCodigo.Text, null, null);
                                SelecionarProximoCampo();
                            }
                            return;
                        }
                        else
                        {
                            Pesquisar(null, txtCodigo.Text, null);
                            SelecionarProximoCampo();
                            return;
                        }
                    }

                    Limpar();
                }
                else if (vKey == 114)
                {
                    bPesquisa.Click.InvokeEvent(bPesquisa, null);
                }
            }
            catch (Exception err)
            {
                AraTools.Alert("Erro ao carregar chave extrangeira:\n" + err.Message);
                return;
            }
        }

        public override void txtCodigo_LostFocus(object sender, EventArgs e)
        {
            txtCodigo_KeyDown(txtCodigo, 13);
        }

        public override void bPesquisa_Return(object Row)
        {
            if (Row != null)
            {
                if (CampoCodigoVisual == ECampoCodigoVisual.Codigo)
                {
                    if (((AraGridRow)Row)[ColunaCodigo].ToString().Trim()!="")
                        Pesquisar(((AraGridRow)Row)[ColunaCodigo].ToString().Trim(), null, null);
                }
                else if (CampoCodigoVisual == ECampoCodigoVisual.CodExterno)
                {
                    Pesquisar(null, ((AraGridRow)Row)[ColunaCodExterno].ToString(), null);
                }
            }
        }

        public override void txtNome_LostFocus(object sender, EventArgs e)
        {
            if (txtNome.Text.Trim() != (_nome == null ? "" : _nome).Trim())
                Pesquisar(null, null, txtNome.Text);
        }

        public delegate IQueryable<object> DFiltroCustom(IQueryable<object> vQuery, string ColunaCodigo, Type ColunaCodigoTipo,string ColunaNome, string vText);

        public static event DFiltroCustom FiltroCustomStatic = null;
        public AraEvent<DFiltroCustom> FiltroCustom = new AraEvent<DFiltroCustom>();

        public AraTextBoxAutoCompleteResu txtNome_AutoCompleteSearch(AraTextBox Object, string vSearch)
        {
            VerificaTipoCampos();

            AraTextBoxAutoCompleteResu Itens = new AraTextBoxAutoCompleteResu();

            IQueryable<object> vQuery = GetQuerybPesquisa();

            if (vQuery != null)
            {

                string vTmpColuna = (CampoCodigoVisual == ECampoCodigoVisual.Codigo ? ColunaCodigo : ColunaCodExterno);
                Type vTmpColunaType = (CampoCodigoVisual == ECampoCodigoVisual.Codigo ? _codigoType : _codexternoType); ;

                bool vFiltroNormal = true;

                if (FiltroCustom.InvokeEvent != null)
                {
                    var vTmpQ = FiltroCustom.InvokeEvent(vQuery, vTmpColuna, vTmpColunaType, ColunaNome, vSearch);
                    if (vTmpQ != null)
                    {
                        vQuery = vTmpQ;
                        vFiltroNormal = false;
                    }
                }

                if (vFiltroNormal && FiltroCustomStatic != null)
                {
                    var vTmpQ = FiltroCustomStatic(vQuery, vTmpColuna, vTmpColunaType, ColunaNome, vSearch);
                    if (vTmpQ != null)
                    {
                        vQuery = vTmpQ;
                        vFiltroNormal = false;
                    }
                }

                if (vFiltroNormal)
                {
                    string Filtro = "(" + (vTmpColunaType.Equals(typeof(string)) ? vTmpColuna : vTmpColuna + ".ToString()") + " + " + ColunaNome + ").ToUpper().Contains(@0.ToUpper())";

                    foreach (var vPalavra in vSearch.Split(' ').AsQueryable().Where(a => a.Trim() != ""))
                        vQuery = vQuery.Where(Filtro, vPalavra);
                }


                if (OrderByAutoComplite == ECampoOrderBy.Nome)
                    vQuery = vQuery.OrderBy(ColunaNome);
                else if (OrderByAutoComplite == ECampoOrderBy.Codigo)
                    vQuery = vQuery.OrderBy(ColunaCodigo);
                else if (OrderByAutoComplite == ECampoOrderBy.CodExterno && ColunaCodExterno == null)
                    vQuery = vQuery.OrderBy(ColunaCodExterno);
                else
                    throw new Exception("OrderByAutoComplite invalido !");

                foreach (object vObj in vQuery.Take(20).ToArray())
                {
                    //string Nome = (vObj.GetType().GetProperty(ColunaNome) != null ? vObj.GetType().GetProperty(ColunaNome).GetValue(vObj, null) : vObj.GetType().GetField(ColunaNome).GetValue(vObj)).ToString();
                    //Itens.Add(Nome);
                    if (vObj != null)
                    {
                        object vTmpCodigoValueObj = (vObj.GetType().GetProperty(vTmpColuna) != null ? vObj.GetType().GetProperty(vTmpColuna).GetValue(vObj, null) : vObj.GetType().GetField(vTmpColuna).GetValue(vObj));
                        object vTmpNomeValueObj = (vObj.GetType().GetProperty(ColunaNome) != null ? vObj.GetType().GetProperty(ColunaNome).GetValue(vObj, null) : vObj.GetType().GetField(ColunaNome).GetValue(vObj));


                        string vTmpCodigoValue = (vTmpCodigoValueObj==null?string.Empty: vTmpCodigoValueObj.ToString());
                        string vTmpNomeValue = (vTmpNomeValueObj == null ? string.Empty : vTmpNomeValueObj.ToString());

                        if (!string.IsNullOrWhiteSpace(vTmpCodigoValue) && !string.IsNullOrWhiteSpace(vTmpNomeValue))
                            Itens.Add(vTmpCodigoValue + " - " + vTmpNomeValue, vTmpNomeValue);
                    }
                }

                return Itens;
            }
            else
                return null;
        }

        public AraTextBoxAutoCompleteResu txtCodigo_AutoCompleteSearch(AraTextBox Object, string vSearch)
        {
            VerificaTipoCampos();

            AraTextBoxAutoCompleteResu Itens = new AraTextBoxAutoCompleteResu();
            IQueryable<object> vQuery = GetQuerybPesquisa();

            if (vQuery != null)
            {
                string vTmpColuna = (CampoCodigoVisual == ECampoCodigoVisual.Codigo ? ColunaCodigo : ColunaCodExterno);
                Type vTmpColunaType = (CampoCodigoVisual == ECampoCodigoVisual.Codigo ? _codigoType : _codexternoType); ;

                bool vFiltroNormal = true;

                if (FiltroCustom.InvokeEvent!=null)
                {
                    var vTmpQ = FiltroCustom.InvokeEvent(vQuery, vTmpColuna, vTmpColunaType,ColunaNome, vSearch);
                    if (vTmpQ!=null)
                    {
                        vQuery = vTmpQ;
                        vFiltroNormal = false;
                    }
                }

                if (vFiltroNormal && FiltroCustomStatic != null)
                {
                    var vTmpQ = FiltroCustomStatic(vQuery, vTmpColuna, vTmpColunaType,ColunaNome, vSearch);
                    if (vTmpQ != null)
                    {
                        vQuery = vTmpQ;
                        vFiltroNormal = false;
                    }
                }

                if (vFiltroNormal)
                {
                    string Filtro = "(" + (vTmpColunaType.Equals(typeof(string)) ? vTmpColuna : vTmpColuna + ".ToString()") + " + " + ColunaNome + ").ToUpper().Contains(@0.ToUpper())";

                    foreach (var vPalavra in vSearch.Split(' ').AsQueryable().Where(a => a.Trim() != ""))
                        vQuery = vQuery.Where(Filtro, vPalavra);
                }

                //vQuery = vQuery.Where(Filtro, vSearch.ToUpper());

                if (OrderByAutoComplite == ECampoOrderBy.Nome)
                    vQuery = vQuery.OrderBy(ColunaNome);
                else if (OrderByAutoComplite == ECampoOrderBy.Codigo)
                    vQuery = vQuery.OrderBy(ColunaCodigo);
                else if (OrderByAutoComplite == ECampoOrderBy.CodExterno && ColunaCodExterno == null)
                    vQuery = vQuery.OrderBy(ColunaCodExterno);
                else
                    throw new Exception("OrderByAutoComplite invalido !");

                foreach (object vObj in vQuery.Take(20))
                {
                    string vTmpCodigoValue = (vObj.GetType().GetProperty(vTmpColuna) != null ? vObj.GetType().GetProperty(vTmpColuna).GetValue(vObj, null) : vObj.GetType().GetField(vTmpColuna).GetValue(vObj)).ToString();
                    string vTmpNomeValue = (vObj.GetType().GetProperty(ColunaNome) != null ? vObj.GetType().GetProperty(ColunaNome).GetValue(vObj, null) : vObj.GetType().GetField(ColunaNome).GetValue(vObj)).ToString();
                    Itens.Add(vTmpCodigoValue + " - " + vTmpNomeValue, vTmpCodigoValue);
                }
            }

            return Itens;
        }

        private void VerificaTipoCampos()
        {
            bool CarregarTipos = false;
            if (_codigoType == null)
                CarregarTipos = true;
            
            if (_codexterno !=null && _ColunaCodExterno==null)
                CarregarTipos = true;

            if (CarregarTipos)
            {
                object vObj = GetQuery.InvokeEvent().Take(1).FirstOrDefault();
                if (vObj != null)
                {
                    _codigoType = (vObj.GetType().GetProperty(ColunaCodigo) != null ? vObj.GetType().GetProperty(ColunaCodigo).PropertyType : vObj.GetType().GetField(ColunaCodigo).FieldType);
                    if (ColunaCodExterno!=null)
                        _codexternoType = (vObj.GetType().GetProperty(ColunaCodExterno) != null ? vObj.GetType().GetProperty(ColunaCodExterno).PropertyType : vObj.GetType().GetField(ColunaCodExterno).FieldType);

                }
            }
        }

        public override void bSelect_Click(object sender, EventArgs e)  // !
        {
            txtNome.SetFocus();
            txtNome.AutoCompleteShow();
        }

        public override void bNew_Click(object sender, EventArgs e)
        {
            if (New.InvokeEvent != null)
            {
                try
                {
                    New.InvokeEvent();
                }
                catch(Exception err)
                {
                    throw new Exception("Erro on button new.\n" + err.Message);
                }
            }
        }

        #region Dev

        private AraEvent<DStartEditPropertys> _StartEditPropertys = null;
        public AraEvent<DStartEditPropertys> StartEditPropertys
        {
            get
            {
                if (_StartEditPropertys == null)
                {
                    _StartEditPropertys = new AraEvent<DStartEditPropertys>();
                    this.Click += this_ClickEdit;
                }

                return _StartEditPropertys;
            }
            set
            {
                _StartEditPropertys = value;
            }
        }

        private void this_ClickEdit(object sender, EventArgs e)
        {
            if (_StartEditPropertys.InvokeEvent != null)
                _StartEditPropertys.InvokeEvent(this);
        }

        private AraEvent<DStartEditPropertys> _ChangeProperty = new AraEvent<DStartEditPropertys>();
        public AraEvent<DStartEditPropertys> ChangeProperty
        {
            get
            {
                return _ChangeProperty;
            }
            set
            {
                _ChangeProperty = value;
            }
        }

        #endregion
    }
}