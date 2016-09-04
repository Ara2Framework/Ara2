// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14

function ClassAraScrollBar() {
    this.Create = function (vName,vParans) {
        setTimeout(function () {
            var vObjS = $('#' + vName);
            vObjS.getNiceScroll().remove();
            vObjS.niceScroll(vParans);
        }, 100);
    };

    this.ScrollTop = function (vName) {
        setTimeout(function () {
            var vObjS = $('#' + vName);
            vObjS.scrollTop(vObjS.scrollTop() + 1000);
        }, 100);
    };

    this.Remove = function (vName) {
        var vObjS = $('#' + vName);
        vObjS.getNiceScroll().remove();
    };

    this.ResizeShow = function (vObj) {
        $(vObj).find(':nicescroll').getNiceScroll().show().resize();
    }

    this.ResizeHide = function (vObj) {
        $(vObj).find(':nicescroll').getNiceScroll().show().resize();
    }
}

Ara.AraScrollBar = new ClassAraScrollBar();