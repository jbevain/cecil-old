//
// Unit tests for TypeExposeFieldsRule
//
// Authors:
//	Sebastien Pouliot <sebastien@ximian.com>
//
// Copyright (C) 2005 Novell, Inc (http://www.novell.com)
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

using System;
using System.Reflection;
using System.Security.Permissions;
using SSP = System.Security.Permissions;

using Gendarme.Framework;
using Gendarme.Rules.Security;
using Mono.Cecil;
using NUnit.Framework;

namespace Test.Rules.Security {

	[TestFixture]
	public class TypeExposeFieldsTest {

		[SecurityPermission (System.Security.Permissions.SecurityAction.InheritanceDemand, Unrestricted = true)]
		class NonPublicClass {

			public object Field = new object ();

			public NonPublicClass ()
			{
			}
		}

		public class NoSecurityClass {

			public object Field = new object ();

			public NoSecurityClass ()
			{
			}
		}

		[SecurityPermission (SSP.SecurityAction.Deny, Unrestricted = true)]
		public class NoDemandClass {

			public object Field = new object ();

			public NoDemandClass ()
			{
			}
		}

		[SecurityPermission (SSP.SecurityAction.LinkDemand, Unrestricted = true)]
		public class NoPublicFieldClass {

			protected object Field = new object ();

			public NoPublicFieldClass ()
			{
			}
		}

		[SecurityPermission (SSP.SecurityAction.LinkDemand, Unrestricted = true)]
		public class LinkDemandWithFieldClass {

			public object Field = new object ();

			public LinkDemandWithFieldClass ()
			{
			}
		}

		[SecurityPermission (SSP.SecurityAction.LinkDemand, Unrestricted = true)]
		public class DemandWithFieldClass {

			public object Field = new object ();

			public DemandWithFieldClass ()
			{
			}
		}

		private ITypeRule rule;
		private IAssemblyDefinition assembly;
		private IModuleDefinition module;

		[TestFixtureSetUp]
		public void FixtureSetUp ()
		{
			string unit = Assembly.GetExecutingAssembly ().Location;
			assembly = AssemblyFactory.GetAssembly (unit);
			module = assembly.MainModule;
			rule = new TypeExposeFieldsRule ();
		}

		private ITypeDefinition GetTest (string name)
		{
			string fullname = "Test.Rules.Security.TypeExposeFieldsTest/" + name;
			return assembly.MainModule.Types[fullname];
		}

		[Test]
		public void NonPublic ()
		{
			ITypeDefinition type = GetTest ("NonPublicClass");
			Assert.IsTrue (rule.CheckType (assembly, module, type));
		}

		[Test]
		public void NoSecurity ()
		{
			ITypeDefinition type = GetTest ("NoSecurityClass");
			Assert.IsTrue (rule.CheckType (assembly, module, type));
		}

		[Test]
		public void NoDemand ()
		{
			ITypeDefinition type = GetTest ("NoDemandClass");
			Assert.IsTrue (rule.CheckType (assembly, module, type));
		}

		[Test]
		public void NoPublicField ()
		{
			ITypeDefinition type = GetTest ("NoPublicFieldClass");
			Assert.IsTrue (rule.CheckType (assembly, module, type));
		}

		[Test]
		public void LinkDemandWithField ()
		{
			ITypeDefinition type = GetTest ("LinkDemandWithFieldClass");
			Assert.IsFalse (rule.CheckType (assembly, module, type));
		}

		[Test]
		public void DemandWithField ()
		{
			ITypeDefinition type = GetTest ("DemandWithFieldClass");
			Assert.IsFalse (rule.CheckType (assembly, module, type));
		}
	}
}