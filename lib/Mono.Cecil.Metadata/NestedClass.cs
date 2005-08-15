/*
 * Copyright (c) 2004, 2005 DotNetGuru and the individuals listed
 * on the ChangeLog entries.
 *
 * Authors :
 *   Jb Evain   (jbevain@gmail.com)
 *
 * This is a free software distributed under a MIT/X11 license
 * See LICENSE.MIT file for more details
 *
 * Generated by /CodeGen/cecil-gen.rb do not edit
 * Mon Aug 15 02:23:18 CEST 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Metadata {

	[RId (0x29)]
	public sealed class NestedClassTable : IMetadataTable {

		private RowCollection m_rows;

		public NestedClassRow this [int index] {
			get { return m_rows [index] as NestedClassRow; }
			set { m_rows [index] = value; }
		}

		public RowCollection Rows {
			get { return m_rows; }
			set { m_rows = value; }
		}

		internal NestedClassTable ()
		{
		}

		public void Accept (IMetadataTableVisitor visitor)
		{
			visitor.VisitNestedClassTable (this);
			this.Rows.Accept (visitor.GetRowVisitor ());
		}
	}

	public sealed class NestedClassRow : IMetadataRow {

		public static readonly int RowSize = 8;
		public static readonly int RowColumns = 2;

		public uint NestedClass;
		public uint EnclosingClass;

		internal NestedClassRow ()
		{
		}

		public void Accept (IMetadataRowVisitor visitor)
		{
			visitor.VisitNestedClassRow (this);
		}
	}
}
