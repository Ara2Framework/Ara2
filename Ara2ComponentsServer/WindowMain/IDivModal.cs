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
    public interface IDivModal : IAraComponentVisualAnchor
    {
        AraEvent<Action<object>> Unload { get; set; }

        void Close();
        void Close(object vObj);
        void Show();
        void Show(Action<object> vEventReturn);
        void Hide();
        void Hide(object vObj);

        bool StyleContainer { get; set; }
    }
}
