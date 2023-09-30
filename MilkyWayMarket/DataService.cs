using GetDataFromGithub;
using MilkyWayMarket.Code;
using MudBlazor;
using System.IO;
using static MilkyWayMarket.DataService;

namespace MilkyWayMarket
{
	public interface IDataService
	{
		MudTheme CurrentTheme { get; set; }

		Dictionary<string, ItemHistory> History { get; }
		List<string> HistoryKeys { get; }
		event EventHandler<string> DataUpdated;

		bool Initiated { get; }
		bool LatestDeploymentFound { get; }
		bool DbDownloaded { get; }
		bool DatabaseParced { get; }

		string DatabasePath { get; }

		Task Init();
	}
	public class DataService : IDataService
	{
		private bool initiated = false;
		private bool latestDeploymentFound = false;
		private bool dbDownloaded = false;
		private bool databaseParced = false;

		private MudTheme _currentTheme;
		private string path;

		private Dictionary<string, ItemHistory> history = new Dictionary<string, ItemHistory>();
		private List<string> historyKeys = new List<string>();

		public event EventHandler<string> DataUpdated;
		
		public MudTheme CurrentTheme
		{
			get => _currentTheme;
			set => _currentTheme = value;
		}

		public Dictionary<string, ItemHistory> History => history;
		public List<string> HistoryKeys => historyKeys;

		public bool Initiated => initiated;
		public bool LatestDeploymentFound => latestDeploymentFound;
		public bool DbDownloaded => dbDownloaded;
		public bool DatabaseParced => databaseParced;

		public string DatabasePath => path;

		public async Task Init()
		{
			if(initiated)
				return;

			path = $@"{System.IO.Path.GetTempPath()}MilkyWayMarket.db";

			//var latest = await GetDeployments.Call(1, 0);
			latestDeploymentFound = true;
			DataUpdated?.Invoke(null, string.Empty);

			//await GetCommitData.Call(latest.First(),path);
			dbDownloaded = true;
			DataUpdated?.Invoke(null, string.Empty);

			history = await ReadFromDatabase.Call(path, DataUpdated);
			databaseParced = true;
			DataUpdated?.Invoke(null, string.Empty);

            historyKeys = history.Keys.ToList();

            initiated = true;
            DataUpdated?.Invoke(null, string.Empty);
		}
	}


}
