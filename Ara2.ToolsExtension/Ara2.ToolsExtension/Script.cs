// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ara2.Log;
using Ara2.Components;

namespace Ara2
{
    public static class ScriptEx
    {

        public static void CustomerAnalysisShowResult(this Script vScript, string NameAnalysis,dynamic vArray)
        {

            StringBuilder TmpS = new StringBuilder();
            TmpS.Append("tipo;name;cliente;server;\n");
            foreach (dynamic Tmp in vArray)
            {
                TmpS.Append(Tmp.NameType + ";" + Tmp.name + ";" + Tmp.cliente + ";" + Tmp.server + ";\n");
            }

            string vTmpFile = System.IO.Path.Combine(AraTools.GetPath() , "tmp");
            if (!System.IO.Directory.Exists(vTmpFile))
                System.IO.Directory.CreateDirectory(vTmpFile);

            string vFileName = Guid.NewGuid().ToString() + ".csv";
            string vFileURL = "tmp/" + vFileName;
            string vFilePathName = System.IO.Path.Combine(vTmpFile, vFileName);
            System.IO.File.WriteAllText(vFilePathName, TmpS.ToString());

            Ara2.Components.AraWindow Win = new Components.AraWindow(Tick.GetTick().Session.WindowMain);
            Win.Width = 500;
            Win.Height = 400;
            Ara2.Components.AraIFrame iFrame = new Components.AraIFrame(Win);
            iFrame.Src = vFileURL;
            iFrame.Anchor.Left = 10;
            iFrame.Anchor.Top = 10;
            iFrame.Anchor.Bottom = 10;
            iFrame.Anchor.Right = 10;
            Win.Show();

            //AraTools.Alert("Resultado " + NameAnalysis + " \nFile: " + vFileName );
        }

    }
}
