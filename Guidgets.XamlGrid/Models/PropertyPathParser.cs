//
// PropertyPathWalker.cs
//
// Contact:
//   Moonlight List (moonlight-list@lists.ximian.com)
//
// Copyright 2010 Novell, Inc.
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

namespace Guidgets.XamlGrid.Models
{
	public class PropertyPathParser
	{
		string Path
		{
			get;
			set;
		}

		public PropertyPathParser(string path)
		{
			Path = path;
		}

		public PropertyNodeType Step(out string typeName, out string propertyName, out string index)
		{
			var type = PropertyNodeType.None;
			if (Path.Length == 0)
			{
				typeName = null;
				propertyName = null;
				index = null;
				return type;
			}

			int end;
			if (Path.StartsWith("("))
			{
				type = PropertyNodeType.AttachedProperty;
				end = Path.IndexOf(")");
				if (end == -1)
					throw new ArgumentException("Invalid property path. Attached property is missing the closing bracket");

				int type_open;
				int type_close;
				int prop_open;
				int prop_close;

				type_open = Path.IndexOf('\'');
				if (type_open > 0)
				{
					// move past the ' char
					++type_open;

					type_close = Path.IndexOf('\'', type_open + 1);
					if (type_close < 0)
						throw new Exception(String.Format("Invalid property path, Unclosed type name '{0}'.", Path));

					prop_open = Path.IndexOf('.', type_close);
					if (prop_open < 0)
						throw new Exception(String.Format("Invalid property path, No property indexer found '{0}'.", Path));

					// move past the . char
					++prop_open;
				}
				else
				{
					type_open = 1;

					type_close = Path.IndexOf('.', type_open);
					if (type_close < 0)
						throw new Exception(String.Format("Invalid property path, No property indexer found on '{0}'.", Path));

					prop_open = type_close + 1;
				}

				prop_close = end;

				typeName = Path.Substring(type_open, type_close - type_open);
				propertyName = Path.Substring(prop_open, prop_close - prop_open);

				index = null;
				Path = Path.Substring(end + 1);
			}
			else if (Path.StartsWith("["))
			{
				type = PropertyNodeType.Indexed;
				end = Path.IndexOf("]");

				typeName = null;
				propertyName = null;
				index = Path.Substring(1, end - 1);
				Path = Path.Substring(end + 1);
				// You can do stuff like: [someIndex].SomeProp
				// as well as: [SomeIndex]SomeProp
				if (Path.StartsWith("."))
					Path = Path.Substring(1);
			}
			else
			{
				type = PropertyNodeType.Property;
				end = Path.IndexOfAny(new[] { '.', '[' });
				if (end == -1)
				{
					propertyName = Path;
					Path = "";
				}
				else
				{
					propertyName = Path.Substring(0, end);
					this.Path = this.Path[end] == '.' ? this.Path.Substring(end + 1) : this.Path.Substring(end);
				}

				typeName = null;
				index = null;
			}

			return type;
		}
	}
}

