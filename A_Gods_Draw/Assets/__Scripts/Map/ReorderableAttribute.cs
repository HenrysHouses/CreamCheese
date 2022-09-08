//charlie

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace Malee
{
    public class ReorderableAttribute : PropertyAttribute
    {
        public bool add, remove, draggable, singleLine, seperate, sortable;
        public int pageSize;
        public string elementNameProperty, elementNameOverride, elementIconPath, surrogateProperty;
        public Type surrogateType;

        public ReorderableAttribute() : this(null)
        {

        }
        public ReorderableAttribute(string elementNameProperty) : this(true, true, true, elementNameProperty, null, null)
        {

        }

        public ReorderableAttribute(string elementNameProperty, string elementIconPath) : this(true,true,true, elementNameProperty, null, elementIconPath)
        {

        }

        public ReorderableAttribute(string elementNameProperty, string elementNameOverride, string elementIconPath) : this(true, true, true, elementNameProperty, elementNameOverride, elementIconPath)
        {

        }

        public ReorderableAttribute(bool add, bool remove, bool draggable, string elementNameProperty = null, string elementIconPath = null) : this(add, remove, draggable, elementNameProperty, null, elementIconPath)
        {

        }

        public ReorderableAttribute(bool add, bool remove, bool draggable, string elementNameProperty = null, string elementNameOverride = null, string elementIconPath = null)
        {

            this.add = add;
            this.remove = remove;
            this.draggable = draggable;
            this.elementNameProperty = elementNameProperty;
            this.elementNameOverride = elementNameOverride;
            this.elementIconPath = elementIconPath;

            sortable = true;
        }

    }
}

