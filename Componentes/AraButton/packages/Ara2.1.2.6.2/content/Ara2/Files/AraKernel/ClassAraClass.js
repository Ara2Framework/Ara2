// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
function ClassAraClass() {
    this.NextClassAppId = null;
    this.Classes = new Array();

    this.Add = function (vNome, vClass) {
        if (this.NextClassAppId == null)
            throw "NextClassAppId unfilled";

        if (!this.Classes[this.NextClassAppId])
            this.Classes[this.NextClassAppId] = new Array();

        this.Classes[this.NextClassAppId][vNome] = vClass;
        

        this.NextClassAppId = null;
    };

    this.NewClass = function (vNome, vAppId, vObjId, ConteinerFather) {
        return new this.Classes[vAppId][vNome](vAppId, vObjId, ConteinerFather);
    };

    this.LoadClassUrl = new Array();
    this.LoadClass = function (vAppId, vTickId, vUrl, vFunction) {
        if (Ara.AraClass.NextClassAppId != null)
            throw "Ara.AraClass.NextClassAppId already defined";

        if (!this.LoadClassUrl[vUrl]) {
            Ara.AraClass.NextClassAppId = vAppId;

            this.LoadClassUrl[vUrl] = {};
            this.LoadClassUrl[vUrl].AppId = vAppId;
            this.LoadClassUrl[vUrl].load = false;
            this.LoadClassUrl[vUrl].queue = new Array();


            this.LoadClassUrl[vUrl].queue.push({ Function: vFunction, TickId: vTickId });
            Ara.Tick.AddPendency(vTickId, "LoadJs_" + vUrl);


            var TmpThis = this;
            $.getScript(vUrl, function (data, textStatus, jqxhr) {
                if (jqxhr.status == 200) {
                    Ara.AraClass.NextClassAppId = null;
                    TmpThis.EndLoadClass(vUrl);
                }
            });
        }
        else {
            if (this.LoadClassUrl[vUrl].load == false) {
                this.LoadClassUrl[vUrl].queue.push({ Function: vFunction, TickId: vTickId });
                Ara.Tick.AddPendency(vTickId, "LoadJs_" + vUrl);
            } else {
                vFunction();
            }
        }
    };

    this.EndLoadClass = function (vUrl) {
        this.LoadClassUrl[vUrl].load = true;

        // Error IE 7
        //for (var TmpN in this.LoadClassUrl[vUrl].queue) {
        //    var vTickId = this.LoadClassUrl[vUrl].queue[TmpN].TickId;
        //    this.LoadClassUrl[vUrl].queue[TmpN].Function();
        //    Ara.Tick.DelPendency(vTickId, "LoadJs_" + vUrl);
        //}

        // IE 7
        $(this.LoadClassUrl[vUrl].queue).each(function () {
            this.Function();
            Ara.Tick.DelPendency(this.TickId, "LoadJs_" + vUrl);
        });
    };
}