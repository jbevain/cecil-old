/*
 * Copyright (c) 2004 DotNetGuru and the individuals listed
 * on the ChangeLog entries.
 *
 * Authors :
 *   Jb Evain   (jb.evain@dotnetguru.org)
 *
 * This is a free software distributed under a MIT/X11 license
 * See LICENSE.MIT file for more details
 *
 * Generated by /CodeGen/cecil-gen.rb do not edit
 * Sun Feb 20 20:05:55 Paris, Madrid 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Implem {

    using System;
    using System.Collections;

    using Mono.Cecil;
    using Mono.Cecil.Cil;

    internal class CustomAttributeCollection : ICustomAttributeCollection {

        private IList m_items;
        private object m_container;

        public ICustomAttribute this [int index] {
            get { return m_items [index] as ICustomAttribute; }
            set { m_items [index] = value; }
        }

        public object Container {
            get { return m_container; }
        }

        public int Count {
            get { return m_items.Count; }
        }

        public bool IsSynchronized {
            get { return false; }
        }

        public object SyncRoot {
            get { return this; }
        }

        public CustomAttributeCollection (object container)
        {
            m_container = container;
            m_items = new ArrayList ();
        }

        public void Add (ICustomAttribute value)
        {
            m_items.Add (value);
        }

        public void Clear ()
        {
            m_items.Clear ();
        }

        public bool Contains (ICustomAttribute value)
        {
            return m_items.Contains (value);
        }

        public int IndexOf (ICustomAttribute value)
        {
            return m_items.IndexOf (value);
        }

        public void Insert (int index, ICustomAttribute value)
        {
            m_items.Insert (index, value);
        }

        public void Remove (ICustomAttribute value)
        {
            m_items.Remove (value);
        }

        public void RemoveAt (int index)
        {
            m_items.Remove (index);
        }

        public void CopyTo (Array ary, int index)
        {
            m_items.CopyTo (ary, index);
        }

        public IEnumerator GetEnumerator ()
        {
            return m_items.GetEnumerator ();
        }

        public void Accept (IReflectionVisitor visitor)
        {
            visitor.Visit (this);
            ICustomAttribute [] items = new ICustomAttribute [m_items.Count];
            m_items.CopyTo (items, 0);
            for (int i = 0; i < items.Length; i++)
                items [i].Accept (visitor);
        }
    }
}