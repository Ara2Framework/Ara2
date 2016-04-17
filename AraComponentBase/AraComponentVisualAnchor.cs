// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace Ara2.Components
{
    [Serializable]
    public abstract class AraComponentVisualAnchor : AraComponentVisual, IAraComponentVisualAnchor
    {
        public AraComponentVisualAnchor(string vNameObject, IAraObject vConteinerFather, string vTypeNameJS) :
            base(vNameObject, vConteinerFather, vTypeNameJS)
        {
            Anchor = new AraAnchor(this);
        }

        AraObjectInstance<AraAnchor> _Anchor = new AraObjectInstance<AraAnchor>();

        [Ara2.Dev.AraDevProperty]
        public AraAnchor Anchor
        {
            get { return _Anchor.Object; }
            set { _Anchor.Object = value; }
        }

        public abstract override void LoadJS();
    }
}
