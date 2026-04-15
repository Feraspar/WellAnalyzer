namespace WellAnalyzer.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using WellAnalyzer.Abstractions;
	using WellAnalyzer.Models;

	/// <summary>
	/// Сервис сборки моделей скважин из импортированных строк.
	/// </summary>
	public class WellBuilderService : IWellBuilderService
	{
		#region Public Methods

		/// <inheritdoc />
		public List<Well> Build(List<ImportedWellRow> rows)
		{
			ArgumentNullException.ThrowIfNull(rows);

			List<Well> wells = new List<Well>();

			List<IGrouping<string, ImportedWellRow>> rowsByWell = rows.GroupBy(row => row.WellId).ToList();

			foreach (IGrouping<string, ImportedWellRow> wellGroup in rowsByWell)
			{
				ImportedWellRow firstRow = wellGroup.First();

				Well well = new Well(firstRow.WellId, firstRow.X, firstRow.Y);

				foreach (ImportedWellRow row in wellGroup.OrderBy(row => row.DepthFrom))
				{
					Interval interval = new Interval(row.DepthFrom, row.DepthTo, row.Rock, row.Porosity);
					well.AddInterval(interval);
				}

				wells.Add(well);
			}

			return wells;
		}

		#endregion Public Methods
	}
}