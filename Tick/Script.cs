// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ara2.Log;

namespace Ara2
{
    public class Script
    {
        Tick Tick;
        string LastObject = "";
        int _Level = 0;
        Dictionary<int, string> ScriptsEnd = new Dictionary<int, string>();

        public event Action Load;
        public event Action UnLoad;


        public Script(Tick vTick)
        {
            Tick = vTick;
            ScriptsEnd.Add(0, "");
        }

        public void ClearCallFormObject()
        {
            LastObject = "";
        }

        //Falta revisar
        public void CallObject(Ara2.Components.AraObjectClienteServer vObject)
        {
            if (LastObject != vObject.InstanceID) // + "_" + vObject.Index
            {
                Send(" var vObj = Ara.GetObject('" + vObject.InstanceID + "'); \n");
                LastObject = vObject.InstanceID; // + "_" + vObject.Index
            }
        }

        public int Level
        {
            get
            {
                return _Level;
            }
        }

        List<string> _AlreadyIsSent = new List<string>();

        public bool AlreadyIsSent(string vCommand)
        {
            if (_AlreadyIsSent.Contains(vCommand) == true)
                return true;
            else
            {
                _AlreadyIsSent.Add(vCommand);
                return false;
            }
        }

        public void Send(string vScript)
        {
            try
            {
                Tick.Page.Response.Write(vScript);
            }
            catch (Exception err)
            {
                //AraLog.Add("Erro Script Send '" + err.Message + "'\nScript:" + vScript);
            }
        }

        public void Send(int vLevel, string vScript)
        {
            ScriptsEnd[vLevel] += vScript;
        }

        public int GetNewLevel()
        {
            ClearCallFormObject();
            Send(_Level, "} \n");
            _Level++;
            ScriptsEnd.Add(_Level, "");
            return _Level;
        }

        public void SendScriptsEnd()
        {
            for (int n = ScriptsEnd.Count - 1; n >= 0; n--)
            {
                Tick.Page.Response.Write(ScriptsEnd[n]);
            }
            ScriptsEnd.Clear();
        }

        public void RumLoad()
        {
            try
            {
                if (Load != null)
                    Load();
            }
            catch (Exception err)
            {
                throw new Exception("Error on Client.Script.RumLoad.\n " + err.Message);
            }
        }

        public void RumUnLoad()
        {
            try
            {
                if (UnLoad != null)
                    UnLoad();
            }
            catch (Exception err)
            {
                throw new Exception("Error on Client.Script.RumUnLoad.\n " + err.Message);
            }

           
        }



        
        
        public void CustomerAnalysisBegin(string NameAnalysis)
        {
            Tick.GetTick().Script.Send(" var Analysis" + NameAnalysis + " = new Array();\n");
        }

        public void CustomerAnalysisAddArea(string NameAnalysis, string NameArea, string NameType, Action vEvent)
        {

            DateTime DataIni = DateTime.Now;
            Tick.GetTick().Script.Send(" var TmpDataIni" + NameArea + " = new Date();\n");            
            DateTime DataINi = DateTime.Now;

            vEvent();

            Tick.GetTick().Script.Send(" Analysis" + NameAnalysis + ".push({NameType:'" + NameType + "',name:'" + NameArea + "',cliente:  ((new Date())-TmpDataIni" + NameArea + "),server:" + (DateTime.Now - DataINi).TotalMilliseconds.ToString().Replace(",", ".") + "});\n");

        }

        public void CustomerAnalysisEnd(string NameAnalysis)
        {
            Tick vTick = Tick.GetTick();
            vTick.Script.Send(" Ara.Tick.Send(4, " + vTick.Session.AppId + ", 'Ara', 'AnalysisResult', {name:'" + NameAnalysis + "', Analysis: JSON.stringify(Analysis" + NameAnalysis + ")});\n");
        }
    }
}
