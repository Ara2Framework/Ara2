// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
function ClassAraGenVarSend(vObj) {
    


    this.AddPrototype = function (vNamePrototype) {
        vN = this.Prototypes_Name.length;
        this.Prototypes_Name[vN] = vNamePrototype;
        this.PrototypesValueUtm[vN] = this.GetReturnPrototypes(vN);
        this.Prototypes_Name_Pos[vNamePrototype] = vN;

    }

    this.RemuvePrototype = function (vName) {
        var vTmpArray_Name = new Array();
        var vTmpArray_ValueUtm = new Array();
        var vTmpArray_Name_Pos = new Array();

        for (n = 0; n < this.Prototypes_Name.length; n++) {
            if (this.Prototypes_Name[n] != vName) {
                vTmpArray_Name_Pos.push(this.Prototypes_Name_Pos[this.Prototypes_Name[n]]);
                vTmpArray_Name.push(this.Prototypes_Name[n]);
                vTmpArray_ValueUtm.push(this.PrototypesValueUtm[n]);
            }
        }
        this.Prototypes_Name = vTmpArray_Name;
        this.PrototypesValueUtm = vTmpArray_ValueUtm;
        this.Prototypes_Name_Pos = vTmpArray_Name_Pos;

    }

    this.GetReturnPrototypes = function (vN) {
        try {
            var vReturn;
            eval(" vReturn = this.Obj." + this.Prototypes_Name[vN] + ";");
            return vReturn;
        }
        catch (e) {
            return null;
        }
    }

    this.SetValueUtm = function (vName, vValue) {
        n = this.Prototypes_Name_Pos[vName];
        if (n > -1) {

            this.PrototypesValueUtm[n] = vValue;
            return;
        }
        if (vName != "") {
            this.AddPrototype(vName);
            this.SetValueUtm(vName, vValue);
        }
    }

    this.GetValueUtm = function (vName) {
        n = this.Prototypes_Name_Pos[vName];
        if (n > 0) {
            return this.PrototypesValueUtm[n];
        }
    }

    this.GetChanges = function () {
        var vReturn = new Array();
        for (np = 0; np < this.Prototypes_Name.length; np++) {
            var vTmp = this.GetReturnPrototypes(np);
            if (this.PrototypesValueUtm[np] != vTmp) {
                vReturn.push({name:this.Prototypes_Name[np],value: vTmp});
            }
        }
        return vReturn;
    }

    this.Obj = vObj;

    this.Prototypes_Name = Array();
    this.Prototypes_Name_Pos = Array();
    this.PrototypesValueUtm = Array();

    this.AddPrototype("IsDestroyed()");
}