using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using Ara2;
using Ara2.Components;
using Ara2.Dev;
using System.Linq.Dynamic;
using Ara2.Grid.Export;
using System.Linq.Expressions;
using Ara2.Grid.Filters.Forms;

namespace Ara2.Grid
{
    [Serializable]
    [AraDevComponent(vConteiner:false)]
    public class AraGridSearchLinq : AraGridBottomNav, IAraDev
    {
        // Padão Ara 
         
        private bool _GridIsCreate = false;
        public bool GridIsCreate
        {
            get
            {
                return _GridIsCreate;
            }
        }
        private bool GridCreateIsLoad = false;

        private AraEvent<Func<IQueryable<object>>> _GetQuery = new AraEvent<Func<IQueryable<object>>>();
        [AraDevEvent]
        public AraEvent<Func<IQueryable<object>>> GetQuery
        {
            get { return _GetQuery; }
            set
            {
                _GetQuery = value; 
                //TentaCarregarComponent();
                if (GridCreateIsLoad ==false && GetQuery.InvokeEvent() != null)
                    LoadComponent();
            }
        }

        

        [Serializable]
        public enum EClick_ReturnSelect
        {
            Click, DoubleClick
        }

        public static EClick_ReturnSelect? ClickReturnSelectStandardForAll = null;

        private EClick_ReturnSelect? _ClickReturnSelect = null;

        [AraDevProperty(EClick_ReturnSelect.Click)]
        public EClick_ReturnSelect ClickReturnSelect
        {
            get
            {
                if (_ClickReturnSelect != null)
                    return (EClick_ReturnSelect)_ClickReturnSelect;
                else if (ClickReturnSelectStandardForAll != null)
                    return (EClick_ReturnSelect)ClickReturnSelectStandardForAll;
                else
                    return EClick_ReturnSelect.Click;
            }
            set
            {
                _ClickReturnSelect = value;
            }
        } 
            //= EClick_ReturnSelect.Click;

        public AraGridSearchLinq(IAraObjectClienteServer Container, Func<IQueryable<object>> vGetQuery) :
            this(Container, vGetQuery, null)
        {

        }
        public AraGridSearchLinq(IAraObjectClienteServer Container, Func<IQueryable<object>> vGetQuery, dOnCommitBefore vOnCommitBefore) :
            this(Container)
        {
            if (vOnCommitBefore != null)
            {
                OnCommitBefore = new AraEvent<dOnCommitBefore>();
                OnCommitBefore += vOnCommitBefore;
            }

            GetQuery += vGetQuery;
        }
        
        public delegate void dOnCommitBefore(AraGridSearchLinq vGrid);
        [AraDevEvent]
        public AraEvent<dOnCommitBefore> OnCommitBefore { get; set; }

        public AraGridSearchLinq(IAraObjectClienteServer Container) :
            base(Container)
        {
            ReturnSearch = new AraEvent<ReturnSearch_delegate>();
            OnLoadData = new AraEvent<Action<AraGridSearchLinq>>();
            FiltroCustom = new AraEvent<DFiltroCustom>();
            OnGridActive = new AraEvent<Action>();
            OnCommitBefore = new AraEvent<dOnCommitBefore>();
            ColsGroup = new AraGridColGroups(this);
            OnPageReload += this_OnPageReload;
            this.Grid.OnCommitBefore += Tmp_OnCommitBefore;
            this.Grid.ChangeCell += FBChangeCell;
            //this.KeyDown += FBKeyDown;
            this.Grid.KeyDown.Only = new int[] { 13 };
            this.Grid.KeyDown.ReturnFalse = new int[] { 13 };
            this.Grid.ClickDblCell += FBClickDblCell;
            this.Grid.ClickCell += FBClickCell;
            this.Grid.Cols.Orderby += ColsOrderBy;
            this.Grid.Cols.AutoOrder = false;

            this.Grid.OnCommitBefore += Grid_OnCommitBefore_2;
            if (this.Grid.TreeGroup != null)
                this.Grid.TreeGroup.OnSelColsGrop += Grid_TreeGroup_OnSelColsGrop;

            CreateButtons();
        }

        //private bool _CarregouEventos = false;
        //private void TentaCarregarComponent()
        //{
        //    if (GetQuery.InvokeEvent() != null)
        //    {
        //        if (_CarregouEventos == false)
        //        {
                    

        //            _CarregouEventos = true;
        //        }

        //        LoadComponent();
        //    }
        //}

        private void Tmp_OnCommitBefore(AraGrid vGrid)
        {
            if (OnCommitBefore.InvokeEvent !=null)
                OnCommitBefore.InvokeEvent(this);
        }

        AraObjectInstance<AraButton> _bMenuConfiguracoes = new AraObjectInstance<AraButton>();
        public AraButton bMenuConfiguracoes
        {
            get { return _bMenuConfiguracoes.Object; }
            set { _bMenuConfiguracoes.Object = value; }
        }

        AraObjectInstance<AraMenu> _MenuConfiguracoes = new AraObjectInstance<AraMenu>();
        AraMenu MenuConfiguracoes
        {
            get { return _MenuConfiguracoes.Object; }
            set { _MenuConfiguracoes.Object = value; }
        }

        private void CreateButtons()
        { 
            bMenuConfiguracoes = new AraButton(this.NewConteiner());
            bMenuConfiguracoes.Ico = AraButton.ButtonIco.gear;
            bMenuConfiguracoes.Click += bMenuConfiguracoes_Click;
            bMenuConfiguracoes.Visible = VisibleNav;
            bMenuConfiguracoes.Height = new AraDistance("100%");
            bMenuConfiguracoes.Width = new AraDistance("26px");

            MenuConfiguracoes = new AraMenu(this);
            //MenuConfiguracoes.Itens.Add(new AraMenuItens("bAgrupar", "Agrupar", bAgrupa_Click));
            //MenuConfiguracoes.Itens.Add(new AraMenuItens("bOrdena", "Ordenar", bOrdena_Click));
            //MenuConfiguracoes.Itens.Add(new AraMenuItens("bSubFiltro", "SubFiltro", bFilterSub_Click));
            MenuConfiguracoes.Itens.Add(new AraMenuItens("bExportar", "Exportar", bExportarCSV_Click));
            MenuConfiguracoes.Commit();
           
        }

        private void bMenuConfiguracoes_Click(object sender, EventArgs e)
        {
            MenuConfiguracoes.Show(bMenuConfiguracoes);
        }

        private void bOrdena_Click(AraMenuItens Object)
        {
            //AraGridSearchOracleFrmSort AraGridSearchOracleFrmGroup = new AraGridSearchOracleFrmSort(this);
            //AraGridSearchOracleFrmGroup.Show((object obj) =>
            //{

            //});
        }

        private void bAgrupa_Click(AraMenuItens Object)
        {
            //AraGridSearchOracleFrmGroup AraGridSearchOracleFrmGroup = new AraGridSearchOracleFrmGroup(this);
            //AraGridSearchOracleFrmGroup.Show((object obj) =>
            //{

            //});
        }

        private void bFilterSub_Click(AraMenuItens Object)
        {
            //AraGridSearchOracleFrmFilter AraGridSearchOracleFrmFilter = new AraGridSearchOracleFrmFilter(this);
            //AraGridSearchOracleFrmFilter.Show((object obj) =>
            //{

            //});
        }

