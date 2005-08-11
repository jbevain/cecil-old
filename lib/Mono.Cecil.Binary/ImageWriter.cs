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
 * Wed Aug 10 01:00:21 CEST 2005
 *
 *****************************************************************************/

namespace Mono.Cecil.Binary {

	using System;
	using System.IO;
	using System.Text;

	using Mono.Cecil;
	using Mono.Cecil.Metadata;

	internal sealed class ImageWriter : BaseImageVisitor {

		private Image m_img;
		private AssemblyKind m_kind;
		private MetadataWriter m_mdWriter;
		private BinaryWriter m_binaryWriter;

		private Section m_textSect;
		private BinaryWriter m_textWriter;
		private Section m_relocSect;
		private BinaryWriter m_relocWriter;

		public ImageWriter (MetadataWriter writer, AssemblyKind kind, BinaryWriter bw)
		{
			m_mdWriter= writer;
			m_img = writer.GetMetadataRoot ().GetImage ();
			m_kind = kind;
			m_binaryWriter = bw;

			m_textWriter = new BinaryWriter (new MemoryStream ());
			m_textWriter.BaseStream.Position = 80;
			m_relocWriter = new BinaryWriter (new MemoryStream ());
		}

		public Image GetImage ()
		{
			return m_img;
		}

		public BinaryWriter GetTextWriter ()
		{
			return m_textWriter;
		}

		public uint GetAligned (uint integer, uint alignWith)
		{
			return (integer + alignWith - 1) & ~(alignWith - 1);
		}

		public void Initialize ()
		{
			Image img = m_img;
			uint sectAlign = img.PEOptionalHeader.NTSpecificFields.SectionAlignment;
			uint fileAlign = img.PEOptionalHeader.NTSpecificFields.FileAlignment;

			m_textSect = img.TextSection;
			foreach (Section s in img.Sections)
				if (s.Name == Section.Relocs)
					m_relocSect = s;

			// size computations, fields setting, etc.
			uint nbSects = (uint) img.Sections.Count;
			img.PEFileHeader.NumberOfSections = (ushort) nbSects;

			// build the reloc section data
			uint relocSize = 12;
			m_relocWriter.Write ((uint) 0x2000);
			m_relocWriter.Write (relocSize);
			m_relocWriter.Write ((ushort) 0);
			m_relocWriter.Write ((ushort) 0);

			m_textSect.VirtualSize = (uint) m_textWriter.BaseStream.Length;
			m_relocSect.VirtualSize = (uint) m_relocWriter.BaseStream.Length;

			// start counting before sections headers
			// section start + section header sixe * number of sections
			uint headersEnd = 0x178 + 0x28 * nbSects;
			uint fileOffset = headersEnd;
			uint sectOffset = sectAlign;
			uint imageSize = 0;

			foreach (Section sect in img.Sections) {
				fileOffset = GetAligned (fileOffset, fileAlign);
				sectOffset = GetAligned (sectOffset, sectAlign);

				sect.PointerToRawData = new RVA (fileOffset);
				sect.VirtualAddress = new RVA (sectOffset);
				sect.SizeOfRawData = GetAligned (sect.VirtualSize, fileAlign);

				fileOffset += sect.SizeOfRawData;
				sectOffset += sect.SizeOfRawData;
				imageSize += GetAligned (sect.SizeOfRawData, sectAlign);
			}

			if (m_textSect.VirtualAddress.Value != 0x2000)
				throw new ImageFormatException ("Wrong RVA for .text section");

			img.PEOptionalHeader.StandardFields.CodeSize = GetAligned (
				m_textSect.SizeOfRawData, fileAlign);
			img.PEOptionalHeader.StandardFields.InitializedDataSize = 0x200; // + rsrc.SizeOfRawData ?
			img.PEOptionalHeader.StandardFields.BaseOfCode = m_textSect.VirtualAddress;
			img.PEOptionalHeader.StandardFields.BaseOfData = m_relocSect.VirtualAddress;

			imageSize += headersEnd;
			img.PEOptionalHeader.NTSpecificFields.ImageSize = GetAligned (imageSize, sectAlign);

			img.PEOptionalHeader.DataDirectories.BaseRelocationTable = new DataDirectory (
				m_relocSect.VirtualAddress, m_relocSect.VirtualSize);

			if (m_kind == AssemblyKind.Dll) {
				img.PEFileHeader.Characteristics = ImageCharacteristics.CILOnlyDll;
				img.HintNameTable.RuntimeMain = HintNameTable.RuntimeMainDll;
				img.PEOptionalHeader.NTSpecificFields.DLLFlags = 0x400;
			} else {
				img.PEFileHeader.Characteristics = ImageCharacteristics.CILOnlyExe;
				img.HintNameTable.RuntimeMain = HintNameTable.RuntimeMainExe;
			}

			img.PEOptionalHeader.NTSpecificFields.SubSystem = (SubSystem) m_kind;

			RVA importTable = new RVA (img.TextSection.VirtualAddress + m_mdWriter.ItStartPos);

			img.PEOptionalHeader.DataDirectories.ImportTable = new DataDirectory (importTable, 0x57);

			img.ImportTable.ImportLookupTable = new RVA ((uint) importTable + 0x28);

			img.ImportLookupTable.HintNameRVA = img.ImportAddressTable.HintNameTableRVA =
				new RVA ((uint) img.ImportTable.ImportLookupTable + 0x14);
			img.ImportTable.Name = new RVA ((uint) img.ImportLookupTable.HintNameRVA + 0xe);
		}

