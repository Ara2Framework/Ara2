// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ara2.Components
{
    [Serializable]
    public class AraGridEditrules
    {
        // http://www.trirand.com/jqgridwiki/doku.php?id=wiki:common_rules&s[]=editable
        /*
        edithidden	boolean	This option is valid only in form editing module. By default the hidden fields are not editable. If the field is hidden in the grid and edithidden is set to true, the field can be edited when add or edit methods are called.
        required	boolean	 (true or false) if set to true, the value will be checked and if empty, an error message will be displayed.
        number	boolean	 (true or false) if set to true, the value will be checked and if this is not a number, an error message will be displayed.
        integer	boolean	(true or false) if set to true, the value will be checked and if this is not a integer, an error message will be displayed.
        minValue	number(integer)	if set, the value will be checked and if the value is less than this, an error message will be displayed.
        maxValue	number(integer)	if set, the value will be checked and if the value is more than this, an error message will be displayed.
        email	boolean	if set to true, the value will be checked and if this is not valid e-mail, an error message will be displayed
        url	boolean	if set to true, the value will be checked and if this is not valid url, an error message will be displayed
        date	boolean	 if set to true a value from datefmt option is get (if not set ISO date is used) and the value will be checked and if this is not valid date, an error message will be displayed
        time	boolean	if set to true, the value will be checked and if this is not valid time, an error message will be displayed. Currently we support only hh:mm format and optional am/pm at the end
         */

        public bool edithidden = false;
        public bool required = false;
        public bool number = false;
        public bool integer = false;
        public int minValue = 0;
        public int maxValue = 0;
        public bool email = false;
        public bool url = false;
        public bool date = false;
        public bool time = false;

        public string GetScript()
        {
            string vTmp = "{";
            vTmp += "edithidden: " + (edithidden == true ? "true" : "false") + ",";
            vTmp += "required: " + (required == true ? "true" : "false") + ",";
            vTmp += "number: " + (number == true ? "true" : "false") + ",";
            vTmp += "integer: " + (integer == true ? "true" : "false") + ",";
            if (minValue > 0)
                vTmp += "minValue: " + minValue + ",";
            if (maxValue > 0)
                vTmp += "maxValue: " + maxValue + ",";
            vTmp += "email: " + (email == true ? "true" : "false") + ",";
            vTmp += "url: " + (url == true ? "true" : "false") + ",";
            vTmp += "date: " + (date == true ? "true" : "false") + ",";
            vTmp += "time: " + (time == true ? "true" : "false") + ",";

            vTmp = vTmp.Substring(0, vTmp.Length - 1);
            vTmp += "}";
            return vTmp;
        }


    }

}
