// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ara2;
using Ara2.Components;

namespace Ara2.Dev
{

    [System.AttributeUsage(System.AttributeTargets.Class)]
    [Serializable]
    public class AraDevComponent : Attribute
    {
        public bool Conteiner;
        public bool Base = false;
        public bool DiplayToolBar = true;
        public bool Resizable = true;
        public bool Draggable = true;
        public Type[] CompatibleChildrenTypes;
        public Type AddAlsoToStart = null;
        public AraDevComponent(
            bool vConteiner = true,
            bool vBase = false,
            bool vDisplayToolBar = true,
            bool vResizable = true,
            bool vDraggable = true,
            Type[] vCompatibleChildrenTypes = null,
            Type vAddAlsoToStart = null
            )
        {
            Conteiner = vConteiner;
            Base = vBase;
            DiplayToolBar = vDisplayToolBar;
            Resizable = vResizable;
            Draggable = vDraggable;
            CompatibleChildrenTypes = vCompatibleChildrenTypes;
            AddAlsoToStart = vAddAlsoToStart;
        }

        public static AraDevComponent Get(Ara2.Components.IAraObject vObj)
        {
            return Get(vObj.GetType());
        }
        public static AraDevComponent Get(Type vType)
        {
            return ((AraDevComponent)(vType.GetCustomAttributes(true).FirstOrDefault(a => a.GetType().Equals(typeof(AraDevComponent)))));
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Field)]
    [Serializable]
    public class AraDevProperty : Attribute
    {
        public object ValueDefault;

        public AraDevProperty(object vValueDefault = null)
        {
            ValueDefault = vValueDefault;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Field)]
    [Serializable]
    public abstract class AraDevPropertyCustomWindow : AraDevProperty
    {
        public AraDevPropertyCustomWindow(object vValueDefault)
            :base(vValueDefault)
        {
        }
        
        public virtual IWindowAraDevPropertyCustomWindow Window()
        {
            throw new Exception("AraDevPropertyCustomWindow.Window not override");
        }
        
    }

    public interface IWindowAraDevPropertyCustomWindow
    {
        object ObjectCanvas { get; set; }
        object ObjectCanvasReal { get; set; }
        string Value { get; set; }
    }
    [System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Field)]
    [Serializable]
    public class AraDevPropertyHide : Attribute
    {
    }

    [System.AttributeUsage(System.AttributeTargets.Class)]
    [Serializable]
    public class AraDevClassPropertyString : Attribute
    {

    }

    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
    [Serializable]
    public class AraDevEvent : Attribute
    {
    }

    
}
