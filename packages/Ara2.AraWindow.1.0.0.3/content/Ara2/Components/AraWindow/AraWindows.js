// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14

// Gerenciador de Janelas do Ara
if (!Ara.AraWindows) {
    function AraWindows() {
        this.BorderLeft = 0;
        this.SetBorderLeft = function (vValue) {
            this.BorderLeft = vValue;
            this.WindowResise();
        };

        this.BorderTop = 0;
        this.SetBorderTop = function (vValue) {
            this.BorderTop = vValue;
            this.WindowResise();
        };

        this.BorderRight = 5;
        this.SetBorderRight = function (vValue) {
            this.BorderRight = vValue;
            this.WindowResise();
        };

        this.BorderBottom = 5;
        this.SetBorderBottom = function (vValue) {
            this.BorderBottom = vValue;
            this.WindowResise();
        };

        this.WindowWidth = $(window).width();
        this.WindowHeight = $(window).height();

        var TmpThis = this;
        this.BasedSizeByObject = window;
        this.SetBasedSizeByObject = function (vObj) {
            TmpThis.BasedSizeByObject = vObj;
            TmpThis.WindowWidth = $(TmpThis.BasedSizeByObject).width();
            TmpThis.WindowHeight = $(TmpThis.BasedSizeByObject).height();
            TmpThis.WindowResise();
        };

        $(window).resize(function (v1, v2) {
            if (!v2 && TmpThis.BasedSizeByObject == window) { // Vindo do rezise da janela
                TmpThis.WindowWidth = $(window).width();
                TmpThis.WindowHeight = $(window).height();
                TmpThis.WindowResise();
            }
        });

        $(document).resize(function (v1, v2) {
            if (!v2 && TmpThis.BasedSizeByObject == document) { // Vindo do rezise da janela
                TmpThis.WindowWidth = $(document).width();
                TmpThis.WindowHeight = $(document).height();
                TmpThis.WindowResise();
            }
        });

        this.WindowResise = function () {
            // IE 7 ERRO
            /*
            for (var ObjName in Objs) {
                var Obj = Objs[ObjName];
                if (Obj.Maximized == false) {
                    this.ValidateWidthHeightWindow(Obj, true);
                    this.ValidateLeftTopWindow(Obj, true);
                }
                else
                    Obj.Maximize();
            }
            */

            // IE 7
            var vTmpThis = this;
            $(Ara.GetObjectsbyType("AraWindow")).each(function () {
                var Obj = this;
                if (Obj.Maximized == false) {
                    vTmpThis.ValidateWidthHeightWindow(Obj, true);
                    vTmpThis.ValidateLeftTopWindow(Obj, true);
                }
                else
                    Obj.Maximize();
            });

        };

        this.ValidateWidthHeightWindow = function (Obj, vRuwEvent) {
            var EventOnResizeStop = false;

            var PWidth = Obj.outerWidth() - $(Obj.Obj).width();
            var PHeight = Obj.outerHeight() - $(Obj.Obj).height();

            var PBWidth = 0;
            var PBHeight = 0;

            if ($.browser.chrome) {
                PBWidth = 13;
                PBHeight = 13;
            }

            if (Obj.outerWidth() > this.WindowWidth - this.BorderLeft - this.BorderRight - PWidth - PBWidth) {
                EventOnResizeStop = true;
                Obj.SetWidth(this.WindowWidth - this.BorderLeft - this.BorderRight - PWidth - PBWidth);
            }

            if (Obj.outerHeight() > this.WindowHeight - this.BorderTop - this.BorderBottom - PHeight - PBHeight) {
                EventOnResizeStop = true;
                Obj.SetHeight(this.WindowHeight - this.BorderTop - this.BorderBottom - PHeight - PBHeight);
            }

            if (vRuwEvent)
                if (EventOnResizeStop)
                    Obj.Events.ResizeStop.Function();
        };

        this.ValidateLeftTopWindow = function (Obj, vRuwEvent) {
            var EventOnDragStop = false;

            if (Obj.Left < this.BorderLeft) 
                Obj.SetLeft(this.BorderLeft);

            if (Obj.Top < this.BorderTop)
                Obj.SetTop(this.BorderTop);
            

            if (Obj.Left + Obj.outerWidth() > this.WindowWidth - this.BorderRight) {
                EventOnDragStop = true;
                var vTmpL = this.WindowWidth - this.BorderRight - Obj.outerWidth();
                if (vTmpL < 0)
                    vTmpL = 0;
                Obj.SetLeft(vTmpL);
            }

            if (Obj.Top + Obj.outerHeight() > this.WindowHeight -  this.BorderBottom) {
                EventOnDragStop = true;
                var vTmpT = this.WindowHeight  - this.BorderBottom - Obj.outerHeight();
                if (vTmpT < 0)
                    vTmpT = 0;
                Obj.SetTop(vTmpT);
            }

            if (vRuwEvent)
                if (EventOnDragStop)
                    Obj.Events.DragStop.Function();
        };


        if (!$.maxZIndex) {
            $.maxZIndex = $.fn.maxZIndex = function (opt) {
                /// <summary>
                /// Returns the max zOrder in the document (no parameter)
                /// Sets max zOrder by passing a non-zero number
                /// which gets added to the highest zOrder.
                /// </summary>    
                /// <param name="opt" type="object">
                /// inc: increment value, 
                /// group: selector for zIndex elements to find max for
                /// </param>
                /// <returns type="jQuery" />
                var def = { inc: 10, group: "*" };
                $.extend(def, opt);
                var zmax = 0;
                $(def.group).each(function () {
                    var cur = parseInt($(this).css('z-index'));
                    zmax = cur > zmax ? cur : zmax;
                });
                if (!this.jquery)
                    return zmax;

                return this.each(function () {
                    zmax += def.inc;
                    $(this).css("z-index", zmax);
                });
            }
        }

        this.NewZIndex = 1000;
        this.GetNewZIndex = function () {
            this.NewZIndex+=2;
            return this.NewZIndex;
            //return $.maxZIndex()+1;
        };
        
        $("<div>").attr({
            id: "AraWindowsModal"
        }).addClass("ui-widget-overlay")
          .addClass("ui-front")
          .hide().appendTo(document.body);

        this.ModalDiv = $("#AraWindowsModal");
        this.modal = new Array();
        this.modalLen = 0;

        this.ModalOpen = function (vObjWindow) {
            var ZIndex = vObjWindow.GetZIndex();
            this.modal.push({ window: vObjWindow, zindex: ZIndex });
            this.modalLen++;
            
            if (this.modalLen == 1) {
                this.ModalDiv.stop().css({ opacity: 0 }).fadeTo('slow', 0.5);
            }
            this.ModalDiv.css("z-index", ZIndex - 1);
            this.EventSetModalZIndex.Function(ZIndex - 1);
        };

        this.ModalSetNewIndex = function (vObjWindow) {
            var NC = 0;
            
            for (var n = 0; n <= this.modal.length; n++) {
                if (this.modal[n]) {
                    NC++;
                    if (this.modal[n].window.id == vObjWindow.id) {
                        this.modal[n].zindex = vObjWindow.GetZIndex();
                        if (NC == this.modalLen) {
                            this.ModalDiv.css("z-index", this.modal[n].zindex - 1);
                            this.EventSetModalZIndex.Function(this.modal[n].zindex - 1);
                        }
                        return;
                    }
                }
            }
        };

        this.ModalClose = function (vObjWindow) {
            for (var n in this.modal) {
                if (this.modal[n].window.id == vObjWindow.id) {
                    this.modalLen--;
                    delete this.modal[n];
                    break;
                }
            }

            if (this.modalLen == 0) {
                this.ModalDiv.stop().hide();
                this.EventSetModalZIndex.Function(null);
            }
            else {

                var NC = 0;
                for (var n = 0; n <= this.modal.length; n++) {
                    if (this.modal[n]) {
                        NC++;
                        if (NC == this.modalLen) {
                            this.ModalDiv.css("z-index", this.modal[n].zindex - 1);
                            this.EventSetModalZIndex.Function(this.modal[n].zindex - 1);
                            return;
                        }
                    }
                }
            }
        };

        var TmpThis = this;
        this.EventSetTitle = {
            Events:new Array(),
            Add: function (vEvent) {
                this.Events.push(vEvent);
            },
            Function: function (vId, vTitle) {
                for (var n = 0; n <= this.Events.length; n++) {
                    if (this.Events[n])
                        this.Events[n](vId, vTitle);
                }
            }
        };

        this.EventAddObject = {
            Events: new Array(),
            Add: function (vEvent) {
                this.Events.push(vEvent);
            },
            Function: function (vId,vTitle) {
                for (var n = 0; n <= this.Events.length; n++) {
                    if (this.Events[n])
                        this.Events[n](vId, vTitle);
                }
            }
        };

        this.EventDelObject = {
            Events: new Array(),
            Add: function (vEvent) {
                this.Events.push(vEvent);
            },
            Function: function (vId) {
                for (var n = 0; n <= this.Events.length; n++) {
                    if (this.Events[n])
                        this.Events[n](vId);
                }
            }
        };

        this.EventSetVisible = {
            Events: new Array(),
            Add: function (vEvent) {
                this.Events.push(vEvent);
            },
            Function: function (vId, vVisible) {
                for (var n = 0; n <= this.Events.length; n++) {
                    if (this.Events[n])
                        this.Events[n](vId, vVisible);
                }
            }
        };

        this.EventSetModalZIndex = {
            Events: new Array(),
            Add: function (vEvent) {
                this.Events.push(vEvent);
            },
            Function: function (vZIndex) {
                for (var n = 0; n <= this.Events.length; n++) {
                    if (this.Events[n])
                        this.Events[n](vZIndex);
                }
            }
        };

        this.Taskbar = false;

        this.AjustaNiceScrollAll = function()
        {
            var vTmpThis = this;
            $(Ara.GetObjectsbyType("AraWindow")).each(function () {
                this.AjustaNiceScrollAncy(true);
            });
        }
    }
    Ara.AraWindows = new AraWindows();
}