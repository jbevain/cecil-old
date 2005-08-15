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
 * Mon Aug 15 02:23:17 CEST 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Metadata {

	[RId (0x09)]
	public sealed class InterfaceImplTable : IMetadataTable {

		private RowCollection m_rows;

		public InterfaceImplRow this [int index] {
			get { return m_rows [index] as InterfaceImplRow; }
			set { m_rows [index] = value; }
		}

		public RowCollection Rows {
			get { return m_rows; }
			set { m_rows = value; }
		}

		internal InterfaceImplTable ()
		{
		}

		public void Accept (IMetadataTableVisitor visitor)
		{
			visitor.VisitInterfaceImplTable (this);
			this.Rows.Accept (visitor.GetRowVisitor ());
		}
	}

	public sealed class InterfaceImplRow : IMetadataRow {

		public static readonly int RowSize = 8;
		public static readonly int RowColumns = 2;

		public uint Class;
		public MetadataToken Interface;

		internal InterfaceImplRow ()
		{
		}

		public void Accept (IMetadataRowVisitor visitor)
		{
			visitor.VisitInterfaceImplRow (this);
		}
	}
}
