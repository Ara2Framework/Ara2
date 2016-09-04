// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
Ara.AraClass.Add('AraTimer', function (vAppId, vId, ConteinerFather) {
    // Eventos  ---------------------------------------
    this.Events = {};
	this.datediff = function (fromDate,toDate,interval) { 
        /*
         * DateFormat month/day/year hh:mm:ss
         * ex.
         * datediff('01/01/2011 12:00:00','01/01/2011 13:30:00','seconds');
         */
        var second=1000, minute=second*60, hour=minute*60, day=hour*24, week=day*7; 
        fromDate = new Date(fromDate); 
        toDate = new Date(toDate); 
        var timediff = toDate - fromDate; 
        if (isNaN(timediff)) return NaN; 
        switch (interval) { 
            case "years": return toDate.getFullYear() - fromDate.getFullYear(); 
            case "months": return ( 
                ( toDate.getFullYear() * 12 + toDate.getMonth() ) 
                - 
                ( fromDate.getFullYear() * 12 + fromDate.getMonth() ) 
            ); 
            case "weeks"  : return Math.floor(timediff / week); 
            case "days"   : return Math.floor(timediff / day);  
            case "hours"  : return Math.floor(timediff / hour);  
            case "minutes": return Math.floor(timediff / minute); 
            case "seconds": return Math.floor(timediff / second); 
            default: return undefined; 
        } 
    }
            
    var TmpThis = this;
    this.Events.tick =
    {
        Enabled: false,
        ThreadType: 1, // Multi_thread
        Function: function () {
            if (TmpThis.Enabled) {
            	
            	if (TmpThis.DateLastTick != null)
            	{
            		if (TmpThis.datediff(TmpThis.DateLastTick, new Date(),'seconds')>TmpThis.Timeout)
            			TmpThis.DateLastTick=null;
            	}
            
            	if (TmpThis.DateLastTick == null)
            	{
            		TmpThis.DateLastTick = new Date();
                	Ara.Tick.Send(this.ThreadType, TmpThis.AppId, TmpThis.id, "tick", null);
               	}
            }
        }
    };
    //---------------------------------------------------

    this.Interval = 0;
    this.SetInterval = function (vTmp) {
        if (this.Interval != vTmp) {
            this.Interval = vTmp;
            this.SetEnabled(this.Enabled);
        }
    };
    
    this.Timeout = 15;
    this.SetTimeout = function (vTmp) {
        if (this.Timeout != vTmp) {
            this.Timeout = vTmp;
        }
    };
    
    this.DateLastTick = null;
    this.TickEnd = function () {
        this.DateLastTick = null;
    };
    
    

    this.ObjTime = null;
    this.Enabled = false;
    this.SetEnabled = function (vValue) {
        if (vValue == true) {
            if (this.Interval > 0) {
                if (this.ObjTime != null)
                    clearInterval(this.ObjTime);
                var Tmpthis = this;
                this.ObjTime = setInterval(function () { Tmpthis.Events.tick.Function(); }, this.Interval);
                this.Enabled = vValue;
            }
        }
        else {
            if (this.ObjTime != null)
                clearInterval(this.ObjTime);
            this.Enabled = vValue;
        }
    }

    this.destruct = function () {
        this._IsDestroyed = true;
        if (this.ObjTime != null)
            clearInterval(this.ObjTime);
    }

    this._IsDestroyed = false;
    this.IsDestroyed = function () {
        return this._IsDestroyed;
    }


    this.AppId = vAppId;
    this.id = vId;
    this.ConteinerFather = ConteinerFather;

    this.Obj = null;
    this.ControlVar = new ClassAraGenVarSend(this);   

});