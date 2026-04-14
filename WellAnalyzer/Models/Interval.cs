namespace WellAnalyzer.Models
{
	/// <summary>
	/// Модель для интервала скважины.
	/// </summary>
	public class Interval
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
		/// Пористость.
		/// </summary>
		public double Porosity { get; }

		/// <summary>
		/// Порода.
		/// </summary>
		public string Rock { get; }

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="depthFrom">Начальная глубина.</param>
		/// <param name="depthTo">Конечная глубина.</param>
		/// <param name="rock">Порода.</param>
		/// <param name="porosity">Пористость.</param>
		public Interval(double depthFrom, double depthTo, string rock, double porosity)
		{
			DepthFrom = depthFrom;
			DepthTo = depthTo;
			Rock = rock;
			Porosity = porosity;
		}

		#endregion Public Constructors
	}
}