		public override void Visit (DOSHeader header)
		{
			m_binaryWriter.Write (header.Start);
			m_binaryWriter.Write (header.Lfanew);
			m_binaryWriter.Write (header.End);

			m_binaryWriter.Write ((ushort) 0x4550);
			m_binaryWriter.Write ((ushort) 0);
		}

		public override void Visit (PEFileHeader header)
		{
			m_binaryWriter.Write (header.Machine);
			m_binaryWriter.Write (header.NumberOfSections);
			m_binaryWriter.Write (header.TimeDateStamp);
			m_binaryWriter.Write (header.PointerToSymbolTable);
			m_binaryWriter.Write (header.NumberOfSymbols);
			m_binaryWriter.Write (header.OptionalHeaderSize);
			m_binaryWriter.Write ((ushort) header.Characteristics);
		}

		public override void Visit (PEOptionalHeader.NTSpecificFieldsHeader header)
		{
			m_binaryWriter.Write (header.ImageBase);
			m_binaryWriter.Write (header.SectionAlignment);
			m_binaryWriter.Write (header.FileAlignment);
			m_binaryWriter.Write (header.OSMajor);
			m_binaryWriter.Write (header.OSMinor);
			m_binaryWriter.Write (header.UserMajor);
			m_binaryWriter.Write (header.UserMinor);
			m_binaryWriter.Write (header.SubSysMajor);
			m_binaryWriter.Write (header.SubSysMinor);
			m_binaryWriter.Write (header.Reserved);
			m_binaryWriter.Write (header.ImageSize);
			m_binaryWriter.Write (header.HeaderSize);
			m_binaryWriter.Write (header.FileChecksum);
			m_binaryWriter.Write ((ushort) header.SubSystem);
			m_binaryWriter.Write (header.DLLFlags);
			m_binaryWriter.Write (header.StackReserveSize);
			m_binaryWriter.Write (header.StackCommitSize);
			m_binaryWriter.Write (header.HeapReserveSize);
			m_binaryWriter.Write (header.HeapCommitSize);
			m_binaryWriter.Write (header.LoaderFlags);
			m_binaryWriter.Write (header.NumberOfDataDir);
		}

