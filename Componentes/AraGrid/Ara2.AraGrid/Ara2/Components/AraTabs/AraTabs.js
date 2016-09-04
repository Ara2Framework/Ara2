// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
Ara.AraClass.Add('AraTabs', function (vAppId, vId, ConteinerFather) {
    

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

    this.Events.ClickCloseTab =
    {
        Enabled: false,
        ThreadType: 2, // Single_thread
        Function: function (vTabKey) {
            //if (this.Enabled) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "ClickCloseTab", { "TabKey": vTabKey });
            //}
            //else
            //    TmpThis.delTab(vTabKey);
        }
    };

    this.TabActiveId = null;

    this.Events.TabActiveChange =
    {
        Enabled: false,
        ThreadType: 2, // Single_thread
        Function: function (vTabKey) {
            if (this.Enabled) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "TabActiveChange", { "TabKey": vTabKey });
            }
        }
    };

    this.Events.OnSort =
    {
        Enabled: false,
        ThreadType: 2, // Single_thread
        Function: function (vTabKey) {
            if (this.Enabled) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "OnSort", null);
            }
        }
    };
    
    this.Events.ChangeTabsHeigth =
    {
        Enabled: false,
        ThreadType: 1, // Single_thread
        Function: function (vTabKey) {
            if (this.Enabled) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "ChangeTabsHeigth", { "GetTabsHeigth": TmpThis.GetTabsHeigth() });
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

    this.SeCaptionTab = function (vKey, vCaption) {
        $("#" + vKey + "_caption").html(vCaption);

        this.IsChangeTabsHeigth();
    }

    this.SeTabPos = function (vKey, vPos) {
        var vTmpThis = this;
        setTimeout(function () { vTmpThis.SeTabPosAnc(vKey, vPos); }, 100);
    }

    this.SeTabPosAnc = function (vKey, vPos) {
        
        var PosAtual = this.GetPosTab(vKey);
        var Dif = PosAtual - vPos;
        if (Dif > 0) {
            for (n = 0; n < Dif; n++) {
                $("#" + vKey + "_li").prev().insertAfter($("#" + vKey + "_li"));
            }
        }
        else {
            Dif = Dif * -1;
            for (n = 0; n < Dif; n++) {
                $("#" + vKey + "_li").next().insertBefore($("#" + vKey + "_li"));
            }
        }

        if (Dif!=0)
            this.Events.OnSort.Function();
    }

    this.GetPosTab = function (vKey) {
        //
        //return $("#" + vKey + "_li")[0].childElementCount - 1;
        return $("#" + vKey + "_li").prevAll().length;
        //return $("#" + vKey + "_li").attr("tabindex");
    }

    this.GetPosTabs = function () {
        var Tmp = new Array();
        var tmpthis = this;
        $(this.Obj).find(".ui-tabs-nav").children().each(function (index) {
            Tmp.push({ key: $(this).attr("arakey"), pos: tmpthis.GetPosTab($(this).attr("arakey")) });
        });

        return JSON.stringify(Tmp);
    }

    this.GetDisabledTab = function (vKey) {
        return this.tabsDisabled.contains(vKey);
    }

    this.tabsDisabled = new Array();
    if (!this.tabsDisabled.contains)
        this.tabsDisabled.contains = function (a) { return this.indexOf(a) != -1 };

    if (!this.tabsDisabled.remove)
        this.tabsDisabled.remove = function (a) { if (this.contains(a)) { this.splice(this.indexOf(a), 1) }; return this; };

    this.SetEnableTab = function (vKey, vValue) {
        if (!vValue) {
            if (!this.tabsDisabled.contains(vKey))
                this.tabsDisabled.push(vKey);
        }
        else 
            this.tabsDisabled.remove(vKey);
        
        this.ReplaceDisabledTabs();
    }

    this.ReplaceDisabledTabs = function () {
        var vTmp = new Array();

        for (var vTmpKey = 0; vTmpKey < this.tabsDisabled.length; vTmpKey++)
            vTmp.push(this.GetPosTab(this.tabsDisabled[vTmpKey]))

        $(this.Obj).tabs("option", "disabled", vTmp);
    }

    this.SetVisibleTab = function (vKey, vValue) {
        if (vValue)
            $("#" + vKey + "_li").show();
        else
            $("#" + vKey + "_li").hide();
    }
    

    this.SeCloseTab = function (vKey, vValue) {
        if (vValue) {
            if ($("#" + vKey + "_li").find("span.ui-icon-close").length == 0) {
                setTimeout(function () {
                    $("<span role='presentation'> </span>")
                        .addClass("ui-icon ui-icon-close")
                        .appendTo($("#" + vKey + "_li"));
                }, 100);
            }
        }
        else {
            setTimeout(function () {
                $("#" + vKey + "_li").find("span.ui-icon-close").remove();
            }, 150);
        }
    }

    this.GetTabActive = function () {
        $(".selector").tabs("option", "active");
    }

    this.AddTab = function (vKey) { 
        var li = "<li id='" + vKey + "_li' arakey='" + vKey + "'><a href='#" + vKey + "' id='" + vKey + "_caption' arakey='" + vKey + "'> </a> </li>";

        $(this.Obj).find(".ui-tabs-nav").append(li);
        this.TabsRefreshAsc();
    }

    this.TabsRefreshAsc = function () {
        var TmpThis = this;
        setTimeout(function () { TmpThis.TabsRefresh(); }, 100);
    }

    this.TabsRefresh = function () {
        $(this.Obj).tabs("refresh");
        this.ReplaceDisabledTabs();
        this.IsChangeTabsHeigth();
    }


    this.delTab = function (vKey) {
        try
        {
            $("#" + vKey + "_li").remove();
            $("#" + vKey).remove();
            $(this.Obj).tabs("refresh");

            this.IsChangeTabsHeigth();
            this.ReplaceDisabledTabs();
        } catch (err) { }
    }

    this.GetValue = function () {
        return this.Obj.innerHTML;
    }

    this.SetValue = function (vTmp) {
        this.Obj.innerHTML = vTmp;
    }

    this.SetVisible = function (vTmp) {
        if (vTmp)
            this.Obj.style.display = "block";
        else
            this.Obj.style.display = "none";
    }

    this.TextAdd = function (vTmp) {
        //this.Obj.innerHTML += vTmp;
        this.TmpTextAdd += vTmp;
    }

    this.TextAddEnd = function () {
        this.Obj.innerHTML += this.TmpTextAdd;
        this.TmpTextAdd = "";
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

    
    this.SetOverFlow = function (vTmp) {
        this.Obj.style.overflow = vTmp;
    }

    this.SetAling = function (vTmp) {
        this.Obj.innerHTML = vTmp;
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

    this.TabsHeigthOld = 41;
    this.GetTabsHeigth = function () {
        var vValue = $(this.Obj).find(".ui-tabs-nav").height();
        if (vValue < 32) vValue = 32;
        return parseInt(vValue + 9,10);
    }

    this.IsChangeTabsHeigth = function () {
        var Resu = (this.TabsHeigthOld != this.GetTabsHeigth());
        this.TabsHeigthOld = this.GetTabsHeigth();
        if (Resu)
            this.Events.ChangeTabsHeigth.Function();
        return Resu;
    }

    this.SetTypePosition = function (vTypePosition) {
        if (vTypePosition != "static")
            $(this.Obj).css({ position: vTypePosition });
        else 
            $(this.Obj).css({ position: "", left: "", top: "" });
    }

    this.SetSortable = function (vValue) {
        if (vValue) {
            var TmpThis = this;
            $(this.Obj).find(".ui-tabs-nav").sortable({
                axis: "x",
                stop: function () {
                    $(TmpThis.Obj).tabs("refresh");
                    TmpThis.Events.OnSort.Function();
                }
            });
        }
        else {
            try
            {
                $(this.Obj).find(".ui-tabs-nav").sortable("destroy");
            } catch (err) { }
        }

    }

    this.SetTabActiveId = function (vKey) {
        var vTmpThis = this;
        setTimeout(function () { vTmpThis.SetTabActiveIdAncy(vKey); }, 500);
    }

    this.SetTabActiveIdAncy = function (vKey) {
        this.TabsRefresh();
        $(this.Obj).tabs("option", "active", this.GetPosTab(vKey));
        //$(this.Obj).tabs({ active: this.GetPosTab(vKey) });
        //$(this.Obj).tabs({ selected: this.GetPosTab(vKey) });
    }

    this.AjustaNiceScroll = function () {
        var AraKeyTabs = new Array();
        var tmpthis = this;
        $(this.Obj).find(".ui-tabs-nav").children().each(function (index) {
            var vTmpID = $(this).attr("arakey");
            if (this.TabActiveId != vTmpID)
                AraKeyTabs.push(vTmpID);
        });

        $(AraKeyTabs).each(function (index) {
            var vIdTab = this;
            Ara.AraScrollBar.ResizeHide($("#" + vIdTab));
        });

        Ara.AraScrollBar.ResizeShow($("#" + this.TabActiveId));
    }

    this.AppId = vAppId;
    this.id = vId;
    this.ConteinerFather = ConteinerFather;

    this.Obj = document.getElementById(this.id);
    if (!this.Obj) {
        alert("Object '" + this.id + "' Not Found");
        return;
    }

    this.Obj.innerHTML = "<ul></ul>";

    var TmpThis = this;
    $(this.Obj).css({ position: "absolute", top: "0px", left: "0px" });
    this.Left = 0;
    this.Top = 0;

    var TmpThis = this;

    $(this.Obj).tabs();

    $(this.Obj).on("tabsactivate", function (event, ui) {
        if (ui.newPanel.parent()[0].id == TmpThis.Obj.id) {// Verifica se é da mesma tab
            if (TmpThis.TabActiveId != ui.newPanel[0].id) {
                TmpThis.TabActiveId = ui.newPanel[0].id;
                TmpThis.Events.TabActiveChange.Function(TmpThis.TabActiveId);

                TmpThis.AjustaNiceScroll();
            }
        }
    });

    
    $(this.Obj).delegate("span.ui-icon-close", "click", function () {
        var panelId = $(this).closest("li").attr("aria-controls");
        if (TmpThis.GetDisabledTab(panelId)==false)
            TmpThis.Events.ClickCloseTab.Function(panelId);
        return false;
    });

    this.ControlVar = new ClassAraGenVarSend(this);
    this.ControlVar.AddPrototype("Top");
    this.ControlVar.AddPrototype("Left");
    this.ControlVar.AddPrototype("Width");
    this.ControlVar.AddPrototype("Height");
    this.ControlVar.AddPrototype("TabActiveId");
    this.ControlVar.AddPrototype("GetPosTabs()");
    this.ControlVar.AddPrototype("GetTabsHeigth()");
    
    

    this.Anchor = new ClassAraAnchor(this);

    this.SetWidth('200px', true);
    this.SetHeight('200px', true);
});