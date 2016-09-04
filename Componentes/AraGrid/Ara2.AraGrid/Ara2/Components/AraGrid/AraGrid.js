// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
Ara.AraClass.Add('AraGrid', function (vAppId, vId, ConteinerFather) {


    // Eventos  ---------------------------------------
    this.Events = {};

    var TmpThis = this;

    this.Events.Click =
    {
        Enabled: false,
        SetEnabled: function (vValue) {
            var TmpThis2 = this;
            if (vValue && this.PrimeiraAtivacaoEvento != true) {
                $(TmpThis.Obj).bind('contextmenu click', function (e) { TmpThis2.Function(e); return false; });
                this.PrimeiraAtivacaoEvento = true;
            }

            this.Enabled = vValue;
        },
        ThreadType: 2, // Single_thread
        Function: function (event) {
            if (this.Enabled) {

                var vParans = {
                    Mouse_which: event.which,
                    Mouse_layerX: event.layerX,
                    Mouse_layerY: event.layerY,
                    Mouse_clientX: event.clientX,
                    Mouse_clientY: event.clientY,
                    Mouse_offsetX: event.offsetX,
                    Mouse_offsetY: event.offsetY,
                    Mouse_pageX: event.pageX,
                    Mouse_pageY: event.pageY,
                    Mouse_screenX: event.screenX,
                    Mouse_screenY: event.screenY,
                    Mouse_x: event.x,
                    Mouse_y: event.y,
                    Mouse_altKey: event.altKey,
                    Mouse_ctrlKey: event.ctrlKey,
                    Mouse_shiftKey: event.shiftKey
                };

                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "Click", vParans);
            }
        }
    };

    this.Events.WidthChangeAfter =
    {
        Enabled: false,
        ThreadType: 1, // Multiple_thread
        Function: function () {
            if (this.Enabled) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "WidthChangeAfter", null);
            }
        }
    };

    this.Events.HeightChangeAfter =
    {
        Enabled: false,
        ThreadType: 1, // Multiple_thread
        Function: function () {
            if (this.Enabled) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "HeightChangeAfter", null);
            }
        }
    };

    this.Events.KeyDown =
    {
        Enabled: false,
        ThreadType: 1, // Single_thread
        Ignore: [],
        Only: [],
        ReturnTrue: [],
        ReturnFalse: [],
        Function: function (e) {
            var vkeyCode = 0;
            if (e.keyCode) vkeyCode = e.keyCode;
            else if (e.which) vkeyCode = e.which;
            else if (e.charCode) vkeyCode = e.charCode;

            var IgnoreEvent = false;


            for (n = 0; n < TmpThis.Events.KeyDown.Ignore.length; n++) {
                if (TmpThis.Events.KeyDown.Ignore[n] == vkeyCode) {
                    IgnoreEvent = true;
                    break;
                }
            }

            if (IgnoreEvent == false) {
                if (TmpThis.Events.KeyDown.Only.length > 0) {
                    var TmpAcho = false;
                    for (n = 0; n < TmpThis.Events.KeyDown.Only.length; n++) {
                        if (TmpThis.Events.KeyDown.Only[n] == vkeyCode) {
                            TmpAcho = true;
                            break;
                        }
                    }
                    if (TmpAcho == false) {
                        IgnoreEvent = true;
                    }
                }


                if (!IgnoreEvent) {
                    if (this.Enabled) {
                        Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "KeyDown", { KeyDown: vkeyCode });
                    }
                }
            }



            for (n = 0; n < TmpThis.Events.KeyDown.ReturnTrue.length; n++) {
                if (TmpThis.Events.KeyDown.ReturnTrue[n] == vkeyCode)
                    return true;
            }

            for (n = 0; n < TmpThis.Events.KeyDown.ReturnFalse.length; n++) {
                if (TmpThis.Events.KeyDown.ReturnFalse[n] == vkeyCode) {
                    if (e.returnValue) {
                        e.returnValue = false;
                        e.cancelBubble = true;
                    }
                    if (e.preventDefault) {
                        e.preventDefault();
                        e.stopPropagation();
                    }
                    return false;
                }
            }
        }
    };

    this.Events.KeyUp =
    {
        Enabled: false,
        ThreadType: 1, // Single_thread
        Ignore: [],
        Only: [],
        ReturnTrue: [],
        ReturnFalse: [],
        Function: function (e) {
            var vkeyCode = 0;
            if (e.keyCode) vkeyCode = e.keyCode;
            else if (e.which) vkeyCode = e.which;
            else if (e.charCode) vkeyCode = e.charCode;

            var IgnoreEvent = false;


            for (n = 0; n < TmpThis.Events.KeyUp.Ignore.length; n++) {
                if (TmpThis.Events.KeyUp.Ignore[n] == vkeyCode) {
                    IgnoreEvent = true;
                    break;
                }
            }

            if (IgnoreEvent == false) {
                if (TmpThis.Events.KeyUp.Only.length > 0) {
                    var TmpAcho = false;
                    for (n = 0; n < TmpThis.Events.KeyUp.Only.length; n++) {
                        if (TmpThis.Events.KeyUp.Only[n] == vkeyCode) {
                            TmpAcho = true;
                            break;
                        }
                    }
                    if (TmpAcho == false) {
                        IgnoreEvent = true;
                    }
                }


                if (!IgnoreEvent) {
                    if (this.Enabled) {
                        Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "KeyUp", { KeyUp: vkeyCode });
                    }
                }
            }



            for (n = 0; n < TmpThis.Events.KeyUp.ReturnTrue.length; n++) {
                if (TmpThis.Events.KeyUp.ReturnTrue[n] == vkeyCode)
                    return true;
            }

            for (n = 0; n < TmpThis.Events.KeyUp.ReturnFalse.length; n++) {
                if (TmpThis.Events.KeyUp.ReturnFalse[n] == vkeyCode) {
                    if (e.returnValue) {
                        e.returnValue = false;
                        e.cancelBubble = true;
                    }
                    if (e.preventDefault) {
                        e.preventDefault();
                        e.stopPropagation();
                    }
                    return false;
                }
            }
        }
    };


    this.Events.KeyPress =
    {
        Enabled: false,
        ThreadType: 1, // Single_thread
        Ignore: [],
        Only: [],
        ReturnTrue: [],
        ReturnFalse: [],
        Function: function (e) {
            var vkeyCode = 0;
            if (e.keyCode) vkeyCode = e.keyCode;
            else if (e.which) vkeyCode = e.which;
            else if (e.charCode) vkeyCode = e.charCode;

            var IgnoreEvent = false;


            for (n = 0; n < TmpThis.Events.KeyPress.Ignore.length; n++) {
                if (TmpThis.Events.KeyPress.Ignore[n] == vkeyCode) {
                    IgnoreEvent = true;
                    break;
                }
            }

            if (IgnoreEvent == false) {
                if (TmpThis.Events.KeyPress.Only.length > 0) {
                    var TmpAcho = false;
                    for (n = 0; n < TmpThis.Events.KeyPress.Only.length; n++) {
                        if (TmpThis.Events.KeyPress.Only[n] == vkeyCode) {
                            TmpAcho = true;
                            break;
                        }
                    }
                    if (TmpAcho == false) {
                        IgnoreEvent = true;
                    }
                }


                if (!IgnoreEvent) {
                    if (this.Enabled) {
                        Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "KeyPress", { KeyPress: vkeyCode });
                    }
                }
            }



            for (n = 0; n < TmpThis.Events.KeyPress.ReturnTrue.length; n++) {
                if (TmpThis.Events.KeyPress.ReturnTrue[n] == vkeyCode)
                    return true;
            }

            for (n = 0; n < TmpThis.Events.KeyPress.ReturnFalse.length; n++) {
                if (TmpThis.Events.KeyPress.ReturnFalse[n] == vkeyCode) {
                    if (e.returnValue) {
                        e.returnValue = false;
                        e.cancelBubble = true;
                    }
                    if (e.preventDefault) {
                        e.preventDefault();
                        e.stopPropagation();
                    }
                    return false;
                }
            }
        }
    };

    this.Events.ClickCell =
    {
        Enabled: false,
        ThreadType: 2, // Single_thread
        Function: function (event) {
            if (this.Enabled) {
                var vParans = {
                    Mouse_which: event.which,
                    Mouse_layerX: event.layerX,
                    Mouse_layerY: event.layerY,
                    Mouse_clientX: event.clientX,
                    Mouse_clientY: event.clientY,
                    Mouse_offsetX: event.offsetX,
                    Mouse_offsetY: event.offsetY,
                    Mouse_pageX: event.pageX,
                    Mouse_pageY: event.pageY,
                    Mouse_screenX: event.screenX,
                    Mouse_screenY: event.screenY,
                    Mouse_x: event.x,
                    Mouse_y: event.y,
                    Mouse_altKey: event.altKey,
                    Mouse_ctrlKey: event.ctrlKey,
                    Mouse_shiftKey: event.shiftKey
                };
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "ClickCell", vParans);
            }
        }
    };

    this.Events.ClickDblCell =
    {
        Enabled: false,
        ThreadType: 2, // Single_thread
        Function: function (event) {
            if (this.Enabled) {
                var vParans = {
                    Mouse_which: event.which,
                    Mouse_layerX: event.layerX,
                    Mouse_layerY: event.layerY,
                    Mouse_clientX: event.clientX,
                    Mouse_clientY: event.clientY,
                    Mouse_offsetX: event.offsetX,
                    Mouse_offsetY: event.offsetY,
                    Mouse_pageX: event.pageX,
                    Mouse_pageY: event.pageY,
                    Mouse_screenX: event.screenX,
                    Mouse_screenY: event.screenY,
                    Mouse_x: event.x,
                    Mouse_y: event.y,
                    Mouse_altKey: event.altKey,
                    Mouse_ctrlKey: event.ctrlKey,
                    Mouse_shiftKey: event.shiftKey
                };
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "ClickDblCell", vParans);
            }
        }
    };

    this.Events.SelectCell =
    {
        Enabled: false,
        ThreadType: 2, // Single_thread
        Function: function () {
            if (this.Enabled) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "SelectCell", null);
            }
        }
    };

    this.Events.SelectRow =
    {
        Enabled: false,
        ThreadType: 2, // Single_thread
        Function: function (rowid) {
            if (TmpThis.setSelection_Maquina == false) {
                if (this.Enabled) {
                    Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "SelectRow", { RowID: rowid });
                }
            }
        }
    };

    this.Events.ChangeCell =
    {
        Enabled: false,
        ThreadType: 2, // Single_thread
        Function: function (rowid, colid, value) {
            if (this.Enabled) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "ChangeCell", { row: rowid, col: colid, value: value });
            }
        }
    };


    this.Events.ColOrderby =
    {
        Enabled: false,
        ThreadType: 2, // Single_thread
        Function: function (col, Sense) {
            if (this.Enabled) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "ColOrderby", { col: col, Sense: Sense });
            }
        }
    };

    // Eventos Fim ---------------------------------------------------------------


    this.GetRowSelect = function () {
        return JSON.stringify($(this.Obj).getGridParam('selarrrow'));
    }


    this.GetChangesInCell = function () {

        //for (var n in this.CellEditTrue) {
        //    if (n != "remove") {
        //        var rowid = this.CellEditTrue[n].rowid;
        //        var colid = this.CellEditTrue[n].colid;
        //        var Value = this.CellEditTrue_Value[rowid + "_" + colid];
        //        Tmp.push({ "rowid": rowid, "colid": colid, "value": Value });
        //    }
        //}

        var Tmp = Array();
        var vTmpThis = this;

        $(this.CellEditTrue).each(function () {
            if (this.rowid) {
                Tmp.push({ "rowid": this.rowid, "colid": this.colid, "value": vTmpThis.CellEditTrue_Value[this.rowid + "_" + this.colid] });
            }
        });


        return JSON.stringify(Tmp);
    }

    this.WarningOfReceiptOfCell = function (vrowid, vcolid, vValue) {
        for (var n in this.CellEditTrue) {
            var rowid = this.CellEditTrue[n].rowid;
            var colid = this.CellEditTrue[n].colid;
            var Value = this.CellEditTrue_Value[rowid + "_" + colid];

            if (rowid == vrowid && colid == vcolid && Value == vValue) {
                delete this.CellEditTrue[n];
                delete this.CellEditTrue_Value[rowid + "_" + colid];
                delete this.CellEditTrue_RowCol[rowid + "_" + colid];
                return;
            }
        }
    }

    //	this.JaSetGridWidth = false;
    //	this.JaSetGridHeight = false;
    this.IsCreate = false;

    this.Create = function (vCommandCreate) {

        var TmpThis = this;
        this.multiselect = vCommandCreate.multiselect;

        var TmpThis = this;
        if (vCommandCreate.colModel) {
            for (n = 0; n < vCommandCreate.colModel.length; n++) {
                TmpThis = this;

                if (!vCommandCreate.colModel[n].editoptions)
                    vCommandCreate.colModel[n].editoptions = {};

                vCommandCreate.colModel[n].editoptions.dataEvents = [
                {
                    type: 'keydown',
                    fn: function (e) {
                        TmpThis.Events.KeyDown.Function(e);
                    }
                },
                {
                    type: 'keypress',
                    fn: function (e) {
                        TmpThis.Events.KeyPress.Function(e);
                    }
                },
                {
                    type: 'keyup',
                    fn: function (e) {
                        TmpThis.Events.KeyUp.Function(e);
                    }
                }
                ];
                /*
	            vCommandCreate.colModel[n].editoptions.custom_element = function (value, options) {
	            TmpThis.custom_element(value, options);
	            };
	            vCommandCreate.colModel[n].editoptions.custom_value = function (elem, operation, value) {
	            TmpThis.custom_value(elem, operation, value);
	            };
	            */

            }
        }
        vCommandCreate.onSelectRow = function (id) {
            TmpThis.onSelectRow(id);
        };
        vCommandCreate.onSelectAll = function (vIds) {
            for (var vTmpIdN in vIds) {
                TmpThis.onSelectRow(vIds[vTmpIdN]);
            }
        };


        vCommandCreate.onRightClickRow = function (rowid, iRow, iCol, e) {
            if (e.which == 0) {
                e.which = 3;
            }
            TmpThis.beforeSelectRow(rowid, e);

            e.preventDefault();
        };


        vCommandCreate.ondblClickRow = function (rowid, iRow, iCol, e) {
            TmpThis.ondblClickRow(rowid, iRow, iCol, e);
        };


        vCommandCreate.beforeSelectRow = function (rowid, e) {
            if (e.which == 0) {
                e.which = 1;
            }
            TmpThis.beforeSelectRow(rowid, e);
        };

        vCommandCreate.afterEditCell = function (rowid, cellname, value, iRow, iCol) {
            TmpThis.afterEditCell(rowid, cellname, value, iRow, iCol);
        };


        vCommandCreate.beforeSaveCell = function (rowid, celname, value, iRow, iCol) {
            TmpThis.beforeSaveCell(rowid, celname, value, iRow, iCol);
        };

        vCommandCreate.onCellSelect = function (rowid, iCol, cellcontent, e) {
            TmpThis.onCellSelect(rowid, iCol, cellcontent, e);
        };

        vCommandCreate.beforeSubmitCell = function (rowid, cellname, value, iRow, iCol) {
            TmpThis.beforeSubmitCell(rowid, cellname, value, iRow, iCol);
        };

        vCommandCreate.formatCell = function (rowid, cellname, value, iRow, iCol) {
            return TmpThis.formatCell(rowid, cellname, value, iRow, iCol);
        };

        vCommandCreate.beforeEditCell = function (rowid, cellname, value, iRow, iCol) {
            TmpThis.beforeEditCell(rowid, cellname, value, iRow, iCol);
        };

        vCommandCreate.onSelectCell = function (rowid, celname, value, iRow, iCol) {
            TmpThis.onSelectCell(rowid, celname, value, iRow, iCol);
        };

        vCommandCreate.afterInsertRow = function (rowid, rowdata, rowelem) {
            TmpThis.afterInsertRow(rowid, rowdata, rowelem);
        };

        vCommandCreate.gridComplete = function () {
            TmpThis.gridComplete();
        };

        vCommandCreate.onSortCol = function (index, iCol, sortorder) {
            return TmpThis.EventAutoOrderCols(iCol, sortorder);
        };

        vCommandCreate.resizeStop = function (newwidth, index) {
            TmpThis.AtualizaScroolBar();
        };

        //vCommandCreate.pager = $(this.Obj)_pager;
        //
        if (this.IsCreate) {
            try {
                $(this.Obj).GridDestroy();
            } catch (e) { }

            $(this.Obj).remove();
            $("<table id='" + this.id + "'></table>").appendTo("#" + this.ConteinerFather);
            this.Obj = document.getElementById(this.id);
        }

        //$(this.Obj)[0].innerHTML = ''
        $(this.Obj).width('');
        $(this.Obj).height('');
        //$(this.Obj).height('');
        //$(this.Obj).css({ position: "" });


        $(this.Obj).jqGrid(vCommandCreate);

        this.ObjDiv = $("#gbox_" + this.id);
        this.ObjDiv.css({ position: "absolute", top: this.Top, left: this.Left });


        this.ObjDivScroolBar = this.Obj.parentNode.parentNode;
        this.ObjDivScroolBar.id = this.id + "_gridscroll";

        this.IsCreate = true;

        // Acerta Larguras
        var TmpW = this.Width;
        var TmpH = this.Height;
        this.Width = null;
        this.Height = null;
        this.SetWidth(TmpW);
        this.SetHeight(TmpH);

        //$(this.ObjDivScroolBar).niceScroll();

        //$(this.ObjDivScroolBar).niceScroll({
        //    cursoropacitymin: 1,
        //    cursorwidth : "20px"
        //});
        //$(this.ObjDivScroolBar).niceScroll({ zindex :99988 });

    }

    this.GetMaxZindexParantNode = function () {
        //
        var MaxIndex = $(this.Obj).zIndex();
        var TmpObjP = this.Obj.parentNode;
        while (TmpObjP != null) {
            if (MaxIndex < $(TmpObjP).zIndex())
                MaxIndex = $(TmpObjP).zIndex();

            TmpObjP = TmpObjP.parentNode;
        }

        return MaxIndex;
    };

    this.EventAutoOrderCols = function (iCol, sortorder) {
        if (!this.AutoOrderCols) {
            var vColId = $(this.Obj).getGridParam('colModel')[iCol].name;

            this.Events.ColOrderby.Function(vColId, (sortorder == "desc" ? 1 : 0));
            return 'stop';
        }
    }

    this.AutoOrderCols = true;
    this.SetAutoOrderCols = function (vValue) {
        this.AutoOrderCols = vValue;
    }

    this.AddRow = function (rowid, vCells, position, srcrowid, vEditRow) {

        this.Event_ClickItem_Return[rowid] = new Array();

        var colModel = $(this.Obj).getGridParam('colModel');
        for (n = 0; n < colModel.length; n++) {
            this.Event_ClickItem_Return[rowid][colModel[n].name] = [false, true, true, true];
        }

        this.PermissionsEditingRow[rowid] = vEditRow;



        if (this.Tree) {
            vCells[this.TreeColCaption] = this.TrataSetText(rowid, this.TreeColCaption, vCells[this.TreeColCaption]);
        }

        $(this.Obj).jqGrid('addRowData', rowid, vCells, position, srcrowid);

        if (this.multiselect) {
            var TmpObjJQuery = $(this.Obj);
            var TmpRowId = rowid;
            $("#jqg_" + this.id + "_" + rowid).change(function () {
                TmpObjJQuery.jqGrid('setSelection', TmpRowId);
            });
        }

        this.AtualizaScroolBar();
    }

    this._AtualizaScroolBarExcutando = false;
    this.AtualizaScroolBar = function () {
        //if (!this._AtualizaScroolBarExcutando) {
        //    this._AtualizaScroolBarExcutando = true;
        //    var TmpThis = this;
        //    setTimeout(function () {
        //        $(TmpThis.ObjDivScroolBar).mCustomScrollbar("update");
        //        TmpThis._AtualizaScroolBarExcutando = false;
        //    }, 500);
        //}
    }

    this.SetTreePa = function (vRowId, vFather, vID, Load, Expand) {
        this.TreeRowFather[vRowId] = vFather;
        this.TreeRowID[vRowId] = vID;
        this.TreeRowLoad[vRowId] = Load;
        this.TreeRowExpand[vRowId] = Expand;
    }

    this.HideRow = function (rowid) {
        var ObjTrRow = this.GetObjRowId(rowid);
        $(ObjTrRow).hide();
    }

    this.ShowRow = function (rowid) {
        var ObjTrRow = this.GetObjRowId(rowid);
        $(ObjTrRow).show();
    }

    this.GetTreeRowsExpand = function () {
        if (this.Tree) {
            var vTmpS = "[";
            if (this.TreeRowExpand.length > 0) {
                for (var vRowId in this.TreeRowExpand) {
                    vTmpS += "{\"rowid\":\"" + vRowId + "\",\"value\":\"" + this.TreeRowExpand[vRowId] + "\"},"
                }
                vTmpS = vTmpS.substring(0, vTmpS.length - 1);
            }
            vTmpS += "]";
            return vTmpS;
        }
        else return "";
    }


    this.DelRow = function (vId) {
        $(this.Obj).delRowData(vId);

        delete this.PermissionsEditingRow[vId];
        delete this.TreeRowFather[vId];
        delete this.TreeRowID[vId];
        delete this.TreeRowLoad[vId];
        delete this.TreeRowExpand[vId];
        delete this.Event_ClickItem_Return[vId];

    }
    this.SetRowText = function (vId, vCells) {
        $(this.Obj).jqGrid('setRowData', vId, vCells);
    }

    this.SetEdit = function (vId, vEdit) {
        $(this.Obj).editRow(vId, vEdit);
    }

    this.ObjDiv = null;

    this.Left = null;
    this.SetLeft = function (vTmp) {
        if (this.Left != vTmp) {
            this.Left = vTmp;
            if (this.IsCreate) {
                this.ObjDiv.css({ left: this.Left });
            } else {
                $(this.Obj).css({ left: this.Left });
            }
        }
    }

    this.Top = null;
    this.SetTop = function (vTmp) {
        if (this.Top != vTmp) {
            this.Top = vTmp;
            if (this.IsCreate) {
                this.ObjDiv.css({ top: this.Top });
            } else {
                $(this.Obj).css({ top: this.Top });
            }
        }
    }

    this.GetSelRow = function () {
        return this.Sel.Row;
    }

    this.GetSelCol = function () {
        return this.Sel.Col;
    }

    this.GetSelNRow = function () {
        return this.Sel.NRow;
    }

    this.GetSelNCol = function () {
        return this.Sel.NCol;
    }

    // Eventos ---------------------------------------------------------------


    this.onSelectRow = function (rowid) {
        this.Events.SelectRow.Function(rowid);
    }

    this.setSelection_Maquina = false;
    this.SetSelectRow = function (rowid) {
        this.setSelection_Maquina = true;
        $(this.Obj).jqGrid('setSelection', rowid);
        this.setSelection_Maquina = false;
    }


    this.beforeSelectRow = function (rowid, evt) {
        var srcElement = evt.srcElement ? evt.srcElement : evt.target;
        if (srcElement.getAttribute("aria-describedby")) {
            var vCol = srcElement.getAttribute("aria-describedby").substring((this.id + "_").length);
            this.ClickCell(rowid, vCol, evt);
        }
    };

    this.CellEditValue = "";
    this.CellEditValueBeforeFormatCell = "";
    this.CellEditTrue = Array();
    this.CellEditTrue_RowCol = Array();
    this.CellEditTrue_Value = Array();
    this.CellEditSaveCellForcebyFormatCell = false;

    this.formatCell = function (rowid, colid, value, iRow, iCol) {
        this.CellEditValueBeforeFormatCell = value;

        if (this.Tree) {
            if (this.TreeColCaption == colid) {
                if (value.indexOf("<!--END-->") > 0) {
                    //
                    this.TreeTmpValorDiv = value.substring(0, value.indexOf("<!--END-->"));
                    value = value.substring(value.indexOf("<!--END-->") + "<!--END-->".length);
                    return value;
                }
            }
        }

        function RemuveTag(vText) {
            return vText.replace(/<(\w+)[^>]*>.*<\/\1>/gi, "")
        }

        if (RemuveTag(value) != value) {
            return $.trim(RemuveTag(value));
        }
    }

    this.beforeEditCell = function (rowid, colid, value, iRow, iCol) {
        this.CellEditValue = value;
    };

    this.afterEditCell = function (rowid, colid, value, iRow, iCol) {
        if (this.PermissionsEditingRow[rowid] != true) {
            $(this.Obj).editCell(iRow, iCol, false);
            return;
        }
    };

    this.beforeSaveCell = function (rowid, colid, value, iRow, iCol) {
        if (this.CellEditValueBeforeFormatCell != value) {
            this.CellEditSaveCellForcebyFormatCell = false;

            if (this.beforeSaveCellTime != null) {
                clearInterval(this.beforeSaveCellTime);
            }

            var TmpThis = this;
            this.beforeSaveCellTime = setTimeout(function () {

                if (TmpThis.CellEditTrue_RowCol[rowid + "_" + colid] != true) {
                    TmpThis.CellEditTrue_RowCol[rowid + "_" + colid] = true;
                    TmpThis.CellEditTrue.push({ "rowid": rowid, "colid": colid })
                }
                TmpThis.CellEditTrue_Value[rowid + "_" + colid] = value;

                TmpThis.Events.ChangeCell.Function(rowid, colid, value);
                TmpThis.beforeSaveCellTime = null;
            }, 100);
        }
    };
    this.beforeSaveCellTime = null;



    this.beforeSubmitCell = function (rowid, cellname, value, iRow, iCol) {

    };



    this.onCellSelect = function (rowid, iCol, cellcontent, e) {

    };

    this.ondblClickRow = function (rowid, iRow, iCol, event) {
        if (this.PermissionsEditingRow[rowid] == true)
            $(this.Obj).editCell(iRow, iCol, true);

        this.Events.ClickDblCell.Function(event);


        var vColId = $(this.Obj).getGridParam('colModel')[iCol].name;

        if (this.Event_ClickItem_Return[rowid][vColId][event.which] == false) {
            try {
                event.returnValue = false;
            } catch (err) { }
            try {
                event.cancelBubble = true;
            } catch (err) { }

            var TmpThis = this;
            //$(this).bind("contextmenu", function (e) {
            //});

            document.oncontextmenu = function () {
                var TmpThis2 = TmpThis;
                setTimeout(function () { TmpThis2.Janela.document.oncontextmenu = function () { return true; }; }, 100);
                return false;
            };
            event.preventDefault();

            return false;
        }

    };

    this.ClickCell = function (rowid, ColId, event) {
        $(this.Obj).editCell(this.GetIRowByRowId(rowid), this.GetIColByColId(ColId), false);
        this.Events.ClickCell.Function(event);
    };

    this.onSelectCell = function (rowid, colid, value, iRow, iCol) {
        this.Sel.Row = rowid;
        this.Sel.Col = colid;
        this.Sel.NRow = iRow;
        this.Sel.NCol = iCol;

        this.StratEvents();

        this.Events.SelectCell.Function();
    };

    this.afterInsertRow = function (rowid, rowdata, rowelem) {

    };

    this.gridComplete = function () {

    };

    this.custom_element = function (value, options) {
        //
    };

    this.custom_value = function (elem, operation, value) {
        //
    };



    this.GetIColByColId = function (vId) {
        var colModel = $(this.Obj).getGridParam('colModel');
        for (n = 0; n < colModel.length; n++) {
            if (colModel[n].name == vId)
                return n;
        }
        return -1;
    }

    this.GetIRowByRowId = function (vId) {
        var TmpRows = document.getElementById(this.id).childNodes[0].childNodes;

        for (var NRow = 0; NRow < TmpRows.length; NRow++) {
            if (TmpRows[NRow])
                if (TmpRows[NRow].id == vId)
                    return NRow;
        }

        return -1;
    }

    this.StratEvents = function () {
        if (!this.StratEvents_) {
            this.StratEvents_ = true;

            var TmpThis = this;
            $('#' + this.id + '_kn').keydown(function (e) {
                TmpThis.Events.KeyDown.Function(e);
            }).keypress(function (e) {
                TmpThis.Events.KeyPress.Function(e);
            }).keyup(function (e) {
                TmpThis.Events.KeyUp.Function(e);
            });
        }
    }

    this._MinWidth = null;
    this.SetMinWidth = function (vTmp) {
        this._MinWidth = vTmp;
        if (this._MinWidth != null && this.Width != null && parseInt(this._MinWidth, 10) > parseInt(this.Width, 10))
            this.SetWidth(this._MinWidth, false);
    }

    this.Width = null;
    this.SetWidth = function (vTmp, vServer) {
        //if (this.Width != parseInt(vTmp, 10)) {
        this.Width = parseInt(vTmp, 10);
        if (vServer) this.ControlVar.SetValueUtm('Width', this.Width);
        if (this.IsCreate) {
            if ($(this.Obj).setGridWidth)
                $(this.Obj).setGridWidth(this.Width);
        } else {
            $(this.Obj).width(vTmp);
        }

        if (!vServer)
            this.Events.WidthChangeAfter.Function();

        if (this.Anchor != null)
            this.Anchor.RenderChildren();
        //}
    }

    this._MinHeight = null;
    this.SetMinHeight = function (vTmp) {
        this._MinHeight = vTmp;
        if (this._MinHeight != null && this.Height != null && parseInt(this._MinHeight, 10) > parseInt(this.Height, 10))
            this.SetHeight(this._MinHeight, false);
    }

    this.Height = null;
    this.HeightBorder = 22;
    this.SetHeight = function (vTmp, vServer) {
        //if (this.Height != parseInt(vTmp,10)) {
        this.Height = parseInt(vTmp, 10);
        if (vServer) this.ControlVar.SetValueUtm('Height', this.Height);
        if (this.IsCreate) {
            if ($(this.Obj).setGridHeight)
                $(this.Obj).setGridHeight(this.Height - this.HeightBorder);
        } else {
            $(this.Obj).height(vTmp);
        }

        if (!vServer)
            this.Events.HeightChangeAfter.Function();

        if (this.Anchor != null)
            this.Anchor.RenderChildren();
        //}
    }


    this.SetVisible = function (vTmp) {
        if (this.ObjDiv) {
            if (vTmp)
                this.ObjDiv.show();
            else
                this.ObjDiv.hide();
        }
    }

    this.ObjEnabled = false;
    this.SetEnabled = function (vTmp) {


        var NameObjEnabled = this.id + "_objEnabled";
        //var ObjEnabled = document.getElementById(NameObjEnabled);
        if (!this.ObjEnabled) {
            //$("<div class='ui-widget-overlay jqgrid-overlay' id='lui_" + this.id + "'></div>").append(ii).insertBefore(gv)


            this.ObjEnabled = document.createElement('div');
            this.ObjEnabled.id = NameObjEnabled;
            this.ObjEnabled.className = "ui-widget-overlay jqgrid-overlay";
            this.Obj.appendChild(this.ObjEnabled);

            //ObjEnabled = document.getElementById(NameObjEnabled);
        }

        if (!vTmp)
            this.ObjEnabled.style.display = "block";
        else
            this.ObjEnabled.style.display = "none";

    }

    this.ClearData = function () {
        $(this.Obj).jqGrid("clearGridData", true);
        this.ControlVar.SetValueUtm('GetRowSelect()', null);

        this.PermissionsEditingRow = new Array();
        this.TreeRowFather = new Array();
        this.TreeRowID = new Array();
        this.TreeRowLoad = new Array();
        this.TreeRowExpand = new Array();
        this.Event_ClickItem_Return = new Array();
    }


    this.GetObjRowId = function (vIdRow) {
        if (this.Obj.getElementsByTagName("tr").namedItem) {
            var ObjTrRow = this.Obj.getElementsByTagName("tr").namedItem(vIdRow);
            if (!ObjTrRow)
                ObjTrRow = this.Obj.getElementsByTagName("tr")[vIdRow];
            return ObjTrRow;
        }
        else {
            //ObjTrRow = this.Obj.getElementsByTagName("tr")[vIdRow];
            for (var TmpKeyRow in this.Obj.getElementsByTagName("tr")) {
                if (this.Obj.getElementsByTagName("tr")[TmpKeyRow].id == vIdRow)
                    return this.Obj.getElementsByTagName("tr")[TmpKeyRow];
            }
        }
    }

    this.TreeRowClick = function (vIdRow) {
        if (this.TreeRowLoad[vIdRow] == false) {
            this.WarningLoading(-1, true, "Carregando...");

            var ObjTrRow = this.GetObjRowId(vIdRow);


            if (!ObjTrRow) {
                var TmpThis = this;
                setTimeout(function () { TmpThis.TreeRowClick(vIdRow); }, 100);
                return;
            }

            var DivIco = $(ObjTrRow.getElementsByTagName("div")["ico"]);
            DivIco
                .removeClass(this.TreeIcoExpand)
                .removeClass(this.TreeIcoContract)
                .removeClass(this.TreeIcoLoad)
            ;


            DivIco
            .addClass(this.TreeIcoLoad);

            Ara.Tick.Send(1, this.AppId, this.id, "TreeLoadRow", { row: vIdRow });
        }
        else {
            this.TreeExpand(vIdRow, !this.TreeRowExpand[vIdRow]);
        }
    }

    this.TreeExpand = function (vIdRow, vExpand) {
        this.TreeRowExpand[vIdRow] = vExpand;

        var ObjTrRow = this.GetObjRowId(vIdRow);
        if (!ObjTrRow) {
            var TmpThis = this;
            setTimeout(function () { TmpThis.TreeExpand(vIdRow, vExpand); }, 100);
            return;
        }

        var DivIco = $(ObjTrRow.getElementsByTagName("div")["ico"]);

        DivIco
            .removeClass(this.TreeIcoExpand)
            .removeClass(this.TreeIcoContract)
            .removeClass(this.TreeIcoLoad)
        ;


        var MyCod = this.TreeRowID[vIdRow];

        if (this.TreeRowExpand[vIdRow] == true) {
            DivIco
                .addClass(this.TreeIcoContract)
            ;

            for (var vTmpRowId in this.TreeRowFather) {
                if (this.TreeRowFather[vTmpRowId] == MyCod) {
                    this.ShowRow(vTmpRowId);
                }
            }

        }
        else {
            DivIco
                .addClass(this.TreeIcoExpand)
            ;

            for (var vTmpRowId in this.TreeRowFather) {

                if (this.TreeRowFather[vTmpRowId] == MyCod) {
                    this.TreeExpand(vTmpRowId, false);
                    this.HideRow(vTmpRowId);
                }
            }
        }

    }

    this.CreateButton = function (vCodB) {
        var vIdB = "AraLabel_Button_" + vCodB;

        if (document.getElementById(vIdB)) {
            $("button").button();
            $(document.getElementById(vIdB).getElementsByTagName("span")[0]).removeClass("ui-button-text");
        }
        else
            setTimeout("Ara.GetForm('" + this.CodJanela + "').AraObjs.GetObject('" + this.id + "').CreateButton(" + vCodB + ");", 20);

    }

    this.OnClickButton = function (vCodB) {
        Ara.Tick.Send(1, this.AppId, this.id, "ClickButton", { codigo: vCodB });
    }

    this.AutoAdjustColumnWidthActive = false;

    this.AutoAdjustColumnWidth = function () {

        if (this.AutoAdjustColumnWidthActive)
            return;

        this.AutoAdjustColumnWidthActive = true;

        this.AutoAdjustColumnWidth_CVisble();
    };

    this.AutoAdjustColumnWidth_CVisble = function () {

        this.Obj = document.getElementById(this.id);
        if (!this.Obj)
            return;

        var ObjTesteW = document.getElementById(this.Obj.id + "DivAraGridAutoAdjustColumnWidth");
        if (ObjTesteW == null) {
            ObjTesteW = document.createElement('div');
            ObjTesteW.id = this.Obj.id + "DivAraGridAutoAdjustColumnWidth";
            //ObjTesteW.style.top = "-5000px";
            //ObjTesteW.style.left = "-5000px";
            ObjTesteW.style.top = "10px";
            ObjTesteW.style.left = "10px";
            ObjTesteW.style.position = "absolute";
            //ObjTesteW.style.display = "block";
            ObjTesteW.style.display = "none";
            this.Obj.appendChild(ObjTesteW);
            //document.body.appendChild(ObjTesteW);
        }
        //ObjTesteW = document.getElementById(this.Obj.id + "DivAraGridAutoAdjustColumnWidth");
        var ObjTesteWJQ = $(ObjTesteW);
        ObjTesteWJQ.html("AAA");

        if (ObjTesteWJQ.width() == 0) {

            var TmpThis = this;
            setTimeout(function () { TmpThis.AutoAdjustColumnWidth_CVisble() }, 500);
            return;
        }

        this.AutoAdjustColumnWidth_row();
    };

    this.ScrollBarResizeTimer = null;
    this.ScrollBarResize = function () {

        if (this.ScrollBarResizeTimer)
            clearInterval(this.ScrollBarResizeTimer);
        var vTmpThis = this;
        this.ScrollBarResizeTimer = setInterval(function () {
            $("#" + vTmpThis.id + "_gridscroll").getNiceScroll().resize();
            clearInterval(vTmpThis.ScrollBarResizeTimer);
            vTmpThis.ScrollBarResizeTimer = null;
        }, 50);
    }

    this.AutoAdjustColumnWidth_row = function () {
        var ObjTesteW = document.getElementById(this.Obj.id + "DivAraGridAutoAdjustColumnWidth");
        var ObjTesteWJQ = $(ObjTesteW);

        var ColsW = new Array();
        var ColsWID = new Array();
        var vDados = $(this.Obj).getRowData();

        ObjTesteW.style.display = "block";

        for (var vColIDX in $(this.Obj)[0].p.colModel) {
            var vCol = $(this.Obj)[0].p.colModel[vColIDX];
            if (vCol.hidden == false) {
                var vCodId = vCol.name;

                var vObjCol = document.getElementById(this.id + "_" + vCodId);

                if (vObjCol.nodeName.toUpperCase() != "TH")
                    vObjCol = vObjCol.parentNode;

                $(ObjTesteW).html($(vObjCol).html());

                ColsW[vCodId] = ObjTesteWJQ.width() + 35;
                if (!ColsWID[vCodId])
                    ColsWID.push(vCodId);
            }
        }

        for (var vRowId in vDados) {
            for (var vColId in vDados[vRowId]) {
                if (this.ColProp(vColId).hidden == false) {
                    var vTexto = vDados[vRowId][vColId];

                    var AddW = 0;

                    if (vTexto.indexOf("<!--END-->") > 0) {
                        //

                        var TextoAntes = vTexto.substring(0, vTexto.indexOf("<!--END-->"));
                        var TextoDepois = vTexto.substring(vTexto.indexOf("<!--END-->") + "<!--END-->".length);

                        $(ObjTesteW).html(TextoAntes);
                        var WTd1 = parseInt(ObjTesteW.childNodes[0].style.width, 10);
                        AddW = WTd1;

                        //ObjTesteW.innerHTML = "<table><tr><td style='width:" + WTd1 + "px;'>" + TextoAntes + "</td><td>" + TextoDepois + "</tb></tr></table>";
                        $(ObjTesteW).html(TextoDepois);
                    }
                    else {
                        $(ObjTesteW).html(vTexto);
                    }

                    //if (vTexto.indexOf("PKS 28") >= 0)
                    //    

                    var vWidth = ObjTesteWJQ.width() + 5 + AddW;


                    if (ColsW[vColId] < vWidth)
                        ColsW[vColId] = vWidth;
                }
            }
        }
        ObjTesteW.style.display = "none";

        // IE 8
        //for (var Col in ColsW) {
        //    this.SetColWidth(Col, ColsW[Col]);
        //}
        var vTmpThis = this;
        $(ColsWID).each(function () {
            vTmpThis.SetColWidth(this, ColsW[this]);
        });
        this.AutoAdjustColumnWidthActive = false;
    }

    this.ColProp = function (vCodId) {
        var vColIDX = this.GetIColByColId(vCodId);
        return $(this.Obj)[0].p.colModel[vColIDX];
    }

    this.SetColWidth = function (vColId, vWidth) {
        var idx = this.GetIColByColId(vColId);

        $(this.Obj)[0].grid.resizing = { "idx": idx };
        $(this.Obj)[0].grid.newWidth = vWidth;
        $(this.Obj)[0].grid.headers[idx].el = document.getElementById(this.id + "_" + vColId);
        $(this.Obj)[0].grid.headers[idx].newWidth = vWidth;
        $(this.Obj)[0].grid.headers[idx].width = vWidth;
        $(this.Obj)[0].grid.dragEnd();

        this.ScrollBarResize();
    }

    this.WarningLoading = function (vId, vVisible, Menssage) {
        this.DivObjLui = $("#lui_" + this.id)[0];
        this.DivObjLoad = $("#load_" + this.id)[0];

        if (vVisible) {
            if (this.DivObjLui) {
                $(this.DivObjLui).css({ position: "absolute" });

                this.DivObjLui.style.display = "block";
                this.DivObjLoad.style.display = "block";

                if (Menssage || Menssage != '')
                    this.DivObjLoad.innerHTML = Menssage;
                else
                    this.DivObjLoad.innerHTML = "Carregando...";
            }

            if (vId >= 0)
                Ara.Tick.Send(1, this.AppId, this.id, "ActionLoad", { codigo: vId });
        }
        else {
            if (this.DivObjLui) {
                this.DivObjLui.style.display = "none";
                this.DivObjLoad.style.display = "none";
            }
        }
    }

    // lui_2__Grid_
    // load_2__Grid_





    this.CellFocus = function (vRowID, vColID) {
        if (!$(this.Obj)) {
            var TmpThis = this;
            setTimeout(function () { TmpThis.CellFocus(vRowID, vColID) }, 100);
            return;
        }
        var TmpThis = this;
        function scrollToRow(id) {

            function getGridRowHeight(targetGrid) {
                var height = null; // Default

                try {
                    height = jQuery(targetGrid).find('tbody').find('tr:first').outerHeight();
                }
                catch (e) {
                    //catch and just suppress error
                }

                return height;
            }

            var rowHeight = getGridRowHeight($(TmpThis.Obj)) || 23; // Default height
            var index = $(TmpThis.Obj).getInd(id);
            $(TmpThis.Obj).closest(".ui-jqgrid-bdiv").scrollTop(rowHeight * index);
        }

        scrollToRow(vRowID);
        $(this.Obj).editCell(this.GetIRowByRowId(vRowID), this.GetIColByColId(vColID), false);
    }

    this.TreeGetScriptLevelType = function (vLevel, vType) {
        var vReturn = "";
        // PassLine, LastLine, LineCrusade, nd
        if (vType == 2) { // ETypeLineByLevel.LastLine

            //LastLine
            vReturn += "<div style=\"top:0px;left:" + (this.TreeIcoWidth * vLevel) + "px;width:" + this.TreeIcoWidth + "px;height" + this.TreeIcoWidth + "px;position: absolute;\">";
            vReturn += "<span style=\"width:1px;height:10px;left:49%;border-left: 1px dotted #000000;position: relative;display: block\"></span> ";
            vReturn += "<span style=\"height:1px;width:50%;top:10px;left:50%;border-top: 1px dotted #000000;position: absolute;\"></span> ";
            vReturn += "</div>";
        }
        else if (vType == 3) { // ETypeLineByLevel.LineCrusade
            //LineCrusade
            vReturn += "<div style=\"top:0px;left:" + (this.TreeIcoWidth * vLevel) + "px;width:" + this.TreeIcoWidth + "px;height" + this.TreeIcoWidth + "px;position: absolute;\">";
            vReturn += "<span style=\"width:1px;height:100%;left:49%;border-left: 1px dotted #000000;position: relative;\"></span>  ";
            vReturn += "<span style=\"height:1px;width:50%;top:49%;left:50%;border-top: 1px dotted #000000;position: absolute;\"></span> ";
            vReturn += "</div>";

        }
        else if (vType == 1) { // ETypeLineByLevel.PassLine

            //PassLine
            vReturn += "<div style=\"top:0px;left:" + (this.TreeIcoWidth * vLevel) + "px;width:" + this.TreeIcoWidth + "px;height" + this.TreeIcoWidth + "px;position: absolute;\">";
            vReturn += "<span style=\"width:1px;height:100%;left:49%;border-left: 1px dotted #000000;position: relative;\"></span> ";
            vReturn += "</div>";

        }

        return vReturn;
    }

    this.TrataSetText = function (vRowId, vColId, vText) {

        if (this.Tree) {
            if (this.TreeColCaption == vColId) {
                if (vText.indexOf("<!--END-->") > 0) {
                    var ScriptTree = vText.substring(0, vText.indexOf("<!--END-->"));
                    vText = vText.substring(vText.indexOf("<!--END-->") + "<!--END-->".length);

                    var TmpObjScript;
                    eval(" TmpObjScript = " + ScriptTree + ";");

                    var vHtmlTree = "";

                    if (TmpObjScript.Container) {
                        var vIco = this.TreeRowExpand[vRowId] == true ? this.TreeIcoContract : this.TreeIcoExpand;
                        var W = (this.TreeIcoWidth * (TmpObjScript.level + 1));
                        vHtmlTree += "<div class='tree-wrap tree-wrap-ltr' style='width: " + W + "px;display:inline;'>";

                        if (TmpObjScript.level > 0) {
                            for (var vL = 0; vL < TmpObjScript.level; vL++) {
                                vHtmlTree += this.TreeGetScriptLevelType(vL, TmpObjScript.LType[vL]);
                            }
                        }

                        vHtmlTree += "<div id='ico' class='ui-icon " + vIco + " tree-plus treeclick' style='left: " + (this.TreeIcoWidth * (TmpObjScript.level)) + "px;' onclick=\"javascript:Ara.GetObject('" + this.id + "').TreeRowClick('" + vRowId + "');return false;\" ></div>";
                        vHtmlTree += "</div>";
                    }
                    else {
                        var W = (this.TreeIcoWidth * (TmpObjScript.level));
                        vHtmlTree += "<div  style='height: 18px;width: " + W + "px;display:inline;position: relative;float:left;'> ";

                        for (var vL = 0; vL < TmpObjScript.level; vL++) {
                            vHtmlTree += this.TreeGetScriptLevelType(vL, TmpObjScript.LType[vL]);
                        }

                        vHtmlTree += "</div>";
                    }

                    vText = vHtmlTree + "<!--END-->" + vText;
                }
            }
        }

        return vText;
    }

    this.SetTextCell = function (vRowId, vColId, vText) {
        var ObjRow = this.GetObjRowId(vRowId);
        if (ObjRow) {
            var ColN = this.GetIColByColId(vColId);

            var CellTb = ObjRow.childNodes[ColN];
            var TmpText = this.TrataSetText(vRowId, vColId, vText);

            //            CellTb.title = TmpText.replace(/<(?:.|\n)*?>/gm, '').trim();
            CellTb.innerHTML = TmpText;
        }
    }

    this.SetCellVisible = function (vRowId, vColId, vVisible) {
        var ObjRow = this.GetObjRowId(vRowId);
        if (ObjRow) {
            var ColN = this.GetIColByColId(vColId);

            var CellTb = ObjRow.childNodes[ColN];
            CellTb.style.display = (vVisible ? "block" : "none");
        }
    }

    this.SetCellColSpan = function (vRowId, vColId, vSpan) {
        var ObjRow = this.GetObjRowId(vRowId);
        if (ObjRow) {
            var ColN = this.GetIColByColId(vColId);

            var CellTb = ObjRow.childNodes[ColN];
            CellTb.colSpan = vSpan;
        }
    }

    this.SetCellRowSpan = function (vRowId, vColId, vSpan) {
        var ObjRow = this.GetObjRowId(vRowId);
        if (ObjRow) {
            var ColN = this.GetIColByColId(vColId);

            var CellTb = ObjRow.childNodes[ColN];
            CellTb.rowSpan = vSpan;
        }
    }

    this.ColHidden = function (vNameCol, vHideen) {
        if (vHideen)
            $(this.Obj).hideCol(vNameCol);
        else
            $(this.Obj).showCol(vNameCol);
    }

    this.destruct = function () {
        try {
            $(this.Obj).GridDestroy();
        } catch (e) { }

        $(this.Obj).remove();
    }

    this.SetTypePosition = function (vTypePosition) {
        if (vTypePosition != "static")
            $(this.Obj).css({ position: vTypePosition });
        else {
            $(this.Obj).css({ position: "", left: "", top: "" });
        }
    }

    this.IsDestroyed = function () {
        if (!document.getElementById(this.id))
            return true;
        else
            return false;
    }

    this.vSpan = "";
    this.vValorAntigo = "";
    this.vValorAntigo_class = "";

    this.Sel = {};
    this.Sel.Row = "";
    this.Sel.Col = "";
    this.Sel.NRow = -1;
    this.Sel.NCol = -1;

    this.AppId = vAppId;
    this.id = vId;
    this.ConteinerFather = ConteinerFather;

    this.Obj = document.getElementById(this.id);
    if (!this.Obj) {
        alert("Object '" + this.id + "' Not Found");
        return;
    }
    //this.Obj.innerHTML = "";
    $(this.Obj).html("");

    var TmpThis = this;
    //$(this.Obj).css({ position: "absolute", top: "0px", left: "0px" });
    this.Left = '0px';
    this.Top = '0px';


    this.PermissionsEditingRow = Array();
    this.TreeContainer = Array();
    this.TreeRowID = Array();
    this.TreeRowFather = Array();
    this.TreeRowLoad = Array();
    this.TreeRowExpand = Array();
    this.TreeColCaption = "";
    this.TreeColID = "";
    this.TreeColFather = "";
    this.Tree = false;
    this.Event_ClickItem_Return = new Array();

    //$(this.Obj).focus(function () { TmpThis.Events.Focus.Function(); });
    //$(this.Obj).blur(function () { TmpThis.Events.LostFocus.Function(); });
    //$(this.Obj).click(function () { TmpThis.Events.Click.Function(); });
    //$(this.Obj).keydown(function (e) { TmpThis.Events.KeyDown.Function(e); });
    //$(this.Obj).keyup(function (e) { TmpThis.Events.KeyUp.Function(e); });
    //$(this.Obj).keypress(function (e) { TmpThis.Events.KeyPress.Function(e); });
    //$(this.Obj).change(function () { TmpThis.Events.Change.Function(); });

    this.ControlVar = new ClassAraGenVarSend(this);
    this.ControlVar.AddPrototype("Top");
    this.ControlVar.AddPrototype("Left");
    this.ControlVar.AddPrototype("Width");
    this.ControlVar.AddPrototype("Height");
    this.ControlVar.AddPrototype("GetSelRow()");
    this.ControlVar.AddPrototype("GetSelCol()");
    this.ControlVar.AddPrototype("GetSelNRow()");
    this.ControlVar.AddPrototype("GetSelNCol()");
    this.ControlVar.AddPrototype("GetChangesInCell()");
    this.ControlVar.AddPrototype("GetRowSelect()");
    this.ControlVar.AddPrototype("GetTreeRowsExpand()");
    this.ControlVar.AddPrototype("IsDestroyed()");

    this.Anchor = new ClassAraAnchor(this);

    this.SetWidth('400px', true);
    this.SetHeight('400px', true);
});