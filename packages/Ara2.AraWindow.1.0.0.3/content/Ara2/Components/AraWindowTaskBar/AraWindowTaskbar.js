// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
Ara.AraClass.Add('AraWindowTaskbar', function (vAppId, vId, ConteinerFather) {
    this.AppId = vAppId;
    this.id = vId;
    this.ConteinerFather = ConteinerFather;

    this.Obj = document.getElementById(this.id);
    if (!this.Obj) {
        alert("Object '" + this.id + "' Not Found");
        return;
    }

    var TmpThis = this;
    $(this.Obj).css({ position: "absolute", top: "0px", left: "0px", overflow: "auto" });
    this.Left = 0;
    this.Top = 0;

    $(this.Obj).addClass("ui-widget ui-state-default ui-corner-all ui-button-text-only");


    $(this.Obj).click(function () { TmpThis.Events.Click.Function(); });

    this.ControlVar = new ClassAraGenVarSend(this);

    this.Anchor = new ClassAraAnchor(this);
    this.Anchor.SetLeft(5);
    this.Anchor.SetRight(5);
    this.Anchor.SetBottom(5);
    

    // Eventos  ---------------------------------------
    this.Events = {};
    
    var TmpThis = this;
    this.Events.Click =
    {
        Enabled: false,
        ThreadType: 2, // Single_thread
        Function: function () {
            if (TmpThis.Events.Click.Enabled) {
                var vParans = new TmpThis.Janela.Ara.ClassAraComunicacao_Param();
                TmpThis.Janela.Send(TmpThis.id, "Click", vParans, TmpThis.Events.Click.ThreadType);
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

    this.Objects = new Array();

    this.GetObject = function (vId) {
        // IE 8
        //for (var n in this.Objects) {
        //    if (this.Objects[n].id == vId)
        //        return this.Objects[n];
        //}

        var vTmpThis = this;
        var vResu = jQuery.grep(this.Objects, function (n, i) {
            return n.id == vId;
        });
        if (vResu.length > 0)
            return vResu[0];

        return false;
    }

    this.DelObj = function (vId) {
        if (this.GetObject(vId))
            this.GetObject(vId).Obj.remove();

        // IE 8
        //for (var n in this.Objects) {
        //    if (this.Objects[n].id == vId) {
        //        delete this.Objects[n];
        //        return;
        //    }
        //}
        this.Objects = jQuery.grep(this.Objects, function (n, i) {
            return n.id != vId;
        });
    };

    this.AddObj = function (vId,vTitle) {
        var TmpThis = this;
        $("<a>").attr({ id: "taskbar_" + vId, href: "#" }).val(vTitle).button().click(function () { TmpThis.ClickButton(vId); }).appendTo(this.Obj);
        this.Objects.push({ id: vId, Obj: $("#taskbar_" + vId)});
    };

    this.SetTitleObj = function (vId,vTitle) {
        var vObj = this.GetObject(vId);
        if (vObj)
            vObj.Obj.button({ label: vTitle });
    };

    this.ModalZIndex = null;
    this.SetModalZIndex = function (vIndex) {
        this.ModalZIndex = vIndex;
        if (this.ModalZIndex == null) {
            // IE 8
            //for (var n in this.Objects) {
            //    this.Objects[n].Obj.button({ disabled: false });
            //}
            var vTmpThis = this;
            $(this.Objects).each(function () {
                this.Obj.button({ disabled: false });
            })
        }
        else {

            // IE 8
            //for (var n in this.Objects) {
            //    if (Ara.GetObject(this.Objects[n].id).GetZIndex() <= this.ModalZIndex)
            //        this.Objects[n].Obj.button({ disabled: true });
            //    else
            //        this.Objects[n].Obj.button({ disabled: false });
            //}

            var vTmpThis = this;
            $(this.Objects).each(function () {
                if (Ara.GetObject(this.id).GetZIndex() <= vTmpThis.ModalZIndex)
                    this.Obj.button({ disabled: true });
                else
                    this.Obj.button({ disabled: false });
            });

            $(this.Obj).css('z-index', this.ModalZIndex+1);
        }
    }

    Ara.AraWindows.EventAddObject.Add(function (vId, vTitle) { TmpThis.AddObj(vId, vTitle); });
    Ara.AraWindows.EventDelObject.Add(function (vId) { TmpThis.DelObj(vId); });
    Ara.AraWindows.EventSetTitle.Add(function (vId, vTitle) { TmpThis.SetTitleObj(vId, vTitle); });
    Ara.AraWindows.EventSetVisible.Add(function (vId, vVisible) { TmpThis.SetVisibleObj(vId, vVisible); });
    Ara.AraWindows.EventSetModalZIndex.Add(function (vZIndex) { TmpThis.SetModalZIndex(vZIndex); });
    

    this.SetVisibleObj = function (vId, vVisible) {
        var vObj = this.GetObject(vId);
        if (vObj) {
            if (vVisible)
                vObj.Obj.show();
            else
                vObj.Obj.hide();
        }
    };

    this.GetValue = function () {
        return this.Obj.innerHTML;
    };

    this.SetValue = function (vTmp) {
        this.Obj.innerHTML = vTmp;
    }

    this._Visible = false;
    this.SetVisible = function (vTmp) {

        if (vTmp != this._Visible) {
            this._Visible = vTmp;

            if (vTmp)
                this.Obj.style.display = "block";
            else
                this.Obj.style.display = "none";

            try {
                if (this.Anchor)
                    this.Anchor.FormResize();
            } catch (err) { }

            if (vTmp) {
                Ara.AraWindows.BorderBottom += parseInt(this.Height,10) + parseInt(this.Anchor.Bottom,10);
                Ara.AraWindows.Taskbar = true;
            }
            else {
                Ara.AraWindows.BorderBottom -= parseInt(this.Height, 10) + parseInt(this.Anchor.Bottom, 10);
                Ara.AraWindows.Taskbar = false;
            }
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


    this.AddClass = function (vTmp) {
        this.ObjJQuery.addClass(vTmp);
    }

    this.DelClass = function (vTmp) {
        this.ObjJQuery.removeClass(vTmp);
    }

    this.destruct = function () {
        $(this.Obj).remove();
        Ara.AraWindows.BorderBottom -= this.Height + this.Anchor.Bottom;
        Ara.AraWindows.Taskbar = false;
    }

    this.ClickButton = function (vId) {
        if (Ara.GetObject(vId).Minimized)
            Ara.GetObject(vId).SetMinimized(false);

        Ara.GetObject(vId).SetZIndex(Ara.AraWindows.GetNewZIndex());
    }

    this.SetTypePosition = function (vTypePosition) {
        if (vTypePosition != "static")
            $(this.Obj).css({ position: vTypePosition });
        else {
            $(this.Obj).css({ position: "", left: "", top: "" });
        }
    }

    this.SetHeight(27);
    this.SetVisible(true);
});