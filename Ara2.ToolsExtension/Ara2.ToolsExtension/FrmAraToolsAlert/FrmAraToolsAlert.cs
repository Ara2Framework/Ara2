// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ara2.Components;
using Ara2;
using System.Data.OleDb;
using Ara2.Dev;

namespace Ara2.Tools.Alert
{
    [Serializable]
    [AraDevComponent(vDisplayToolBar: false)]
    public class FrmAraToolsAlert : AraDesign.FrmAraToolsAlertAraDesign
    {
        public FrmAraToolsAlert(IAraContainerClient ConteinerFather)
            : base(ConteinerFather)
        {
            this.Title = "";
            lMsg.ScrollBar = new AraScrollBar(lMsg);

        }

        public FrmAraToolsAlert(IAraContainerClient ConteinerFather,string vMsg)
            : this(ConteinerFather)
        {
            lMsg.Text = vMsg.Replace("\r\n", "<br>").Replace("\n", "<br>");
        }

        public FrmAraToolsAlert(IAraContainerClient ConteinerFather, string vMsg,string vTitle)
            : this(ConteinerFather, vMsg)
        {
            this.Title = vTitle;
        }

        public override void bOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}