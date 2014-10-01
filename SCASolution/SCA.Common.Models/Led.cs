using System;

namespace SCA.Common.Models
{
	public class Led
	{
		public Led() : this(false, false, false) { }

		public Led(bool r, bool g, bool b)
		{
			this.R = r;
			this.G = g;
			this.B = b;
		}


		public override string ToString()
		{
			return String.Format("{0}{1}{2}",
				this.R ? "R" : String.Empty,
				this.G ? "G" : String.Empty,
				this.B ? "B" : String.Empty);
		}


		public bool R { get; set; }

		public bool G { get; set; }

		public bool B { get; set; }

		public bool IsOn
		{
			get { return this.R || this.G || this.B; }
		}
	}
}