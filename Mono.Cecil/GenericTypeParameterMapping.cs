using System.Collections.Generic;

namespace Mono.Cecil
{
	public class GenericTypeParameterMapping : Dictionary<GenericParameter, TypeReference> 
	{
		public static readonly GenericTypeParameterMapping Empty = new GenericTypeParameterMapping();
			
		private GenericTypeParameterMapping ()
		{
		}

		public static GenericTypeParameterMapping Create (TypeDefinition typeDefinition)
		{
			var mapping = new GenericTypeParameterMapping();

			while (typeDefinition?.BaseType != null)
			{
				var baseTypeReference = typeDefinition.BaseType;
				var baseTypeDefinition = baseTypeReference.Resolve();

				if (baseTypeReference is GenericInstanceType genericInstanceType && baseTypeDefinition != null)
				{
					for (var i = 0; i < genericInstanceType.GenericArguments.Count; i++)
						mapping.Add(baseTypeDefinition.GenericParameters[i], genericInstanceType.GenericArguments[i]);
				}

				typeDefinition = baseTypeDefinition;
			}

			return mapping;
		}
	}
}