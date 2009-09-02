using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Company.DataGrid.Models
{
	public static class PropertyPathParser
	{
		private static readonly ReadOnlyCollection<Property> emptyList = new ReadOnlyCollection<Property>(new Collection<Property>());

		public static ReadOnlyCollection<Property> GetPropertyNames(string propertyPath)
		{
			// no support for attached properties since the namespaces of their types cannot be resolved
			if (propertyPath.StartsWith("("))
			{
				return emptyList;
			}
			if (propertyPath.StartsWith("."))
			{
				throw new ArgumentException("The specified property path has invalid syntax.", "propertyPath");
			}
			string[] propertyNames = propertyPath.Split(new[] { '.', '[' }, StringSplitOptions.RemoveEmptyEntries);
			List<Property> properties = new List<Property>(propertyNames.Length);
			foreach (string propertyName in propertyNames)
			{
				Property property = new Property();
				if (propertyName.Contains("]"))
				{
					// indexers have a "[" (already excluded by string.Split), an integer (bindings support only integer indices) and a "]"
					if (!Regex.IsMatch(propertyName, @"^[0-9]+\]$"))
					{
						ThrowInvalidPropertyPathException(propertyPath, propertyName);
					}
					property.Name = "Item";
					property.Arguments = new object[] { int.Parse(propertyName.Substring(0, propertyName.Length - 1)) };
				}
				else
				{
					// identifiers begin with any (Unicode) letter or an underscore and may contain only letters, numbers and underscores
					if (!Regex.IsMatch(propertyName, @"^(\p{L}|_)\w*$"))
					{
						ThrowInvalidPropertyPathException(propertyPath, propertyName);
					}
					property.Name = propertyName;
				}
				properties.Add(property);
			}
			return properties.AsReadOnly();
		}

		private static void ThrowInvalidPropertyPathException(string propertyPath, string propertyName)
		{
			throw new ArgumentException(string.Format("The specified property path {0} contains the invalid property {1}.",
			                                          propertyPath, propertyName), "propertyPath");
		}
	}
}
