// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Reflection;
using Ara2.Dev;
using System.ComponentModel;


namespace Ara2.Components
{

    [Serializable]
    public class AraObject:IAraObject,IDisposable
    {
        #region Static
        public static AraObject Create(IAraObject ConteinerFather)
        {
            string vId = Tick.GetTick().Session.GetNewID();

            return new AraObject(vId, ConteinerFather);
        }

        #endregion

        public AraObject(string vInstanceID, IAraObject vConteinerFather)
        {
            if (vConteinerFather != null)
                _ConteinerFather = new AraObjectInstance<IAraObject>(vConteinerFather);
            else
                _ConteinerFather = null;

            if (vInstanceID !=null)
                _InstanceID = vInstanceID;
            else
                _InstanceID = Tick.GetTick().Session.GetNewID();

            if (vConteinerFather!=null)
                vConteinerFather.AddChild(this);

            Tick.GetTick().Session.AddObject(this, vConteinerFather);
        }


        

        private string _InstanceID;
        [Browsable(false)]
        [MergableProperty(false)]
        public string InstanceID
        {
            get { return _InstanceID; }
            set { _InstanceID = value; }
        }


        public delegate void DChangeConteinerFatherBefore(IAraObject ToConteinerFather);
        
        private AraEvent<DChangeConteinerFatherBefore> _ChangeConteinerFatherBefore = new AraEvent<DChangeConteinerFatherBefore>();
        [AraDevEvent]
        public AraEvent<DChangeConteinerFatherBefore> ChangeConteinerFatherBefore
        {
            get { return _ChangeConteinerFatherBefore; }
            set { _ChangeConteinerFatherBefore = value; }
        }
        private AraEvent<Action> _ChangeConteinerFatherAfter = new AraEvent<Action>();
        [AraDevEvent]
        public AraEvent<Action> ChangeConteinerFatherAfter
        {
            get { return _ChangeConteinerFatherAfter; }
            set { _ChangeConteinerFatherAfter = value; }
        }

        
        
        private AraObjectInstance<IAraObject> _ConteinerFather=null;

        [Browsable(false)]
        public IAraObject ConteinerFather
        {
            get {

                if (_ConteinerFather != null)
                    return _ConteinerFather.Object;
                else
                    return null;
            }
            set
            {
                if (ChangeConteinerFatherBefore!=null)
                    ChangeConteinerFatherBefore.InvokeEvent(value);

                if (this is IAraObjectClienteServer)
                {
                    ((IAraObjectClienteServer)value).TickScriptCall();
                    Tick.GetTick().Script.Send("var TmpConteinerFather = vObj;");
                    ((IAraObjectClienteServer)this).TickScriptCall();
                    Tick.GetTick().Script.Send("TmpConteinerFather.Obj.appendChild(vObj.Obj);");
                }

                _ConteinerFather = new AraObjectInstance<IAraObject>(value);
                if (ChangeConteinerFatherAfter!=null)
                    ChangeConteinerFatherAfter.InvokeEvent();
            }
        }


        // Flag: Has Dispose already been called?
        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            //GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                disposed = true;

                foreach (IAraObject CompF in Childs)
                {
                    CompF.Dispose();
                }

                Tick.GetTick().Session.DellObject(this);
            }
        }



        private AraEvent<DAddRemuveChild> _AddChildBefore = new AraEvent<DAddRemuveChild>();
        public AraEvent<DAddRemuveChild> AddChildBefore
        {
            get { return _AddChildBefore; }
            set { _AddChildBefore = value; }
        }

        private AraEvent<DAddRemuveChild> _AddChildAfter = new AraEvent<DAddRemuveChild>();
        public AraEvent<DAddRemuveChild> AddChildAfter
        {
            get { return _AddChildAfter; }
            set { _AddChildAfter = value; }
        }

        private AraEvent<DAddRemuveChild> _RemuveChildBefore = new AraEvent<DAddRemuveChild>();
        public AraEvent<DAddRemuveChild> RemuveChildBefore
        {
            get { return _RemuveChildBefore; }
            set { _RemuveChildBefore = value; }
        }

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
            return Child;
        }


        public void RemuveChild(IAraObject Child)
        {
            if (RemuveChildBefore.InvokeEvent != null)
                RemuveChildBefore.InvokeEvent(Child);

            _Childs.RemoveAll(a => a.InstanceID == Child.InstanceID);
        }

        [Browsable(false)]
        public IAraObject[] Childs
        {
            get
            {
                return _Childs.Select(a => a.Object).ToArray();
            }
        }


        
        [OnSerializing()]
        internal void OnSerializingMethod(StreamingContext context)
        {
            
            Tick vTick = Tick.GetTick();
            foreach (FieldInfo a in this.GetType().BaseType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(a => a.GetValue(this) is AraObjectClienteServer))
            {
                throw new Exception("not allowed to serialize an araobject inside another AraObject. Wait only the name and then the AraObject reload on demand.\n Object: " + this.GetType().Name + "." + a.Name);

                //AraObject TmpObj = (AraObject)(a.GetValue(this));
                //if (!Tick.Session.AraObjectsFildsNames.Keys.Contains(this.Name))
                //    Tick.Session.AraObjectsFildsNames.Add(this.Name, new Dictionary<string, string>());
                //Tick.Session.AraObjectsFildsNames[this.Name].Add(a.Name, (TmpObj == null ? null : TmpObj.Name));
                //a.SetValue(this, null);
            }

           
            
        }

        [OnSerialized()]
        internal void OnSerializedMethod(StreamingContext context)
        {
            
        }

        [OnDeserializing()]
        internal void OnDeserializingMethod(StreamingContext context)
        {
            
        }

        [OnDeserialized()]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            //Session.AddAraObjectToReviewPointer(this);
        }
    }
}
