using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeiveEx.Utilities
{
	public static partial class UtilityServices
	{
		public static class ReflectionService
		{
			/// <summary>
			/// Finds all child types of the given type.
			/// </summary>
			/// <param name="baseType">The base type to find the child classes of</param>
			/// <param name="targetAssembly">An optional assembly to use for limiting the search. If none is provided, all assemblies in the current app domain will be included</param>
			/// <returns>And array of all child types that inherits from the base type</returns>
			public static IEnumerable<Type> GetAllChildTypes(Type baseType, Assembly targetAssembly = null)
			{
				Assembly[] assemblies = null;

				if (targetAssembly == null)
					assemblies = AppDomain.CurrentDomain.GetAssemblies();
				else
					assemblies = new[] { targetAssembly };

				var validTypes = assemblies
					.SelectMany(x => x.GetTypes())
					.Where(x =>
						x.IsClass &&
						!x.IsAbstract &&
						(x.IsSubclassOf(baseType) || //If the base type is a class
						 baseType.IsAssignableFrom(x))); //If the base type is an interface

				return validTypes;
			}
		}
	}
}
