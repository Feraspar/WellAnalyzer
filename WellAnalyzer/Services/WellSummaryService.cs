namespace WellAnalyzer.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using WellAnalyzer.Abstractions;
	using WellAnalyzer.Models;

	/// <summary>
	/// Сервис расчета сводки по скважине.
	/// </summary>
	public class WellSummaryService
	{
		#region Public Methods

		/// <summary>
		/// Собирает сводку по скважине.
		/// </summary>
		/// <param name="well">Модель скважины.</param>
		/// <returns>Сводка по скважине.</returns>
		public WellSummary BuildSummary(Well well)
		{
			ArgumentNullException.ThrowIfNull(well);

			double totalDepth = well.Intervals.Max(interval => interval.DepthTo);
			int intevalCount = well.Intervals.Count();
			double averagePorosity = CalculateAveragePorosity(well.Intervals);
			string mostCommonRock = CalculateMostCommonRock(well.Intervals);

			return new WellSummary(well.WellId, totalDepth, intevalCount, averagePorosity, mostCommonRock);
		}

		#endregion Public Methods

		#region Private Methods

		/// <summary>
		/// Расчитывает среднюю пористость по длине интервалов.
		/// </summary>
		/// <param name="intervals">Интервалы.</param>
		/// <returns>Средняя пористость.</returns>
		private double CalculateAveragePorosity(IReadOnlyList<Interval> intervals)
		{
			double totalThickness = 0;
			double weightedPorositySum = 0;

			foreach (Interval interval in intervals)
			{
				double thickness = interval.DepthTo - interval.DepthFrom;

				totalThickness += thickness;
				weightedPorositySum += interval.Porosity * thickness;
			}

			if (totalThickness == 0)
				return 0;

			double averagePorosity = weightedPorositySum / totalThickness;

			return averagePorosity;
		}

		/// <summary>
		/// Расчитывает самую распространённую породу по суммарной толщине.
		/// </summary>
		/// <param name="intervals">Интервалы.</param>
		/// <returns>Самая распространенная порода.</returns>
		private string CalculateMostCommonRock(IReadOnlyList<Interval> intervals)
		{
			Dictionary<string, double> rockThickness = new Dictionary<string, double>();

			foreach (Interval interval in intervals)
			{
				double thickness = interval.DepthTo - interval.DepthFrom;

				if (rockThickness.ContainsKey(interval.Rock))
				{
					rockThickness[interval.Rock] += thickness;
				}
				else
				{
					rockThickness[interval.Rock] = thickness;
				}
			}

			string mostCommonRock = string.Empty;
			double maxThickness = -1;

			foreach (KeyValuePair<string, double> pair in rockThickness)
			{
				if (pair.Value > maxThickness)
				{
					maxThickness = pair.Value;
					mostCommonRock = pair.Key;
				}
			}

			return mostCommonRock;
		}

		#endregion Private Methods
	}
}