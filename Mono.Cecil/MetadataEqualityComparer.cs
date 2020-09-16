using System;
using System.Collections.Generic;

namespace Mono.Cecil {
	public class MetadataEqualityComparer : IEqualityComparer<TypeReference>, IEqualityComparer<GenericParameter>
	{
		// TODO: add further IEqualityComparer implementations
		
		private readonly GenericTypeParameterMapping genericTypeParameterMapping;
		
		public MetadataEqualityComparer (GenericTypeParameterMapping genericTypeParameterMapping = null)
		{
			this.genericTypeParameterMapping = genericTypeParameterMapping;
		}
		
		public bool Equals (TypeReference a, TypeReference b)
		{
			return MetadataResolver.AreSame (a, b, genericTypeParameterMapping ?? GenericTypeParameterMapping.Empty);
		}

		public int GetHashCode (TypeReference obj)
		{
			return obj.MetadataToken.ToInt32().GetHashCode();
		}

		public bool Equals (GenericParameter a, GenericParameter b)
		{
			return MetadataResolver.AreSame (a, b, genericTypeParameterMapping ?? GenericTypeParameterMapping.Empty);
		}

		public int GetHashCode (GenericParameter obj)
		{
			return obj.MetadataToken.ToInt32().GetHashCode();
		}
	}
}