// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
function ClassAraTick(vUrl) {
    
    $("<div>").attr({
        id: "AraTickDivMain",
        style: "display:none;"
    }).appendTo(document.body);
    this.ObjAraTrick = $("#AraTickDivMain");

    // Erro IE 7
    //$("<div>").attr({ 
    //    id: "AraTickLockScreen", 
    //    style: "position: absolute; top: 0px; left: 0px;display:none;width:100%;height:100%;z-index:99999999999999;",
    //    class: "ui-widget-overlay"
    //}).appendTo(document.body);
    
    $("<div>").attr({ 
        id: "AraTickLockScreen", 
        style: "position: absolute; top: 0px; left: 0px;display:none;width:100%;height:100%;z-index:99999999999999;"
    }).appendTo(document.body);

    // IE 7 suport
    $("#AraTickLockScreen").attr("class", "ui-widget-overlay");
    
    this.LockScreenDiv = $("#AraTickLockScreen");


    this.Tick = new Array();
    this.AddPendency = function (vTickId, vNamePendency) {
        if (!this.Tick[vTickId])
            this.Tick[vTickId] = {};
        if (!this.Tick[vTickId].Pendency)
            this.Tick[vTickId].Pendency = new Array();
        if (!this.Tick[vTickId].Pendency[vNamePendency]) 
            this.Tick[vTickId].Pendency.push(vNamePendency);
    }

    this.DelPendency = function (vTickId, vNamePendency) {
        for (n = 0; n < this.Tick[vTickId].Pendency.length; n++) {
            if (this.Tick[vTickId].Pendency[n] == vNamePendency) {
                delete this.Tick[vTickId].Pendency[n];
                break;
            }
        }
        
        //if (this.Tick[vTickId].isUse == false)
        this.DelTick(vTickId);
    }

    this.DelTick = function (vTickId) {
        if (this.Tick[vTickId]) {
            if (!this.Tick[vTickId].Pendency)
                this.Tick[vTickId].Pendency = new Array();
            if (Ara.ArraySize(this.Tick[vTickId].Pendency) == 0) {

                if (this.Tick[vTickId].isUse)
                    this.Tick[vTickId].isUse = false;

                this.Tick[vTickId].Script = null;

                delete this.Tick[vTickId];
                if (this.LockScreen)
                    this.CheckLockScreen();
            }
        }
    }

    this.GetTickByTypeThreadEvent = function (vTypeThreadEvent) {
        for (var vTmpEventId in this.Tick) {
            if (this.Tick[vTmpEventId].ThreadType == vTypeThreadEvent)
                return this.Tick[vTmpEventId];
        }

        return false;
    };

    this.CheckLockScreen = function () {
        for (var vTmpEventId in this.Tick) {
            var ThreadType = this.Tick[vTmpEventId].ThreadType;
            if (ThreadType == 2 || ThreadType == 3 || ThreadType == 4) 
                return false;
        }

        this.SetLockScreen(false);

        return true;
    };

    this.NewTickId = 1;
    this.Send = function (vTypeThreadEvent, vAppId, vObjId, vEvent, vParameters, vAsync) {
        if (!vAsync) vAsync = true;

        if (vTypeThreadEvent == 2) { // Single_thread
            var EventConf = this.GetTickByTypeThreadEvent(2);
            if (EventConf != false) 
                return false;
        }
        else if (vTypeThreadEvent == 3) { // Click
            var EventConf = this.GetTickByTypeThreadEvent(3);
            if (EventConf != false) {
                clearTimeout(EventConf.IdTime);
                delete this.Tick[EventConf.TickId];
            }
        }
        //else if (vTypeThreadEvent == 4) { // Messager
        //}

        var TickId = this.NewTickId;
        this.NewTickId++;

        this.Tick[TickId] = {
            TickId:TickId,
            ThreadType: vTypeThreadEvent,
            AppId : vAppId,
            ObjId : vObjId,
            Event: vEvent,
            async: vAsync,
            Parameters: vParameters,
            ParametersVarChanges: Ara.GetVarChangesAplication(vAppId),
            FunctionExecuta : function () {
                if (this.ThreadType == 2 || this.ThreadType == 3 || this.ThreadType == 4) 
                    Ara.Tick.SetLockScreen(true);
                Ara.Tick.SendTick(this.TickId);
            },
            isUse: true,
            Script:null
        };
    
        var TmpThis = this;
        if (this.Tick[TickId].ThreadType == 3)
            this.Tick[TickId].IdTime = setTimeout(function () { TmpThis.Tick[TickId].FunctionExecuta(); }, 500);
        else
            this.Tick[TickId].FunctionExecuta();
    };

    this.LockScreen = false;
    
    this.SetLockScreen = function(vLock) {
        this.LockScreen=vLock;
        if (vLock)
            this.LockScreenDiv.stop().css({display:"block", opacity: 0 }).fadeTo('slow', 0.1).delay(3000).fadeTo('slow', 0.8);
        else
            this.LockScreenDiv.stop().css({ display: "none" });        
    };

    this.Url = vUrl; //"Default.aspx";

    this.SendTick = function (vTickId) {
        var Parameters = this.Tick[vTickId].Parameters;
        if (Parameters == null)
            Parameters = {};
        Parameters.ARA2 = 1;
        Parameters.TickId = vTickId;
        Parameters.SessionId = Ara.SessionId;
        Parameters.AppId = this.Tick[vTickId].AppId;
        Parameters.ObjId = this.Tick[vTickId].ObjId;
        Parameters.Event = this.Tick[vTickId].Event;
        Parameters.AraTickParametersVarChanges = JSON.stringify(this.Tick[vTickId].ParametersVarChanges);
        var vAsync =this.Tick[vTickId].async;
        //var LogEvent = this.Tick[vTickId].Event;
        
        var TmpThis = this;
        $.ajax({
            type: "post",
            url: TmpThis.Url,
            data: Parameters,
            async: vAsync,
            success: function (data) {
                try {
                    eval(" TmpThis.Tick[vTickId].Script = function(){ " + data + "};");
                    TmpThis.Tick[vTickId].Script();
                } catch (err) {
                    //if (LogEvent != "tick")
                    //    console.log("Erro on execute script.\n " + err + "\n\nScript:'\n" + data + "\n'");
                    TmpThis.DelTick(vTickId);
                    try {
                        window.parent.postMessage("{'Event':'TickScript','Msg':'" + err + "'}", "*");
                    } catch (err) { }
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                //if (LogEvent != "tick")
                //    console.log("Erro on send Ajax.\n " + thrownError);

                TmpThis.DelTick(vTickId);
                try {
                    window.parent.postMessage("{'Event':'TickError'}", "*");
                } catch (err) { }
            }
        });
    };
    
}






