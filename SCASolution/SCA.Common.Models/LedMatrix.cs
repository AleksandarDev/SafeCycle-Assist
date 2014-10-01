using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace SCA.Common.Models
{
	public class SCAMatrix : LedMatrix
	{
		private const int Size = 8;


		public SCAMatrix() : base(Size, Size)
		{
			
		}
	}

    public class LedMatrix
    {
	    private readonly Led[,] matrix;


		public LedMatrix(int size) : this(size, size) { }

	    public LedMatrix(int width, int height)
	    {
			Contract.Requires(width > 0);
			Contract.Requires(height > 0);

		    this.Width = Width;
		    this.Height = height;

			this.matrix = new Led[this.Width, this.Height];
	    }

	    public Led this[int rowIndex, int columnIndex]
	    {
			get { return this.matrix[rowIndex, columnIndex]; }
			set { this.matrix[rowIndex, columnIndex] = value; }
	    }

	    public Led GetAt(int rowIndex, int columnIndex)
	    {
			return this.matrix[rowIndex, columnIndex];
	    }

		public int Width { get; private set; }
		public int Height { get; private set; }
    }
}
