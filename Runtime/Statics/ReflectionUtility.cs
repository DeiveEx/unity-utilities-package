using System;
using System.Linq;
using System.Reflection;

namespace DeiveEx.Utilities
{
	public static class ReflectionUtility
	{
		/// <summary>
		/// Finds all child types of the given type in an assembly.
		/// </summary>
		/// <param name="baseType">The base type to find the child classes of</param>
		/// <param name="assembly">An option assembly to use for the search. I none is provided, the search will be done in the same assembly of the base type</param>
		/// <returns>And array of all child types that inherits from the base type</returns>
		public static Type[] GetAllChildTypes(Type baseType, Assembly assembly = null)
		{
			if (assembly == null)
				assembly = baseType.Assembly;
            
			var validTypes = assembly
				.GetTypes()
				.Where(x =>
					       x.IsClass &&
					       !x.IsAbstract &&
					       (x.IsSubclassOf(baseType) || //If the base type is a class
					        baseType.IsAssignableFrom(x))); //If the base type is an interface

			return validTypes.ToArray();
		}
	}
}
