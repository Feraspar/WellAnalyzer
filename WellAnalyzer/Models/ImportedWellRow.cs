namespace WellAnalyzer.Models
{
	/// <summary>
	/// Модель импортированной строки.
	/// </summary>
	public class ImportedWellRow
	{
		#region Public Properties

		/// <summary>
		/// Начальная глубина.
		/// </summary>
		public double DepthFrom { get; }

		/// <summary>
		/// Конечная глубина.
		/// </summary>
		public double DepthTo { get; }

		/// <summary>
		/// Номер строки.
		/// </summary>
		public int LineNumber { get; }

		/// <summary>
		/// Пористость.
		/// </summary>
		public double Porosity { get; }

		/// <summary>
		/// Порода.
		/// </summary>
		public string Rock { get; }

		/// <summary>
		/// Id скважины.
		/// </summary>
		public string WellId { get; }

		/// <summary>
		/// Координата X.
		/// </summary>
		public double X { get; }

		/// <summary>
		/// Координата Y.
		/// </summary>
		public double Y { get; }

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="lineNumber">Номер строки.</param>
		/// <param name="wellId">Id скважины.</param>
		/// <param name="x">Координата X.</param>
		/// <param name="y">Координата Y.</param>
		/// <param name="depthFrom">Начальная глубина.</param>
		/// <param name="depthTo">Конечная глубина.</param>
		/// <param name="porosity">Пористость.</param>
		/// <param name="rock">Порода.</param>
		public ImportedWellRow(int lineNumber, string wellId, double x, double y, double depthFrom, double depthTo, double porosity, string rock)
		{
			LineNumber = lineNumber;
			WellId = wellId;
			X = x;
			Y = y;
			DepthFrom = depthFrom;
			DepthTo = depthTo;
			Porosity = porosity;
			Rock = rock;
		}

		#endregion Public Constructors
	}
}