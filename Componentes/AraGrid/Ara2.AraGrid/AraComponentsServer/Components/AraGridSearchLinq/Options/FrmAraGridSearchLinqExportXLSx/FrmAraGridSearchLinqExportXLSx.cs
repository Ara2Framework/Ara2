using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ara2.Components;
using Ara2;
using System.Collections;
using System.Data.OleDb;
using System.Text;
using System.IO;
using System.Threading;
using OfficeOpenXml;
using System.Linq.Dynamic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Ara2.Grid.Export
{
    [Serializable]
    public class FrmAraGridSearchLinqExportXLSx : AraDesign.FrmAraGridSearchLinqExportXLSxAraDesign
    {
        #region Timer
        private AraObjectInstance<Ara2.Components.AraTimer> _Time = new AraObjectInstance<Ara2.Components.AraTimer>();
        public Ara2.Components.AraTimer Time
        {
            get { return _Time.Object; }
            set { _Time.Object = value; }
        }
        #endregion

        public FrmAraGridSearchLinqExportXLSx(IAraContainerClient ConteinerFather, Func<IQueryable<object>> vGetQuery)
            : base(ConteinerFather)
        {
            GetQuery += vGetQuery;
            lEstadoExportacao.ScrollBar = new AraScrollBar(lEstadoExportacao);

            Time = new AraTimer(Time);
            Time.Interval = 2000;
            Time.tick += Time_tick;
            Time.Enabled = false;

            AraTools.AsynchronousFunction(Exportar);
        }

        AraEvent<Func<IQueryable<object>>> GetQuery = new AraEvent<Func<IQueryable<object>>>();

        public override void bCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Exportar()
        {
            if (!Directory.Exists(Path.Combine(AraTools.GetPath(), "Tmp")))
                Directory.CreateDirectory(Path.Combine(AraTools.GetPath(), "Tmp"));

            LimpaLixoTmpDonload();


            string vFileTmp = "";
            string vFileTmpName = "";
            do
            {
                Thread.Sleep(1);
                vFileTmpName = "TmpAraGS_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + (new Random()).Next(10000) + ".xlsx";
                vFileTmp = Path.Combine(AraTools.GetPath(), "Tmp", vFileTmpName);
            } while (File.Exists(vFileTmp));


            GeraArquivoCom = new CGeraArquivoCom()
            {
                File = vFileTmp,
                Url = "Tmp/" + vFileTmpName
            };
            GeraArquivoCom.GetQuery += GetQuery.InvokeEvent;

            Time.Enabled = true;

            lEstadoExportacao.Text = "<center>Iniciando Exportação</center>";

            AraTools.AsynchronousFunction(GeraArquivo);
        }

        private void LimpaLixoTmpDonload()
        {
            string vPasta = Path.Combine(AraTools.GetPath(), "Tmp");

            foreach (FileInfo File in (new DirectoryInfo(vPasta)).GetFiles().Where(a => (DateTime.Now - a.LastWriteTime).TotalMinutes > 10))
            {
                try
                {
                    File.Delete();
                }
                catch { }
            }
        }

        private void Time_tick()
        {
            if (GeraArquivoCom != null)
            {
                if (GeraArquivoCom.Erro != null)
                    lEstadoExportacao.Text = "<font color=red>Erro: </font>" + GeraArquivoCom.Erro;
                else
                    lEstadoExportacao.Text = "<center>" + GeraArquivoCom.Estado + "</center>";

                if (GeraArquivoCom.Fim)
                    FimProcessoParalelo();
            }
            else
                FimProcessoParalelo();

        }

        private void FimProcessoParalelo()
        {
            Time.Enabled = false;
            GeraArquivoCom = null;
        }

        CGeraArquivoCom GeraArquivoCom = null;

        private class CGeraArquivoCom
        {
            public string File;
            public string Url;
            public string Estado = "";
            public string Erro = null;
            public bool Fim = false;
            public bool Sucesso { get { return Erro == null; } }
            public Func<IQueryable<object>> GetQuery;
        }

        

        private void GeraArquivo()
        {
            ExcelPackage Ex;
            ExcelWorksheet Worksheets;

            GeraArquivoCom.Estado = "Carregando.";

            try
            {
                Ex = new ExcelPackage(new System.IO.FileInfo(GeraArquivoCom.File));
            }
            catch (Exception err)
            {
                GeraArquivoCom.Erro = "Erro ao abrir '" + GeraArquivoCom.File + "'.\nFormato invalido, use xls ou xlsx.\n" + err.Message;
                GeraArquivoCom.Fim = true;
                return;
            }

            try
            {
                Worksheets = Ex.Workbook.Worksheets.Add("Datas");
            }
            catch (Exception err)
            {
                GeraArquivoCom.Erro = "Erro ao abrir a primeira 'Aba' da planilha.\n" + err.Message;
                GeraArquivoCom.Fim = true;
                return;
            }

            IQueryable<object> Query = GeraArquivoCom.GetQuery();

            int NRowMax = 0;
            try
            {
                GeraArquivoCom.Estado = "Contando dados.";
                NRowMax = Query.Count();
            }
            catch (Exception err)
            {
                GeraArquivoCom.Erro = "Ao contar numero de registros.\n" + err.Message;
                GeraArquivoCom.Fim = true;
                return;
            }

            try
            {
                int NRow = 1;
                int PorcentagemOld = 0;
                SColuna[] Colunas = null;

                Regex regex = new Regex(@"<[^>]+>|&nbsp;");

                foreach (var vLine in Query)
                {
                    #region CarregaColunas
                    if (Colunas == null)
                    {
                        List<SColuna> TmpColunas = new List<SColuna>();
                        int NColuna = 1;
                        foreach (System.Reflection.PropertyInfo Pro in vLine.GetType().GetProperties())
                        {
                            TmpColunas.Add(new SColuna
                            {
                                vType = Pro.PropertyType,
                                Pro = Pro,
                                Name = Pro.Name,
                                Alias = (Pro.GetCustomAttributes(typeof(AraFieldAlias), true).FirstOrDefault() != null ? ((AraFieldAlias)Pro.GetCustomAttributes(typeof(AraFieldAlias), true).FirstOrDefault()).Alias : ""),
                                NColuna = NColuna,
                                Format = Pro.GetCustomAttributes(typeof(IAraFieldFormat), true).Select(a => (IAraFieldFormat)a).OrderBy(a => a.Ordem).ToArray()
                            });
                            NColuna++;
                        }


                        foreach (System.Reflection.FieldInfo Field in vLine.GetType().GetFields())
                        {
                            TmpColunas.Add(new SColuna
                            {
                                vType = Field.FieldType,
                                Fild = Field,
                                Name = Field.Name,
                                Alias = (Field.GetCustomAttributes(typeof(AraFieldAlias), true).FirstOrDefault() != null ? ((AraFieldAlias)Field.GetCustomAttributes(typeof(AraFieldAlias), true).FirstOrDefault()).Alias : ""),
                                NColuna = NColuna,
                                Format = Field.GetCustomAttributes(typeof(IAraFieldFormat), true).Select(a => (IAraFieldFormat)a).OrderBy(a => a.Ordem).ToArray()
                            });
                            NColuna++;
                        }

                        Colunas = TmpColunas.ToArray();

                        foreach (var vColuna in Colunas)
                        {
                            Worksheets.Cells[NRow, vColuna.NColuna].Value = (!string.IsNullOrEmpty(vColuna.Alias)?vColuna.Alias:vColuna.Name);
                        }

                        NRow++;
                    }
                    #endregion

                    try
                    {

                        int Porcentagem = Convert.ToInt32((Convert.ToDecimal(NRow) / Convert.ToDecimal(NRowMax) * 100));
                        if (PorcentagemOld != Porcentagem)
                        {
                            if (Porcentagem > 100) 
                                Porcentagem = 100;
                            else if (Porcentagem < 0) 
                                Porcentagem = 0;
                            GeraArquivoCom.Estado = "Exportando " + Porcentagem + "%";
                            PorcentagemOld = Porcentagem;
                        }

                        foreach (var vColuna in Colunas)
                        {
                            object vValue;
                            if (vColuna.Pro != null)
                                vValue = vColuna.Pro.GetValue(vLine, null);
                            else
                                vValue = vColuna.Fild.GetValue(vLine);

                            if (vColuna.Format != null && vColuna.Format.Count() > 0)
                            {
                                foreach (IAraFieldFormat vTmpFormat in vColuna.Format)
                                    vValue = vTmpFormat.ToString(vLine, vValue);
                            }

                            Worksheets.Cells[NRow, vColuna.NColuna].Value = (vValue == null ? "" : regex.Replace(vValue.ToString(),""));
                        }
                    }
                    catch (Exception err)
                    {
                        GeraArquivoCom.Erro = "Erro na linha " + NRow + ".\n" + err.Message;
                        GeraArquivoCom.Fim = true;
                        return;
                    }

                    NRow++;
                }


                foreach (var vColuna in Colunas)
                {
                    try
                    {
                        Worksheets.Column(vColuna.NColuna).AutoFit();
                    }
                    catch { }
                }

                try
                {
                    Ex.Save();
                }
                catch (Exception err)
                {
                    GeraArquivoCom.Erro = "Ao salvar xlsx.\n" + err.Message;
                    GeraArquivoCom.Fim = true;
                    return;
                }

                GeraArquivoCom.Estado = "Relatório Gerado !<br> <a href='" + GeraArquivoCom.Url + "' target='_blank'>Clique aqui para salvar</a>";
            }
            catch (Exception err)
            {
                GeraArquivoCom.Erro = err.Message;
                return;
            }
            finally
            {
                GeraArquivoCom.Fim = true;
            }


        }


        private class SColuna
        {
            public Type vType;
            public string Name;
            public string Alias;
            public int NColuna;
            public System.Reflection.PropertyInfo Pro = null;
            public System.Reflection.FieldInfo Fild = null;
            public IAraFieldFormat[] Format;
        }


    }
}