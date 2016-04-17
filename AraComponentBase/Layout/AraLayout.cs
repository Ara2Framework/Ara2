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
    [Serializable]
    public class AraLayout 
    {
        [NonSerialized]
        public AraObjectInstance<IAraContainerClient> _Object = new AraObjectInstance<IAraContainerClient>();

        public IAraContainerClient Object 
        {
            get {return _Object.Object;}
            set {
                if (_Object == null)
                    _Object = new AraObjectInstance<IAraContainerClient>();
                _Object.Object=value;
            }
        }

        public string Name;

        public int? LayoutCurrentWidthLess = null;
        public int? LayoutCurrentHeightLess = null;
        public EDeviceType[] DeviceTypes =null;

        public List<AraLayoutObject> Childs = new List<AraLayoutObject>();

        public AraLayout(IAraContainerClient vObj, string vName) 
        {
            Object = vObj;
            Name = vName;
        }

        private AraLayout LayoutDefault
        {
            get
            {
                if (this.Name == null)
                    return this;
                else
                    return Object.Layouts.Where(a => a.Name == null).First();
            }
        }

        public void Save()
        {
            Childs.Clear();

            foreach (IAraDev Obj in AllChilds(Object).Select(a => (IAraDev)a))
            {
                AraLayoutObject vTmpObj = new AraLayoutObject()
                    {
                        Name = Obj.Name,
                        NameFather = ((IAraDev)Obj.ConteinerFather).Name
                    };

                var Propriedades = SAraDevProperty.GetPropertys((IAraDev)Obj)
                                        .Where(a => a.PropertySupportLayout)
                                        .ToArray();

                foreach (SAraDevProperty P in Propriedades)
                {
                    vTmpObj.Propertys.Add(P.Nome, P.Value);
                }

                Childs.Add(vTmpObj);
            }
        }

        public void Load()
        {

            foreach (AraLayoutObject vTmpChild in Childs)
            {
                IAraDev ObjAra = (IAraDev)Object.Childs.Where(a => a is IAraDev && ((IAraDev)a).Name == vTmpChild.Name).FirstOrDefault();
                if (ObjAra != null)
                {
                    if (((IAraDev)ObjAra.ConteinerFather).Name != vTmpChild.NameFather)
                        ObjAra.ConteinerFather = GetObjectMyName(vTmpChild.NameFather);

                    foreach (KeyValuePair<string, string> vPro in vTmpChild.Propertys)
                    {
                        try
                        {
                            SAraDevProperty.SetProperty(ObjAra, vPro.Key, vPro.Value);
                        }
                        catch { }
                    }

                    if (ObjAra is IAraComponentVisualAnchor)
                        ((IAraComponentVisualAnchor)ObjAra).Anchor.Reflesh();
                }
            }
        }

        private IAraObject GetObjectMyName(string vName)
        {
            return GetObjectMyName(Object, vName);
        }

        private IAraObject GetObjectMyName(IAraObject vObj, string vName)
        {
            if (((IAraDev)vObj).Name == vName)
                return vObj;

            IAraObject vObjR = vObj.Childs.Where(a => ((IAraDev)a).Name == vName).FirstOrDefault();
            if (vObjR != null)
                return vObjR;

            foreach (IAraObject vTmp in vObj.Childs)
            {
                IAraObject vObjR2 = GetObjectMyName(vTmp, vName);
                if (vObjR2 != null)
                    return vObjR2;
            }

            return null;
        }

        private List<IAraDev> AllChilds(IAraObject vObj)
        {
            List<IAraDev> vTmp = new List<IAraDev>();

            // Desativado porque FrmLayout agora é instanciado no windowmain
            //.Where(a => a.GetType() != typeof(Ara2.Components.Layout.FrmLayout))
            foreach (IAraDev vObjF in vObj.Childs.Where(a=> a is IAraDev))
            {
                vTmp.Add(vObjF);

                AraDevComponent AraDevComponent = AraDevComponent.Get(vObjF);
                if (AraDevComponent != null && AraDevComponent.Conteiner)
                    vTmp.AddRange(AllChilds(vObjF));
            }

            return vTmp;
        }

        public AraLayout Clone()
        {
            AraLayout vTmp = new AraLayout(this.Object, Name);
            vTmp.Childs = new List<AraLayoutObject>();
            foreach (AraLayoutObject Lo in this.Childs)
            {
                vTmp.Childs.Add(new AraLayoutObject()
                {
                    Name = Lo.Name,
                    NameFather = Lo.NameFather,
                    Propertys = new Dictionary<string,string>(Lo.Propertys)
                });
            }

            vTmp.LayoutCurrentHeightLess = this.LayoutCurrentHeightLess;
            vTmp.LayoutCurrentWidthLess = this.LayoutCurrentWidthLess;
            vTmp.DeviceTypes = this.DeviceTypes;
            return vTmp;
        }

        public void RenameObject(string vDe, string vPara)
        {
            foreach (AraLayoutObject o in Childs)
            {
                if (o.Name == vDe)
                    o.Name = vPara;
                if (o.NameFather == vDe)
                    o.NameFather = vPara;
            }
        }
    }

    [Serializable]
    public class AraLayoutObject
    {
        public string Name;
        public string NameFather;
        public Dictionary<string, string> Propertys = new Dictionary<string, string>();
    }

    [System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Field)]
    public class PropertySupportLayout : Attribute
    {

    }
}
