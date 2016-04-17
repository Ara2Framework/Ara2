// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ara2.Dev;

namespace Ara2.Components
{
    public delegate void DAddRemuveChild(IAraObject Child);

    public interface IAraContainerClient : IAraComponentVisual
    {
        AraScrollBar ScrollBar { get; set; }

        AraLayouts Layouts { get; set; }
        string LayoutsString { get; set; }
        string LayoutCurrent { get; set; }
        void LayoutEventResize();
    } 


    public interface IAraObject:IDisposable
    {
        string InstanceID { get; set; }
        IAraObject ConteinerFather { get; set; }

        AraEvent<AraObject.DChangeConteinerFatherBefore> ChangeConteinerFatherBefore { get; set; }
        AraEvent<Action> ChangeConteinerFatherAfter { get; set; }

        AraEvent<DAddRemuveChild> AddChildBefore { get; set; }
        AraEvent<DAddRemuveChild> AddChildAfter { get; set; }
        AraEvent<DAddRemuveChild> RemuveChildBefore { get; set; }

        IAraObject AddChild(IAraObject Child);
        void RemuveChild(IAraObject Child);
        IAraObject[] Childs { get; }
    }

    public interface IAraObjectClienteServer : IAraObject
    {
        void TickScriptCall();
        void CssAddClass(string vNameClass);
        void CssRemoveClass(string vNameClass);
        void Style(string vInstanceID, string vValue);

        AraEvent<DComponentEventInternal> EventInternal { get; set; }
        AraEvent<DComponentProperty> SetProperty { get; set; }
        void LoadJS();
    }

    public delegate void DComponentEventInternal(string vFunction);
    public delegate void DComponentProperty(String vProperty,dynamic vValue);

  

    public interface IAraComponentVisualPositionLeftTop
    {
        AraDistance Left { get; set; }
        AraDistance Top { get; set; }
    }

    public interface IAraComponentVisualDimensionsWidthHeight 
    {
        AraDistance MinWidth { get; set; }
        AraDistance MinHeight { get; set; }

        AraDistance Width { get; set; }
        AraDistance Height { get; set; }

        AraEvent<AraDistance.DChangeDistance> WidthChangeBefore { get; set; }
        AraEvent<AraDistance.DChangeDistance> HeightChangeBefore { get; set; }

        AraComponentEvent<Action> WidthChangeAfter { get; set; }
        AraComponentEvent<Action> HeightChangeAfter { get; set; }
    }

    public interface IAraComponentVisual : IAraObjectClienteServer, IAraComponentVisualPositionLeftTop, IAraComponentVisualDimensionsWidthHeight
    {
        bool Visible { get; set; }
        AraComponentVisual.ETypePosition TypePosition { get; set; }
        AraResizable Resizable { get; set; }
        AraDraggable Draggable { get; set; }
        AraSelectable Selectable { get; set; }
        int? ZIndex { get; set; }
    }

    public interface IAraComponentVisualAnchor : IAraComponentVisual
    {
        AraAnchor Anchor{ get; }
    }

    public interface IAraEvent
    {
        string Name { get; set; }
    }

    public interface ITaskbar: IAraComponentVisual
    {

    }

}
