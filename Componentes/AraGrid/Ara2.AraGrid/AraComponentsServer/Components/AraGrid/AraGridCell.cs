// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ara2.Components
{
    [Serializable]
    public class AraGridCell
    {

        private AraGrid  _AraGrid;
        private AraGridCol _Col;
        private AraGridRow _Row;
        

        public AraGridCell(AraGrid vAraGrid,AraGridCol vCol, AraGridRow vRow) :
            this(vAraGrid,vCol, vRow, "")
        {
        }

        public AraGridCell(AraGrid vAraGrid,AraGridCol vCol, AraGridRow vRow, string vText)
        {
            _AraGrid = vAraGrid;
            _Col = vCol;
            _Row = vRow;

            if (_Col.FormatterServerType != null)
                _Text = GetFormatText (vText);
            else
                _Text = vText;
        }

        private string GetFormatText(string vTmpText)
        {
            return FormatText(_Col.FormatterServerType, _Col.FormatterServerText, vTmpText);
        }

        public static string FormatText(AraGridCol.EnumFormatterServerType? vType, string FormatterServerText, string vTmpText)
        {
            switch (vType)
            {
                case AraGridCol.EnumFormatterServerType.Date:
                case AraGridCol.EnumFormatterServerType.DateTime:
                    if (AraTools.IsDate(vTmpText))
                        return string.Format("{0:" + FormatterServerText + "}", DateTime.Parse(vTmpText));
                    else
                        return "";
                    break;
                case AraGridCol.EnumFormatterServerType.Int:
                    if (AraTools.IsInt(vTmpText))
                        return string.Format("{0:" + FormatterServerText + "}", Convert.ToInt32(vTmpText));
                    else
                        return "";
                    break;
                case AraGridCol.EnumFormatterServerType.Decimal:
                    if (AraTools.IsDecimal(vTmpText))
                        return string.Format("{0:" + FormatterServerText + "}", Convert.ToDecimal(vTmpText));
                    else
                        return "";
                    break;
                case AraGridCol.EnumFormatterServerType.Bool:
                    if (AraTools.IsBool(vTmpText))
                        return string.Format("{0:" + FormatterServerText + "}", Convert.ToBoolean(vTmpText));
                    else
                        return "";
                    break;
                default:
                    return string.Format("{0:" + FormatterServerText + "}", vTmpText);
            }
        }

        private string _Text = "";
        private string _TextAntScript = "";
        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                _TextAntScript = _Text;

                if (_AraGrid.Tree != null)
                    if (_AraGrid.Tree.ColId == _Col || _AraGrid.Tree.ColFather == _Col)
                        throw new Exception("Tree Grid -> you can not change or ColId ColFather");

                if (_Col.FormatterServerType != null)
                    _Text = GetFormatText(value);
                else
                    _Text = value;
                RumColFormatText();
                //Row.Commit();

                string TmpText = GetTextTratado(_Text);

                if (_TextAntScript != TmpText)
                {
                    _TextAntScript = TmpText;

                    _AraGrid.TickScriptCall();
                    Tick.GetTick().Script.Send(" vObj.SetTextCell('" + AraTools.StringToStringJS(_Row.ID) + "','" + AraTools.StringToStringJS(_Col.NameReal) + "','" + AraTools.StringToStringJS(TmpText) + "'); \n");
                }
            }
        }

        private string GetColFormatText(string vTmpText)
        {
            return this.Col.RumCellFormatText(this, vTmpText);
        }

        public void RumColFormatText()
        {
            _Text = GetColFormatText(_Text);
        }

        public string ToString()
        {
            return Text;
        }

        private bool[] _ClickReturn= new bool[]{false,true,true,true};
        public bool[] ClickReturn
        {
            get 
            {
                return _ClickReturn;
            }
            set 
            {
                string vTmpValue = "[";
                foreach (bool vTmp in _ClickReturn)
                {
                    vTmpValue += (vTmp == true ? "true" : "false") + ",";
                }
                vTmpValue = vTmpValue.Substring(0, vTmpValue.Length - 1);
                vTmpValue += "]";

                Tick.GetTick().Script.Send(" vObj.Event_ClickItem_Return['" + AraTools.StringToStringJS(_Row.ID) + "']['" + AraTools.StringToStringJS(_Col.NameReal) + "'] = " + vTmpValue + "; \n");
            }
        }
        

        public AraGridCol Col
        {
            get { return _Col; }
        }
        public AraGridRow Row
        {
            get { return _Row; }
        }

        public void SetTextinternal(string vtext)
        {
            if (_Col.FormatterServerType != null)
                _Text = GetFormatText(vtext);
            else
                _Text = vtext;
            _Text = GetColFormatText(_Text);
        }

        private bool PrimeiroGetScript = true;


        private string GetTextTratado(string vTmp_Text)
        {

            if (Col.edittype == AraGridCol.enum_edittype.select)
            {
                if (vTmp_Text == null) vTmp_Text = "";
                if (Col.Select(vTmp_Text) != null)
                    vTmp_Text = Col.Select(vTmp_Text);
            }

            if (_AraGrid.Tree != null)
            {
                if (_AraGrid.Tree.ColCaption == _Col)
                {

                    string LevelTypesString="";

                    if (_Row.TreeLevel > 0)
                    {
                        for (int vL = 0; vL < _Row.TreeLevel; vL++)
                            LevelTypesString += Convert.ToString((int)_Row.TypeLineByLevel[vL]) + ",";
                        LevelTypesString = LevelTypesString.Substring(0, LevelTypesString.Length - 1);
                    }


                    string StringExpand = "{";
                    StringExpand += " Container: " + (_Row.TreeContainer == true ? "true" : "false") + ",";
                    StringExpand += " level: " + _Row.TreeLevel + ",";
                    StringExpand += " LType: [" + LevelTypesString + "]";
                    StringExpand += "}";

                    vTmp_Text = StringExpand + "<!--END-->" + vTmp_Text;
                }
            }

            return vTmp_Text;
        }


        public string GetScript()
        {
            return "'" + AraTools.StringToStringJS(Col.NameReal) + "':'" + AraTools.StringToStringJS(GetTextTratado(_Text)) + "'";
        }

        public void SetFocus()
        {
            _AraGrid.TickScriptCall();
            Tick.GetTick().Script.Send(" vObj.CellFocus('" + AraTools.StringToStringJS(_Row.ID) + "','" + AraTools.StringToStringJS(_Col.NameReal) + "');\n");
        }

      

        private bool _Visible = true;
        public bool Visible
        {
            get { return _Visible; }
            set
            {
                _Visible = value;
                _AraGrid.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetCellVisible('" + AraTools.StringToStringJS(_Row.ID) + "','" + AraTools.StringToStringJS(_Col.NameReal) + "'," + (_Visible ? "true" : "false") + ");\n");
            }
        }

        private int _ColSpan = 0;
        public int ColSpan
        {
            get { return _ColSpan; }
            set
            {
                _ColSpan = value;
                _AraGrid.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetCellColSpan('" + AraTools.StringToStringJS(_Row.ID) + "','" + AraTools.StringToStringJS(_Col.NameReal) + "'," + _ColSpan + ");\n");
            }
        }

        private int _RowSpan = 0;
        public int RowSpan
        {
            get { return _RowSpan; }
            set
            {
                _RowSpan = value;
                _AraGrid.TickScriptCall();
                Tick.GetTick().Script.Send(" vObj.SetCellRowSpan('" + AraTools.StringToStringJS(_Row.ID) + "','" + AraTools.StringToStringJS(_Col.NameReal) + "'," + _RowSpan + ");\n");
            }
        }
    }

}
