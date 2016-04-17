// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ara2.Log
{
    public static class  AraLog
    {
        private static object vObjLock=null;

        public static void Add(string vLog)
        {
            if (vObjLock == null)
                vObjLock = new object();

            lock (vObjLock)
            {
                string vPastaLog = Path.Combine(AraTools.GetPath(), "Log");
                if (!Directory.Exists(vPastaLog))
                    Directory.CreateDirectory(vPastaLog);

                string FileLog = Path.Combine(vPastaLog, DateTime.Now.ToString("yyyy-MM-dd") + ".log");

                using (var file = (File.Exists(FileLog) ? File.AppendText(FileLog) : File.CreateText(FileLog)))
                {
                    file.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff") + " " + vLog);
                    file.Close();
                }
            }
        }
    }
}
