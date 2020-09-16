using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using Mono.Cecil.Rocks;

namespace Mono.Cecil.Tests {

	[TestFixture]
	public class MethodDefinitionRocksTests : BaseTestFixture {

		abstract class GenericFoo<T, UFoo> {
			public abstract void DoFoo (T[] t);
			public abstract T DoBar ();

			public virtual void DoBaz (Dictionary<T, UFoo> dictionary)
			{
				
			}
		}

		class GenericBar<UBar> : GenericFoo<string, UBar> 
		{
			public override void DoFoo (string[] t)
			{
			}

			public override string DoBar ()
			{
				return string.Empty;
			}
		}

		class GenericBaz<V> : GenericBar<Dictionary<int, V>>
		{
			public override void DoFoo (string[] t)
			{
			}

			public override string DoBar ()
			{
				return string.Empty;
			}

			public override void DoBaz (Dictionary<string, Dictionary<int, V>> dictionary)
			{
			}
		}

		class OtherGenericFoo<T1, T2> : GenericFoo<T1, T2> {
			public override void DoFoo (T1[] t)
			{
			}

			public override T1 DoBar ()
			{
				throw new System.NotImplementedException ();
			}

			public override void DoBaz (Dictionary<T1, T2> dictionary)
			{
				throw new System.NotImplementedException ();
			}
		}

		abstract class Foo {
			public abstract void DoFoo ();
			public abstract void DoBar ();
		}

		class Bar : Foo {
			public override void DoFoo ()
			{
			}

			public override void DoBar ()
			{
			}
		}

		class Baz : Bar {
			public override void DoFoo ()
			{
			}

			public virtual new void DoBar ()
			{
			}
		}

		[Test]
		public void GetBaseMethod ()
		{
			var baz = typeof (Baz).ToDefinition ();
			var baz_dofoo = baz.GetMethod ("DoFoo");

			var @base = baz_dofoo.GetBaseMethod ();
			Assert.AreEqual ("Bar", @base.DeclaringType.Name);

			@base = @base.GetBaseMethod ();
			Assert.AreEqual ("Foo", @base.DeclaringType.Name);

			Assert.AreEqual (@base, @base.GetBaseMethod ());

			var new_dobar = baz.GetMethod ("DoBar");
			@base = new_dobar.GetBaseMethod();
			Assert.AreEqual("Baz", @base.DeclaringType.Name);
		}

		[Test]
		public void GetOriginalBaseMethod ()
		{
			var baz = typeof (Baz).ToDefinition ();
			var baz_dofoo = baz.GetMethod ("DoFoo");

			var @base = baz_dofoo.GetOriginalBaseMethod ();
			Assert.AreEqual ("Foo", @base.DeclaringType.Name);
		}
		
		[Test]
		public void GetBaseMethod_WithGenerics ()
		{
			var baz = typeof (GenericBaz<>).ToDefinition ();
			var baz_dofoo = baz.GetMethod ("DoFoo");

			var @base = baz_dofoo.GetBaseMethod ();
			Assert.AreEqual ("GenericBar`1", @base.DeclaringType.Name);

			@base = @base.GetBaseMethod ();
			Assert.AreEqual ("GenericFoo`2", @base.DeclaringType.Name);
			Assert.AreEqual (@base, @base.GetBaseMethod ());

			var baz_dobar = baz.GetMethod ("DoBar");
			
			@base = baz_dobar.GetBaseMethod ();
			Assert.AreEqual ("GenericBar`1", @base.DeclaringType.Name);
			
			@base = @base.GetBaseMethod ();
			Assert.AreEqual ("GenericFoo`2", @base.DeclaringType.Name);
			Assert.AreEqual (@base, @base.GetBaseMethod ());
			
			var strangeGenericFoo = typeof (OtherGenericFoo<int, string>).ToDefinition ();
			var strangeGenericFoo_dofoo = strangeGenericFoo.GetMethod ("DoFoo");

			@base = strangeGenericFoo_dofoo.GetBaseMethod ();
			Assert.AreEqual ("GenericFoo`2", @base.DeclaringType.Name);
			
			var baz_dobaz = baz.GetMethod ("DoBaz");
			
			@base = baz_dobaz.GetBaseMethod ();
			// Assert.AreEqual ("GenericBar`1", @base.DeclaringType.Name);
			//
			// @base = @base.GetBaseMethod ();
			Assert.AreEqual ("GenericFoo`2", @base.DeclaringType.Name);
			Assert.AreEqual (@base, @base.GetBaseMethod ());
		}

		[Test]
		public void GetOriginalBaseMethod_WithGenerics ()
		{
			var baz = typeof (GenericBaz<>).ToDefinition ();
			var baz_dofoo = baz.GetMethod ("DoFoo");

			var @base = baz_dofoo.GetOriginalBaseMethod ();
			Assert.AreEqual ("GenericFoo`2", @base.DeclaringType.Name);
			
			var baz_dobar = baz.GetMethod ("DoBar");

			@base = baz_dobar.GetOriginalBaseMethod ();
			Assert.AreEqual ("GenericFoo`2", @base.DeclaringType.Name);
			
			var baz_dobaz = baz.GetMethod ("DoBaz");

			@base = baz_dobaz.GetOriginalBaseMethod ();
			Assert.AreEqual ("GenericFoo`2", @base.DeclaringType.Name);
		}
	}
}