		public override void Visit (PEOptionalHeader.StandardFieldsHeader header)
		{
			m_binaryWriter.Write (header.Magic);
			m_binaryWriter.Write (header.LMajor);
			m_binaryWriter.Write (header.LMinor);
			m_binaryWriter.Write (header.CodeSize);
			m_binaryWriter.Write (header.InitializedDataSize);
			m_binaryWriter.Write (header.UninitializedDataSize);
			m_binaryWriter.Write (header.EntryPointRVA.Value);
			m_binaryWriter.Write (header.BaseOfCode.Value);
			m_binaryWriter.Write (header.BaseOfData.Value);
		}

		public override void Visit (PEOptionalHeader.DataDirectoriesHeader header)
		{
			m_binaryWriter.Write (header.ExportTable.VirtualAddress);
			m_binaryWriter.Write (header.ExportTable.Size);
			m_binaryWriter.Write (header.ImportTable.VirtualAddress);
			m_binaryWriter.Write (header.ImportTable.Size);
			m_binaryWriter.Write (header.ResourceTable.VirtualAddress);
			m_binaryWriter.Write (header.ResourceTable.Size);
			m_binaryWriter.Write (header.ExceptionTable.VirtualAddress);
			m_binaryWriter.Write (header.ExceptionTable.Size);
			m_binaryWriter.Write (header.CertificateTable.VirtualAddress);
			m_binaryWriter.Write (header.CertificateTable.Size);
			m_binaryWriter.Write (header.BaseRelocationTable.VirtualAddress);
			m_binaryWriter.Write (header.BaseRelocationTable.Size);
			m_binaryWriter.Write (header.Debug.VirtualAddress);
			m_binaryWriter.Write (header.Debug.Size);
			m_binaryWriter.Write (header.Copyright.VirtualAddress);
			m_binaryWriter.Write (header.Copyright.Size);
			m_binaryWriter.Write (header.GlobalPtr.VirtualAddress);
			m_binaryWriter.Write (header.GlobalPtr.Size);
			m_binaryWriter.Write (header.TLSTable.VirtualAddress);
			m_binaryWriter.Write (header.TLSTable.Size);
			m_binaryWriter.Write (header.LoadConfigTable.VirtualAddress);
			m_binaryWriter.Write (header.LoadConfigTable.Size);
			m_binaryWriter.Write (header.BoundImport.VirtualAddress);
			m_binaryWriter.Write (header.BoundImport.Size);
			m_binaryWriter.Write (header.IAT.VirtualAddress);
			m_binaryWriter.Write (header.IAT.Size);
			m_binaryWriter.Write (header.DelayImportDescriptor.VirtualAddress);
			m_binaryWriter.Write (header.DelayImportDescriptor.Size);
			m_binaryWriter.Write (header.CLIHeader.VirtualAddress);
			m_binaryWriter.Write (header.CLIHeader.Size);
			m_binaryWriter.Write (header.Reserved.VirtualAddress);
			m_binaryWriter.Write (header.Reserved.Size);
		}

		public override void Visit (Section sect)
		{
			foreach (char c in sect.Name)
				m_binaryWriter.Write (c);
			int more = 8 - sect.Name.Length;
			for (int i = 0; i < more; i++)
				m_binaryWriter.Write ((byte) 0);

			m_binaryWriter.Write (sect.VirtualSize);
			m_binaryWriter.Write (sect.VirtualAddress.Value);
			m_binaryWriter.Write (sect.SizeOfRawData);
			m_binaryWriter.Write (sect.PointerToRawData.Value);
			m_binaryWriter.Write (sect.PointerToRelocations.Value);
			m_binaryWriter.Write (sect.PointerToLineNumbers.Value);
			m_binaryWriter.Write (sect.NumberOfRelocations);
			m_binaryWriter.Write (sect.NumberOfLineNumbers);
			m_binaryWriter.Write ((uint) sect.Characteristics);
		}

