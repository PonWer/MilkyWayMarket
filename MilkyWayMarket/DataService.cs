using MilkyWayMarket.Code;

namespace MilkyWayMarket;

public interface IDataService
{
	Dictionary<string, ItemHistory> History { get; }
	List<string> HistoryKeys { get; }

	bool Initiated { get; }
	event EventHandler<string> DataUpdated;

	Task Init();
}

public class DataService : IDataService
{
	public event EventHandler<string> DataUpdated;


	public Dictionary<string, ItemHistory> History { get; } = new();

	public List<string> HistoryKeys { get; private set; } = new();

	public bool Initiated { get; private set; }

	public async Task Init()
	{
		if (Initiated)
			return;

		HistoryKeys = History.Keys.ToList();

		Initiated = true;
		DataUpdated?.Invoke(null, string.Empty);
	}
}