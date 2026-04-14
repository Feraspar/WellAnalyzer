namespace WellAnalyzer.Models
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Модель скважины.
	/// </summary>
	public class Well
	{
		#region Private Fields

		/// <summary>
		/// Поле для хранения списка интервалов.
		/// </summary>
		private readonly List<Interval> _intervals = new List<Interval>();

		#endregion Private Fields

		#region Public Properties

		/// <summary>
		/// Список интервала.
		/// </summary>
		public IReadOnlyList<Interval> Intervals => _intervals;

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
		///
		/// </summary>
		/// <param name="wellId">Id скважины.</param>
		/// <param name="x">Координата X.</param>
		/// <param name="y">Координата Y.</param>
		public Well(string wellId, double x, double y)
		{
			WellId = wellId;
			X = x;
			Y = y;
		}

		#endregion Public Constructors

		#region Public Methods

		/// <summary>
		/// Добавляет интервал в коллекцию.
		/// </summary>
		/// <param name="interval"></param>
		public void AddInterval(Interval interval)
		{
			ArgumentNullException.ThrowIfNull(interval);

			_intervals.Add(interval);
		}

		#endregion Public Methods
	}
}