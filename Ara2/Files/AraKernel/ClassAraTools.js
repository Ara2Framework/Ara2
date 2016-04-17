// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14

function ClassAraTools() {
    
    this.DivAraLoad = null;
    this.DivAraLoad_caption = null;
    this.WarningLoading = function (vAppId, vKey, vCaption) {

        // Response.Write("<div id='DivAraLoad' class='ui-widget-overlay' style='width: 100%; height: 100%; z-index: 10000;display:none;'><div style='position: absolute;left: 45%; top: 50%;' id='DivAraLoad_caption'>Carregando...</div></div>\n");

        if (this.DivAraLoad == null) {
            $("<div id='DivLoad' class='ui-widget-overlay' style='width: 100%; height: 100%; z-index: 10000;display:none;'></div>").appendTo("body");
            $("<div id='DivLoad_caption'></div>").appendTo("body");
            this.DivAraLoad = $("#DivLoad");
            this.DivAraLoad_caption = $("#DivLoad_caption");
        }

        this.DivAraLoad_caption.html("<font size='18px'>" + vCaption + "</font>");


        this.DivAraLoad_caption.css("position", "absolute");
        this.DivAraLoad_caption.css("top", Math.max(0, (($(window).height() - this.DivAraLoad_caption.outerHeight()) / 2) + $(window).scrollTop()) + "px");
        this.DivAraLoad_caption.css("left", Math.max(0, (($(window).width() - this.DivAraLoad_caption.outerWidth()) / 2) + $(window).scrollLeft()) + "px");

        this.DivAraLoad.css('z-index', 99999999999999);
        this.DivAraLoad_caption.css('z-index', 999999999999999);

        this.DivAraLoad.show();
        this.DivAraLoad_caption.show();

        this.DivAraLoad.css({ opacity: 0.5 });

        //this.DivAraLoad.animate({ opacity: '0.5' }).css({ opacity: 0.5 });

        //this.DivAraLoad.stop().css({ opacity: 0 }).fadeTo('slow', 0.5);

        //var vParans = new Ara.ClassAraComunicacao_Param();
        //this.GetForm('0.').Send('form', 'WarningLoading', vParans, Ara.EAraComponentEventTypeThread.ThreadMulti);
        Ara.Tick.Send(1, vAppId, 'Ara', "WarningLoading", { key: vKey });
    }

    this.WarningLoading_End = function () {
        if (this.DivAraLoad != null) {
            this.DivAraLoad.fadeTo('hide', 0.5).hide();
            this.DivAraLoad_caption.hide();
        }
    }
}

