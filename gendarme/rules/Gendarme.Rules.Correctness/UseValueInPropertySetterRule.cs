//
// Gendarme.Rules.Correctness.UseValueInPropertySetterRule
//
// Authors:
//	Lukasz Knop <lukasz.knop@gmail.com>
//	Sebastien Pouliot <sebastien@ximian.com>
//
// Copyright (C) 2007 Lukasz Knop
// Copyright (C) 2007-2008 Novell, Inc (http://www.novell.com)
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

using Mono.Cecil;
using Mono.Cecil.Cil;

using Gendarme.Framework;
using Gendarme.Framework.Rocks;

namespace Gendarme.Rules.Correctness {

	public class UseValueInPropertySetterRule : IMethodRule {

		private const string MessageString = "Property setter should use the assigned value";
		private const string MessageEmptyString = "Property setter is empty and doesn't use the assigned value";

		public MessageCollection CheckMethod (MethodDefinition method, Runner runner)
		{
			//Skip the test, instead of flooding messages
			//in stubs or empty setters.
			if (!method.HasBody)
				return runner.RuleSuccess;

			// rule applies to setters methods
			if (!method.IsSetter ())
				return runner.RuleSuccess;

			// rule applies
			bool flow = false;
			bool empty = true;
			foreach (Instruction instruction in method.Body.Instructions) {
				switch (instruction.OpCode.Code) {
				// check if the IL use value
				case Code.Ldarg_0:
					// first argument if property is static
					if (method.IsStatic)
						return runner.RuleSuccess;
					empty = false;
					break;
				case Code.Ldarg_1:
					// second argument if property is not static (instance)
					if (!method.IsStatic)
						return runner.RuleSuccess;
					empty = false;
					break;
				case Code.Ldarga:
				case Code.Ldarga_S:
					if ((instruction.Operand as ParameterDefinition).Name == "value")
						return runner.RuleSuccess;
					empty = false;
					break;
				// check if the IL simply throws an exception
				case Code.Throw:
					if (!flow)
						return runner.RuleSuccess;
					empty = false;
					break;
				case Code.Nop:
					break;
				case Code.Ret:
					flow = true;
					break;
				default:
					empty = false;
					// lots of thing can occurs before the throw
					// e.g. loading the string (ldstr)
					//	or calling a method to translate this string
					FlowControl fc = instruction.OpCode.FlowControl;
					flow |= ((fc != FlowControl.Next) && (fc != FlowControl.Call));
					// but as long as the flow continue uninterruped to the throw
					// we consider this a simple throw
					break;
				}
			}

			Location location = new Location (method);
			string msg = empty ? MessageEmptyString : MessageString;
			MessageType mt = empty ? MessageType.Warning : MessageType.Error;
			Message message = new Message (msg, location, mt);
			return new MessageCollection (message);
		}
	}
}
