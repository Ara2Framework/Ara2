// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Ara2.Components
{

    public static class AraObjectInstanceStatic
    {
        public static Func<string, object> GetObjectInstanceCustom = null;
    }

    [Serializable]
    public class AraObjectInstance<T> : IAraObjectInstance
    {
        public AraObjectInstance()
        { }

        public AraObjectInstance(string vInstanceID)
        {
            _InstanceID = vInstanceID;
        }

        public AraObjectInstance(IAraObject vAraObject)
        {
            _InstanceID = vAraObject.InstanceID;
            _Object = (T)(object)vAraObject;
            _ObjectIsLoad = true;
        }

        [JsonProperty]
        string _InstanceID = null;

        [Browsable(false)]
        [JsonIgnore]
        public string InstanceID
        {
            get { return _InstanceID; }
        }

        [NonSerialized]
        [JsonIgnore]
        bool _ObjectIsLoad = false;

        [NonSerialized]
        [JsonIgnore]
        T _Object;

        [Browsable(false)]
        [JsonIgnore]
        public T Object
        {
            get
            {
                LoadObject();
                return _Object;
            }
            set
            {
                LoadObject();

                if (value != null)
                {
                    _Object = value;
                    _InstanceID = ((IAraObject)((object)value)).InstanceID;
                    _ObjectIsLoad = true;
                }
                else
                {
                    if (_Object!=null)
                        ((IAraObject)(object)_Object).Dispose();
                    _ObjectIsLoad = false;
                    _InstanceID = null;
                }
            }
        }

        [JsonIgnore]
        public dynamic ObjectDynamic
        {
            get
            {
                return this.Object;
            }
            set
            {
                this.Object = value;
            }
        }

        

        private void LoadObject()
        {
            if (_ObjectIsLoad == false && AraObjectInstanceStatic.GetObjectInstanceCustom != null && this._InstanceID != null)
            {
                _Object = (T)AraObjectInstanceStatic.GetObjectInstanceCustom(this._InstanceID);
                _ObjectIsLoad = _Object != null;
            }
            else
            {
                Ara2.Tick Tick = Ara2.Tick.GetTick();
                if (Tick != null && this._InstanceID != null)
                {
                    if (_ObjectIsLoad == false)
                    {
                        _Object = (T)(object)Tick.Session.GetObject(this._InstanceID);
                        _ObjectIsLoad = _Object != null;
                    }
                    else if (Tick.Session.GetObject(this._InstanceID) == null)
                    {
                        _ObjectIsLoad = false;
                        _InstanceID = null;
                        _Object = (T)(object)null;
                        //Ara2.Tick.GetTick().Session.Objects.RemoveAll(a => a.Object == null);
                    }
                }
            }
        }
    }

    public interface IAraObjectInstance
    {
        string InstanceID { get; }
        dynamic ObjectDynamic { get; set; }
    }
}
