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
 * Thu Feb 24 01:20:27 Paris, Madrid 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Metadata {

    [RId (0x24)]
    internal sealed class AssemblyRefProcessorTable : IMetadataTable {

        private RowCollection m_rows;

        public AssemblyRefProcessorRow this [int index] {
            get { return m_rows [index] as AssemblyRefProcessorRow; }
            set { m_rows [index] = value; }
        }

        public RowCollection Rows {
            get { return m_rows; }
            set { m_rows = value; }
        }

        public void Accept (IMetadataTableVisitor visitor)
        {
            visitor.Visit (this);
            this.Rows.Accept (visitor.GetRowVisitor ());
        }
    }

    internal sealed class AssemblyRefProcessorRow : IMetadataRow {

        public static readonly int RowSize = 8;
        public static readonly int RowColumns = 2;

        private uint m_processor;
        private uint m_assemblyRef;

        public uint Processor {
            get { return m_processor; }
            set { m_processor = value; }
        }

        public uint AssemblyRef {
            get { return m_assemblyRef; }
            set { m_assemblyRef = value; }
        }

        public int Size {
            get { return AssemblyRefProcessorRow.RowSize; }
        }

        public int Columns {
            get { return AssemblyRefProcessorRow.RowColumns; }
        }

        public void Accept (IMetadataRowVisitor visitor)
        {
            visitor.Visit (this);
        }
    }
}
