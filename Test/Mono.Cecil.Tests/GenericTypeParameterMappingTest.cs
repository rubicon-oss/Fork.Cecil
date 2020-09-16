using System.Collections.Generic;
using NUnit.Framework;

namespace Mono.Cecil.Tests {
	[TestFixture]
	public class GenericTypeParameterMappingTest : BaseTestFixture {
		public interface IFoo<T> {
		}
		
		public interface IBar<T> {
		}
		
		public class Foo<T, U> : IFoo<T> {
		}
		
		public class Bar<T, U> : Foo<T, U>, IBar<U> {
		}

		public class Baz : Bar<string, int> {
		}

		[Test]
		public void Create ()
		{
			var baz = typeof(Baz).ToDefinition ();
			var bar = typeof(Bar<,>).ToDefinition ();
			var ibar = typeof(IBar<>).ToDefinition ();
			var foo = typeof(Foo<,>).ToDefinition ();
			var ifoo = typeof(IFoo<>).ToDefinition ();
			var @string = baz.Module.TypeSystem.String;
			var @int = baz.Module.TypeSystem.Int32;

			var mapping = GenericTypeParameterMapping.Create (baz);

			IEqualityComparer<TypeReference> equalityComparer = new MetadataEqualityComparer();
			Assert.That (mapping [bar.GenericParameters [0]], Is.EqualTo (@string).Using (equalityComparer)); // bar.T -> string
			Assert.That (mapping [bar.GenericParameters [1]], Is.EqualTo (@int).Using (equalityComparer)); // bar.U -> int
			Assert.That (mapping [foo.GenericParameters [0]], Is.EqualTo (bar.GenericParameters[0]).Using (equalityComparer)); // foo.T -> bar.T
			Assert.That (mapping [foo.GenericParameters [1]], Is.EqualTo (bar.GenericParameters[1]).Using (equalityComparer)); // foo.U -> bar.U
			Assert.That (mapping [ibar.GenericParameters [0]], Is.EqualTo (bar.GenericParameters[1]).Using (equalityComparer)); // IBar.U -> bar.U
			Assert.That (mapping [ifoo.GenericParameters [0]], Is.EqualTo (foo.GenericParameters[0]).Using (equalityComparer)); // IFoo.T -> foo.T
			Assert.That (mapping.Count, Is.EqualTo (6));
		}
	}
}