// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14

function ClassAra(vSessionId,vUrl) {
    this.SessionId = vSessionId;
    this.Tools = new ClassAraTools();

    this.OpenAplication = function (vUrl, vAppId) {
        if (vAppId) {
            // Envia recado para nova app abrir 
        }
        else {
            //envia open para servidor principal
        }
        
    }

     
    this.Tick = new ClassAraTick(vUrl);
    this.Tick.NewTickId++;

    this.AraClass = new ClassAraClass();

    this.Object = Array();
    this.ObjectIdByAplication = Array();

    this.GetObject = function (vObjId) {
        return this.Object[vObjId];
    }

    

    this.GetObjectsbyAplication = function (vAppId) {
        return this.ObjectIdByAplication[vAppId];
    }
    this.GetObjectsbyType = function (vType) {
        var vObjs = new Array();
        for (var vName in this.Object) {
            if (this.Object[vName].type ==vType) 
                vObjs.push(this.Object[vName]);
        }

        return vObjs;
    }

    this.AddObject = function (vAppId, vObjId, vType, ConteinerFather) {
        this.Object[vObjId] = this.AraClass.NewClass(vType, vAppId, vObjId, ConteinerFather);
        this.Object[vObjId].type = vType;

        if (!this.ObjectIdByAplication[vAppId])
            this.ObjectIdByAplication[vAppId] = new Array();
        if (!this.ObjectIdByAplication[vAppId][vObjId])
            this.ObjectIdByAplication[vAppId].push(this.Object[vObjId]);
    }

    this.DelObject = function (vObjId) {
        if (this.Object[vObjId]) {
            var vAppId = this.Object[vObjId].AppId;
            this.Object[vObjId].destruct();

            // IE 7 ERRO
            //for (var n in this.ObjectIdByAplication[vAppId]) {
            //    if (this.ObjectIdByAplication[vAppId][n].id == vObjId) {
            //        delete this.ObjectIdByAplication[vAppId][n];
            //        break;
            //    }
            //}

            // IE 7
            for (var n = 0 ; n <= this.ObjectIdByAplication[vAppId].length; n++) {
                if (this.ObjectIdByAplication[vAppId][n] && this.ObjectIdByAplication[vAppId][n].id == vObjId) {
                    delete this.ObjectIdByAplication[vAppId][n];
                    break;
                }
            }

            delete this.Object[vObjId]
        }
    }

    this.CreateObject = function (vConteinerFatherName, vNodeName, vId, vAttributes) {
        var ObjFater;
        if (vConteinerFatherName==null)
            ObjFater=$(document.body);
        else
            ObjFater=$("#" +  vConteinerFatherName);

        var attr;
        if (vAttributes != null)
            attr = vAttributes;
        else
            attr = {};
        attr.id = vId;

        $("<" + vNodeName + ">").attr(attr).appendTo(ObjFater);
    }

    this.GetVarChangesAplication = function (vAppId) {
        var Return = new Array();

        // IE 7 ERRO
        //for (var n in this.ObjectIdByAplication[vAppId]) {
        //    var Obj = this.ObjectIdByAplication[vAppId][n];
        //    var Changes = Obj.ControlVar.GetChanges();
        //    if (Changes.length > 0) {
        //        Return.push({ objid: Obj.id, Changes: Changes });
        //    }
        //}

        // IE 7
        for (var n = 1; n <= this.ObjectIdByAplication[vAppId].length; n++) {
            var Obj = this.ObjectIdByAplication[vAppId][n];
            if (Obj) {
                var Changes = Obj.ControlVar.GetChanges();
                if (Changes.length > 0) {
                    Return.push({ objid: Obj.id, Changes: Changes });
                }
            }
        }
        return Return;
    };

    this.EndLoadAra = function () {
        try
        {
            window.parent.postMessage("{'Event':'EndLoadAra'}", "*");
        } catch (err) { }

        $("#DivLoadMain").css({display:"none"});
    }

    this.AddCss = function (vFile) {
        $.get(vFile, function (cssContent) {
            // Error IE 7
            //$('<style />').text(cssContent).appendTo($('head'));
            // IE 7
            $("head").append("<style>" + cssContent + "</style>");
        });
    }

    this.GetMaxZIndexByObject = function (vObjAra) {
        var MaxZIndex = 0;
        $(vObjAra.Obj).parents().each(function (i) {
            //if ($(this).css("z-index") != "auto") {
            //    if (MaxZIndex < parseFloat($(this).css("z-index"), 10))
            //        MaxZIndex = parseFloat($(this).css("z-index"), 10);
            //}
            if ($(this).zIndex() != "auto") {
                if (MaxZIndex < parseFloat($(this).zIndex(), 10))
                    MaxZIndex = parseFloat($(this).zIndex(), 10);
            }
        });

        return MaxZIndex;
    };

    this.ArraySize = function (vObj) {
        if (!Array.prototype.filter) {
            Array.prototype.filter = function (fun /*, thisp */) {
                "use strict";

                if (this === void 0 || this === null)
                    throw new TypeError();

                var t = Object(this);
                var len = t.length >>> 0;
                if (typeof fun !== "function")
                    throw new TypeError();

                var res = [];
                var thisp = arguments[1];
                for (var i = 0; i < len; i++) {
                    if (i in t) {
                        var val = t[i]; // in case fun mutates this
                        if (fun.call(thisp, val, i, t))
                            res.push(val);
                    }
                }

                return res;
            };
        }

        return vObj.filter(function (a) { return a !== undefined; }).length
    };
    
}