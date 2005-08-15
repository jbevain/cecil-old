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

namespace Mono.Cecil.Binary {

	public sealed class PEFileHeader : IHeader, IBinaryVisitable {

		public ushort Machine;
		public ushort NumberOfSections;
		public uint TimeDateStamp;
		public uint PointerToSymbolTable;
		public uint NumberOfSymbols;
		public ushort OptionalHeaderSize;
		public ImageCharacteristics Characteristics;

		internal PEFileHeader ()
		{
		}

		public void SetDefaultValues ()
		{
			Machine = 0x14c;
			PointerToSymbolTable = 0;
			NumberOfSymbols = 0;
			OptionalHeaderSize = 0xe0;
		}

		public void Accept (IBinaryVisitor visitor)
		{
			visitor.VisitPEFileHeader (this);
		}
	}
}
