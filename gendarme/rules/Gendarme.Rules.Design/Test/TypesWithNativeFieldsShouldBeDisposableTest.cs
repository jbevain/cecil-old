//
// Unit tests for TypesWithNativeFieldsShouldBeDisposableRule
//
// Authors:
//	Andreas Noever <andreas.noever@gmail.com>
//
//  (C) 2008 Andreas Noever
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

using Gendarme.Framework;
using Gendarme.Rules.Design;
using Mono.Cecil;
using NUnit.Framework;

namespace Test.Rules.Design {

	class NoNativeFields {
		int A;
		object b;
	}

	class NativeFieldsImplementsIDisposeable : IDisposable {
		object A;
		IntPtr B;

		public void Dispose ()
		{
			throw new NotImplementedException ();
		}
	}

	class NativeFieldsExplicit : IDisposable {
		object A;
		IntPtr B;

		void IDisposable.Dispose ()
		{
			throw new NotImplementedException ();
		}
	}


	class NativeFieldsIntPtr : ICloneable {
		object A;
		IntPtr B;

		public object Clone ()
		{
			throw new NotImplementedException ();
		}
	}

	class NativeFieldsUIntPtr : ICloneable {
		object A;
		UIntPtr B;

		public object Clone ()
		{
			throw new NotImplementedException ();
		}
	}

	class NativeFieldsHandleRef : ICloneable {
		object A;
		System.Runtime.InteropServices.HandleRef B;

		public object Clone ()
		{
			throw new NotImplementedException ();
		}
	}

	abstract class AbstractNativeFields : IDisposable {
		object A;
		System.Runtime.InteropServices.HandleRef B;

		public abstract void Dispose ();
	}

	abstract class AbstractNativeFields2 : IDisposable {
		object A;
		System.Runtime.InteropServices.HandleRef B;

		public abstract void Dispose ();


		void IDisposable.Dispose ()
		{
			throw new NotImplementedException ();
		}
	}

	class NativeFieldsArray : ICloneable {
		object A;
		UIntPtr [] B;

		public object Clone ()
		{
			throw new NotImplementedException ();
		}
	}

	[TestFixture]
	public class TypesWithNativeFieldsShouldBeDisposableTest {

		private TypesWithNativeFieldsShouldBeDisposableRule rule;
		private AssemblyDefinition assembly;


		[TestFixtureSetUp]
		public void FixtureSetUp ()
		{
			string unit = Assembly.GetExecutingAssembly ().Location;
			assembly = AssemblyFactory.GetAssembly (unit);
			rule = new TypesWithNativeFieldsShouldBeDisposableRule ();
		}

		public TypeDefinition GetTest (string name)
		{
			return assembly.MainModule.Types [name];
		}

		[Test]
		public void TestNoNativeFields ()
		{
			TypeDefinition type = GetTest ("Test.Rules.Design.NoNativeFields");
			Assert.IsNull (rule.CheckType (type, new MinimalRunner ()));
		}

		[Test]
		public void TestNativeFieldsImplementsIDisposeable ()
		{
			TypeDefinition type = GetTest ("Test.Rules.Design.NativeFieldsImplementsIDisposeable");
			Assert.IsNull (rule.CheckType (type, new MinimalRunner ()));
		}

		[Test]
		public void TestNativeFieldsExplicit ()
		{
			TypeDefinition type = GetTest ("Test.Rules.Design.NativeFieldsExplicit");
			Assert.IsNull (rule.CheckType (type, new MinimalRunner ()));
		}

		[Test]
		public void TestNativeFieldsIntPtr ()
		{
			TypeDefinition type = GetTest ("Test.Rules.Design.NativeFieldsIntPtr");
			Assert.IsNotNull (rule.CheckType (type, new MinimalRunner ()));
		}

		[Test]
		public void TestNativeFieldsUIntPtr ()
		{
			TypeDefinition type = GetTest ("Test.Rules.Design.NativeFieldsUIntPtr");
			Assert.IsNotNull (rule.CheckType (type, new MinimalRunner ()));
		}

		[Test]
		public void TestNativeFieldsHandleRef ()
		{
			TypeDefinition type = GetTest ("Test.Rules.Design.NativeFieldsHandleRef");
			Assert.IsNotNull (rule.CheckType (type, new MinimalRunner ()));
		}

		[Test]
		public void TestAbstractNativeFields ()
		{
			TypeDefinition type = GetTest ("Test.Rules.Design.AbstractNativeFields");
			Assert.IsNotNull (rule.CheckType (type, new MinimalRunner ()));
		}

		[Test]
		public void TestAbstractNativeFields2 ()
		{
			TypeDefinition type = GetTest ("Test.Rules.Design.AbstractNativeFields2");
			Assert.IsNotNull (rule.CheckType (type, new MinimalRunner ()));
		}

		[Test]
		public void TestNativeFieldsArray ()
		{
			TypeDefinition type = GetTest ("Test.Rules.Design.NativeFieldsArray");
			Assert.IsNotNull (rule.CheckType (type, new MinimalRunner ()));
		}
	}
}