        private void bExportarCSV_Click(AraMenuItens Object)
        {
            FrmAraGridSearchLinqExportXLSx FrmAraGridSearchLinqExportXLSx = new FrmAraGridSearchLinqExportXLSx(this, GetSqlTratado);
            FrmAraGridSearchLinqExportXLSx.Show(true);
        }

        private IQueryable<object> GetSqlTratado()
        {
            return SqlTratado();
        }

        private void LoadComponent()
        {
            GridCreateIsLoad = false;
            if (LoadColsBySql())
            {
                GridCreateIsLoad = true;

                
                this.Grid.Commit();

                _GridIsCreate = true;
                if (OnGridActive.InvokeEvent != null)
                    OnGridActive.InvokeEvent();

                LoadDataAny();
            }
        }

        public void PageReloadAndReloadCols()
        {
            GridCreateIsLoad=false;
            Filter.Clear();
            FilterSub.Clear();
            _OrderBy = new SOrderBy[] { };
            PageReload();
        }

        private void Grid_OnCommitBefore_2(AraGrid Grid)
        {
            if (Grid.TreeGroup != null)
                Grid.TreeGroup.OnSelColsGrop += OnSelColsGrop;
        }

        private void Grid_TreeGroup_OnSelColsGrop()
        {
            this.SetOrderBy(this.Grid.TreeGroup.ColsGrup);
        }
        
        [AraDevEvent]
        public AraEvent<Action> OnGridActive { get; set; }
        
        private void OnSelColsGrop()
        {
            this.PageReload();
        }

        void this_OnPageReload()
        {
            if (GridCreateIsLoad == false)
            {
                if (this.GetQuery.InvokeEvent() !=null)
                    LoadComponent();
            }
            else
            {
                if (GridCreateIsLoad)
                    LoadDataAny();
            }
        }

        [Serializable]
        public enum enum_TipoColuna
        {
            Texto, Numero, Numerointero, Data, Hora, DataHora
        }

        

        [Serializable]
        public class SColuna
        {
            public string Name;
            public enum_TipoColuna Tipo;
            public string Alias;
            public bool Hide;
        }

        public Dictionary<string, SColuna> ColsInfo = new Dictionary<string, SColuna>();
        public Dictionary<string, SColuna> ColsInfoSub = new Dictionary<string, SColuna>();

        //private string _Sql = null;
        //public string Sql
        //{
        //    get { return _Sql; }
        //    set {
        //        bool Mudou = false;
        //        if (_Sql != null && _Sql != value) Mudou = true;
        //        _Sql = value;

        //        TentaCarregarComponent();
        //    }
        //}

        public IQueryable<object> SqlTratado()
        {
            if (!GridCreateIsLoad)
                LoadComponent();

            if (GridCreateIsLoad)
            {
                IQueryable<object> vSql = GetQuery.InvokeEvent();

                vSql = SqlFiltro(vSql, FilterSub, ColsInfoSub);
                vSql = SqlGroup(vSql);
                vSql = SqlFiltro(vSql, Filter, ColsInfo);
                vSql = SqlOrderBy(vSql);


                return vSql;
            }
            else
                return null;
        }

        public IQueryable<object> SqlTratadoGroup()
        {
            IQueryable<object> vSql = GetQuery.InvokeEvent();
            vSql = SqlGroup(vSql);
            return vSql;
        }

        struct SFild
        {
            public Type vType;
            public string Name;
            public string Alias;
            public bool Hide;
        }

        public Dictionary<string, SColuna> LoadColsTypeAndAliasBySql(IQueryable<object> vQuery)
        {
            Dictionary<string, SColuna> TmpFilter = new Dictionary<string, SColuna>();

            dynamic vFirstLine = vQuery.Take(1).FirstOrDefault();

            if (vFirstLine != null)
            {

                if (this.Grid.TreeGroup != null)
                {
                    foreach (AraGridCol TmpCol in this.Grid.Cols.ToArray())
                    {
                        if (TmpCol.Name != this.Grid.TreeGroup.NameColCod &&
                            TmpCol.Name != this.Grid.TreeGroup.NameColCodCaption &&
                            TmpCol.Name != this.Grid.TreeGroup.NameColCodFather &&
                            TmpCol.Name != this.Grid.TreeGroup.NameColCodIdentification
                        )
                            this.Grid.Cols.Del(TmpCol);
                    }
                }
                else if (this.Grid.Tree != null)
                {
                    foreach (AraGridCol TmpCol in this.Grid.Cols.ToArray())
                    {
                        if (TmpCol.Name != this.Grid.Tree.ColCaption.Name &&
                            TmpCol.Name != this.Grid.Tree.ColFather.Name &&
                            TmpCol.Name != this.Grid.Tree.ColId.Name
                        )
                            this.Grid.Cols.Del(TmpCol);
                    }
                }
                else
                    this.Grid.Cols.Clear();

                


                List<SFild> Filds = new List<SFild>();
                
        
                foreach (System.Reflection.PropertyInfo Pro in vFirstLine.GetType().GetProperties())
                {
                    Filds.Add(new SFild
                    {
                        vType=Pro.PropertyType,
                        Name = Pro.Name,
                        Alias = (Pro.GetCustomAttributes(typeof(AraFieldAlias), true).FirstOrDefault() != null?((AraFieldAlias)Pro.GetCustomAttributes(typeof(AraFieldAlias), true).FirstOrDefault()).Alias:""),
                        Hide = Pro.GetCustomAttributes(typeof(AraFieldHide), true).FirstOrDefault() != null 
                    });

                }


                foreach (System.Reflection.FieldInfo Field in vFirstLine.GetType().GetFields())
                {
                    Filds.Add(new SFild
                    {
                        vType = Field.FieldType,
                        Name = Field.Name,
                        Alias = (Field.GetCustomAttributes(typeof(AraFieldAlias), true).FirstOrDefault() != null ? ((AraFieldAlias)Field.GetCustomAttributes(typeof(AraFieldAlias), true).FirstOrDefault()).Alias : ""),
                        Hide = Field.GetCustomAttributes(typeof(AraFieldHide), true).FirstOrDefault() != null 
                    });
                }


                foreach(SFild Fild in Filds)
                {
                    SColuna TmpCol = new SColuna()
                        {
                            Name = Fild.Name,
                            Alias=Fild.Alias,
                            Hide=Fild.Hide
                        };
                    

                    if (
                        Fild.vType.Equals(typeof(Int16)) ||
                        Fild.vType.Equals(typeof(Int32)) ||
                        Fild.vType.Equals(typeof(Int64)) ||
                        Fild.vType.Equals(typeof(Int16?)) ||
                        Fild.vType.Equals(typeof(Int32?)) ||
                        Fild.vType.Equals(typeof(Int64?)) 
                     )
                    {
                        TmpCol.Tipo = enum_TipoColuna.Numerointero;
                    }
                    else if (Fild.vType.Equals(typeof(decimal)) ||
                        Fild.vType.Equals(typeof(double)) ||
                        Fild.vType.Equals(typeof(decimal?)) ||
                        Fild.vType.Equals(typeof(double?)) 
                        )
                    {
                        TmpCol.Tipo = enum_TipoColuna.Numero;
                    }
                    else if (Fild.vType.Equals(typeof(TimeSpan)) ||
                        Fild.vType.Equals(typeof(TimeSpan?)) 
                        )
                    {
                        TmpCol.Tipo = enum_TipoColuna.Hora;
                    }
                    else if (Fild.vType.Equals(typeof(DateTime)) ||
                        Fild.vType.Equals(typeof(DateTime?)) 
                        )
                    {
                        TmpCol.Tipo = enum_TipoColuna.DataHora;
                    }
                    else 
                    {
                        TmpCol.Tipo = enum_TipoColuna.Texto;
                    }
                    TmpFilter.Add(TmpCol.Name,TmpCol);

                }
            }
            return TmpFilter;
        }

