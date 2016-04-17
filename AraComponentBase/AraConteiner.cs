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
    public class AraContainer :  IDisposable
    {
       
        public delegate void DAddRemuveChild(IAraObject Child);
        public AraEvent<DAddRemuveChild> AddChildBefore = new AraEvent<DAddRemuveChild>();
        public AraEvent<DAddRemuveChild> AddChildAfter = new AraEvent<DAddRemuveChild>();
        public AraEvent<DAddRemuveChild> RemuveChildBefore = new AraEvent<DAddRemuveChild>();

        List<AraObjectInstance<IAraObject>> _Childs = new List<AraObjectInstance<IAraObject>>();

        public IAraObject AddChild(IAraObject Child)
        {
            if (this is IAraComponentVisual && Child is IAraComponentVisual)
            {
                if (((IAraComponentVisual)this).TypePosition == AraComponentVisual.ETypePosition.Static && ((IAraComponentVisual)Child).TypePosition == AraComponentVisual.ETypePosition.Static)
                    throw new Exception("is not possible to add an absolute object within a static");
            }

            if (AddChildBefore.InvokeEvent != null)
                AddChildBefore.InvokeEvent(Child);

            _Childs.Add(new AraObjectInstance<IAraObject>(Child));

            if (AddChildAfter.InvokeEvent != null)
                AddChildAfter.InvokeEvent(Child);
            return Child;
        }


        public void RemuveChild(IAraObject Child)
        {
            if (Child != null)
            {
                if (RemuveChildBefore.InvokeEvent != null)
                    RemuveChildBefore.InvokeEvent(Child);

                _Childs.RemoveAll(a => a.InstanceID == Child.InstanceID);
            }
        }

        public IAraObject[] Childs
        {
            get
            {
                return _Childs.Select(a=>a.Object).ToArray();
            }
        }

        public void Dispose()
        {
            foreach (IAraObject CompF in Childs)
            {
                if (CompF!=null)
                    CompF.Dispose();
            }
        }

        //private AraScrollBar _ScrollBar = null;
        //public AraScrollBar ScrollBar
        //{
        //    get { return _ScrollBar; }
        //    set
        //    {
        //        _ScrollBar = value;
        //        _ScrollBar.ComponentVisual = this;
        //        _ScrollBar.Commit();
        //    }
        //}
    }
}
