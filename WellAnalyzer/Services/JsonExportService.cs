using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WellAnalyzer.Abstractions;
using WellAnalyzer.Models;

namespace WellAnalyzer.Services
{
	/// <summary>
	/// Cервис экспорта информации в Json-строку.
	/// </summary>
	public class JsonExportService : IJsonExportService
	{
		#region Public Methods

		/// <inheritdoc />
		public async Task ExportAsync(IReadOnlyCollection<WellSummary> wellSummaries, string filePath, CancellationToken cancellationToken = default)
		{
			ArgumentNullException.ThrowIfNull(wellSummaries);

			if (string.IsNullOrWhiteSpace(filePath))
			{
				throw new ArgumentException("File path must not be empty.", nameof(filePath));
			}

			JsonSerializerOptions options = new JsonSerializerOptions
			{
				WriteIndented = true
			};

			string json = JsonSerializer.Serialize(wellSummaries, options);

			await File.WriteAllTextAsync(filePath, json, cancellationToken);
		}

		#endregion Public Methods
	}
}