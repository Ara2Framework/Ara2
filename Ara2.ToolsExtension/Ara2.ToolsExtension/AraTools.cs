// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Ara2.Tools.Alert;
using System.Threading;

namespace Ara2
{
    public static class AraToolsEx
    {
        #region Alert Form
        static public void AlertInForm(this AraTools vAraTools,  string vM)
        {
            FrmAraToolsAlert FrmAraToolsAlert = new FrmAraToolsAlert(Tick.GetTick().Session.WindowMain, vM);
            FrmAraToolsAlert.Show(true);
        }

        static public void AlertInForm(this AraTools vAraTools, string vM, string vTitle)
        {
            FrmAraToolsAlert FrmAraToolsAlert = new FrmAraToolsAlert(Tick.GetTick().Session.WindowMain, vM, vTitle);
            FrmAraToolsAlert.Show(true);
        }
        #endregion
    }
}
