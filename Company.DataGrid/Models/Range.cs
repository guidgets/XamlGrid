namespace Company.Widgets.Models
{
	/// <summary>
	/// Represents a range of indices of items, for example items that participate in a selection.
	/// </summary>
	public struct Range
	{
		public static Range Empty;

		static Range()
		{
			Empty = new Range(-1, -1);
		}

		/// <summary>
		/// Represents a range of indices of items, for example items that participate in a selection.
		/// </summary>
		/// <param name="start">The start of the range, the index of the first item.</param>
		/// <param name="end">The end of the range, the index of the last item.</param>
		public Range(int start, int end) : this()
		{
			this.Start = start;
			this.End = end;
		}

		/// <summary>
		/// Gets the start (the index of the first item) of this <see cref="Range"/>.
		/// </summary>
		public int Start
		{
			get; 
			private set;
		}

		/// <summary>
		/// Gets or sets the end (the index of the first item) of this <see cref="Range"/>..
		/// </summary>
		public int End
		{
			get; 
			private set;
		}

		public static bool operator ==(Range left, Range right)
		{
			return left.Start == right.Start && left.End == right.End;
		}

		public static bool operator !=(Range left, Range right)
		{
			return !(left == right);
		}

		public bool Equals(Range other)
		{
			return this == other;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			return obj.GetType() == typeof(Range) && this.Equals((Range) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (this.Start * 397) ^ this.End;
			}
		}

		public override string ToString()
		{
			return string.Format("Start: {0}; End {1}", this.Start, this.End);
		}

		public bool ContainsRange(Range range)
		{
			return this.Start <= range.Start && range.End <= this.End;
		}
	}
}
