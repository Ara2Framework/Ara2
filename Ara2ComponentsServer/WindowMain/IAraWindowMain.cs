// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ara2.Keyboard;
using Ara2.Dev;

namespace Ara2.Components
{
    public delegate void DMenssage(string vMenssage);
    public delegate void DGetCookie(string vValue);
    public delegate void DPopState(Uri URL, System.Collections.Specialized.NameValueCollection Parans);
    public interface IAraWindowMain:IAraDev
    {

        void Show();
        #region Div
        void ShowDiv(IAraComponentVisualAnchor vForm);
        void ShowDivModal(IDivModal vForm);
        void CloseDivModal(IDivModal vForm);
        void CloseDivModal(IDivModal vForm, object vObjReturn);
        void HideDivModal(IDivModal vForm);
        void HideDivModal(IDivModal vForm, object vObjReturn);
        IAraComponentVisualAnchor DivCanvas { get; set; }
        #endregion

        void SetCookie(string vName, string vValue, int? ExpiresDays = null, string path = null);
        void GetCookie(string vName, DGetCookie vEventReturn);
        void DelCookie(string vName, string path = null);

        void SetHistoryReplaceState(string vValue);
        void SetHistoryPushState(string vValue);

        void WaitLoading(string vMessage, Action vAction);

        string Name { get; set; }

        

        AraComponentEvent<Action> ResizeStop { get; set; }
        AraComponentEvent<Action> Unload { get; set; }
        AraComponentEvent<EventHandler> Click { get; set; }
        AraComponentEventKey<DArakeyboard> KeyDown { get; set; }
        AraComponentEventKey<DArakeyboard> KeyUp { get; set; }
        AraComponentEventKey<DArakeyboard> KeyPress { get; set; }
        AraComponentEvent<DPopState> PopState { get; set; }
        AraEvent<Action> ChangelocationHash { get; set; }
        AraEvent<Action> Active { get; set; }

        AraComponentEvent<DMenssage> Menssage { get; set; }

        void RumActionWaitLoading(int vKey);

        void Alert(string vM);
        void Alert<T>(string vM, T Retorno, params object[] vParametros);
        void AlertYesOrNo<T>(string vM, T EventReturn, params object[] vParametros);
        void AsynchronousFunction<T>(T vEventReturn, params object[] vParametros) ;
        void AlertGetString<T>(string vM, string vValuedefault, T EventReturn, params object[] vParametros);
    }
}