        private void LoadAddAraGridCols(SColuna[] TmpFilter)
        {
            foreach (SColuna TmpCol in TmpFilter)
            {
                AraGridCol Col = new AraGridCol(Grid, (TmpCol.Alias == "" ? TmpCol.Name : TmpCol.Alias), TmpCol.Name, vHidden: TmpCol.Hide, vEditable: true);
                this.Grid.Cols.Add(Col);
            }
        }

        private void ColunaOpcoes_CLick(AraGrid Object, System.Collections.Hashtable Parans)
        {
            AraTools.Alert("teste");
        }

        Dictionary<string, int> customOrder = null;

        /// <summary>
        /// Dicionario usado para ordenar as colunas
        /// Dictionary<string, int> ("IDCOLUNA", ordem)
        /// null = ordem padrão
        /// </summary>
        public Dictionary<string, int> CustomOrder
        {
            set
            {
                customOrder = value;

                if (LoadColsBySql())
                    this.Grid.Commit();
            }

            get { return customOrder; }
        }

        //private int posicaoColuna(string idColuna)
        //{
        //    int posMaxima =
        //        (

        //        );
        //}

        private int posicao(string colName)
        {
            try
            {
                return customOrder[colName];
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool LoadColsBySql()
        {
            try
            {
                this.ColsInfoSub = LoadColsTypeAndAliasBySql(GetQuery.InvokeEvent());

                // Grup Desativado!
                //this.ColsInfo = LoadColsTypeAndAliasBySql(SqlTratadoGroup());

                this.ColsInfo = this.ColsInfoSub;
                
                if (this.ColsInfo.Count == 0)
                {
                    this.Grid.Cols.Clear();
                    this.Grid.Commit();
                    this.Grid.Rows.Clear();
                    if (AutoAdjustColumnWidthEnabled)
                        this.Grid.AutoAdjustColumnWidth();
                    return false;
                }

                

                try
                {
                    if (customOrder != null)
                    {
                        var sortedColsInfo =
                            (
                                from coluna in ColsInfo
                                orderby posicao(coluna.Value.Name)
                                select coluna
                            ).ToArray();

                        this.ColsInfo.Clear();

                        foreach (var item in sortedColsInfo)
                        {
                            this.ColsInfo.Add(item.Key, item.Value);
                        }
                    }
                }
                catch (Exception er)
                {
                    AraTools.Alert("Erro ao ordenar colunas, verifique se o dicionario possue todas as colunas.\n" + er.Message);
                }

                LoadAddAraGridCols(this.ColsInfo.Values.ToArray());
                return true;
            }
            catch(Exception err)
            {
                AraTools.Alert(err.Message);
                return false; 
            }
        }

        public Dictionary<string, string> Filter = new Dictionary<string, string>();
        public Dictionary<string, string> FilterSub = new Dictionary<string, string>();

        public void LoadFilter()
        {
            if (!Tocuh)
            {
                Filter.Clear();
                if (this.Grid.Rows["Search"] != null)
                {
                    foreach (AraGridCell Cell in this.Grid.Rows["Search"].Cells)
                    {
                        bool Ignorar = false;

                        if (this.Grid.TreeGroup != null)
                        {
                            if (
                                Cell.Col.Name == this.Grid.TreeGroup.NameColCod ||
                                Cell.Col.Name == this.Grid.TreeGroup.NameColCodCaption ||
                                Cell.Col.Name == this.Grid.TreeGroup.NameColCodFather ||
                                Cell.Col.Name == this.Grid.TreeGroup.NameColCodIdentification
                                )
                                Ignorar = true;
                        }

                        if (!Ignorar)
                        {
                            if (Cell.Text == null
                                || string.IsNullOrEmpty(Cell.Text)
                                || string.IsNullOrEmpty(System.Text.RegularExpressions.Regex.Replace(Cell.Text, @"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>", String.Empty))
                                )
                                Ignorar = true;
                        }

                        if (!Ignorar)
                            Filter[Cell.Col.Name] = System.Text.RegularExpressions.Regex.Replace(Cell.Text, @"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>", String.Empty).Trim();
                    }
                }
            }
        }

        public void FilterSaveToGrid()
        {
            bool vTouch = Tocuh;

            if (this.Grid.Rows != null)
            {
                if (this.Grid.Rows["Search"] != null)
                {
                    foreach (string ColName in Filter.Keys)
                    {
                        if (this.Grid.Rows["Search"][ColName].Text != Filter[ColName])
                            this.Grid.Rows["Search"][ColName].Text = Filter[ColName];
                    }

                    if (Tocuh)
                    {
                        foreach (AraGridCol Col in this.Grid.Cols.ToArray())
                        {
                            this.Grid.Rows["Search"][Col].Text += " " +
                                Grid.Buttons.Add(AraGridButton.ButtonIco.pencil, new System.Collections.Hashtable { { "ColName", Col.Name } }, EditCell);
                        }
                    }


                    foreach (AraGridCol Col in this.Grid.Cols.ToArray())
                    {
                        if (ColsInfo[Col.Name].Tipo == enum_TipoColuna.Texto)
                            this.Grid.Rows["Search"][Col].Text +=
                                " <div style='position:relative;float:right;'>" +
                                Grid.Buttons.Add<Action<string>>(AraGridButton.ButtonIco.triangle_1_s, FilterStringPlus, Col.Name)
                                + "</div>";
                        else if (ColsInfo[Col.Name].Tipo == enum_TipoColuna.Numero || ColsInfo[Col.Name].Tipo == enum_TipoColuna.Numerointero)
                            this.Grid.Rows["Search"][Col].Text +=
                                " <div style='position:relative;float:right;'>" +
                                Grid.Buttons.Add<Action<string>>(AraGridButton.ButtonIco.triangle_1_s, FilterNumeroPlus, Col.Name)
                                + "</div>";
                        else if (ColsInfo[Col.Name].Tipo == enum_TipoColuna.Data || ColsInfo[Col.Name].Tipo == enum_TipoColuna.DataHora || ColsInfo[Col.Name].Tipo == enum_TipoColuna.Hora)
                            this.Grid.Rows["Search"][Col].Text +=
                                " <div style='position:relative;float:right;'>" +
                                Grid.Buttons.Add<Action<string>>(AraGridButton.ButtonIco.triangle_1_s, FilterDateTimePlus, Col.Name)
                                + "</div>";
                    }

                }
            }
        }

        string EditCell_ColName;
        private void EditCell(AraGrid Object, System.Collections.Hashtable Parans)
        {
            EditCell_ColName = Parans["ColName"].ToString();
            string vText = (Filter.ContainsKey(EditCell_ColName) ? Filter[EditCell_ColName] : "");

            AraTools.AlertGetString("Digite o texto de pesquisa.", vText, EditCell_Return);
        }

        private void EditCell_Return(string vResu)
        {
            if (vResu.Trim() != "")
            {
                if (this.Filter.ContainsKey(EditCell_ColName))
                    this.Filter[EditCell_ColName] = vResu.Trim();
                else
                    this.Filter.Add(EditCell_ColName, vResu.Trim());                
            }
            else if (this.Filter.ContainsKey(EditCell_ColName))
                this.Filter.Remove(EditCell_ColName);

            this.FilterSaveToGrid();

            EditCell_ColName = null;
            this.PageReload();
        }

        private string TmpFiltroAntigoPlus = null;
        public void FilterStringPlus(string vColName)
        {
            EditCell_ColName = vColName;

            if (this.Filter.ContainsKey(EditCell_ColName) && !string.IsNullOrEmpty(this.Filter[EditCell_ColName]))
            {
                TmpFiltroAntigoPlus = this.Filter[EditCell_ColName];
                this.Filter[EditCell_ColName]="";
                this.FilterSaveToGrid();
            }
            else
                TmpFiltroAntigoPlus = null;

            FrmAraGridSearchLinqFilterString FrmAraGridSearchLinqFilterString = new FrmAraGridSearchLinqFilterString(this, this, ColsInfo[vColName], this.Filter[vColName]);
            FrmAraGridSearchLinqFilterString.Show(FrmAraGridSearchLinqFilterString_Return);
        }

        private void FrmAraGridSearchLinqFilterString_Return(object vObj)
        {
            if (vObj !=null)
            {
                if (this.Filter.ContainsKey(EditCell_ColName))
                    this.Filter[EditCell_ColName] = vObj.ToString();
                else
                    this.Filter.Add(EditCell_ColName, vObj.ToString());
                this.FilterSaveToGrid();
            }
            else if (TmpFiltroAntigoPlus !=null)
            { 
                if (this.Filter.ContainsKey(EditCell_ColName))
                    this.Filter[EditCell_ColName] = TmpFiltroAntigoPlus;
                else
                    this.Filter.Add(EditCell_ColName, TmpFiltroAntigoPlus);
                this.FilterSaveToGrid();
            }


            EditCell_ColName = null;
            this.PageReload();
        }

        public void FilterNumeroPlus(string vColName)
        {
            EditCell_ColName = vColName;

            if (this.Filter.ContainsKey(EditCell_ColName) && !string.IsNullOrEmpty(this.Filter[EditCell_ColName]))
            {
                TmpFiltroAntigoPlus = this.Filter[EditCell_ColName];
                this.Filter[EditCell_ColName] = "";
                this.FilterSaveToGrid();
            }
            else
                TmpFiltroAntigoPlus = null;

            FrmAraGridSearchLinqFilterNumero FrmAraGridSearchLinqFilterNumero = new FrmAraGridSearchLinqFilterNumero(this, this, ColsInfo[vColName], this.Filter[vColName]);
            FrmAraGridSearchLinqFilterNumero.Show(FrmAraGridSearchLinqFilterNumero_Return);
        }

        private void FrmAraGridSearchLinqFilterNumero_Return(object vObj)
        {
            if (vObj != null)
            {
                if (this.Filter.ContainsKey(EditCell_ColName))
                    this.Filter[EditCell_ColName] = vObj.ToString();
                else
                    this.Filter.Add(EditCell_ColName, vObj.ToString());
                this.FilterSaveToGrid();
            }
            else if (TmpFiltroAntigoPlus != null)
            {
                if (this.Filter.ContainsKey(EditCell_ColName))
                    this.Filter[EditCell_ColName] = TmpFiltroAntigoPlus;
                else
                    this.Filter.Add(EditCell_ColName, TmpFiltroAntigoPlus);
                this.FilterSaveToGrid();
            }

            EditCell_ColName = null;
            this.PageReload();
        }

        public void FilterDateTimePlus(string vColName)
        {
            EditCell_ColName = vColName;

            if (this.Filter.ContainsKey(EditCell_ColName) && !string.IsNullOrEmpty(this.Filter[EditCell_ColName]))
            {
                TmpFiltroAntigoPlus = this.Filter[EditCell_ColName];
                this.Filter[EditCell_ColName] = "";
                this.FilterSaveToGrid();
            }
            else
                TmpFiltroAntigoPlus = null;

            FrmAraGridSearchLinqFilterDateTime FrmAraGridSearchLinqFilterDateTime = new FrmAraGridSearchLinqFilterDateTime(this, this, ColsInfo[vColName], this.Filter[vColName]);
            FrmAraGridSearchLinqFilterDateTime.Show(FrmAraGridSearchLinqFilterDateTime_Return);
        }

        private void FrmAraGridSearchLinqFilterDateTime_Return(object vObj)
        {
            if (vObj != null)
            {
                if (this.Filter.ContainsKey(EditCell_ColName))
                    this.Filter[EditCell_ColName] = vObj.ToString();
                else
                    this.Filter.Add(EditCell_ColName, vObj.ToString());
                this.FilterSaveToGrid();
            }
            else if (TmpFiltroAntigoPlus != null)
            {
                if (this.Filter.ContainsKey(EditCell_ColName))
                    this.Filter[EditCell_ColName] = TmpFiltroAntigoPlus;
                else
                    this.Filter.Add(EditCell_ColName, TmpFiltroAntigoPlus);
                this.FilterSaveToGrid();
            }

            EditCell_ColName = null;
            this.PageReload();
        }

        

        public delegate IQueryable<object> DFiltroCustom(IQueryable<object> vQuery, string ColName, enum_TipoColuna Tipo, string vText);

        public static event DFiltroCustom FiltroCustomStatic = null;
        public AraEvent<DFiltroCustom> FiltroCustom { get; set; }


        public IQueryable<object> SqlFiltro(IQueryable<object> vQuery, Dictionary<string, string> Filter, Dictionary<string, SColuna> ColsInfo)
        {
            
            IQueryable<object> vReturnQuery=vQuery;

            if (this.Grid.Rows["Search"] != null)
                LoadFilter();
                       
            foreach (string ColName in Filter.Keys.ToArray())
            {
                string vText = System.Text.RegularExpressions.Regex.Replace(Filter[ColName], @"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>", String.Empty).Trim();
                var vTipo = ColsInfo[ColName].Tipo;

                bool vFiltroNormal = true;

                if (FiltroCustom.InvokeEvent!=null)
                {
                    var vTmpQ = FiltroCustom.InvokeEvent(vReturnQuery, ColName, vTipo, vText);
                    if (vTmpQ!=null)
                    {
                        vReturnQuery = vTmpQ;
                        vFiltroNormal = false;
                    }
                }

                if (vFiltroNormal && FiltroCustomStatic != null)
                {
                    var vTmpQ = FiltroCustomStatic(vReturnQuery, ColName, vTipo, vText);
                    if (vTmpQ != null)
                    {
                        vReturnQuery = vTmpQ;
                        vFiltroNormal = false;
                    }
                }


                if (vFiltroNormal)
                {
                    switch (vTipo)
                    {
                        case enum_TipoColuna.Texto:
                            {
                                if (vText.Length > 0 && vText.Substring(0, 1) == "=")
                                {
                                    var JSon = TryJsonParse(vText.Substring(1, vText.Length - 1));
                                    if (JSon != null)
                                    {
                                        List<object> vTmpStringParans = new List<object>();
                                        StringBuilder vTmpScript = new StringBuilder();

                                        foreach (string vTextoParte in (string[])JSon)
                                        {
                                            if (vTextoParte.Length > 0)
                                            {
                                                if (vTmpScript.Length > 0)
                                                    vTmpScript.Append(" || ");

                                                vTmpScript.Append("(");

                                                if (vTextoParte.Substring(0, 1) == "=")
                                                {
                                                    vTmpScript.Append("" + ColName + ".ToUpper().Trim() == @" + vTmpStringParans.Count );
                                                    vTmpStringParans.Add(vTextoParte.Substring(1, vTextoParte.Length - 1).ToUpper().Trim());
                                                }
                                                else
                                                {
                                                    bool Primeiro = true;
                                                    foreach (var vPalavra in vTextoParte.Split(' ').AsQueryable().Where(a => a.Trim() != ""))
                                                    {
                                                        if (Primeiro)
                                                            Primeiro = false;
                                                        else
                                                            vTmpScript.Append(" && ");
                                                        vTmpScript.Append("" + ColName + ".ToUpper().Contains(@" + vTmpStringParans.Count + ".ToUpper())");
                                                        vTmpStringParans.Add(vPalavra);
                                                    }
                                                }
                                                vTmpScript.Append(") ");
                                            }
                                        }

                                        vReturnQuery = vReturnQuery.Where(vTmpScript.ToString(), vTmpStringParans.ToArray());
                                        vTmpScript.Clear();
                                        vTmpScript = null;
                                    }
                                    else
                                        vReturnQuery = vReturnQuery.Where("" + ColName + ".ToUpper().Trim() == @0", vText.Substring(1, vText.Length - 1).ToUpper().Trim());
                                }
                                else
                                {
                                    foreach (var vPalavra in vText.Split(' ').AsQueryable().Where(a => a.Trim() != ""))
                                        vReturnQuery = vReturnQuery.Where("" + ColName + ".ToUpper().Contains(@0.ToUpper())", vPalavra);
                                }
                            }
                            break;
                        case enum_TipoColuna.Numerointero:
                        case enum_TipoColuna.Numero:
                            {
                                if (vText.Length >= 1)
                                {
                                    if (vText.Substring(0, 1) == "=")
                                    {
                                        var JSon = TryJsonParse(vText.Substring(1, vText.Length - 1));
                                        if (JSon != null)
                                        {
                                            foreach (string vTextoParte in (string[])JSon)
                                            {
                                                SqlFiltroNumero(ref vReturnQuery, vTextoParte, ColName);
                                            }
                                        }
                                        else
                                            AraTools.Alert("Filtro Invalido!");
                                    }
                                    else
                                        SqlFiltroNumero(ref vReturnQuery, vText, ColName);
                                }
                            }
                            break;
                        case enum_TipoColuna.Hora:

                            if (vText.Length > 0)
                            {
                                if (vText.Substring(0, 1) == "=")
                                {
                                    var JSon = TryJsonParse(vText.Substring(1, vText.Length - 1));
                                    if (JSon != null)
                                    {
                                        foreach (string vTextoParte in (string[])JSon)
                                        {
                                            SqlFiltroHora(ref vReturnQuery, vTextoParte, ColName);
                                        }
                                    }
                                    else
                                        AraTools.Alert("Filtro Invalido!");
                                }
                                else
                                    SqlFiltroHora(ref vReturnQuery, vText, ColName);

                            }
                            break;
                        case enum_TipoColuna.DataHora:
                            if (vText.Length > 0)
                            {
                                if (vText.Substring(0, 1) == "=")
                                {
                                    var JSon = TryJsonParse(vText.Substring(1, vText.Length - 1));
                                    if (JSon != null)
                                    {
                                        foreach (string vTextoParte in (string[])JSon)
                                        {
                                            SqlFiltroDataHora(ref vReturnQuery, vTextoParte, ColName);
                                        }
                                    }
                                    else
                                        SqlFiltroDataHora(ref vReturnQuery, vText.Substring(1, vText.Length - 1), ColName);
                                }
                                else
                                    SqlFiltroDataHora(ref vReturnQuery, vText, ColName);                               
                            }
                            break;
                        
                    }
                }
                Filter[ColName] = vText;
            }

            if (this.Grid.Rows["Search"] != null)
                FilterSaveToGrid();


            return vReturnQuery;
        }

        private void SqlFiltroDataHora(ref IQueryable<object> vReturnQuery, string vText, string ColName)
        {
            ClassCondicaoDateTime CN = ClassCondicaoDateTime.TryParse(vText);

            if (CN != null)
            {

                if (CN.Sinal == ">")
                {
                    vReturnQuery = vReturnQuery.Where(" " + ColName + " != null ");
                    vReturnQuery = vReturnQuery.Where(ColName + " > @0 ", CN.DataHora);
                }
                else if (CN.Sinal == "<")
                {
                    vReturnQuery = vReturnQuery.Where(" " + ColName + " != null ");
                    vReturnQuery = vReturnQuery.Where(ColName + " < @0 ", CN.DataHora);
                }
                else if (CN.Sinal == "=")
                {
                    vReturnQuery = vReturnQuery.Where(" " + ColName + " != null ");
                    vReturnQuery = vReturnQuery.Where(ColName + " = @0 ", CN.DataHora);
                }
                else
                    AraTools.Alert("Sinal invalido.");

            }
            else
            {
                ClassDateTimeIntervalo Intervalo = ClassDateTimeIntervalo.TryParse(vText);
                if (Intervalo != null)
                {
                    vReturnQuery = vReturnQuery.Where(ColName + " > @0 ", Intervalo.DataIni);
                    vReturnQuery = vReturnQuery.Where(ColName + " < @0 ", Intervalo.DataFim);
                }
                else if ((new System.Text.RegularExpressions.Regex(@"(^((((0[1-9])|([1-2][0-9])|(3[0-1]))|([1-9]))\x2F(((0[1-9])|(1[0-2]))|([1-9]))\x2F(([0-9]{2})|(((19)|([2]([0]{1})))([0-9]{2}))))$)")).IsMatch(vText))
                    vReturnQuery = vReturnQuery.Where(" " + ColName + " != null && " + ColName + ".Value.Day  == @0 && " + ColName + ".Value.Month  == @1 && " + ColName + ".Value.Year  == @2 ", DateTime.Parse(vText).Day, DateTime.Parse(vText).Month, DateTime.Parse(vText).Year);
                else if (AraTools.IsDate(vText))
                    vReturnQuery = vReturnQuery.Where("" + ColName + " == @0 ", DateTime.Parse(vText));
                else
                {
                    AraTools.Alert("O valor \"" + vText + "\" não é uma valor valido.");
                    vText = "";
                }
            }
        }

        private void SqlFiltroNumero(ref IQueryable<object> vReturnQuery, string vText, string ColName)
        {
            ClassCondicaoNumero CN = ClassCondicaoNumero.TryParse(vText);

            if (CN != null)
            {
                if (CN.Sinal == ">")
                    vReturnQuery = vReturnQuery.Where("(" + ColName + " == null?0:" + ColName + ") > " + CN.Numero.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US")));
                else if (CN.Sinal == "<")
                    vReturnQuery = vReturnQuery.Where("(" + ColName + " == null?0:" + ColName + ") < " + CN.Numero.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US")));
                else if (CN.Sinal == "=")
                    vReturnQuery = vReturnQuery.Where("(" + ColName + " == null?0:" + ColName + ") == " + CN.Numero.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US")));
                else
                    throw new Exception("Sinal '" + CN.Sinal + "' não suportado!");
            }
            else
            {
                decimal vTmpint64 = 0;

                if (decimal.TryParse(vText, out vTmpint64))
                {
                    vReturnQuery = vReturnQuery.Where("(" + ColName + " == null?0:" + ColName + ") == @0", vTmpint64);
                }
                else
                    throw new Exception(" Numero '" + vText + "' invalido.");
            }
        }

        private void SqlFiltroHora(ref IQueryable<object> vReturnQuery, string vText, string ColName)
        {
            ClassCondicaoTimeSpan CondicaoTimeSpan = ClassCondicaoTimeSpan.TryParse(vText);

            if (CondicaoTimeSpan != null)
            {
                if (CondicaoTimeSpan.Sinal == "=")
                    vReturnQuery = vReturnQuery.Where(ColName + " == @0 ", CondicaoTimeSpan.Hora);
                else if (CondicaoTimeSpan.Sinal == "<")
                    vReturnQuery = vReturnQuery.Where(ColName + " < @0 ", CondicaoTimeSpan.Hora);
                else if (CondicaoTimeSpan.Sinal == ">")
                    vReturnQuery = vReturnQuery.Where(ColName + " > @0 ", CondicaoTimeSpan.Hora);
                else
                    AraTools.Alert("Sinal do iltro Invalido!");
            }
            else
            {
                ClassTimeSpanIntervalo Intervalo = ClassTimeSpanIntervalo.TryParse(vText);

                if (Intervalo != null)
                {
                    vReturnQuery = vReturnQuery.Where(ColName + " > @0 ", Intervalo.Inicio);
                    vReturnQuery = vReturnQuery.Where(ColName + " < @0 ", Intervalo.Fim);
                }
                else
                {
                    TimeSpan vTmpHora;
                    if (TimeSpan.TryParse(vText, out vTmpHora))
                        vReturnQuery = vReturnQuery.Where(ColName + " == @0 ", vTmpHora);
                    else
                        AraTools.Alert("Valor do filtro Invalido!");
                }
            }
        }

        private dynamic TryJsonParse(string vJson)
        {
            try
            {
                return Json.DynamicJson.Parse(vJson);
            }
            catch { return null; }
        }

        public IQueryable<object> SqlGroup(IQueryable<object> vQuery)
        {
            //if (ColsGroup.Count > 0)
            //{

            //    foreach (AraGridColGroup Group in ColsGroup.ToArray())
            //    {
            //        switch (Group.Operator)
            //        {
            //            case AraGridColGroup.EOperator.Group:
            //                vTmpSqlGroup += "\"" + Group.Col.Name + "\",";
            //                break;
            //            case AraGridColGroup.EOperator.Avg:
            //                vTmpSqlGroup += "avg(\"" + Group.Col.Name + "\") as \"" + Group.Col.Name + "\",";
            //                break;
            //            case AraGridColGroup.EOperator.Sum:
            //                vTmpSqlGroup += "sum(\"" + Group.Col.Name + "\") as \"" + Group.Col.Name + "\",";
            //                break;
            //            case AraGridColGroup.EOperator.Max:
            //                vTmpSqlGroup += "max(\"" + Group.Col.Name + "\") as \"" + Group.Col.Name + "\",";
            //                break;
            //            case AraGridColGroup.EOperator.Min:
            //                vTmpSqlGroup += "min(\"" + Group.Col.Name + "\") as \"" + Group.Col.Name + "\",";
            //                break;
            //            case AraGridColGroup.EOperator.Count:
            //                vTmpSqlGroup += "count(\"" + Group.Col.Name + "\") as \"" + Group.Col.Name + "\",";
            //                break;
            //        }
            //    }
            //    vTmpSqlGroup = vTmpSqlGroup.Substring(0, vTmpSqlGroup.Length - 1);

            //    vTmpSqlGroup += " from (" + vQuery + " ) as tmpgroup";

            //    int NGroup = 0;
            //    foreach (AraGridColGroup Group in ColsGroup.ToArray())
            //    {
            //        if (Group.Operator == AraGridColGroup.EOperator.Group)
            //            NGroup++;
            //    }

            //    if (NGroup > 0)
            //    {
            //        vTmpSqlGroup += " group by ";
            //        foreach (AraGridColGroup Group in ColsGroup.ToArray())
            //        {
            //            switch (Group.Operator)
            //            {
            //                case AraGridColGroup.EOperator.Group:
            //                    vTmpSqlGroup += "\"" + Group.Col.Name + "\",";
            //                    break;
            //            }
            //        }
            //        vTmpSqlGroup = vTmpSqlGroup.Substring(0, vTmpSqlGroup.Length - 1);
            //    }


            //    return vTmpSqlGroup;
            //}
            //else
            return vQuery;
        }

        public IQueryable<object> SqlOrderBy(IQueryable<object> vQuery)
        {
            if (OrderBy != null && OrderBy.Count()>0)
            {
                string vTmpSql ="";
                foreach (SOrderBy vColG in OrderBy)
                {
                    if (ColsInfo.Keys.Contains(vColG.Col.Name))
                            vTmpSql += vColG.Col.Name + (vColG.Sense == AraGridCols.OrderBySense.Descending ? " descending" : "") + ",";
                }
                vTmpSql = vTmpSql.Substring(0, vTmpSql.Length - 1);

                vQuery =vQuery.OrderBy(vTmpSql);
            }


            return vQuery;
        }

        public void LoadDataAny()
        {
            //AraTools.DebuggerJS();
            this.Grid.WaitingLoading("Carregando...", MsgCarregando);
        }

        private void MsgCarregando()
        {
            lock (this)
            {
                LoadData();
            }
        }

        public bool Tocuh
        {
            get
            {
                EDeviceType DT = AraTools.DeviceType;
                return DT == EDeviceType.Phone || DT == EDeviceType.Tablet;
            }
        }

        [AraDevEvent]
        public AraEvent<Action<AraGridSearchLinq>> OnLoadData { get; set; }

        public void LoadData()
        {
            lock (this)
            {
                IQueryable<object> vQuery = SqlTratado();

                string[] TmpRowSearch = new string[this.Grid.Cols.Count];

                if (this.Grid.TreeGroup != null)
                {
                    TmpRowSearch[this.Grid.Cols[this.Grid.TreeGroup.NameColCod].Pos] = "Search";
                    TmpRowSearch[this.Grid.Cols[this.Grid.TreeGroup.NameColCodIdentification].Pos] = "1";
                }

                this.Grid.Rows.Clear();

                
                this.Grid.Rows.Add("Search", TmpRowSearch,!Tocuh);
                this.FilterSaveToGrid();              

                int vLine = 0;
                AraGridCol[] Cols = this.Grid.Cols.ToArray();

                if (vQuery != null)
                {
                    foreach (object vObj in vQuery.Skip((Page - 1) * PageMaxRecords).Take(PageMaxRecords).ToArray())
                    {
                        Dictionary<string, string> TmpRow = new Dictionary<string, string>();

                        foreach (AraGridCol Col in Cols)
                        {
                            object vValue;
                            IAraFieldFormat[] Format;
                            if (vObj.GetType().GetProperty(Col.Name) != null)
                            {
                                System.Reflection.PropertyInfo Pro = vObj.GetType().GetProperty(Col.Name);
                                vValue = Pro.GetValue(vObj, null);
                                Format = Pro.GetCustomAttributes(typeof(IAraFieldFormat), true).Select(a => (IAraFieldFormat)a).OrderBy(a=>a.Ordem).ToArray();
                            }
                            else if (vObj.GetType().GetField(Col.Name) != null)
                            {
                                System.Reflection.FieldInfo Fild = vObj.GetType().GetField(Col.Name);
                                vValue = Fild.GetValue(vObj);
                                Format = Fild.GetCustomAttributes(typeof(IAraFieldFormat), true).Select(a => (IAraFieldFormat)a).OrderBy(a => a.Ordem).ToArray();
                            }
                            else
                            {
                                vValue = null;
                                Format = null;
                            }

                            if (Format != null && Format.Count() > 0)
                            {
                                foreach (IAraFieldFormat vTmpFormat in Format)
                                    vValue = vTmpFormat.ToString(vObj, vValue);
                            }

                            TmpRow[Col.Name] = (vValue == null ? "" : vValue.ToString());
                        }

                        try
                        {
                            this.Grid.Rows.Add(vLine.ToString(), TmpRow, false);
                        }
                        catch
                        {
                            System.Diagnostics.Debugger.Break();
                        }
                        vLine++;
                    }
                }

                if (this.Grid.TreeGroup != null)
                    this.Grid.Tree.ExpandAllRows();
            }

            if (AutoAdjustColumnWidthEnabled)
                this.Grid.AutoAdjustColumnWidth();


            if (this.Grid.Tree != null)
                this.Grid.Tree.RetraiAllRows();

            AraTools.AsynchronousFunction(LoadTotalPages);

            if (OnLoadData.InvokeEvent != null)
                OnLoadData.InvokeEvent(this);

        }

        public void LoadTotalPages()
        {
            IQueryable<object> vQuery = SqlTratado();
            int vTotalreg = (vQuery != null ? vQuery.Count() : 0);

            
            if (vTotalreg > PageMaxRecords)
            {
                decimal TmpPageTotal = Convert.ToDecimal(vTotalreg) / Convert.ToDecimal(PageMaxRecords);
                if (TmpPageTotal != (int)TmpPageTotal)
                    TmpPageTotal = (int)TmpPageTotal + 1;
                this.PageMax = (int)TmpPageTotal;
            }
            else
                if (this.PageMax != 1)
                    this.PageMax = 1;

            if (Page > this.PageMax)
            {
                Page = this.PageMax;
                Grid.WaitingLoading("Carregando", LoadData);
            }
        }

        private bool _AutoAdjustColumnWidthEnabled = true;
        [AraDevProperty(true)]
        public bool AutoAdjustColumnWidthEnabled 
        {
            get { return _AutoAdjustColumnWidthEnabled; }
            set
            {
                _AutoAdjustColumnWidthEnabled = value;
            }
        }

        public string Text
        {
            get { return ""; }
            set { }
        }
        

        #region Event
        

        //private AraDiv DivvisibleNextPage;
        //int? LinhaDivvisibleNextPage = null;
        //private void DivvisibleNextPage_Click(object vObj,EventArgs Event)
        //{
        //    if (Page + 1 <= this.PageTotal)
        //    {
        //        Page++;
        //        PageReload();
        //    }
        //    else
        //    {
        //        if (LinhaDivvisibleNextPage != null)
        //            this.Rows.Del(((int)LinhaDivvisibleNextPage).ToString());
        //    }
        //}

        private void FBChangeCell(AraGrid Object, AraGridRow Row, AraGridCol Col, string Value)
        {
            this.PageReload();
        }

        //private void FBKeyDown(AraGrid Object, int vKey)
        //{
        //    if (vKey == 13)
        //    {
        //        if (this.SelNRow > 1)
        //            RunEventReturnSelect(this.Rows[this.SelRow]);
        //    }
        //}

        private void FBClickDblCell(AraGridCol vCol, AraGridRow vRow, AraEventMouse vMouse)
        {
            if (vMouse.Button == 1)
            {
                if (ClickReturnSelect == EClick_ReturnSelect.DoubleClick)
                {
                    if (this.Grid.SelNRow > 1)
                        RunEventReturnSelect(this.Grid.Rows[this.Grid.SelRow]);
                }
            }
        }

        private void FBClickCell(AraGridCol vCol, AraGridRow vRow, AraEventMouse vMouse)
        {
            if (vMouse.Button == 1)
            {
                if (ClickReturnSelect == EClick_ReturnSelect.Click)
                {
                    if (this.Grid.SelNRow > 1)
                    {
                        AraGridRow vRowTmp = this.Grid.Rows[this.Grid.SelRow];
                        if (vRowTmp != null)
                        {
                            RunEventReturnSelect(vRowTmp);
                        }
                    }
                }
            }
        }

        public delegate void ReturnSearch_delegate(AraGrid Object, AraGridRow Row);
        [AraDevEvent]
        public AraEvent<ReturnSearch_delegate> ReturnSearch { get; set; }
        private void RunEventReturnSelect(AraGridRow Row)
        {
            try
            {
                if (this.ReturnSearch.InvokeEvent != null)
                    this.ReturnSearch.InvokeEvent(this.Grid, Row);
            }
            catch (Exception err)
            {
                throw new Exception("On error AraGridSearchOracle.RunEventReturnSelect .\n" + err.Message);
            }
        }

        #endregion

        #region Ordem
        [Serializable]
        public struct SOrderBy
        {
            public SOrderBy(AraGridCol vCol, AraGridCols.OrderBySense vSense)
            {
                Col = vCol;
                Sense = vSense;
            }

            public SOrderBy(AraGridCol vCol)
            {
                Col = vCol;
                Sense = AraGridCols.OrderBySense.Ascending;
            }

            public AraGridCol Col;
            public AraGridCols.OrderBySense Sense;
        }

        bool _OrderByEnable=true;
        [AraDevProperty(true)]
        public bool OrderByEnable
        {
            get { return _OrderByEnable; }
            set {

                if (_OrderByEnable != value)
                {
                    bool Desabilitou = _OrderByEnable == true && value == false;
                    _OrderByEnable = value;

                    if (Desabilitou)
                        this.Grid.Cols.Orderby -= ColsOrderBy;
                    else
                        this.Grid.Cols.Orderby += ColsOrderBy;
                }
            }
        }

        private SOrderBy[] _OrderBy;

        public SOrderBy[] OrderBy
        {
            get { return _OrderBy; }
            set
            {
                if (OrderByEnable)
                    _OrderBy = value;
                //PageReload();
            }
        }

        public void SetOrderBy(SOrderBy[] vCols)
        {
            OrderBy = vCols;
        }

        public void SetOrderBy(AraGridCol[] vCols)
        {
            List<SOrderBy> TmpOrderBy = new List<SOrderBy>();
            foreach (AraGridCol Col in vCols)
            {
                TmpOrderBy.Add(new SOrderBy(Col));
            }

            OrderBy = TmpOrderBy.ToArray();
        }

        private void ColsOrderBy(AraGridCol Col, AraGridCols.OrderBySense Sense)
        {
            SetOrderBy(new SOrderBy[] { new SOrderBy(Col, Sense) });
            PageReload();
        }
        #endregion

        #region Group
        public AraGridColGroups ColsGroup;
        #endregion

        public bool IsInternalCol(AraGridCol coluna)
        {
            bool ColunaInterna = false;

            if (this.Grid.Tree != null)
            {
                if (
                    this.Grid.Tree.ColCaption.Name == coluna.Name ||
                    this.Grid.Tree.ColFather.Name == coluna.Name ||
                    this.Grid.Tree.ColId.Name == coluna.Name
                ) ColunaInterna = true;
            }

            if (this.Grid.TreeGroup != null)
            {
                if (
                    this.Grid.TreeGroup.NameColCod == coluna.Name ||
                    this.Grid.TreeGroup.NameColCodCaption == coluna.Name ||
                    this.Grid.TreeGroup.NameColCodFather == coluna.Name ||
                    this.Grid.TreeGroup.NameColCodIdentification == coluna.Name
                ) ColunaInterna = true;
            }

            return ColunaInterna;
        }

        #region FrmAraGridSearchLinqFilterNumero

        

        public class ClassCondicaoNumero
        {
            public string Sinal;
            public decimal Numero;

            public static readonly string[] SinaisValidos = new string[]{
                ">",
                "<",
                "="
            };

            public static ClassCondicaoNumero TryParse(string vTexto)
            {
                try
                {
                    if (!string.IsNullOrEmpty(vTexto))
                    {
                        string vTmpSinal;
                        string vTmpNumero;

                        var vTmp = vTexto.Trim().Replace("  ", " ").Split(' ');
                        if (vTmp.Length == 2)
                        {
                            vTmpSinal = vTmp[0];
                            vTmpNumero = vTmp[1];
                        }
                        else if (vTexto.Length >= 2)
                        {
                            vTmpSinal = vTexto.Substring(0, 1);
                            vTmpNumero = vTexto.Substring(1, vTexto.Length - 1);
                        }
                        else
                            return null;

                        ClassCondicaoNumero CondicaoNumero = new ClassCondicaoNumero();
                        CondicaoNumero.Sinal = vTmpSinal;

                        if (!SinaisValidos.Contains( CondicaoNumero.Sinal))
                            return null;
                        if (!decimal.TryParse(vTmpNumero, out CondicaoNumero.Numero))
                            return null;
                        return CondicaoNumero;
                    }
                    else
                        return null;
                }
                catch { return null; }
            }
        }
        #endregion

        #region FrmAraGridSearchLinqFilterDateTime

        public class ClassCondicaoDateTime
        {
            public string Sinal;
            public DateTime DataHora;

            public static readonly string[] SinaisValidos = new string[]{
                ">",
                "<",
                "="
            };

            public static ClassCondicaoDateTime TryParse(string vTexto)
            {
                try
                {
                    if (!string.IsNullOrEmpty(vTexto))
                    {
                        string vTmpSinal;
                        string vTmpNumero;

                        var vTmp = vTexto.Trim().Replace("  ", " ").Split(' ');
                        if (vTmp.Length == 2)
                        {
                            vTmpSinal = vTmp[0];
                            vTmpNumero = vTmp[1];
                        }
                        else if (vTexto.Length >= 2)
                        {
                            vTmpSinal = vTexto.Substring(0, 1);
                            vTmpNumero = vTexto.Substring(1, vTexto.Length - 1);
                        }
                        else
                            return null;

                        ClassCondicaoDateTime CondicaoDateTime = new ClassCondicaoDateTime();
                        CondicaoDateTime.Sinal = vTmpSinal;

                        if (!SinaisValidos.Contains(CondicaoDateTime.Sinal))
                            return null;
                        if (!DateTime.TryParse(vTmpNumero, out CondicaoDateTime.DataHora))
                            return null;
                        return CondicaoDateTime;
                    }
                    else
                        return null;
                }
                catch { return null; }
            }
        }

        public class ClassCondicaoTimeSpan
        {
            public string Sinal;
            public TimeSpan Hora;

            public static readonly string[] SinaisValidos = new string[]{
                ">",
                "<",
                "="
            };

            public static ClassCondicaoTimeSpan TryParse(string vTexto)
            {
                try
                {
                    if (!string.IsNullOrEmpty(vTexto))
                    {
                        string vTmpSinal;
                        string vTmpNumero;

                        var vTmp = vTexto.Trim().Replace("  ", " ").Split(' ');
                        if (vTmp.Length == 2)
                        {
                            vTmpSinal = vTmp[0];
                            vTmpNumero = vTmp[1];
                        }
                        else if (vTexto.Length >= 2)
                        {
                            vTmpSinal = vTexto.Substring(0, 1);
                            vTmpNumero = vTexto.Substring(1, vTexto.Length - 1);
                        }
                        else
                            return null;

                        ClassCondicaoTimeSpan CondicaoTimeSpan = new ClassCondicaoTimeSpan();
                        CondicaoTimeSpan.Sinal = vTmpSinal;

                        if (!SinaisValidos.Contains(CondicaoTimeSpan.Sinal))
                            return null;
                        if (!TimeSpan.TryParse(vTmpNumero, out CondicaoTimeSpan.Hora))
                            return null;
                        return CondicaoTimeSpan;
                    }
                    else
                        return null;
                }
                catch { return null; }
            }
        }

        public class ClassDateTimeIntervalo
        {
            public DateTime DataIni;
            public DateTime DataFim;

            public static ClassDateTimeIntervalo TryParse(string vTexto)
            {
                try
                {
                    if (!string.IsNullOrEmpty(vTexto))
                    {

                        if (!(vTexto.ToUpper().Contains("ATÉ") || vTexto.ToUpper().Contains("ATE")))
                            return null;

                        int Ate = vTexto.ToUpper().IndexOf("ATE");
                        if (Ate == -1)
                            Ate = vTexto.ToUpper().IndexOf("ATÉ");
                        if (Ate == -1)
                            return null;

                        if (vTexto.Length < Ate + 3)
                            return null;

                        ClassDateTimeIntervalo ClassDateTimeIntervalo = new ClassDateTimeIntervalo();

                        if (!DateTime.TryParse(vTexto.Substring(0,Ate).Trim(), out ClassDateTimeIntervalo.DataIni))
                            return null;

                        if (!DateTime.TryParse(vTexto.Substring(Ate + 3, vTexto.Length - Ate-3).Trim(), out ClassDateTimeIntervalo.DataFim))
                            return null;
                        return ClassDateTimeIntervalo;

                    }
                    else
                        return null;
                }
                catch { return null; }
            }
        }

        public class ClassTimeSpanIntervalo
        {
            public TimeSpan Inicio;
            public TimeSpan Fim;

            public static ClassTimeSpanIntervalo TryParse(string vTexto)
            {
                try
                {
                    if (!string.IsNullOrEmpty(vTexto))
                    {

                        var vTmp = vTexto.Trim().Replace("  ", " ").Split(' ');
                        if (vTmp.Length == 3)
                        {
                            if (vTmp[1].ToUpper().Trim() != "ATÉ" && vTmp[1].ToUpper().Trim() == "ATE")
                                return null;

                            ClassTimeSpanIntervalo ClassTimeSpanIntervalo = new ClassTimeSpanIntervalo();

                            if (!TimeSpan.TryParse(vTmp[0], out ClassTimeSpanIntervalo.Inicio))
                                return null;

                            if (!TimeSpan.TryParse(vTmp[2], out ClassTimeSpanIntervalo.Fim))
                                return null;
                            return ClassTimeSpanIntervalo;
                        }
                        else
                            return null;
                    }
                    else
                        return null;
                }
                catch { return null; }
            }
        }

        #endregion

        #region Ara2Dev
        private string _Name = "";

        [AraDevProperty]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        #endregion
    }
}
