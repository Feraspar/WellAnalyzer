using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WellAnalyzer.Abstractions;
using WellAnalyzer.Models;
using WellAnalyzer.Results;

namespace WellAnalyzer.ViewModels
{
	/// <summary>
	/// Модель представления главного окна.
	/// </summary>
	public class MainWindowViewModel : ViewModelBase
	{
		#region Private Fields

		/// <inheritdoc <see cref="IFilePickerService" />
		private readonly IFilePickerService _filePickerService;

		/// <inheritdoc <see cref="IJsonExportService" />
		private readonly IJsonExportService _jsonExportService;

		/// <inheritdoc <see cref="IWellFacade" />
		private readonly IWellFacade _wellFacade;

		/// <summary>
		/// Поле для хранения булевого значения доступности кнопки выбора файла.
		/// </summary>
		private bool _isBusy;

		/// <summary>
		/// Поле для хранения статуса выполняемой операции.
		/// </summary>
		private string _statusMessage = "Select a CSV file to upload.";

		#endregion Private Fields

		#region Public Properties

		/// <summary>
		/// Доступна ли кнопка выбора файла.
		/// </summary>
		public bool IsBusy
		{
			get => _isBusy;
			set
			{
				if (SetProperty(ref _isBusy, value))
				{
					OpenFileCommand.NotifyCanExecuteChanged();
					ExportJsonCommand.NotifyCanExecuteChanged();
				}
			}
		}

		/// <summary>
		/// Команда выбора файла.
		/// </summary>
		public IAsyncRelayCommand OpenFileCommand { get; }

		/// <summary>
		/// Команда экспорта сводной информации в JSON.
		/// </summary>
		public IAsyncRelayCommand ExportJsonCommand { get; }

		/// <summary>
		/// Сообщение о статусе выполняемой операции.
		/// </summary>
		public string StatusMessage
		{
			get => _statusMessage;
			set => SetProperty(ref _statusMessage, value);
		}

		/// <summary>
		/// Коллекция ошибок.
		/// </summary>
		public ObservableCollection<ValidationError> ValidationErrors { get; }

		/// <summary>
		/// Коллекция сводок по скважине.
		/// </summary>
		public ObservableCollection<WellSummary> WellSummaries { get; }

		#endregion Public Properties

		#region Public Constructors

		/// <summary>
		/// Конструктор класса.
		/// </summary>
		/// <param name="filePickerService">Сервис выбора файла из файловой системы.</param>
		/// <param name="jsonExportService">Сервис экспорта информации в Json-строку.</param>
		/// <param name="wellFacade">Сервис сборки всей сводки по скважине.</param>
		public MainWindowViewModel(IFilePickerService filePickerService, IJsonExportService jsonExportService, IWellFacade wellFacade)
		{
			_filePickerService = filePickerService;
			_jsonExportService = jsonExportService;
			_wellFacade = wellFacade;

			WellSummaries = new ObservableCollection<WellSummary>();
			ValidationErrors = new ObservableCollection<ValidationError>();

			OpenFileCommand = new AsyncRelayCommand(OpenFileAsync, CanOpenFile);
			ExportJsonCommand = new AsyncRelayCommand(ExportJsonAsync, CanSaveJson);
		}

		#endregion Public Constructors

		#region Private Methods


		/// <summary>
		/// Экспортирует сводку по скважине в JSON.
		/// </summary>
		private async Task ExportJsonAsync()
		{
			try
			{
				IsBusy = true;

				string? filePath = await _filePickerService.PickJsonSaveFileAsync();

				if (string.IsNullOrWhiteSpace(filePath))
				{
					return;
				}

				await _jsonExportService.ExportAsync(WellSummaries, filePath);

				StatusMessage = $"JSON exported successfully: {filePath}";
			}
			catch (Exception ex)
			{
				StatusMessage = $"JSON export failed: {ex.Message}";
			}
			finally
			{
				IsBusy = false;
			}
		}

		/// <summary>
		/// Доступна ли кнопка выбора файла.
		/// </summary>
		/// <returns>Булевое значение.</returns>
		private bool CanOpenFile()
		{
			return !IsBusy;
		}

		/// <summary>
		/// Доступна ли кнопка экспорта JSON.
		/// </summary>
		/// <returns>Булевое значение.</returns>
		private bool CanSaveJson()
		{
			return !IsBusy && WellSummaries.Count > 0;
		}

		/// <summary>
		/// Очищает коллекции UI.
		/// </summary>
		private void ClearCollections()
		{
			WellSummaries.Clear();
			ValidationErrors.Clear();
		}

		/// <summary>
		/// Заполняет коллекцию с ошибками.
		/// </summary>
		/// <param name="errors">Ошибки.</param>
		private void FillErrors(List<ValidationError> errors)
		{
			ValidationErrors.Clear();

			foreach (ValidationError error in errors.OrderBy(error => error.LineNumber))
			{
				ValidationErrors.Add(error);
			}
		}

		/// <summary>
		/// Заполняет коллекцию со сводками.
		/// </summary>
		/// <param name="summaries">Сводки.</param>
		private void FillSummaries(List<WellSummary> summaries)
		{
			WellSummaries.Clear();

			foreach (WellSummary summary in summaries)
			{
				WellSummaries.Add(summary);
			}
		}

		/// <summary>
		/// Пытается прочитать файл и обработать данные.
		/// </summary>
		/// <param name="filePath">Путь к файлу.</param>
		/// <param name="cancellationToken">Токен для отмены выполняемой операции.</param>
		private async Task LoadFileAsync(string filePath, CancellationToken cancellationToken = default)
		{
			try
			{
				IsBusy = true;
				StatusMessage = "Loading file...";

				ClearCollections();

				WellProcessingResult result = await _wellFacade.BuildWellSummariesAsync(filePath, cancellationToken);

				FillSummaries(result.WellSummaries);
				FillErrors(result.ValidationErrors);

				StatusMessage = $"Loaded wells: {result.WellSummaries.Count}. Errors: {result.ValidationErrors.Count}.";
			}
			catch (Exception ex)
			{
				StatusMessage = $"An error occurred: {ex.Message}";
			}
			finally
			{
				IsBusy = false;
			}
		}

		/// <summary>
		/// Вызывает диалог выбора файла.
		/// </summary>
		private async Task OpenFileAsync()
		{
			string? filePath = await _filePickerService.PickCsvFileAsync();

			if (string.IsNullOrWhiteSpace(filePath))
			{
				return;
			}

			await LoadFileAsync(filePath);
		}

		#endregion Private Methods
	}
}