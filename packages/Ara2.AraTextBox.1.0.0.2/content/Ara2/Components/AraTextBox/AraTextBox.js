// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
Ara.AraClass.Add('AraTextBox', function (vAppId, vId, ConteinerFather) {

    this.Color_Focus = "lightyellow";
    this.Color_LostFocus = "white";

    this.AutoCompleteSearch_Validate = false;
    this.SetAutoComplete_Active = false;

    // Eventos  ---------------------------------------
    this.Events = {};

    var TmpThis = this;
    this.Events.AutoCompleteSearch =
    {
        Enabled: false,
        ThreadType: 1, // Mult
        Function: function (vValue) {
            if (this.Enabled) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "AUTOCOMPLETESEARCH", { value: vValue });
            }
        }
    };

    this.Events.Focus =
    {
        Enabled: false,
        ThreadType: 1, // Mult
        Function: function () {
            TmpThis.Obj.style.backgroundColor = TmpThis.Color_Focus;
            if (this.Enabled) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "Focus", null);
            }
        }
    };


    this.Events.LostFocus =
    {
        Enabled: false,
        ThreadType: 1, // Mult
        Function: function () {



            if (TmpThis.AutoCompleteSearch_Validate) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "AutoCompleteSearch_Validate", { value: TmpThis.GetValue() });
            }

            TmpThis.Obj.style.backgroundColor = TmpThis.Color_LostFocus;
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
        ThreadType: 1, // Mult
        Function: function () {
            if (this.Enabled) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "Change", null);
            }
        }
    };

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
        if (this._MinHeight != null && this.Height != null && parseInt(this._MinHeight, 10) > parseInt(this.Height, 10))
            this.SetHeight(this._MinHeight, false);
    }

    this.Height = null;
    this.SetHeight = function (vTmp, vServer) {
        if (vTmp != null && this._MinHeight != null && parseInt(this._MinHeight, 10) > parseInt(vTmp, 10))
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


    this.GetValue = function () {
        return this.Obj.value;
    }

    this.SetValue = function (vTmp) {
        var vTmp2 = vTmp;
        if (this._Mask != null && vTmp2 != $.mask.string(vTmp2, this._Mask)) {
            vTmp2 = $.mask.string(vTmp2, this._Mask);
            this.Obj.value = vTmp2;
        }
        else
            this.Obj.value = vTmp2;
        this.ControlVar.SetValueUtm('GetValue()', vTmp);


        //this.Obj.selectionStart = this.Obj.value.length;
        //this.Obj.selectionEnd = this.Obj.selectionStart;
    }



    this.SetFocus = function () {
        var Tmpthis = this;
        $(Tmpthis.Obj).focus();
        $(Tmpthis.Obj).focus().delay(500);
        $(Tmpthis.Obj).click();
        $(Tmpthis.Obj).click().delay(500);
        $(Tmpthis.Obj).trigger('click');
        $(Tmpthis.Obj).trigger('click').delay(500);
        setTimeout(function () {
            try { Tmpthis.Obj.focus(); } catch (ex) { }
        }, 100);
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

    this.SetToolTip = function (vToolTip) {
        this.Obj.title = vToolTip;
    }

    this.SetPlaceholder = function (vPlaceholder) {
        this.Obj.placeholder = vPlaceholder;
    }

    this.SetMaxLength = function (vMaxLength) {
        try {
            if (vMaxLength == null)
                $(this.Obj).removeAttr("maxLength");
            else
                $(this.Obj).attr("maxLength", vMaxLength);
        } catch (err)
        { }
    }

    this.FormResize = function () {
        if (this.Anchor)
            this.Anchor.FormResize();
    }

    this.SetAutoComplete = function (vValue) {
        $(this.Obj).autocomplete(vValue);
    }

    this.AutoComplete_minLength = 2;
    this.AutoComplete_autoFocus = false;
    this.AutoComplete_disabled = false;
    this.SetAutoComplete_Create = function () {
        this.SetAutoComplete_Active = true;
        var TmpThis = this;

        this.AcertaZIndex();

        var vTmpCreate =
        {
            source: function (request, response) {
                TmpThis.AutoComplete_response = response;
                TmpThis.Events.AutoCompleteSearch.Function(request.term);
            },
            minLength: this.AutoComplete_minLength,
            autoFocus: this.AutoComplete_autoFocus,
            disabled: this.AutoComplete_disabled,
            /*search: function (event, ui) {

            },*/
            select: function (event, ui) {
                TmpThis.AutoComplete_id = ui.item.id;
                TmpThis.AutoComplete_Value = ui.item.value;
            },
            open: function () {
                $(this).autocomplete('widget').zIndex(Ara.GetMaxZIndexByObject(TmpThis) + 1);
                return false;
            },
        };

        $(this.Obj).autocomplete(vTmpCreate);
    }

    this.SetAutoComplete_Show = function () {
        this.AcertaZIndex();
        $(this.Obj).autocomplete("search", "");
    }

    this.SetAutoComplete_minLength = function (vminLength) {
        this.AutoComplete_minLength = vminLength;
        $(this.Obj).autocomplete({ minLength: vminLength });
    }

    this.SetAutoComplete_autoFocus = function (vautoFocus) {
        this.AutoComplete_autoFocus = vautoFocus;
        $(this.Obj).autocomplete({ autoFocus: vautoFocus });
    }

    this.SetAutoComplete_disabled = function (vdisabled) {
        this.AutoComplete_disabled = vdisabled;
        $(this.Obj).autocomplete({ disabled: vdisabled });
    }

    this.SetAutoComplete_response = function (vValue) {
        this.AcertaZIndex();
        this.AutoComplete_response(vValue);
    }

    this._Mask = null;
    this.SetMask = function (vValue) {
        if (vValue != null && vValue.mask != "")
            this._Mask = vValue;
        else
            this._Mask = null;
        $(this.Obj).setMask(vValue);
    }

    this.AcertaZIndex = function () {
        //$(this.Obj).css("z-index", Ara.GetMaxZIndexByObject(this) + 1);
        $(this.Obj).zIndex(Ara.GetMaxZIndexByObject(this) + 1);
    }


    this.DatePickerType = null;
    this.SetDatePicker = function (vValue) {

        var TmpThis = this;
        if (vValue != null) {
            vValue.scriptcuston.onSelect = function (dateText, inst) { TmpThis.datepicker_onSelect(dateText, inst); };
            vValue.scriptcuston.beforeShow = function () { TmpThis.AcertaZIndex(); };

            if (vValue.type == 1) // Data
            {
                this.DatePickerType = 1;
                var TmpThis = this;
                setTimeout(function () {
                    $(TmpThis.Obj).datepicker(vValue.scriptcuston);
                }, 1000);
            }
            else if (vValue.type == 2) // DataHora
            {
                this.DatePickerType = 2;
                var TmpThis = this;
                setTimeout(function () {
                    $(TmpThis.Obj).datetimepicker(vValue.scriptcuston);
                }, 1000);
            }
            else if (vValue.type == 3) // Hora
            {
                this.DatePickerType = 3;
                var TmpThis = this;
                setTimeout(function () {
                    $(TmpThis.Obj).timepicker(vValue.scriptcuston);
                }, 1000);
            }
        }
        else {
            if (this.DatePickerType == 1) {
                $(this.Obj).datepicker("destroy");
            }
            else if (this.DatePickerType == 2) {
                $(this.Obj).datetimepicker("destroy");
            }
            else if (this.DatePickerType == 3) {
                $(this.Obj).timepicker("destroy");
            }
        }

        //$(this.Obj).setMask(vValue);
    }

    this.datepicker_onSelect = function (dateText, inst) {
        this.Events.Change.Function();
    };

    this.SetVisible = function (vTmp) {
        if (vTmp)
            this.Obj.style.display = "block";
        else
            this.Obj.style.display = "none";
    }

    this.destruct = function () {
        if (this.SetAutoComplete_Active) {
            $(this.Obj).autocomplete("destroy");
        }

        $(this.Obj).remove();
    }

    this.IsDestroyed = function () {
        if (!document.getElementById(this.id))
            return true;
        else
            return false;
    }

    this.Password = false;
    this.SetPassword = function (vPassword) {
        this.Password = vPassword;
        if (this.Password)
            this.SetType("password");
        else
            this.SetType("text");

    }

    this.SetTypePosition = function (vTypePosition) {
        if (vTypePosition != "static")
            $(this.Obj).css({ position: vTypePosition });
        else {
            $(this.Obj).css({ position: "", left: "", top: "" });
        }
    }

    this.SetType = function (vType) {
        this.Obj = this.changeType(this.Obj, vType);
    }

    this.changeType = function (x, type) {
        x = $(x);
        if (x.prop('type') == type)
            return x[0]; //That was easy.
        try {
            return x.prop('type', type)[0]; //Stupid IE security will not allow this
        } catch (e) {
            //Try re-creating the element (yep... this sucks)
            //jQuery has no html() method for the element, so we have to put into a div first
            var html = $("<div>").append(x.clone()).html();
            var regex = /type=(\")?([^\"\s]+)(\")?/; //matches type=text or type="text"
            //If no match, we add the type attribute to the end; otherwise, we replace
            var tmp = $(html.match(regex) == null ?
                html.replace(">", ' type="' + type + '">') :
                html.replace(regex, 'type="' + type + '"'));
            //Copy data from old element
            tmp.data('type', x.data('type'));
            var events = x.data('events');
            var cb = function (events) {
                return function () {
                    //Bind all prior events
                    for (i in events) {
                        var y = events[i];
                        for (j in y)
                            tmp.bind(i, y[j].handler);
                    }
                }
            }(events);
            x.replaceWith(tmp);
            setTimeout(cb, 10); //Wait a bit to call function
            return tmp[0];
        }
    }

    this.SetStep = function (vstep) {
        this.Obj.step = vstep;
    }

    this.SetMax = function (vmax) {
        this.Obj.max = vmax;
    }

    this.SetMin = function (vMin) {
        this.Obj.min = vMin;
    }

    this.SetPattern = function (vpattern) {
        this.Obj.pattern = vpattern;
    }

    this.SetSelectionStart = function (vValue) {
        this.Obj.selectionStart = vValue;
    }

    this.SetSelectionEnd = function (vValue) {
        this.Obj.selectionEnd = vValue;
    }

    this.GetSelectionStart = function () {
        return this.Obj.selectionStart;
    }
    this.GetSelectionEnd = function () {
        return this.Obj.selectionEnd;
    }

    /*
    KeyBoard Old
    this.SetKeyboardEnable = function (vEnable) {
        try
        {
            $(this.Obj).keyboard().getkeyboard().destroy();
        } catch(err) { }

        if (vEnable) {
            if (this.KeyboardEnabletrueAncyTimer!=null)
                clearInterval(this.KeyboardEnabletrueAncyTimer);

            var vTmpthis = this;
            this.KeyboardEnabletrueAncyTimer = setInterval(function () { vTmpthis.KeyboardEnabletrueAncy(); }, 100);
        }
    }

    this.KeyboardEnabletrueAncyTimer = null;
    this.KeyboardEnabletrueAncy = function () {
        clearInterval(this.KeyboardEnabletrueAncyTimer);
        this.KeyboardEnabletrueAncyTimer = null;

       
        var customLayout = null;

        if (this._KeyboardCustomLayoutDefault != "")
            customLayout = { 'default': this._KeyboardCustomLayoutDefault };

        var vTmpThis = this;
        $(this.Obj).keyboard({
            visible: function(e, keyboard, el) {
                keyboard.$preview[0].select();
            },
            layout: this._KeyboardLayout,
            autoAccept: this._KeyboardAutoAccept,
            //usePreview: false,
            customLayout: customLayout,
            accepted: function (e, keyboard, el) {
                if (vTmpThis._Mask != null)
                    if ($(vTmpThis.Obj).val() != $.mask.string(el.value, vTmpThis._Mask))
                        $(vTmpThis.Obj).val($.mask.string(el.value, vTmpThis._Mask));
            }
        });
    }
    
    this._KeyboardLayout = "none";
    this.SetKeyboardLayout = function (vValue) {
        this._KeyboardLayout = vValue;
    }

    this._KeyboardAutoAccept = false;
    this.SetKeyboardAutoAccept = function (vValue) {
        this._KeyboardAutoAccept = vValue;
    }

    this._KeyboardCustomLayoutDefault = "";
    this.SetKeyboardCustomLayoutDefault = function (vValue) {
        this._KeyboardCustomLayoutDefault = vValue;
    }
    */


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

    $(this.Obj)
        .focus(function (e) { TmpThis.Events.Focus.Function(e); })
        .blur(function (e) { TmpThis.Events.LostFocus.Function(e); })
        //.click(function (e) { TmpThis.Events.Click.Function(e); })
        .keydown(function (e) { TmpThis.Events.KeyDown.Function(e); })
        .keyup(function (e) { TmpThis.Events.KeyUp.Function(e); })
        .keypress(function (e) { TmpThis.Events.KeyPress.Function(e); })
        .change(function (e) { TmpThis.Events.Change.Function(e); })
        .bind('input propertychange', function () {
            var maxLength = $(this).attr('maxlength');
            if (maxLength && $(this).val().length > maxLength) {
                $(this).val($(this).val().substring(0, maxLength));
            }
        });
    ;

    this.ControlVar = new ClassAraGenVarSend(this);
    this.ControlVar.AddPrototype("GetValue()");
    this.ControlVar.AddPrototype("Top");
    this.ControlVar.AddPrototype("Left");
    this.ControlVar.AddPrototype("Width");
    this.ControlVar.AddPrototype("Height");
    this.ControlVar.AddPrototype("GetSelectionStart()");
    this.ControlVar.AddPrototype("GetSelectionEnd()");

    this.Anchor = new ClassAraAnchor(this);
});