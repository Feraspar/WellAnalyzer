namespace WellAnalyzer.Models
{
	/// <summary>
	/// Модель сводки по скважине.
	/// </summary>
	public class WellSummary
	{
		#region Public Properties

		/// <summary>
		/// Средняя пористость.
		/// </summary>
		public double AveragePorosity { get; }

		/// <summary>
		/// Количество интервалов.
		/// </summary>
		public int IntervalCount { get; }

		/// <summary>
		/// Самая распространённая порода.
		/// </summary>
		public string MostCommonRock { get; }

		/// <summary>
		/// Общая глубина.
		/// </summary>
		public double TotalDepth { get; }

		/// <summary>
		/// Id скважины.
		/// </summary>
		public string WellId { get; }

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// Конструтор класса.
		/// </summary>
		/// <param name="wellId">Id скважины.</param>
		/// <param name="totalDepth">Общая глубина.</param>
		/// <param name="intervalCount">Количество интервалов.</param>
		/// <param name="averagePorosity">Средняя пористость.</param>
		/// <param name="mostCommonRock">Самая распространённая порода.</param>
		public WellSummary(string wellId, double totalDepth, int intervalCount, double averagePorosity, string mostCommonRock)
		{
			WellId = wellId;
			TotalDepth = totalDepth;
			IntervalCount = intervalCount;
			AveragePorosity = averagePorosity;
			MostCommonRock = mostCommonRock;
		}

		#endregion Public Constructors
	}
}