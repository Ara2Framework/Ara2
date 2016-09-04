// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ara2.Components
{
    [Serializable]
    public class AraGridCol:AraObject
    {
        AraGrid Grid;

        public AraGridCol(AraGrid vGrid):
            base(Tick.GetTick().Session.GetNewID(),vGrid)
        {
            Grid = vGrid;
        }

        
        public AraGridCol(AraGrid vGrid,string vCaption, string vName):
            this(vGrid)
        {
            Caption = vCaption;
            _InstanceID = vName;
            Index = vName;
        }

        public AraGridCol(AraGrid vGrid,string vCaption, string vName, bool vEditable):
            this(vGrid,vCaption, vName)
        {
            editable = vEditable;
        }

        public AraGridCol(AraGrid vGrid, string vCaption, string vName, bool vEditable = false, bool vHidden = false) :
            this(vGrid,vCaption, vName, vEditable)
        {
            hidden = vHidden;
        }

        [Obsolete]
        public AraGridCol(AraGrid vGrid, string vCaption, string vName, int vwidth) :
            this(vGrid,vCaption, vName)
        {   
            _width = vwidth;
        }

        [Obsolete]
        public AraGridCol(AraGrid vGrid, string vCaption, string vName, int vwidth, bool vEditable) :
            this(vGrid,vCaption, vName, vwidth)
        {
            editable = vEditable;
        }

        public string Caption = "";
        private string _InstanceID = "";

        public string Name
        {
            get { return _InstanceID; }
        }

        internal string NameReal
        {
            get { return AraTools.StringToIDJS(_InstanceID); }
        }

        

        public string Index = "";
        
        public enum_sorttype sorttype = enum_sorttype.Text;
        public enum_align align = enum_align.left;
        public bool sortable = true;
        public enum_formatter formatter = enum_formatter.none;
        public bool editable = false;
        public enum_edittype edittype = enum_edittype.none;
        public Hashtable editoptions = new Hashtable();
        public AraGridEditrules editrules = new AraGridEditrules();
        public bool Group = false;
        private bool _hidden = false;

        public bool hidden
        {
            get { return _hidden; }
            set { 
                _hidden = value;
                if (Grid.IsCommit)
                {
                    Grid.TickScriptCall();
                    Tick.GetTick().Script.Send(" vObj.ColHidden('" + AraTools.StringToStringJS(this.NameReal) + "'," + (_hidden== true ? "true" : "false") + "); \n");
                }
            }
        }

        public string FormatterServerText = null;
        public EnumFormatterServerType? FormatterServerType = null;

        public enum EnumFormatterServerType
        {
            Date,DateTime,Decimal,Int,String,Bool
        }

        public string GetScript()
        {
            string vTmp = "{ ";

            vTmp += "name:'" + AraTools.StringToStringJS(NameReal) + "',";
            vTmp += "index:'" + AraTools.StringToStringJS(Index) + "',";
            vTmp += "width:" + _width + ",";
            vTmp += "label:'" + AraTools.StringToStringJS(Caption) + "',";

            vTmp += "hidedlg:" + (hidden ? "true" : "false") + ",";
            vTmp += "hidden:" + (hidden ? "true" : "false") + ",";
            //vTmp += "fixed:" +"true"  + ",";
            

            if (enum_sorttypeToString(sorttype) != "")
                vTmp += "sorttype:'" + enum_sorttypeToString(sorttype) + "',";
            if (enum_alignToString(align) != "")
                vTmp += "align:'" + enum_alignToString(align) + "',";

            if (sortable == false)
                vTmp += "sortable:" + (sortable == true ? "true" : "false") + ",";
            vTmp += "editable:" + (editable == true ? "true" : "false") + ",";

            if (enum_formatterToString(formatter) != "")
                vTmp += "formatter:'" + enum_formatterToString(formatter) + "',";

            if (enum_edittypeToString(edittype) != "")
                vTmp += "edittype:'" + enum_edittypeToString(edittype) + "',";

            if (editoptions.Count > 0)
            {
                vTmp += "editoptions:{";
                foreach (string key in editoptions.Keys)
                {
                    vTmp += "'" + key + "':" + editoptions[key].ToString() + ",";
                }
                vTmp = vTmp.Substring(0, vTmp.Length - 1);
                vTmp += "},";
            }

            vTmp += "editrules:" + editrules.GetScript() + ",";

            
            vTmp = vTmp.Substring(0, vTmp.Length - 1);
            vTmp += " }";

            return vTmp;
        }



        public enum enum_sorttype
        {
            Text, Int, Date, Float
        }

        private string enum_sorttypeToString(enum_sorttype vTmp)
        {
            switch (vTmp)
            {
                case enum_sorttype.Date:
                    return "date";
                case enum_sorttype.Float:
                    return "float";
                case enum_sorttype.Int:
                    return "int";
            }
            return "";
        }

        public enum enum_align
        {
            right, left, center
        }

        private string enum_alignToString(enum_align vTmp)
        {
            switch (vTmp)
            {
                case enum_align.right:
                    return "right";
                case enum_align.center:
                    return "center";
            }
            return "";
        }

        public enum enum_formatter
        {
            none, currency, number, email, strongFmatter
        }

        private string enum_formatterToString(enum_formatter vTmp)
        {
            switch (vTmp)
            {
                case enum_formatter.currency:
                    return "currency";
                case enum_formatter.number:
                    return "number";
                case enum_formatter.email:
                    return "email";
                case enum_formatter.strongFmatter:
                    return "strongFmatter";
            }
            return "";
        }

        public enum enum_edittype
        {
            none, text, textarea, select, checkbox, password, button, image, file
        }

        private string enum_edittypeToString(enum_edittype vTmp)
        {
            switch (vTmp)
            {
                case enum_edittype.text:
                    return "";
                case enum_edittype.textarea:
                    return "textarea";
                case enum_edittype.select:
                    return "select";
                case enum_edittype.checkbox:
                    return "checkbox";
                case enum_edittype.password:
                    return "password";
                case enum_edittype.button:
                    return "button";
                case enum_edittype.image:
                    return "image";
                case enum_edittype.file:
                    return "file";

            }
            return "";
        }

        private Dictionary<string, string> _SelectAdd = new Dictionary<string, string>();
        public void SelectAdd(string vKey, string vCaption)
        {
            _SelectAdd[vKey] = vCaption;

            string vScript = "{";
            int n = 1;
            foreach (string vKey2 in _SelectAdd.Keys)
            {
                //vScript += "'" + AraTools.StringToStringJS( vKey2) + "':'" + AraTools.StringToStringJS(_SelectAdd[vKey2]) + "',";
                vScript += "'" + AraTools.StringToStringJS(vKey2) + "':'" + AraTools.StringToStringJS(_SelectAdd[vKey2]) + "',";
                n++;
            }
            vScript = vScript.Substring(0, vScript.Length - 1);
            vScript += "}";

            //editoptions["value"] += (editoptions["value"]==null?"":";") + vKey + ":" + vCaption;
            editoptions["value"] = vScript;
        }

        public Dictionary<string, string>.KeyCollection SelectKeys
        {
            get
            {
                return _SelectAdd.Keys;
            }
        }

        public string Select(string vKey)
        {
            try
            {
                return _SelectAdd[vKey];
            }
            catch { return null; }
        }

        private int _width = 0;
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;

                if (Grid.IsCommit)
                {
                    Grid.TickScriptCall();
                    Tick.GetTick().Script.Send(" vObj.SetColWidth('" + AraTools.StringToStringJS(NameReal) + "'," + _width + "); \n");
                }
            }
        }

        public int Pos
        {
            get
            {
                return Grid.Cols.GetPosCol(this);
            }
        }

        public delegate string DCellFormatText(AraGridCell Cell, string vText);
        public AraEvent<DCellFormatText> CellFormatText = new AraEvent<DCellFormatText>();

        public string RumCellFormatText(AraGridCell Cell, string vText)
        {
            if (CellFormatText.InvokeEvent !=null)
                return CellFormatText.InvokeEvent(Cell, vText);
            else
                return vText;
        }

    }


}
