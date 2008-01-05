// 
// Unit tests for FieldRocks
//
// Authors:
//	Sebastien Pouliot  <sebastien@ximian.com>
//
// Copyright (C) 2008 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Reflection;

using Gendarme.Framework;
using Gendarme.Framework.Rocks;

using Mono.Cecil;
using NUnit.Framework;

namespace Test.Framework.Rocks {

	[TestFixture]
	public class FieldRocksTest {

		[System.Runtime.CompilerServices.CompilerGeneratedAttribute]
		private static int cga = 1;

		[System.CodeDom.Compiler.GeneratedCodeAttribute ("unit test", "1.0")]
		protected double gca = 1.0;

		private AssemblyDefinition assembly;

		private TypeDefinition type;

		[TestFixtureSetUp]
		public void FixtureSetUp ()
		{
			string unit = Assembly.GetExecutingAssembly ().Location;
			assembly = AssemblyFactory.GetAssembly (unit);
			type = assembly.MainModule.Types ["Test.Framework.Rocks.FieldRocksTest"];
		}

		private FieldDefinition GetField (string fieldName)
		{
			foreach (FieldDefinition field in type.Fields) {
				if (field.Name == fieldName)
					return field;
			}
			Assert.Fail ("Field {0} was not found.", fieldName);
			return null;
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void HasAttribute_Null ()
		{
			GetField ("assembly").HasAttribute (null);
		}

		[Test]
		public void HasAttribute ()
		{
			Assert.IsTrue (GetField ("cga").HasAttribute ("System.Runtime.CompilerServices.CompilerGeneratedAttribute"), "CompilerGeneratedAttribute");
			Assert.IsFalse (GetField ("cga").HasAttribute ("NUnit.Framework.TestFixtureAttribute"), "TestFixtureAttribute");
		}

		[Test]
		public void IsGeneratedCode_CompilerGenerated ()
		{
			Assert.IsTrue (GetField ("cga").IsGeneratedCode (), "IsCompilerGenerated");
			Assert.IsFalse (GetField ("assembly").IsGeneratedCode (), "FixtureSetUp");
		}

		[Test]
		public void IsGeneratedCode_GeneratedCode ()
		{
			Assert.IsTrue (GetField ("gca").IsGeneratedCode (), "IsCompilerGenerated");
			Assert.IsFalse (GetField ("assembly").IsGeneratedCode (), "FixtureSetUp");
		}
	}
}