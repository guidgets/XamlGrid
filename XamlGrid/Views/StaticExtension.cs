using System;
using System.Reflection;
using System.Windows.Markup;

namespace XamlGrid.Views
{
	public class StaticExtension : MarkupExtension
	{
		private String member;
		private Type memberType;

		/// <summary>
		///  Obtient un objet correspondant au type fournit en paramètre. Permet de récupérer un champ ou une propriété
		/// </summary>
		/// <param name="serviceProvider">Service permettant de récuperer le service utilisé pour les MarkupExtension.</param>
		/// <returns>
		///  Un objet correspondant à la chaine fournie en paramettre (Member)
		/// </returns>
		public override Object ProvideValue(IServiceProvider serviceProvider)
		{
			Object ret = null;
			Boolean typeResolveFailed = true;
			Type type = MemberType;
			String fieldMemberName = null;

			if (Member != null)
			{
				if (MemberType != null) //on a le type et le membre
				{
					fieldMemberName = Member;
				}
				else
					//on a pas le type, on regarde si la chaine est bien formatée ex : local:MyEnum.MyEnumeValue et on essaie de résoudre le type
				{
					Int32 index = Member.IndexOf('.');

					if (index >= 0)
					{
						string typeName = Member.Substring(0, index);

						if (!String.IsNullOrEmpty(typeName))
						{
							IXamlTypeResolver xamlTypeResolver =
								serviceProvider.GetService(typeof(IXamlTypeResolver)) as IXamlTypeResolver;

							if (xamlTypeResolver != null)
							{
								type = xamlTypeResolver.Resolve(typeName);
								fieldMemberName = Member.Substring(index + 1); //, Member.Length - index - 1
								typeResolveFailed = String.IsNullOrEmpty(fieldMemberName);
							}
						}
					}
				}

				if (typeResolveFailed)
				{
					throw new InvalidOperationException("Member");
				}
				if (type.IsEnum) //si c'est un enum alors on essaie de résoudre le membre
				{
					ret = Enum.Parse(type, fieldMemberName, true);
				}
				else //ce n'est pas un enum : probablement un champ ou une propriété
				{
					Boolean fail = true;

					FieldInfo field = type.GetField(fieldMemberName,
					                                BindingFlags.Public | BindingFlags.FlattenHierarchy |
					                                BindingFlags.Static);

					if (field != null)
					{
						fail = false;
						ret = field.GetValue(null);
					}
					else //ce n'est pas un champ, on regarde si c'est une propriété
					{
						//on regarde si c'est une propriété
						PropertyInfo property = type.GetProperty(fieldMemberName,
						                                         BindingFlags.Public | BindingFlags.FlattenHierarchy |
						                                         BindingFlags.Static);

						if (property != null) //c'est une propriété
						{
							fail = false;
							ret = property.GetValue(null, null);
						}
					}

					if (fail)
					{
						throw new ArgumentException("fullFieldMemberName");
					}
				}
			}
			else
			{
				throw new InvalidOperationException();
			}

			return ret;
		}

		/// <summary>
		/// Obtient ou définit le membre statique à résoudre ex : local:MyEnum.MyEnumValue si pas de MemberType sinon MyEnumValue
		/// </summary>
		public string Member
		{
			get { return this.member; }
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Member value");
				}

				this.member = value;
			}
		}

		/// <summary>
		/// Obtient ou définit le type du membre à résoudre
		/// </summary>
		public Type MemberType
		{
			get { return this.memberType; }
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("MemberType value");
				}

				this.memberType = value;
			}
		}
	}
}
