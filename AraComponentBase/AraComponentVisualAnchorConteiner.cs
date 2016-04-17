// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using Ara2.Dev;
using System.ComponentModel;
using System.Globalization;

namespace Ara2.Components
{
    [Serializable]
    public abstract class AraComponentVisualAnchorConteiner : AraComponentVisualAnchor, IAraContainerClient, IDisposable
    {
        AraContainer _Conteiner = new AraContainer();

        public AraComponentVisualAnchorConteiner(string vNameObject, IAraObject vConteinerFather, string vTypeNameJS) :
            base(vNameObject, vConteinerFather, vTypeNameJS)
        {
            //if (vConteinerFather != null)
            //{
            //    if (vConteinerFather is AraComponentVisual)
            //        if (((AraComponentVisual)vConteinerFather).TypePosition == ETypePosition.Static && this.TypePosition == ETypePosition.Absolute)
            //            this.TypePosition = ETypePosition.Static;
                
                
            //}

        }


        [AraDevEvent]
        public AraEvent<AraContainer.DAddRemuveChild> AddChildBefore
        {
            get { return _Conteiner.AddChildBefore; }
            set { _Conteiner.AddChildBefore = value; }
        }

        [AraDevEvent]
        public AraEvent<AraContainer.DAddRemuveChild> AddChildAfter
        {
            get { return _Conteiner.AddChildAfter; }
            set { _Conteiner.AddChildAfter = value; }
        }

        [AraDevEvent]
        public AraEvent<AraContainer.DAddRemuveChild> RemuveChildBefore
        {
            get { return _Conteiner.RemuveChildBefore; }
            set { _Conteiner.RemuveChildBefore = value; }
        }

        public IAraObject AddChild(IAraObject Child)
        {
            return _Conteiner.AddChild(Child);
        }

        public void RemuveChild(IAraObject Child)
        {
            _Conteiner.RemuveChild(Child);
        }

        [Browsable(false)]
        public IAraObject[] Childs
        {
            get
            {
                return _Conteiner.Childs;
            }
        }

        public void Dispose()
        {
            if (_ScrollBar != null)
            {
                _ScrollBar.Dispose();
                _ScrollBar = null;
            }

            _Conteiner.Dispose();
            base.Dispose();
        }

        public abstract override void LoadJS();

        private  AraScrollBar _ScrollBar = null;

        [DefaultValue(null)]
        public AraScrollBar ScrollBar
        {
            get { return _ScrollBar; }
            set
            {
                if (value != null)
                {
                    _ScrollBar = value;
                    _ScrollBar.ComponentVisual = this;
                    _ScrollBar.Commit();
                }
                else if (_ScrollBar != null)
                {
                    _ScrollBar.Dispose();
                    _ScrollBar = null;
                }
            }
        }


        #region Layout
        private AraLayouts _Layouts = null;


        //public class LayoutsEmployeeCollectionEditor : CollectionEditor
        //{
        //    public LayoutsEmployeeCollectionEditor(Type type)
        //        : base(type)
        //    {
        //    }

        //    protected override string GetDisplayText(object value)
        //    {
        //        if (value is AraLayout)
        //            return base.GetDisplayText(((AraLayout)value).Name);
        //        else
        //            return string.Empty;
        //    }

        //    protected override object CreateInstance(Type itemType)
        //    {
        //        dynamic vObj = ((dynamic)this.Context.Instance);
        //        IAraContainerClient vObjCS = Convert.ChangeType(vObj, typeof(IAraContainerClient));

        //        //var nTentativas = 0;

        //        //while(vObj.Layouts!=null && vObj.Layouts.Where(a=>a.Name == "Novo Layout" + (nTentativas > 0 ? " " + nTentativas : string.Empty)).Any())
        //        //{
        //        //    nTentativas++;
        //        //}
        //        //return new AraLayout(vObj, "Novo Layout" + (nTentativas > 0 ? " " + nTentativas : string.Empty));
        //        return new AraLayout(vObjCS, "Novo Layout" );
        //    }
        //}

        [Browsable(false)]
        //[Editor(typeof(LayoutsEmployeeCollectionEditor),
        //    typeof(System.Drawing.Design.UITypeEditor))]
        public AraLayouts Layouts
        {
            get { return _Layouts; }
            set { _Layouts = value; }
        }

        [AraDevProperty(null)]
        [AraDevPropertyHide]
        [Browsable(false)]
        [DefaultValue(null)]
        public string LayoutsString
        {
            get
            {
                if (Layouts != null && Layouts.Count() > 1)
                    return Convert.ToBase64String((new CustomBinarySerializer<AraLayouts>()).Serialize2Bytes(Layouts));
                else
                    return null;
            }
            set
            {
                if (value != null)
                {
                    Layouts = (new CustomBinarySerializer<AraLayouts>()).DeserializeFromBytes(Convert.FromBase64String(value));
                    if (Layouts.Count() <= 1)
                        Layouts = null;

                    if (Layouts != null && (Layouts.Object==null || Layouts.Object.InstanceID != this.InstanceID))
                        Layouts.Object = this;
                }
                else
                    Layouts = null;
            }
        }

        



        [Browsable(true)]
        [AraDevPropertyLayoutCurrent(null)]
        [DefaultValue(null)]
        [TypeConverter(typeof(LayoutCurrentConverter))]
        [AraCustomEditorFrmLayout]
        public string LayoutCurrent
        {
            get
            {
                if (Layouts != null)
                    return Layouts.LayoutCurrent;
                else

                    return null;
            }
            set
            {
                if (Layouts == null)
                    Layouts = new AraLayouts(this);

                Layouts.LayoutCurrent = value;
            }
        }

        public void LayoutEventResize()
        {
            if (Layouts != null)
            {
                lock (_Layouts)
                {
                    Layouts.OnObjectResize();
                }
            }
        }

        #endregion

    }

    
}
