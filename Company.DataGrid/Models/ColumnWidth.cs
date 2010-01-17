using System.ComponentModel;
using System.Windows;
using Company.DataGrid.Controllers;

namespace Company.DataGrid.Models
{
	[TypeConverter(typeof(StringToColumnWidthConverter))]
	public struct ColumnWidth
	{
		public ColumnWidth(double value, GridUnitType unitType) : this()
		{
			this.Value = value;
			this.UnitType = unitType;
		}

		public double Value
		{
			get; 
			set;
		}

		public GridUnitType UnitType
		{
			get; 
			set;
		}

		public static bool operator ==(ColumnWidth columnWidthLeft, ColumnWidth columnWidthRight)
		{
			return columnWidthLeft.UnitType == columnWidthRight.UnitType &&
			       columnWidthLeft.Value == columnWidthRight.Value;
		}

		public static bool operator !=(ColumnWidth columnWidthLeft, ColumnWidth columnWidthRight)
		{
			return !(columnWidthLeft == columnWidthRight);
		}

		public bool Equals(ColumnWidth other)
		{
			return other.Value == this.Value && Equals(other.UnitType, this.UnitType);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (obj.GetType() != typeof(ColumnWidth)) return false;
			return Equals((ColumnWidth) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (this.Value.GetHashCode() * 397) ^ this.UnitType.GetHashCode();
			}
		}
	}
}
