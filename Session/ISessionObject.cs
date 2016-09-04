﻿// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ara2.Components;

namespace Ara2
{
    public interface ISessionObject
    {
        Type Type { get; }
        IAraObject Object { get; }
        string InstanceID { get; }
        bool NeedSave { get; }
        void SaveObject();
    }
}
