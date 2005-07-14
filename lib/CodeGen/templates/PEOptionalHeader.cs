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
 * <%=Time.now%>
 *
 *****************************************************************************/

namespace Mono.Cecil.Binary {

	public sealed class PEOptionalHeader : IHeader, IBinaryVisitable {

		public StandardFieldsHeader StandardFields;
		public NTSpecificFieldsHeader NTSpecificFields;
		public DataDirectoriesHeader DataDirectories;

		internal PEOptionalHeader ()
		{
			StandardFields = new StandardFieldsHeader ();
			NTSpecificFields = new NTSpecificFieldsHeader ();
			DataDirectories = new DataDirectoriesHeader ();
		}

		public void SetDefaultValues ()
		{
		}

		public void Accept (IBinaryVisitor visitor)
		{
			visitor.Visit (this);

			StandardFields.Accept (visitor);
			NTSpecificFields.Accept (visitor);
			DataDirectories.Accept (visitor);
		}
<% header = $headers["PEOptionalHeader.StandardFieldsHeader"] %>
		public sealed class StandardFieldsHeader : IHeader, IBinaryVisitable {

<% header.fields.each { |f| %>			public <%=f.type%> <%=f.property_name%>;<% print("\n") } %>
			internal StandardFieldsHeader ()
			{
			}

			public void SetDefaultValues ()
			{<% header.fields.each { |f| print("\n\t\t\t" +  f.property_name + " = " + f.default + ";") unless f.default.nil? } %>
			}

			public void Accept (IBinaryVisitor visitor)
			{
				visitor.Visit (this);
			}
		}
<% header = $headers["PEOptionalHeader.NTSpecificFieldsHeader"] %>
		public sealed class NTSpecificFieldsHeader : IHeader, IBinaryVisitable {

<% header.fields.each { |f| %>			public <%=f.type%> <%=f.property_name%>;<% print("\n") } %>
			internal NTSpecificFieldsHeader ()
			{
			}

			public void SetDefaultValues ()
			{<% header.fields.each { |f| print("\n\t\t\t" +  f.property_name + " = " + f.default + ";") unless f.default.nil? } %>
			}

			public void Accept (IBinaryVisitor visitor)
			{
				visitor.Visit (this);
			}
		}
<% header = $headers["PEOptionalHeader.DataDirectoriesHeader"] %>
		public sealed class DataDirectoriesHeader : IHeader, IBinaryVisitable {

<% header.fields.each { |f| %>			public <%=f.type%> <%=f.property_name%>;<% print("\n") } %>
			internal DataDirectoriesHeader ()
			{
			}

			public void SetDefaultValues ()
			{<% header.fields.each { |f| print("\n\t\t\t" +  f.property_name + " = " + f.default + ";") unless f.default.nil? } %>
			}

			public void Accept (IBinaryVisitor visitor)
			{
				visitor.Visit (this);
			}
		}
	}
}
