namespace Company.DataGrid.Models
{
	public struct SelectionRange
	{
		public SelectionRange(int start, int end) : this()
		{
			this.Start = start;
			this.End = end;
		}

		public int Start { get; private set; }
		public int End { get; private set; }
	}
}
