using System.Collections.Generic;

namespace Mono.Cecil
{
	public class GenericTypeParameterMapping : Dictionary<GenericParameter, TypeReference> 
	{ 
		public static readonly GenericTypeParameterMapping Empty = new GenericTypeParameterMapping();
			
		private GenericTypeParameterMapping () : base (new MetadataEqualityComparer(Empty))
		{
		}

		public static GenericTypeParameterMapping Create (TypeDefinition typeDefinition)
		{
			var mapping = new GenericTypeParameterMapping();

			AddGenericInterfaceTypes(typeDefinition, mapping);
			
			while (typeDefinition?.BaseType != null)
			{
				var baseTypeReference = typeDefinition.BaseType;
				var baseTypeDefinition = baseTypeReference.Resolve();

				if (baseTypeDefinition != null) {
					if (baseTypeReference is GenericInstanceType genericInstanceType)
					{
						for (var i = 0; i < genericInstanceType.GenericArguments.Count; i++)
							mapping.Add(baseTypeDefinition.GenericParameters[i], genericInstanceType.GenericArguments[i]);
					}

					AddGenericInterfaceTypes(baseTypeDefinition, mapping);
				}

				typeDefinition = baseTypeDefinition;
			}

			return mapping;
		}

		private static void AddGenericInterfaceTypes(TypeDefinition baseTypeDefinition, GenericTypeParameterMapping mapping)
		{
			foreach (var interfaceImplementation in baseTypeDefinition.Interfaces)
			{
				if (interfaceImplementation.InterfaceType is GenericInstanceType genericInterfaceInstanceType)
				{
					var interfaceTypeDefinition = interfaceImplementation.InterfaceType.Resolve();

					if (interfaceTypeDefinition != null)
					{
						for (var i = 0; i < genericInterfaceInstanceType.GenericArguments.Count; i++)
							mapping.Add(interfaceTypeDefinition.GenericParameters[i], genericInterfaceInstanceType.GenericArguments[i]);
					}
				}
			}
		}
	}
}