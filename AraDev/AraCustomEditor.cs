// Copyright (c) 2010-2016, Rafael Leonel Pontani. All rights reserved.
// For licensing, see LICENSE.md or http://www.araframework.com.br/license
// This file is part of AraFramework project details visit http://www.arafrework.com.br
// AraFramework - Rafael Leonel Pontani, 2016-4-14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ara2.Components;
using System.Reflection;

namespace System.ComponentModel
{
    //
    // Summary:
    //     Specifies what type to use as a converter for the object this attribute is bound
    //     to.
    public class AraCustomEditor : Attribute
    {
        readonly Type TypeForm;

        public AraCustomEditor(Type vTypeForm)
        {
            if (!vTypeForm.GetInterfaces().Contains(typeof(IFormAraCustomEditor)))
                throw new Exception("TypeForm not implement IFormAraCustomEditor");

            TypeForm = vTypeForm;
        }

        protected AraCustomEditor()
        {

        }

        public virtual Type GetTypeForm()
        {
            return TypeForm;
        }
    }

    public interface IFormAraCustomEditor
    {
        object ObjectCanvas { get; set; }
        object ObjectCanvasReal { get; set; }
        PropertyInfo PropertyInfo { get; set; }
        System.Action onRefreshScreen { get; set; }

        bool IsOpenCustomEditor(object vObj);
    }
}