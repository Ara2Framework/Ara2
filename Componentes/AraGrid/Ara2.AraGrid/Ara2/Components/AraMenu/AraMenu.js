// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
Ara.AraClass.Add('AraMenu', function (vAppId, vId, ConteinerFather) {

    this.Create = function (vCreate) {
        this.ObjCreate = vCreate;

        if (this.Obj) {

            var Tmpthis = this;

            var positionHelper = document.getElementById( this.id + "_positionHelper");
            if (positionHelper) {
                positionHelper.parentNode.removeChild(positionHelper);
            }
            /*
            this.ObjMenuPlugin = $(this.Obj).menuplugin({
                content: vCreate.data,
                flyOut: vCreate.FlyOut,
                onShowMenu: function () {
                    Tmpthis.onShowMenu();
                },
                onCloseMenu: function () {
                    Tmpthis.onCloseMenu();
                }
            });
            */

            vCreate.onShowMenu = function () {
                Tmpthis.onShowMenu();
            };

            vCreate.onCloseMenu = function () {
                Tmpthis.onCloseMenu();
            };

            this.ObjMenuPlugin = $(this.Obj).menuplugin(vCreate);
            

            $(this.Obj).bind('contextmenu click', function (e) {
                Tmpthis.OnClick("-1", e);
                return false;
            });

        }
    }

    

    this.CapturaMouse = false;
    this.CapturaMouse_X = 0;
    this.CapturaMouse_Y = 0;

    this.CapturaMouse_Abilita = function (vF) {
        if (this.CapturaMouse == false) {
            var TmpThis = this;
            $(document).mousemove(function (e) {
                TmpThis.CapturaMouse_X = e.pageX;
                TmpThis.CapturaMouse_Y = e.pageY;

                if (vF) {
                    TmpThis.CapturaMouse = true;

                    vF();
                    vF = false;
                }
            });

        }
        else {
            vF();
        }
    }

    this.getPosicaoElemento = function (offsetTrail) {
        var offsetLeft = 0;
        var offsetTop = 0;
        while (offsetTrail) {
            offsetLeft += offsetTrail.offsetLeft;
            offsetTop += offsetTrail.offsetTop;
            offsetTrail = offsetTrail.offsetParent;
        }
        if (navigator.userAgent.indexOf("Mac") != -1 &&
        typeof document.body.leftMargin != "undefined") {
            offsetLeft += document.body.leftMargin;
            offsetTop += document.body.topMargin;
        }
        return { left: offsetLeft, top: offsetTop };
    }


    this.ShowMenusPos = null;
    this.showMenu = function (vPos) {
        if (vPos) {
            if (vPos.left) {
                this.ShowMenusPos = vPos;

                this.ObjMenuPlugin.showMenu();
                this.ShowMenusPos = null;
            } else if (vPos.vObjId) {
                var TmpLeftTop = this.getPosicaoElemento(Ara.GetObject(vPos.vObjId).Obj);

                if (vPos.Side == 2) // BottomLeftCorner 
                {
                    TmpLeftTop.top += $(Ara.GetObject(vPos.vObjId).Obj).height();
                } else if (vPos.Side == 3) // BottomRightCorner 
                {
                    TmpLeftTop.left += $(Ara.GetObject(vPos.vObjId).Obj).width();
                    TmpLeftTop.top += $(Ara.GetObject(vPos.vObjId).Obj).height();
                }

                this.showMenu(TmpLeftTop);
            }

        }
        else {
            var TmpThis = this;
            this.CapturaMouse_Abilita(function () {
                TmpThis.ShowMenusPos = { left: TmpThis.CapturaMouse_X, top: TmpThis.CapturaMouse_Y };

                TmpThis.ObjMenuPlugin.showMenu();
                TmpThis.ShowMenusPos = null;
            });
        }

        
    }

    this.onShowMenu = function () {
        this.EstanciaEventos();

        var Tmp = { left: 0, top: 0 };
        if (this.ShowMenusPos)
            Tmp = this.ShowMenusPos;
        else
            Tmp = getPosicaoElemento(this.Obj);

        $(".positionHelper").css({
            top: Tmp.top + "px",
            left: Tmp.left + "px",
            zIndex: Ara.GetMaxZIndexByObject(this) + 1,
        });

        

        var container = document.getElementById( this.id + "_container");
        if (container) {
            container.style.overflow = "visible";
            container.style.zIndex = (Ara.GetMaxZIndexByObject(this) + 1);
        }

        if (!this.Enabled)
            container.style.display = 'none';
        else
            container.style.display = '';
    }

    this.onCloseMenu = function () {
        /*
        if (this.ObjCreate.ButtonShowCaption == "") {
            this.Obj.style.position = "absolute";
            this.Obj.style.top = "-500px";
            this.Obj.style.left = "-500px";
        }
        */
    }

    

    this.SetVisible = function (vTmp) {
        if (vTmp)
            this.Obj.style.display = "block";
        else
            this.Obj.style.display = "none";
    }



    this.OnClick = function (vKey, event) {

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
            Mouse_shiftKey: event.shiftKey,
            key: vKey
        };

        Ara.Tick.Send(2, this.AppId, this.id, "Click", vParans);
    }

    this.Events = {};
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

    this.IsDestroyed = function () {
        if (!document.getElementById(this.id))
            return true;
        else
            return false;
    }

    this.SetTypePosition = function (vTypePosition) {
        if (vTypePosition != "static")
            $(this.Obj).css({ position: vTypePosition });
        else {
            $(this.Obj).css({ position: "", left: "", top: "" });
        }
    }

    this.Enabled = true;
    this.SetEnabled = function (vValue) {
        this.Enabled = vValue;
        if (!this.Enabled)
            $(this.Obj).addClass("ui-button-disabled ui-state-disabled");
        else
            $(this.Obj).removeClass("ui-button-disabled ui-state-disabled");
    }

    this.ElementoClick = new Array();

    this.CriaEventoClick = function (vId, vKey) {
        this.ElementoClick.push({ id: vId, Key: vKey });
    }

    this.destruct = function () {
        try {
            this.ObjMenuPlugin.kill();
        } catch (err) { }
        $(this.Obj).remove();
    }

    this.EstanciaEventos = function () {
        var TmpThis = this;
        for (var Tmp in this.ElementoClick) {
            var Obj = this.ElementoClick[Tmp];
            
            $("#" + Obj.id).bind('contextmenu click', function (e) {
                var vKey = e.target.getAttribute("IdInternal");
                if (!vKey)
                    vKey = e.target.parentNode.getAttribute("IdInternal");
                TmpThis.OnClick(vKey, e);
                return false;
            });
        }
        this.ElementoClick = new Array();
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

    this.ControlVar = new ClassAraGenVarSend(this);
    this.Anchor = new ClassAraAnchor(this);
});