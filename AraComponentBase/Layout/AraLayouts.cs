// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ara2.Dev;
using Newtonsoft.Json;

namespace Ara2.Components
{
    [Serializable]
    public class AraLayouts :List<AraLayout>
    {
        public AraObjectInstance<IAraContainerClient> _Object = new AraObjectInstance<IAraContainerClient>();

        [JsonIgnore]
        public IAraContainerClient Object 
        {
            get {
                if (_Object != null)
                    return _Object.Object;
                else
                    return null;
            }
            set {

                if (_Object == null)
                    _Object = new AraObjectInstance<IAraContainerClient>();

                _Object.Object=value;

                foreach (AraLayout L in this)
                    L.Object = value;

                VerificaEventoResize();
            }
        }

        [NonSerialized]
        public bool? _Enable = true;

        [JsonIgnore]
        public bool Enable
        {
            get { 
                if (_Enable == null) 
                    _Enable = true;  
                return (bool)_Enable; 
            }
            set { _Enable = value; }
        }

        public AraLayouts()
        {
            VerificaEventoResize();
        }

        public AraLayouts(IAraContainerClient vObj) :
            this()
        {
            Object = vObj;
            
        }
        static object SpinLock = new object();

        string _LayoutCurrent = null;

        [AraDevPropertyLayoutCurrent(null)]
        [JsonIgnore]
        public string LayoutCurrent
        {
            get { return _LayoutCurrent; }
            set
            {
                if (Enable)
                {
                    lock (SpinLock)
                    {
                        if (_LayoutCurrent != value)
                        {

                            if (!this.Exists(a => a.Name == value))
                                throw new Exception("Layout not fount.");

                            if (this.Count() == 0)
                                this.Add(new AraLayout(Object, null));


                            if (!NoSave)
                                GetLayoutCurrent().Save();

                            _LayoutCurrent = value;

                            GetLayoutCurrent().Load();
                        }
                    }
                }
            }
        }

        public bool NoSave = false;

        public void RenameCurrent(string vNewName)
        {
            GetLayoutCurrent().Name = vNewName;
            _LayoutCurrent = vNewName;
        }

        public AraLayout GetLayoutCurrent()
        {
            return this.Where(a => a.Name == _LayoutCurrent).FirstOrDefault();
        }

        public void Add(string vName)
        {
            if (vName == null || this.Exists(a => a.Name == vName))
                throw new Exception("Layout name invalid or duplicate.");

            if (this.Count() == 0)
            {
                this.Add(new AraLayout(Object, null));
                GetLayoutCurrent().Save();
            }

            AraLayout vTmpL = GetLayoutCurrent().Clone();
            vTmpL.Name = vName;
            this.Add(vTmpL);
            VerificaEventoResize();
        }

        public void Remove(string vName)
        {

            if (vName == null)
                throw new Exception("Erro on remove defaut layout.");

            if (_LayoutCurrent == vName)
                LayoutCurrent = this.Where(a => a.Name == null).First().Name;

            this.RemoveAll(a => a.Name == vName);
            VerificaEventoResize();
        }

        public void RenameObject(string vDe, string vPara)
        {
            foreach (AraLayout L in this)
                L.RenameObject(vDe, vPara);
        }

        public void VerificaEventoResize()
        {
            if (Enable)
            {
                if (Object != null)
                {
                    IAraComponentVisualDimensionsWidthHeight vTmpObj = (IAraComponentVisualDimensionsWidthHeight)Object;

                    if (this.Count() > 0)
                    {
                        if (!vTmpObj.WidthChangeAfter.Equals(Object.LayoutEventResize))
                        {
                            vTmpObj.WidthChangeAfter += Object.LayoutEventResize;
                            vTmpObj.WidthChangeAfter.TypeThreadEvent = EAraComponentEventTypeThread.ThreadSingle;
                            vTmpObj.HeightChangeAfter += Object.LayoutEventResize;
                            vTmpObj.HeightChangeAfter.TypeThreadEvent = EAraComponentEventTypeThread.ThreadSingle;
                        }
                    }
                    else
                    {
                        if (vTmpObj.WidthChangeAfter.Equals(Object.LayoutEventResize))
                        {
                            vTmpObj.WidthChangeAfter -= Object.LayoutEventResize;
                            vTmpObj.HeightChangeAfter -= Object.LayoutEventResize;
                        }
                    }
                }
            }
        }

        [NonSerialized]
        private decimal ObjectResize_Width;
        [NonSerialized]
        private decimal ObjectResize_Height;

        public void OnObjectResize()
        {
            if (Enable)
            {
                if (Object != null)
                {
                    IAraComponentVisualDimensionsWidthHeight vTmpObj = (IAraComponentVisualDimensionsWidthHeight)Object;


                    if (ObjectResize_Width != vTmpObj.Width.Value || ObjectResize_Height != vTmpObj.Height.Value)
                    {
                        ObjectResize_Width = vTmpObj.Width.Value;
                        ObjectResize_Height = vTmpObj.Height.Value;

                        Render();
                    }

                }
            }
        }

        private bool _Render = false;
        public void Render()
        {
            if (Enable)
            {
                IAraComponentVisualDimensionsWidthHeight vTmpObj = (IAraComponentVisualDimensionsWidthHeight)Object;
                EDeviceType DType = AraTools.DeviceType;

                if (vTmpObj != null)
                {
                    var vLayoutTmp = this.Where(L =>
                                      L.Name != null
                                   && (L.LayoutCurrentWidthLess == null || vTmpObj.Width.Value <= (int)L.LayoutCurrentWidthLess)
                                   && (L.LayoutCurrentHeightLess == null || vTmpObj.Height.Value <= (int)L.LayoutCurrentHeightLess)
                                   && (L.DeviceTypes == null || L.DeviceTypes.Where(a => a.Equals(DType)).Count() > 0)
                                   && (L.LayoutCurrentWidthLess != null || L.LayoutCurrentHeightLess != null || L.DeviceTypes != null)
                                   );

                    AraLayout vLayout;
                    int Count = vLayoutTmp.Count();
                    if (Count == 0)
                        vLayout = this.Where(a => a.Name == null).First();
                    else if (Count == 1)
                        vLayout = vLayoutTmp.First();
                    else
                        vLayout = (from a in vLayoutTmp
                                   orderby (a.LayoutCurrentWidthLess == null ? 10000 : a.LayoutCurrentWidthLess),
                                           (a.LayoutCurrentHeightLess == null ? 10000 : a.LayoutCurrentHeightLess)
                                   select a)
                                   .FirstOrDefault();

                    if (LayoutCurrent != vLayout.Name)
                        LayoutCurrent = vLayout.Name;
                }
            }
        }
    }

}
