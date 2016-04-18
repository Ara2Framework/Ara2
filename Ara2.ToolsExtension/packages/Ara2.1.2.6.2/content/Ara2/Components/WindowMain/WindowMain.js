// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
Ara.AraClass.Add('AraWindowMain', function (vAppId, vId, ConteinerFather) {
    

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

    this.Events.PopState =
    {
        Enabled: false,
        SetEnabled: function (vValue) {
            var TmpThis2 = this;
            if (vValue && this.PrimeiraAtivacaoEvento != true) {
                $(window).bind("popstate", function (e) { TmpThis2.Function(e); });

                this.PrimeiraAtivacaoEvento = true;
            }

            this.Enabled = vValue;
        },
        ThreadType: 2, // Single_thread
        Function: function (event) {
            if (this.Enabled) {
                var vParans = {
                    URL: window.location.href
                };

                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "PopState", vParans);
            }
        }
    };

    this.Events.ResizeStop =
    {
        Enabled: false,
        ThreadType: 2, // Single_thread
        Function: function () {
            if (this.Enabled) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "ResizeStop", null);
            }
        }
    }

    this.GetCookieReturn = function (vId,vName) {
        Ara.Tick.Send(1, this.AppId, this.id, "GetCookieReturn", { id: vId, value: $.cookie(vName) });
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
            var ctrlKey = e.ctrlKey;
            var altKey = e.altKey;
            var shiftKey = e.shiftKey;

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
                        Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "KeyDown", { Key: vkeyCode, "ctrlKey": ctrlKey, "altKey": altKey, "shiftKey": shiftKey });
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
            var ctrlKey = e.ctrlKey;
            var altKey = e.altKey;
            var shiftKey = e.shiftKey;

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
                        Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "KeyUp", { Key: vkeyCode, "ctrlKey": ctrlKey, "altKey": altKey, "shiftKey": shiftKey });
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
            var ctrlKey = e.ctrlKey;
            var altKey = e.altKey;
            var shiftKey = e.shiftKey;

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
                        Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "KeyPress", { Key: vkeyCode, "ctrlKey": ctrlKey, "altKey": altKey, "shiftKey": shiftKey });
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

    this.Events.Menssage =
    {
        Enabled: false,
        SetEnabled: function (vValue) {
            var TmpThis2 = this;
            if (vValue && this.PrimeiraAtivacaoEvento != true) {
                $(window).on("message onmessage", function (e) {
                    TmpThis2.Function(e);
                });
                this.PrimeiraAtivacaoEvento = true;
            }
            this.Enabled = vValue;
        },
        ThreadType: 2, // Single_thread
        Function: function (e) {
            if (this.Enabled) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "Menssage", { vMenssage: e.originalEvent.data });
                
            }
        }
    };



    this.Events.unload =
    {
        Enabled: false,
        ThreadType: 1, // Multiple_thread
        AlreadySent: false,
        Function: function () {
            if (this.Enabled && !this.AlreadySent ) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "unload", null, false);
                this.AlreadySent = true;
                waitSeconds(5);
            }
        }
    };

    function waitSeconds(iMilliSeconds) {
        var counter = 0
            , start = new Date().getTime()
            , end = 0;
        while (counter < iMilliSeconds) {
            end = new Date().getTime();
            counter = end - start;
        }
    }

    $(window).resize(function (v1,v2) {
        if (!v2) { // Vindo do rezise da janela
            TmpThis.Width = $(window).width();
            TmpThis.ControlVar.SetValueUtm("Width", this.Width);
            TmpThis.Obj.style.width = TmpThis.Width + "px";

            TmpThis.Height = $(window).height();
            TmpThis.ControlVar.SetValueUtm("Height", this.Height);
            TmpThis.Obj.style.height = TmpThis.Height + "px";

            TmpThis.Anchor.RenderChildren();
            TmpThis.Events.ResizeStop.Function();
        }
    });

    this.Left = null;
    this.SetLeft = function (vTmp) {
        this.Left = vTmp;
        //Error IE 7
        //this.Obj.style.left = vTmp + "px";
        // IE 7
        this.Obj.style.left = vTmp ;
    }

    this.Top = null;
    this.SetTop = function (vTmp) {
        this.Top = vTmp;
        //Error IE 7
        //this.Obj.style.top = vTmp + "px";
        // IE 7
        this.Obj.style.top = vTmp;
    }

    this._MinWidth = null;
    this.SetMinWidth = function (vTmp) {
        this._MinWidth = vTmp;
        if (this._MinWidth != null && this.Width != null && parseInt(this._MinWidth, 10) > parseInt(this.Width, 10))
            this.SetWidth(this._MinWidth, false);

        this.SetWidth($(window).width() + "px");
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

        this.SetHeight($(window).height() + "px");
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


    this.AppId = vAppId;
    this.id = vId;
    this.ConteinerFather = ConteinerFather;

    var ObjFatther;
    if (ConteinerFather == null)
        ObjFatther = document;
    else
        alert("Falta");
    this.Obj = ObjFatther.getElementById(this.id);
    if (!this.Obj) {
        alert("Object '" + this.id + "' Not Found");
        return;
    }

    $(this.Obj).css({ position: "absolute", top: "0px", left: "0px" });

    $(window)
        .keydown(function (e) { TmpThis.Events.KeyDown.Function(e); })
        .keyup(function (e) { TmpThis.Events.KeyUp.Function(e); })
        .keypress(function (e) { TmpThis.Events.KeyPress.Function(e); })
        .unload(function (e) { TmpThis.Events.unload.Function(e); })
        .on('beforeunload', function (e) { TmpThis.Events.unload.Function(e); })
    ;   

    var TmpThis = this;
    //$(this.Obj).click(function () { TmpThis.Events.Click.Function(); });

    this.ControlVar = new ClassAraGenVarSend(this);
    this.ControlVar.AddPrototype("Width");
    this.ControlVar.AddPrototype("Height");
    

    this.Anchor = new ClassAraAnchor(this);
    //this.Anchor.SetLeft(0);
    //this.Anchor.SetTop(0);
    //this.Anchor.SetRight(0);
    //this.Anchor.SetBottom(0);


    var TmpThis = this;
    setTimeout(function () {
        TmpThis.SetWidth($(window).width() + "px");
        TmpThis.SetHeight($(window).height() + "px");
    }, 100);
});