// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
Ara.AraClass.Add('AraSplitContainers', function (vAppId, vId, ConteinerFather) {
    

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

    this.Events.IsVisible =
    {
        Enabled: false,
        ThreadType: 1,
        Function: function () {
            if (this.Enabled) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "IsVisible", null);
            }
        }
    }

    this.Events.onReziseSplitContainer =
    {
        Enabled: false,
        ThreadType: 1,
        Function: function (vIdSplitContainer) {
            if (this.Enabled) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "onReziseSplitContainer", { "IdSplitContainer": vIdSplitContainer });
            }
        }
    }

    this.Orientation = "V";
    this.SetOrientation = function (vValue) {
        this.Orientation = vValue;
        this.RecalculaPerReal();
        this.Refresh();
    }
    

    this.Objects = new Array();

    // IE 8
    //if (!this.Objects.contains)
    //    this.Objects.contains = function (a) { return this.indexOf(a) != -1 }
    //if (!this.Objects.remove)
    //    this.Objects.remove = function (a) { if (this.contains(a)) { this.splice(this.indexOf(a), 1) }; return this }

    this.AddSplitContainer = function (vKey) {
        this.Objects.push(vKey);

        $("#" + vKey).css("overflow", "hidden");
        //$("#" + vKey).attr("style", "overflow:hidden !important");

        this.RecalculaPerReal();

        var TmpThis = this;
        setTimeout(function () { TmpThis.Refresh(); }, 100);
    }

    this.delSplitContainer = function (vKey) {
        // IE 8
        //this.Objects.remove(vKey);
        this.Objects = jQuery.grep(this.Objects, function (n, i) {
            return n != vKey;
        });

        this.RecalculaPerReal();

        var TmpThis = this;
        setTimeout(function () { TmpThis.Refresh(); }, 100);
    }

    this.SetPercent = function (vKey, vValue) {
        $("#" + vKey).attr("Percent", vValue);
        this.RecalculaPerReal();
        this.Refresh();
    }

    this.RecalculaPerReal = function () {
        var PercentMax = 0;
        for (vKey = 0; vKey < this.Objects.length; vKey++) {
            var vTmpPercent = $("#" + this.Objects[vKey]).attr("Percent");
            if (vTmpPercent > 0)
                PercentMax = PercentMax + parseInt(vTmpPercent, 10);

        }

        //Acetar Porcentagem Real
        for (vKey = 0; vKey < this.Objects.length; vKey++) {
            var vTmpPercent = $("#" + this.Objects[vKey]).attr("Percent");
            if (vTmpPercent >0) {
                var PercentReal = (vTmpPercent / PercentMax) * 100;
                $("#" + this.Objects[vKey]).attr("PercentReal", PercentReal)
            }
        }
    }

    this.RecalculaPerRealReferencia = function (vId, vValor,vPercNova) {
        
        var PercAtual = $("#" + vId).attr("PercentReal");
        var PercDif = PercAtual - vPercNova;

        var PercentMax = 0;
        for (vKey = 0; vKey < this.Objects.length; vKey++) {
            if (vId != this.Objects[vKey]) {
                var vTmpPercent = $("#" + this.Objects[vKey]).attr("PercentReal");
                if (vTmpPercent > 0) 
                    PercentMax += parseFloat(vTmpPercent, 10);
            }
        }

        for (vKey = 0; vKey < this.Objects.length; vKey++) {
            if (vId != this.Objects[vKey]) {
                var vTmpPercent = $("#" + this.Objects[vKey]).attr("PercentReal");
                if (vTmpPercent > 0) {
                    var vTmpPerc = ((parseFloat(vTmpPercent, 10) / PercentMax) * 100);
                    if (vTmpPerc > 100)
                        vTmpPerc = 100;
                    $("#" + this.Objects[vKey]).attr("PercentReal2", vTmpPerc)
                }
            }
        }

        for (vKey = 0; vKey < this.Objects.length; vKey++) {
            if (vId != this.Objects[vKey]) {
                var vTmpPercent = $("#" + this.Objects[vKey]).attr("PercentReal2");
                if (vTmpPercent > 0) {                    
                    var vPerc = parseFloat($("#" + this.Objects[vKey]).attr("PercentReal"), 10) + (PercDif * (parseFloat(vTmpPercent, 10) / 100));
                    if (vPerc < 0) {
                        vPerc = 0.01;
                        vPercNova -= 0.01;
                    }
                    $("#" + this.Objects[vKey]).attr("PercentReal", vPerc);
                }
            }

        }

        $("#" + vId).attr("PercentReal", vPercNova);
    }

    this.GetAreaUtil = function () {
        
        // Calcula Fixo
        var TmFixos = 0;
        for (vKey = 0; vKey < this.Objects.length; vKey++) {
            if (!Ara.GetObject(this.Objects[vKey]))
                return;

            var vTmpPercent = $("#" + this.Objects[vKey]).attr("Percent");
            if (vTmpPercent == null) {
                $("#" + this.Objects[vKey]).attr("PercentReal", "")
                if (this.Orientation == "V")
                    TmFixos += parseFloat(Ara.GetObject(this.Objects[vKey]).Width, 10);
                else
                    TmFixos += parseFloat(Ara.GetObject(this.Objects[vKey]).Height, 10);
            }
        }

        var AreaUtil = 0;
        if (this.Orientation == "V")
            AreaUtil = parseInt(this.Width, 10) - TmFixos;
        else
            AreaUtil = parseInt(this.Height, 10) - TmFixos;

        return AreaUtil;
    }

    this.RefreshExc = false;
    this.Refresh = function () {
        if (this.RefreshExc)
            return;
        this.RefreshExc = true;
        
        var AreaUtil = this.GetAreaUtil();

        
        // Acerta Largura e Inicio
        var vPos = 0;
        for (vKey = 0; vKey < this.Objects.length; vKey++) {
            var PercReal = $("#" + this.Objects[vKey]).attr("PercentReal");
            var Primeiro = vKey == 0;
            var Ultimo = vKey == this.Objects.length-1;

            try
            {
                $("#" + this.Objects[vKey]).resizable("destroy");
            } catch (err) { }

            if (PercReal != "") {
                var vValor = AreaUtil * (PercReal/100);

                if (this.Orientation == "V") {
                    Ara.GetObject(this.Objects[vKey]).SetTop('0px');
                    Ara.GetObject(this.Objects[vKey]).SetHeight(parseInt(this.Height, 10) + "px");

                    Ara.GetObject(this.Objects[vKey]).SetLeft(vPos + "px");
                    Ara.GetObject(this.Objects[vKey]).SetWidth(parseInt(vValor,10) + "px");
                }
                else {
                    
                    Ara.GetObject(this.Objects[vKey]).SetLeft('0px');
                    Ara.GetObject(this.Objects[vKey]).SetWidth(parseInt(this.Width, 10) + "px");

                    Ara.GetObject(this.Objects[vKey]).SetTop(vPos + "px");
                    Ara.GetObject(this.Objects[vKey]).SetHeight(parseInt(vValor,10) + "px");
                }

                var vHandles;
                if (this.Orientation == "V") {
                    if (Primeiro)
                        vHandles = "e";
                    else if (Ultimo)
                        vHandles = "w";
                    else
                        vHandles = "w, e ";
                }
                else {
                    if (Primeiro)
                        vHandles = "s";
                    else if (Ultimo)
                        vHandles = "n";
                    else
                        vHandles = "n, s ";
                }

                var TmpThis = this;
                $("#" + this.Objects[vKey]).resizable({
                    handles:vHandles,
                    resize: function (event, ui) {
                        if (TmpThis.Orientation == "V") {
                            ui.size.height = ui.originalSize.height;
                        }
                        else {
                            ui.size.width = ui.originalSize.width;
                        }
                    },
                    stop: function (event, ui) {
                        var vId = ui.element[0].id;
                        var vTmpValor = 0;
                        if (TmpThis.Orientation == "V") {
                            vTmpValor = ui.size.width;
                            //Ara.GetObject(vId).Height = ui.size.height + "px";
                        }
                        else {
                            vTmpValor = ui.size.height;
                            //Ara.GetObject(vId).Width = ui.size.width + "px";
                        }
                        
                        TmpThis.SetLaguraContainer(vId, vTmpValor);
                    }
                });
            }
            else {

                if (this.Orientation == "V") {
                    Ara.GetObject(this.Objects[vKey]).SetTop('0px');
                    Ara.GetObject(this.Objects[vKey]).SetHeight(parseInt(this.Height, 10) + "px");
                }
                else {
                    Ara.GetObject(this.Objects[vKey]).SetLeft('0px');
                    Ara.GetObject(this.Objects[vKey]).SetWidth(parseInt(this.Width, 10) + "px");
                }

                if (this.Orientation == "V") 
                    Ara.GetObject(this.Objects[vKey]).SetLeft(vPos + "px");
                else 
                    Ara.GetObject(this.Objects[vKey]).SetTop(vPos + "px");

                var vValor = 0;
                if (this.Orientation == "V")
                    vValor = parseInt(Ara.GetObject(this.Objects[vKey]).Width,10);
                else 
                    vValor = parseInt(Ara.GetObject(this.Objects[vKey]).Height,10);
                
            }
            vPos += vValor;
        }


        this.RefreshExc = false;

    }

    this.SetLaguraContainer = function (vId, vTmpValor)
    {
        var AreaUtil = this.GetAreaUtil();

        var vPerc = ((vTmpValor / AreaUtil) * 100);
        if (vPerc > 100) {
            vPerc = 100;
            vTmpValor = AreaUtil;
        }

        this.RecalculaPerRealReferencia(vId, vTmpValor, vPerc);
        this.Refresh();
        this.Events.onReziseSplitContainer.Function(vId);
    }


    this.SetVisible = function (vTmp) {
        if (vTmp)
            this.Obj.style.display = "block";
        else
            this.Obj.style.display = "none";
    }

    
    this.Left = null;
    this.SetLeft = function (vTmp) {
        if (this.Left != vTmp) {
            this.Left = vTmp;
            this.Obj.style.left = vTmp ;
        }
    }

    this.Top = null;
    this.SetTop = function (vTmp) {
        if (this.Top != vTmp) {
            this.Top = vTmp;
            this.Obj.style.top = vTmp ;
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

            this.Refresh();
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

            this.Refresh();
        }
    }

    
    this.SetOverFlow = function (vTmp) {
        this.Obj.style.overflow = vTmp;
    }

    this.AddClass = function (vTmp) {
        this.ObjJQuery.addClass(vTmp);
    }

    this.DelClass = function (vTmp) {
        this.ObjJQuery.removeClass(vTmp);
    }

    this.destruct = function () {
        $(this.Obj).remove();
    }

    this.IsDestroyed = function () {
        if (!document.getElementById(this.id))
            return true;
        else
            return false;
    }

    this.SetTypePosition = function (vTypePosition) {
        if (vTypePosition != "static")
            $(this.Obj).css({ position: vTypePosition });
        else 
            $(this.Obj).css({ position: "", left: "", top: "" });
    }

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

    var TmpThis = this;


    this.ControlVar = new ClassAraGenVarSend(this);
    this.ControlVar.AddPrototype("Top");
    this.ControlVar.AddPrototype("Left");
    this.ControlVar.AddPrototype("Width");
    this.ControlVar.AddPrototype("Height");
    //this.ControlVar.AddPrototype("TabActiveId");
    //this.ControlVar.AddPrototype("GetPosTabs()");
    

    this.Anchor = new ClassAraAnchor(this);
});