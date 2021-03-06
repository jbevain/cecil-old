namespace Mono.Cecil.Tests

import System.IO
import System.Reflection
import NUnit.Framework
import Mono.Cecil

[TestFixture]
class RegressionTestFixture:
	
	[Test]
	def OddInt64Initializer():
		AssertAssemblyDefinition("OddInt64Initializer.dll")

	[Test]
	def SingleClass():
		AssertAssemblyDefinition("SingleClass.exe")
		
	[Test]
	def SingleGenericClass():
		AssertAssemblyDefinition("SingleGenericClass.dll")
		
	def AssertAssemblyDefinition(name as string):
		location = Path.Combine(GetTestCasesLocation(), name)
		
		reflectionAssembly = Assembly.ReflectionOnlyLoadFrom(location)
		assert reflectionAssembly is not null
		
		cecilAssembly = AssemblyFactory.GetAssembly(location)
		assert cecilAssembly is not null
		
		Assert.AreEqual(
			ToString(reflectionAssembly),
			ToString(cecilAssembly))
			
	def ToString(asm as System.Reflection.Assembly):
		return ReflectionAssemblyPrinter.ToString(asm)
		
	def ToString(asm as Mono.Cecil.IAssemblyDefinition):
		return CecilAssemblyPrinter.ToString(asm)
		
	static def GetTestCasesLocation():
		basePath = Path.GetDirectoryName(System.Uri(typeof(RegressionTestFixture).Assembly.CodeBase).LocalPath)
		while true:
			location = Path.Combine(basePath, "TestCases")
			return location if Directory.Exists(location)
			oldBasePath = basePath
			basePath = Path.GetDirectoryName(basePath)
			break if oldBasePath == basePath
		raise "TestCases path not found!"
	
