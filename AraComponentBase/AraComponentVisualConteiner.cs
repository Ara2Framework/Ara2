// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ara2.Dev;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Ara2.Components
{
    [Serializable]
    public abstract class AraComponentVisualConteiner : AraComponentVisual, IAraContainerClient
    {
        public AraComponentVisualConteiner(string vNameObject, IAraContainerClient vConteinerFather, string vTypeNameJS) :
            base(vNameObject, vConteinerFather, vTypeNameJS)
        {
        }

        [AraDevEvent]
        public new AraEvent<AraContainer.DAddRemuveChild> AddChildBefore
        {
            get { return AraContainer.AddChildBefore; }
            set { AraContainer.AddChildBefore = value; }
        }

        [AraDevEvent]
        public new AraEvent<AraContainer.DAddRemuveChild> AddChildAfter
        {
            get { return AraContainer.AddChildAfter; }
            set { AraContainer.AddChildAfter = value; }
        }

        [AraDevEvent]
        public new AraEvent<AraContainer.DAddRemuveChild> RemuveChildBefore
        {
            get { return AraContainer.RemuveChildBefore; }
            set { AraContainer.RemuveChildBefore = value; }
        }

        private AraContainer AraContainer = new AraContainer();

        public new IAraObject AddChild(IAraObject Child)
        {
            return AraContainer.AddChild(Child);
        }


        public new void RemuveChild(IAraObject Child)
        {
            AraContainer.RemuveChild(Child);
        }

        [JsonIgnore]
        public new IAraObject[] Childs
        {
            get
            {
                return AraContainer.Childs;
            }
        }

        public new void Dispose()
        {
            if (_ScrollBar != null)
            {
                _ScrollBar.Dispose();
                _ScrollBar = null;
            }
            AraContainer.Dispose();
            base.Dispose();
        }


        private AraScrollBar _ScrollBar=null;
        public AraScrollBar ScrollBar
        {
            get { return _ScrollBar; }
            set
            {
                _ScrollBar = value;
                _ScrollBar.ComponentVisual = this;
                _ScrollBar.Commit();
            }
        }


        #region Layout
        private AraLayouts _Layouts = null;

        [Browsable(false)]
        public AraLayouts Layouts
        {
            get { return _Layouts; }
            set { _Layouts = value; }
        }

        [AraDevProperty(null)]
        [AraDevPropertyHide]
        [Browsable(false)]
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

                    if (Layouts != null && Layouts.Object == null)
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

    public class AraDevPropertyLayoutCurrent : AraDevPropertyCustomWindow
    {
        public AraDevPropertyLayoutCurrent(object vValueDefault=null)
            :base(vValueDefault)
        {
        }

        public static Type CustomWindowType = null;

        public override IWindowAraDevPropertyCustomWindow Window()
        {
            //return new Layout.FrmLayout(Tick.GetTick().Session.WindowMain);
            return (IWindowAraDevPropertyCustomWindow)Activator.CreateInstance(CustomWindowType, Tick.GetTick().Session.WindowMain);
        }
    }

    public class AraCustomEditorFrmLayout: AraCustomEditor
    {
        public AraCustomEditorFrmLayout():
            base()
        {

        }

        public static Type CustomWindowType = null;

        public override Type GetTypeForm()
        {
            return CustomWindowType;
        }
    }
}
