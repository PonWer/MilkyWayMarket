using MilkyWayMarket.Code;

namespace MilkyWayMarket
{
	public interface IDataService
	{
		Dictionary<string, ItemHistory> History { get; }
		List<string> HistoryKeys { get; }
		event EventHandler<string> DataUpdated;

		bool Initiated { get; }

		Task Init();
	}
	public class DataService : IDataService
	{
		private bool initiated = false;

		private Dictionary<string, ItemHistory> history = new Dictionary<string, ItemHistory>();
		private List<string> historyKeys = new List<string>();

		public event EventHandler<string> DataUpdated;
		

		public Dictionary<string, ItemHistory> History => history;
		public List<string> HistoryKeys => historyKeys;

		public bool Initiated => initiated;

		public async Task Init()
		{
			if(initiated)
				return;

			historyKeys = history.Keys.ToList();

            initiated = true;
            DataUpdated?.Invoke(null, string.Empty);
		}
	}


}