		public override void Visit (ImportAddressTable iat)
		{
			m_textWriter.BaseStream.Position = 0;
			m_textWriter.Write (iat.HintNameTableRVA.Value);
			m_textWriter.Write (new byte [4]);
		}

		public override void Visit (CLIHeader header)
		{
			m_textWriter.Write (header.Cb);
			m_textWriter.Write (header.MajorRuntimeVersion);
			m_textWriter.Write (header.MinorRuntimeVersion);
			m_textWriter.Write (header.Metadata.VirtualAddress);
			m_textWriter.Write (header.Metadata.Size);
			m_textWriter.Write ((uint) header.Flags);
			m_textWriter.Write (header.EntryPointToken);
			m_textWriter.Write (header.Resources.VirtualAddress);
			m_textWriter.Write (header.Resources.Size);
			m_textWriter.Write (header.StrongNameSignature.VirtualAddress);
			m_textWriter.Write (header.StrongNameSignature.Size);
			m_textWriter.Write (header.CodeManagerTable.VirtualAddress);
			m_textWriter.Write (header.CodeManagerTable.Size);
			m_textWriter.Write (header.VTableFixups.VirtualAddress);
			m_textWriter.Write (header.VTableFixups.Size);
			m_textWriter.Write (header.ExportAddressTableJumps.VirtualAddress);
			m_textWriter.Write (header.ExportAddressTableJumps.Size);
			m_textWriter.Write (header.ManagedNativeHeader.VirtualAddress);
			m_textWriter.Write (header.ManagedNativeHeader.Size);
		}

		public override void Visit (ImportTable it)
		{
			m_textWriter.BaseStream.Position = m_mdWriter.ItStartPos;
			m_textWriter.Write (it.ImportLookupTable.Value);
			m_textWriter.Write (it.DateTimeStamp);
			m_textWriter.Write (it.ForwardChain);
			m_textWriter.Write (it.Name.Value);
			m_textWriter.Write (it.ImportAddressTable.Value);
			m_textWriter.Write (new byte [20]);
		}

		public override void Visit (ImportLookupTable ilt)
		{
			m_textWriter.Write (ilt.HintNameRVA.Value);
			m_textWriter.Write (new byte [16]);
		}

		public override void Visit (HintNameTable hnt)
		{
			m_textWriter.Write (hnt.Hint);
			foreach (char c in hnt.RuntimeMain)
				m_textWriter.Write (c);
			m_textWriter.Write ('\0');
			foreach (char c in hnt.RuntimeLibrary)
				m_textWriter.Write (c);
			m_textWriter.Write ('\0');
			m_textWriter.Write (new byte [4]);

			// patch header with ep rva
			RVA ep = m_img.TextSection.VirtualAddress +
				(uint) m_textWriter.BaseStream.Position;
			long pos = m_binaryWriter.BaseStream.Position;
			m_binaryWriter.BaseStream.Position = 0xa8;
			m_binaryWriter.Write (ep.Value);
			m_binaryWriter.BaseStream.Position = pos;

			// patch reloc Sect with ep
			m_relocWriter.BaseStream.Position = 8;
			m_relocWriter.Write ((ushort) (3 << 12) + (
				ep - m_img.TextSection.VirtualAddress + 2));

			m_textWriter.Write (hnt.EntryPoint);
			m_textWriter.Write (hnt.RVA);
		}

		private void WriteMemStream (BinaryWriter bw)
		{
			m_binaryWriter.Write ((bw.BaseStream as MemoryStream).ToArray ());
		}

		public override void Terminate (Image img)
		{
			m_binaryWriter.BaseStream.Position = 0x200;

			WriteMemStream (m_textWriter);
			m_binaryWriter.Write (
				new byte [(int) (m_textSect.SizeOfRawData - m_textWriter.BaseStream.Length)]);

			WriteMemStream (m_relocWriter);
			m_binaryWriter.Write (
				new byte [(int) (m_relocSect.SizeOfRawData - m_relocWriter.BaseStream.Length)]);
		}
	}
}