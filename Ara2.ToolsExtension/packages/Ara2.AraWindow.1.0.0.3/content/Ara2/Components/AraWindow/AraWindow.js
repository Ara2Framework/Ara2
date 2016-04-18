// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
Ara.AraClass.Add('AraWindow', function (vAppId, vId, ConteinerFather) {
    var TmpThis = this;

    // Eventos  ---------------------------------------
    this.Events = {};

    this.Events.Unload =
    {
        Enabled: false,
        ThreadType: 2, // Single_thread
        Function: function () {
            Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "Unload", null);
        }
    }

    this.Events.DragStop =
    {
        Enabled: false,
        ThreadType: 2, // Single_thread
        Function: function () {
            if (this.Enabled) {
                Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "DragStop", null);
            }
        }
    }

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


    this.ButtonMnimize = false;
    this.SetButtonMnimize = function (vValue) {
        this.ButtonMnimize = vValue;
        if (vValue)
            this.ObjMini.show();
        else
            this.ObjMini.hide();
    };

    this.ButtonClose = false;
    this.SetButtonClose = function (vValue) {
        this.ButtonClose = vValue;
        if (vValue)
            $(this.ObjButtonClose).show();
        else
            $(this.ObjButtonClose).hide();
    };
    

    this.Modal = false;
    this.SetModal = function (vModal) {
        if (this.Modal != vModal && vModal==true) {
            Ara.AraWindows.ModalOpen(this);
        } else if (this.Modal != vModal && vModal == false) {
            Ara.AraWindows.ModalClose(this);
        }

        this.Modal = vModal;
    }

    this.dragStart = function (event, ui) {
        //Ara.AraWindows.GetNewZIndex();
        this.SetZIndex(Ara.AraWindows.GetNewZIndex());
        $(this.Obj).find(':nicescroll').getNiceScroll().hide();
    }
    
    

    this.dragStop = function (event, ui) {
        this.Top = $(this.Obj).parent().offset().top;
        this.Left = $(this.Obj).parent().offset().left;
        Ara.AraWindows.ValidateLeftTopWindow(this, false);
        this.Events.DragStop.Function();

        this.AjustaNiceScroll();
    }

    this.focus = function (event, ui) {        
        this.SetZIndex(Ara.AraWindows.GetNewZIndex());
        this.AjustaNiceScroll();
    }

    this.vAjustaNiceScrollTime =null;
    this.AjustaNiceScroll = function () {
        if (this.vAjustaNiceScrollTime != null) {
            clearTimeout(this.vAjustaNiceScrollTime)
            this.vAjustaNiceScrollTime = null;
        }

        var TmpThis = this;
        this.vAjustaNiceScrollTime = setTimeout(function () { TmpThis.AjustaNiceScrollAncy(false); }, 500);
    };

    this.AjustaNiceScrollAncy = function (EventAjustaNiceScrollAll) {
        $(this.Obj).find(':nicescroll').getNiceScroll().show().resize();

        var TmpThis = this;
        $(this.Obj).find(':nicescroll').getNiceScroll().each(function () {
            this.rail.zIndex(parseInt(TmpThis.GetZIndex(), 10) + 1);
            this.railh.zIndex(parseInt(TmpThis.GetZIndex(), 10) + 1);
        });
        
        if (!EventAjustaNiceScrollAll)
            Ara.AraWindows.AjustaNiceScrollAll();
        else 
            this.vAjustaNiceScrollTime = null;
    };

    this.resizeStop = function (event, ui) {
        this.SetWidth(($(this.Obj).width()- this.BorderInnerWidth) + "px");
        this.SetHeight(($(this.Obj).height()- this.BorderInnerHeight) + "px");
        Ara.AraWindows.ValidateLeftTopWindow(this, false);
        Ara.AraWindows.ValidateWidthHeightWindow(this, false);
        this.Anchor.RenderChildren();
        this.Events.ResizeStop.Function();
        
    }

    this.Show = function () {
        this.Visible = true;
        $(this.Obj.parentNode).show();
        $(this.Obj).dialog("open");


        //this.ToFrontEnd();
        this.SetZIndex(Ara.AraWindows.GetNewZIndex());
        

        var TmpThis = this;
        setTimeout(function () {
            if (!TmpThis.Maximized) {
                if (TmpThis.Width==null)
                    TmpThis.Width = $(TmpThis.Obj).width();
                if (TmpThis.Height==null)
                    TmpThis.Height = $(TmpThis.Obj).height();
                if (TmpThis.Left == null)
                    TmpThis.SetLeft(($(TmpThis.Obj.parentNode.parentNode.parentNode).width() / 2) - (TmpThis.Width / 2));

                var Fader_height;
                if (TmpThis.Obj.parentNode.parentNode.parentNode.nodeName != "HTML")
                    Fader_height = $(TmpThis.Obj.parentNode.parentNode.parentNode).height();
                else
                    Fader_height = $(window).height();

                if (TmpThis.Top == null)
                    TmpThis.SetTop($(document).scrollTop() + (Fader_height / 2) - (TmpThis.Height / 2));
            }
            else
                TmpThis.Maximize();
            //TmpThis.SetLeft($(TmpThis.Obj).position().left);
            //TmpThis.SetTop($(TmpThis.Obj).position().top);

            Ara.AraWindows.ValidateLeftTopWindow(TmpThis, false);
            Ara.AraWindows.ValidateWidthHeightWindow(TmpThis, false);
            TmpThis.Anchor.RenderChildren();
            TmpThis.Events.ResizeStop.Function();
            TmpThis.AjustaNiceScroll();
        },1);
    };

    this.ClickMini = function () {
        this.SetMinimized(true);
    }

    this.Minimized = false;
    this.SetMinimized = function (vValue) {
        this.Minimized = vValue;
        if (this.Minimized) {
            $(this.Obj.parentNode).hide();
            this.AjustaNiceScroll();
        }
        else
            $(this.Obj.parentNode).show();
    }

    this.ReloadWh = function () {
    };

    this.ToFrontEnd = function () {
        $(this.Obj).dialog("moveToTop");
    }

    this.DisposeBefore = function () {
        $(this.Obj).find(':nicescroll').getNiceScroll().hide();
        $(this.Obj).find(':nicescroll').getNiceScroll().remove();
    }

    this.destruct = function () {
        this.DisposeBefore();
        if (this.Modal)
            this.SetModal(false);

        try {
            $(this.Obj).dialog("close");
        } catch (err) { }

        $(this.Obj).dialog("destroy");

        Ara.AraWindows.EventDelObject.Function(this.id);
    }

    this.CloseInf = function () {
        this.DisposeBefore();

        this.Events.Unload.Function();
        return false;
    }

    this.BorderInnerWidth = 5;
    this.BorderInnerHeight = 30;

    var isChrome = navigator.userAgent.toLowerCase().indexOf('chrome') > -1;
    if (isChrome) {
        this.BorderInnerWidth += 14;
        this.BorderInnerHeight += 14;
    }

    //this.Width = null;
    
    //this.SetWidth = function (vWidth) {
    //    if (this.Width != parseInt(vWidth, 10)) {
    //        this.Width = parseInt(vWidth, 10);
    //        this.ControlVar.SetValueUtm("Width", this.Width);
    //        var TmpWidth = vWidth + this.BorderInnerWidth;
    //        this.Width_Windows = TmpWidth;
    //        $(this.Obj).dialog({ width: TmpWidth });
    //        this.Anchor.RenderChildren();
    //    }
    //}

    this.outerWidth = function () {
        return $(this.Obj.parentNode).outerWidth();
    };

    this.outerHeight = function () {
        return $(this.Obj.parentNode).outerHeight();
    };

    //this.Height = null;
    
    //this.SetHeight = function (vHeight) {
    //    if (this.Height != parseInt(vHeight, 10)) {
    //        this.Height = parseInt(vHeight, 10);
    //        this.ControlVar.SetValueUtm("Height", this.Height);
    //        var TmpHeight = vHeight + this.BorderInnerHeight;
    //        this.Height_Windows = TmpHeight;
    //        $(this.Obj).dialog({ height: TmpHeight });
    //        this.Anchor.RenderChildren();
    //    }
    //}

    //this.Top = 0;
    
    //this.SetTop = function (vTop) {
    //    this.Top = vTop;
    //    this.ControlVar.SetValueUtm("Top", this.Top);
    //    $(this.Obj).dialog("option", "position", [this.Left, this.Top]);
    //}

    //this.Left = 0;
    
    //this.SetLeft = function (vleft) {
    //    this.Left = vleft;
    //    this.ControlVar.SetValueUtm("Left", this.Left);
    //    $(this.Obj).dialog("option", "position", [this.Left, this.Top]);
    //}

    this.SetCenter = function () {
        if (this.Maximize == false) {
            if (this.IsFormMain() == false) {
                this.left = -1;
                this.top = -1;
                $(this.Obj).dialog("option", "position", 'center');
            }
        }
    }

    
    this.Title = "";
    this.SetTitle = function (vTitle) {
        this.Title = vTitle;
        $(this.Obj).dialog({ title: vTitle });
        Ara.AraWindows.EventSetTitle.Function(this.id, vTitle);
    }

    this.Visible = false;
    this.SetVisible = function (vVisible) {
        this.Visible = vVisible;

        if (vVisible) 
            $(this.Obj.parentNode).show();
        else 
            $(this.Obj.parentNode).hide();
        Ara.AraWindows.EventSetVisible.Function(this.id, vVisible);
    }

    this.Maximized = false;
    this.ClickMaxMini = function () {
        if (this.Maximized==false)
            this.Maximize();
        else
            this.MaximizeRestore();
    }


    this.Maximize = function () {
        this.Maximized = true;

        this.MaximizeRestore_old = {};
        //this.MaximizeRestore_old.position = $(this.Obj).dialog("option", "position");
        this.MaximizeRestore_old.Left = this.Left;
        this.MaximizeRestore_old.Top = this.Top;
        this.MaximizeRestore_old.height = this.Height;
        this.MaximizeRestore_old.width = this.Width;

        var PWidth = this.outerWidth() - $(this.Obj).width();
        var PHeight = this.outerHeight() - $(this.Obj).height();

        var PBWidth = 0;
        var PBHeight = 0;

        if ($.browser.chrome) {
            PBWidth = 13;
            PBHeight = 13;
        }

        this.SetTop(Ara.AraWindows.BorderTop);
        this.SetLeft(Ara.AraWindows.BorderLeft);

        

        //this.SetHeight(Ara.AraWindows.WindowHeight - Ara.AraWindows.BorderTop - Ara.AraWindows.BorderBottom - PHeight - PBHeight - 1);
        //this.SetWidth(Ara.AraWindows.WindowWidth - Ara.AraWindows.BorderLeft - Ara.AraWindows.BorderRight - PWidth - PBWidth-1);
        
        var TmpThis = this;
        setTimeout(function () {
            TmpThis.SetHeight(Ara.AraWindows.WindowHeight - Ara.AraWindows.BorderTop - Ara.AraWindows.BorderBottom - PHeight - PBHeight);
            TmpThis.SetWidth(Ara.AraWindows.WindowWidth - Ara.AraWindows.BorderLeft - Ara.AraWindows.BorderRight - PWidth - PBWidth);
        }, 100);

        //$("#" + this.ObjMaxMini.id + " span").removeClass('ui-icon-extlink');
        //$("#" + this.ObjMaxMini.id + " span").addClass('ui-icon-newwin');
        this.ObjMaxMini.button("option", "icons", { primary: "ui-icon-newwin" });


        //$(this.Obj).dialog({ resizable: false });
        this.SetResizable(false);
        $(this.Obj).dialog({ draggable: false });

        setTimeout(function () { TmpThis.Events.ResizeStop.Function(); },1000);
    }

    this.MaximizeRestore = function () {
        this.Maximized = false;
        this.SetHeight(this.MaximizeRestore_old.height);
        this.SetWidth(this.MaximizeRestore_old.width);
        this.SetLeft(this.MaximizeRestore_old.Left);
        this.SetTop(this.MaximizeRestore_old.Top);
        //$(this.Obj).dialog({ position: this.MaximizeRestore_old.position, resizable: true });


        //$("#" + this.ObjMaxMini.id + " span").removeClass('ui-icon-newwin');
        //$("#" + this.ObjMaxMini.id + " span").addClass('ui-icon-extlink');
        this.ObjMaxMini.button("option", "icons", { primary: "ui-icon-extlink" });

        //$(this.Obj).dialog({ resizable: true });
        this.SetResizable(true);
        $(this.Obj).dialog({ draggable: true });
        var TmpThis = this;
        setTimeout(function () { TmpThis.Events.ResizeStop.Function(); }, 1000);
    }

    this.SetMaximize = function (vValue) {
        if (vValue) {
            if (!this.Maximized)
                this.Maximize();
        }
        else {
            if (this.Maximized)
                this.MaximizeRestore();
        }
    }

    this.Resizable = true;
    this.SetResizable = function (vValue) {
        this.Resizable = vValue;
        $(this.Obj).dialog({ resizable: vValue });
    }

    this.SetVisibleMaximizeButton = function (vVisible) {
        if (vVisible) 
            this.ObjMaxMini.show()
        else 
            this.ObjMaxMini.hide();
    }

    this.startDrag = function () {
        vTmpresu = findPos(this.Obj).split(";");
        this.offsetX = vTmpresu[0];
        this.offsetY = vTmpresu[1];


        this.offsetX = (parseInt(Ara.MouseX, 10) - parseInt(this.offsetX, 10)) + "px";
        this.offsetY = (parseInt(Ara.MouseY, 10) - parseInt(this.offsetY, 10)) + "px";

        this.drag = true;
    }

    this.stopDrag = function () {
        this.drag = false;

        this.Obj.style.cursor = "default";
        this.Obj.style.opacity = 1;
        this.Obj.style.filter = "alpha(opacity=100)";
        this.selecao(document.getElementsByTagName("body").item(0), true);

        //this.Obj.style.zIndex = this.MyManager.MaxZOrder;
        //this.MyManager.MaxZOrder++;
    }

    this.DivWaitLoading = null;
    this.DivCaptionWaitLoading = null;

    this.WaitLoading = function (vCaption) {
        if (this.Obj) {
            // <div id='DivAraLoad' class='ui-widget-overlay' style='width: 100%; height: 100%; z-index: 10000;display:none;'></div>

            if (this.DivWaitLoading == null) {
                this.DivWaitLoading = document.createElement("div");
                this.DivWaitLoading.setAttribute("id", this.CodJanela.replace(/[.]/g, "_") + "_DivWaitLoading");
                this.DivWaitLoading.className = "ui-widget-overlay";
                this.DivWaitLoading.style.width = "100%";
                this.DivWaitLoading.style.height = "100%";
                this.DivWaitLoading.style.zIndex = "10000";
                this.DivWaitLoading.innerHTML = "<div style='position: absolute;left: 45%; top: 50%;' id='" + (this.CodJanela.replace(/[.]/g, "_") + "_DivCaptionWaitLoading") + "'>" + vCaption + "</div>";
                this.Obj.appendChild(this.DivWaitLoading);

                this.DivCaptionWaitLoading = document.getElementById((this.CodJanela.replace(/[.]/g, "_") + "_DivCaptionWaitLoading"));
            }
            this.DivCaptionWaitLoading.innerHTML = vCaption;
            this.DivWaitLoading.style.display = 'block';

            this.$(this.DivWaitLoading).stop().css({ opacity: 0.4 }).fadeTo('slow', 0.4);
        }
    }

    this.WaitLoading_End = function () {
        if (this.DivWaitLoading)
            this.DivWaitLoading.style.display = 'none';
    }

    this.ZIndexWindow = null;
    this.SetZIndex = function (vValue) {
        this.ZIndexWindow = vValue;
        $(this.Obj).dialog("option", "zIndex", vValue);
        this.Obj.parentNode.style.zIndex = vValue;

        if (this.Modal)
            Ara.AraWindows.ModalSetNewIndex(this);

        this.AjustaNiceScroll();
    };

    this.GetZIndex = function () {
        return this.Obj.parentNode.style.zIndex;
        //return $(this.Obj).dialog("option", "zIndex");
    };

    this.Left = null;
    this.SetLeft = function (vTmp) {
        if (vTmp != null) {
            vTmp = parseInt(vTmp, 10);

            if (this.Left != vTmp) {
                this.Left = vTmp;
                this.ControlVar.SetValueUtm('Left', this.Left);
                //this.Obj.style.left = vTmp;
                //if (this.Top!=null)
                //$(this.Obj).dialog("option", "position", [this.Left + "px", this.Top + "px"]);
                if ($(this.Obj).parent().hasClass('ui-dialog'))
                    $(this.Obj).parent().offset({ left: this.Left });
            }
        }
        else {
            this.Left = null;
        }
    }

    this.Top = null;
    this.SetTop = function (vTmp) {
        if (vTmp != null) {
            vTmp = parseInt(vTmp, 10);
            if (this.Top != vTmp) {
                this.Top = vTmp;
                this.ControlVar.SetValueUtm('Top', this.Top);
                //this.Obj.style.top = vTmp;
                //if (this.Left!=null)
                //    $(this.Obj).dialog("option", "position", [this.Left + "px", this.Top + "px"]);
                if ($(this.Obj).parent().hasClass('ui-dialog'))
                    $(this.Obj).parent().offset({ top: this.Top  });
            }
        }
        else
            this.Top = null;
    }

    this._MinWidth = null;
    this.SetMinWidth = function (vTmp) {
        this._MinWidth = vTmp;
        if (this._MinWidth != null && this.Width != null && parseInt(this._MinWidth, 10) > parseInt(this.Width, 10))
            this.SetWidth(this._MinWidth, false);
    }

    this.Width = null;
    this.SetWidth = function (vTmp, vServer) {
        vTmp = parseInt(vTmp, 10);

        if (vTmp != null && this._MinWidth != null && parseInt(this._MinWidth, 10) > parseInt(vTmp, 10))
            vTmp = this._MinWidth;

        if (this.Width != vTmp) {
            this.Width = vTmp;

            if (vServer)
                this.ControlVar.SetValueUtm('Width', this.Width);

            this.Obj.style.width = vTmp;
            var TmpWidth = vTmp + this.BorderInnerWidth;
            this.Width_Windows = TmpWidth;
            $(this.Obj).dialog({ width: TmpWidth });
            $(this.Obj).parent().offset({ left: this.Left, top: this.Top });

            if (!vServer)
                this.Events.WidthChangeAfter.Function();

            if (this.Anchor != null)
                this.Anchor.RenderChildren();

            this.AjustaNiceScroll();
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
        vTmp = parseInt(vTmp, 10);

        if (vTmp != null && this._MinHeight != null && parseInt(this._MinHeight,10) > parseInt(vTmp,10))
            vTmp = this._MinHeight;

        if (this.Height != vTmp) {
            this.Height = vTmp;
            if (vServer)
                this.ControlVar.SetValueUtm('Height', this.Height);

            this.Obj.style.height = vTmp;

            var TmpHeight = vTmp + this.BorderInnerHeight;
            this.Height_Windows = TmpHeight;
            $(this.Obj).dialog({ height: TmpHeight });
            $(this.Obj).parent().offset({ left: this.Left, top: this.Top });

            if (!vServer)
                this.Events.HeightChangeAfter.Function();

            if (this.Anchor != null)
                this.Anchor.RenderChildren();

            this.AjustaNiceScroll();
        }
    }
    
    this.SetTypePosition = function (vTypePosition) {
        if (vTypePosition != "static")
            $(this.Obj).css({ position: vTypePosition });
        else {
            $(this.Obj).css({ position: "", left: "", top: "" });
        }
    }

    this.AppId = vAppId;
    this.id = vId;
    this.ConteinerFather = ConteinerFather;

    this.Obj = document.getElementById(this.id);
    if (!this.Obj) {
        alert("Object '" + this.id + "' Not Found");
        return;
    }

    
    //$(this.Obj).click(function () { TmpThis.Events.Click.Function(); });

    $(this.Obj).dialog({
        autoOpen: false,
        height: 80,
        width: 150,
        bgiframe: false,
        dragStart: function (event, ui) { TmpThis.dragStart(event, ui); },
        dragStop: function (event, ui) { TmpThis.dragStop(event, ui); },
        focus: function (event, ui) { TmpThis.focus(event, ui); },
        //resizeStart: function (event, ui) { $(TmpThis.Obj).hide(); },
        resizeStop: function (event, ui) { TmpThis.resizeStop(event, ui); },
        beforeClose: function (ev, ui) {  return TmpThis.CloseInf(); },
        show: "fade",
        hide: "fade",
        closeOnEscape:false,
        closeText: false,
        position: { my: "center", at: "center", of: window }
    });



    var DivHead = this.Obj.parentNode.childNodes[0];

    this.ObjButtonClose = DivHead.childNodes[1];
    
    //this.ObjMaxMini = $("<a href='#'></a>")
	//.addClass("ui-dialog-titlebar-close  ui-corner-all")
	//.attr("role", "button")
    //.css({ right: "18px" })
	//.click(function (event) {
	//    event.preventDefault();
	//    TmpThis.ClickMaxMini();
	//})
	//.appendTo(DivHead);
    
    //(this.ObjMaxMiniText = $("<span>"))
    //			.addClass("ui-icon ui-icon-extlink ")
    //			.text("")
    //			.appendTo(this.ObjMaxMini);

    this.ObjMaxMini = $("<button type='button'></button>")
        .button({
            icons: {
                primary: "ui-icon-extlink"
            },
            text: false
        })
        .width(20)
        .height(20)
        .addClass("ui-dialog-titlebar-close")
        .css({ right: "25px","padding":"1px" })
        .click(function (event) {
            event.preventDefault();
            TmpThis.ClickMaxMini();
        })
	    .appendTo(DivHead);

    this.ObjMaxMini
        .width("16px")
        .height("16px");
    
    //this.ObjMini = $("<a href='#'></a>")
	//.addClass("ui-dialog-titlebar-close  ui-corner-all")
	//.attr("role", "button")
    //.css({ right: "35px" })
	//.click(function (event) {
	//    event.preventDefault();
	//    TmpThis.ClickMini();
	//})
	//.appendTo(DivHead);


    //(this.ObjMiniText = $("<span>"))
	//			.addClass("ui-icon ui-icon-minus ")
	//			.text("")
    //			.appendTo(this.ObjMini);


    this.ObjMini = $("<button type='button'></button>")
        .button({
            icons: {
                primary: "ui-icon-minus"
            },
            text: false
        })
        .width(20)
        .height(20)
        .addClass("ui-dialog-titlebar-close")
        .css({ right: "50px", "padding": "1px" })
        .click(function (event) {
            event.preventDefault();
            TmpThis.ClickMini();
        })
	    .appendTo(DivHead);

    this.ObjMini
        .width("16px")
        .height("16px");

    this.ControlVar = new ClassAraGenVarSend(this);
    this.ControlVar.AddPrototype("Minimized");
    this.ControlVar.AddPrototype("Maximized");
    this.ControlVar.AddPrototype("Top");
    this.ControlVar.AddPrototype("Left");
    this.ControlVar.AddPrototype("Width");
    this.ControlVar.AddPrototype("Height");
    this.ControlVar.AddPrototype("ZIndexWindow");
    this.ControlVar.AddPrototype("Resizable");
    

    this.Anchor = new ClassAraAnchor(this);

    Ara.AraWindows.EventAddObject.Function(this.id, this.Title);

    this.SetButtonMnimize(Ara.AraWindows.Taskbar);

    
});