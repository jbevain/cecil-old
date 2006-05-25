//
// MethodCollection.cs
//
// Author:
//   Jb Evain (jbevain@gmail.com)
//
// (C) 2006 Jb Evain
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

namespace Mono.CompilerServices.SymbolStore {

	using System;
	using System.Collections;

	public sealed class MethodCollection : ICollection, ISymbolStoreVisitable {

		ArrayList m_items;
		SymbolStore m_container;

		public Method this [int index] {
			get { return m_items [index] as Method; }
			set { m_items [index] = value; }
		}

		public SymbolStore Container {
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

		public MethodCollection (SymbolStore container)
		{
			m_container = container;
			m_items = new ArrayList ();
		}

		public void Add (Method value)
		{
			m_items.Add (value);
		}

		public void Clear ()
		{
			m_items.Clear ();
		}

		public bool Contains (Method value)
		{
			return m_items.Contains (value);
		}

		public int IndexOf (Method value)
		{
			return m_items.IndexOf (value);
		}

		public void Insert (int index, Method value)
		{
			m_items.Insert (index, value);
		}

		public void Remove (Method value)
		{
			m_items.Remove (value);
		}

		public void RemoveAt (int index)
		{
			m_items.RemoveAt (index);
		}

		public void CopyTo (Array ary, int index)
		{
			m_items.CopyTo (ary, index);
		}

		public void Sort ()
		{
			m_items.Sort (Comparers.MethodComparer.Instance);
		}

		public IEnumerator GetEnumerator ()
		{
			return m_items.GetEnumerator ();
		}

		public void Accept (ISymbolStoreVisitor visitor)
		{
			visitor.VisitMethodCollection (this);
		}
	}
}
