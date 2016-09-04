// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
Ara.AraClass.Add('AraSelect', function (vAppId, vId, ConteinerFather) {

    this.Events = {};

    var TmpThis = this;
    this.Events.Focus =
    {
        Enabled: false,
        ThreadType: 2, // Single_thread
        Function: function () {
            if (this.Enabled) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "Focus", null);
            }
        }
    };

    this.Events.LostFocus =
    {
        Enabled: false,
        ThreadType: 2, // Single_thread
        Function: function () {
            if (this.Enabled) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "LostFocus", null);
            }
        }
    };

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

    this.Events.Change =
    {
        Enabled: false,
        ThreadType: 2, // Single_thread
        Function: function () {
            if (this.Enabled) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "Change", null);
            }
        }
    };

    this.OnFocus = function () {
        this.Obj.style.backgroundColor = this.Color_Focus
        this.Events.Focus.Function();
    }

    this.OnLostFocus = function () {
        this.Obj.style.backgroundColor = this.Color_LostFocus;
        this.Events.LostFocus.Function();
    }

    this.OnChange = function () {
        this.Events.Change.Function();
    }

    this.OnClick = function () {
        if (this.multiple) {
            var vScrollTop = this.Obj.scrollTop;
            var vScrollLeft = this.Obj.scrollLeft;
            //this.ObjectsSelected = new Array();

            for (var i = 0; i < this.Obj.options.length; i++) {
                if (this.Obj.options[i].selected) {
                    var Achou = false;
                    for (var i2 = 0; i2 < this.ObjectsSelected.length; i2++) {
                        if (this.Obj.options[i].value == this.ObjectsSelected[i2]) {
                            Achou = true;
                            break;
                        }
                    }
                    if (!Achou) {
                        this.ObjectsSelected.push(this.Obj.options[i].value);
                    }
                    else {
                        delete this.ObjectsSelected[i2];
                        //this.ObjectsSelected.remove(i2);
                    }
                }
            }

            for (var i = 0; i < this.Obj.options.length; i++) {
                var Achou2 = false;
                for (var i2 = 0; i2 < this.ObjectsSelected.length; i2++) {
                    if (this.Obj.options[i].value == this.ObjectsSelected[i2]) {
                        this.Obj.options[i].selected = true;
                        Achou2 = true;
                        break;
                    }
                }
                if (!Achou2)
                    this.Obj.options[i].selected = false;
            }

            this.Obj.scrollTop = vScrollTop;
            this.Obj.scrollLeft = vScrollLeft;
        }

        this.Events.Click.Function();
    }

    this.Events.KeyDown =
    {
        Enabled: false,
        ThreadType: 2, // Single_thread
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


            for (n = 0; n < this.Ignore.length; n++) {
                if (this.Ignore[n] == vkeyCode) {
                    IgnoreEvent = true;
                    break;
                }
            }

            if (IgnoreEvent == false) {
                if (this.Only.length > 0) {
                    var TmpAcho = false;
                    for (n = 0; n < this.Only.length; n++) {
                        if (this.Only[n] == vkeyCode) {
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



            for (n = 0; n < this.ReturnTrue.length; n++) {
                if (this.ReturnTrue[n] == vkeyCode)
                    return true;
            }

            for (n = 0; n < this.ReturnFalse.length; n++) {
                if (this.ReturnFalse[n] == vkeyCode) {
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
        ThreadType: 2, // Single_thread
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


            for (n = 0; n < this.Ignore.length; n++) {
                if (this.Ignore[n] == vkeyCode) {
                    IgnoreEvent = true;
                    break;
                }
            }

            if (IgnoreEvent == false) {
                if (this.Only.length > 0) {
                    var TmpAcho = false;
                    for (n = 0; n < this.Only.length; n++) {
                        if (this.Only[n] == vkeyCode) {
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



            for (n = 0; n < this.ReturnTrue.length; n++) {
                if (this.ReturnTrue[n] == vkeyCode)
                    return true;
            }

            for (n = 0; n < this.ReturnFalse.length; n++) {
                if (this.ReturnFalse[n] == vkeyCode) {
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
        ThreadType: 2, // Single_thread
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


            for (n = 0; n < this.Ignore.length; n++) {
                if (this.Ignore[n] == vkeyCode) {
                    IgnoreEvent = true;
                    break;
                }
            }

            if (IgnoreEvent == false) {
                if (this.Only.length > 0) {
                    var TmpAcho = false;
                    for (n = 0; n < this.Only.length; n++) {
                        if (this.Only[n] == vkeyCode) {
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



            for (n = 0; n < this.ReturnTrue.length; n++) {
                if (this.ReturnTrue[n] == vkeyCode)
                    return true;
            }

            for (n = 0; n < this.ReturnFalse.length; n++) {
                if (this.ReturnFalse[n] == vkeyCode) {
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


    this.GetValue = function () {
        if (!this.multiple) {
            return this.Obj.value;
        }
        else {
            var TmpReturn = "[";
            for (var i = 0; i < this.Obj.options.length; i++) {
                if (this.Obj.options[i].selected) {
                    TmpReturn += this.Obj.options[i].value + ",";
                }
            }
            if (TmpReturn != "[")
                TmpReturn = TmpReturn.substring(0, TmpReturn.length - 1);
            TmpReturn += "]"
            return TmpReturn;
        }
    }

    this.SetValue = function (vTmp) {
        this.Obj.value = vTmp;
        this.ControlVar.SetValueUtm('GetValue()', this.GetValue());
    }

    this.SetSelects = function (vTmp) {
        var vTmp2 = vTmp;
        this.ObjectsSelected = new Array();

        for (var i = 0; i < this.Obj.options.length; i++) {
            this.Obj.options[i].selected = false;
        }

        for (n = 0; n < vTmp2.length; n++) {
            this.ObjectsSelected[this.ObjectsSelected.length] = vTmp2[n];
            for (var i = 0; i < this.Obj.options.length; i++) {
                if (this.Obj.options[i].value == vTmp2[n]) {
                    this.Obj.options[i].selected = true;
                    break;
                }
            }
        }
    }


    this.SetSize = function (vTmp) {
        this.Obj.size = vTmp;
    }



    this.SetFocus = function () {
        try { this.Obj.focus(); } catch (ex) { }
    }

    this.Clear = function () {
        var options = this.Obj.getElementsByTagName('option');
        while (options.length != 0) {
            this.Obj.removeChild(options[0]);
        }
    }

    this.Remove = function (vKey) {
        var options = this.Obj.getElementsByTagName('option');
        for (var n = 0; n < options.length; n++) {
            if (options[n].value == vKey) {
                this.Obj.removeChild(options[n]);
                break;
            }
        }
    }



    this.ListAdd = function (vValor, vCaption, vSelect) {
        var elOptNew = document.createElement('option');
        elOptNew.text = vCaption;
        elOptNew.value = vValor;
        if (this.multiple)
            elOptNew.selected = vSelect;
        try {
            this.Obj.add(elOptNew, null);
        }
        catch (ex) {
            this.Obj.add(elOptNew);
        }
    }

    this.Left = null;
    this.SetLeft = function (vTmp) {
        if (this.Left != vTmp) {
            this.Left = vTmp;
            this.Obj.style.left = vTmp;
        }
    }

    this.Top = null;
    this.SetTop = function (vTmp) {
        if (this.Top != vTmp) {
            this.Top = vTmp;
            this.Obj.style.top = vTmp;
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
        if (vTmp != null && this._MinWidth != null && parseInt(this._MinWidth, 10) > parseInt(vTmp, 10))
            vTmp = this._MinWidth;

        if (this.Width != vTmp) {
            this.Width = vTmp;
            if (vServer)
                this.ControlVar.SetValueUtm('Width', this.Width);

            this.Obj.style.width = vTmp;

            if (!vServer)
                this.Events.WidthChangeAfter.Function();

            if (this.Anchor != null)
                this.Anchor.RenderChildren();
        }
    }

    this._MinHeight = null;
    this.SetMinHeight = function (vTmp) {
        this._MinHeight = vTmp;
        if (this._MinHeight != null && this.Height != null && parseInt(this._MinHeight,10) > parseInt(this.Height, 10))
            this.SetHeight(this._MinHeight, false);
    }

    this.Height = null;
    this.SetHeight = function (vTmp, vServer) {
        if (vTmp != null && this._MinHeight != null && parseInt(this._MinHeight,10) > parseInt(vTmp,10))
            vTmp = this._MinHeight;

        if (this.Height != vTmp) {
            this.Height = vTmp;
            if (vServer)
                this.ControlVar.SetValueUtm('Height', this.Height);

            this.Obj.style.height = vTmp;

            if (!vServer)
                this.Events.HeightChangeAfter.Function();

            if (this.Anchor != null)
                this.Anchor.RenderChildren();
        }
    }

    this.SetMultiple = function (vValue) {
        this.multiple = vValue == true ? true : false;
        this.Obj.multiple = this.multiple == true ? "this.multiple" : "";
    }

    this.FormResize = function () {
        if (this.Anchor)
            this.Anchor.FormResize();
    }


    this.SetToolTip = function (vToolTip) {
        this.Obj.title = vToolTip;
    }

    this.SetEnable = function (vEnable) {
        try {
            if (vEnable)
                $(this.Obj).removeAttr("disabled");
            else
                $(this.Obj).attr("disabled", true);
        } catch (err)
        { }

    }

    this.SetReadonly = function (vReadonly) {
        try {
            
            if (vReadonly)
                $(this.Obj).attr("readonly", true);
            else
                $(this.Obj).removeAttr("readonly");
        } catch (err)
        { }

    }


    this.SetVisible = function (vTmp) {
        if (vTmp)
            this.Obj.style.display = "block";
        else
            this.Obj.style.display = "none";
    }

    this.destruct = function () {
        $(this.Obj).remove();
    }

    this.SetTypePosition = function (vTypePosition) {
        if (vTypePosition != "static")
            $(this.Obj).css({ position: vTypePosition });
        else {
            $(this.Obj).css({ position: "", left: "", top: "" });
        }
    }

    this.Color_Focus = "lightyellow";
    this.Color_LostFocus = "white";

    this.multiple = false;
    this.ObjectsSelected = new Array();

    this.AppId = vAppId;
    this.id = vId;
    this.ConteinerFather = ConteinerFather;

    this.Obj = document.getElementById(this.id);
    if (!this.Obj) {
        alert("Object '" + this.id + "' Not Found");
        return;
    }

    var TmpThis = this;
    $(this.Obj).css({ position: "absolute", top: "0px", left: "0px" });
    this.Left = 0;
    this.Top = 0;

    
    $(this.Obj).focus(function () { TmpThis.Events.Focus.Function(); });
    $(this.Obj).blur(function () { TmpThis.Events.LostFocus.Function(); });
    //$(this.Obj).click(function () { TmpThis.Events.Click.Function(); });
    $(this.Obj).keydown(function (e) { TmpThis.Events.KeyDown.Function(e); });
    $(this.Obj).keyup(function (e) { TmpThis.Events.KeyUp.Function(e); });
    $(this.Obj).keypress(function (e) { TmpThis.Events.KeyPress.Function(e); });
    $(this.Obj).change(function () { TmpThis.Events.Change.Function(); });

    this.ControlVar = new ClassAraGenVarSend(this);
    this.ControlVar.AddPrototype("Top");
    this.ControlVar.AddPrototype("Left");
    this.ControlVar.AddPrototype("Width");
    this.ControlVar.AddPrototype("Height");
    this.ControlVar.AddPrototype("GetValue()");
    
    this.Anchor = new ClassAraAnchor(this);

});