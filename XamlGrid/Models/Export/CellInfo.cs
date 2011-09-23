namespace XamlGrid.Models.Export
{
	public struct CellInfo
	{
		public static readonly CellInfo Default = new CellInfo(null);

		public object Value { get; set; }

		public CellInfo(object value) : this()
		{
			this.Value = value;
		}
	}
}
