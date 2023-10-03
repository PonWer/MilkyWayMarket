using Newtonsoft.Json.Linq;

namespace MilkyWayMarket.Services;

public interface IDataService
{
    Dictionary<string, ItemHistory> History { get; }
    List<string> HistoryKeys { get; }

    bool Initiated { get; }
    event EventHandler<string> DataUpdated;

    Task Init(HttpClient httpClient);

    void ReceiveData(string itemName, bool isAsk, DateTime date, double value);
}

public class DataService : IDataService
{
    public event EventHandler<string> DataUpdated;

    public Dictionary<string, ItemHistory> History { get; } = new();

    public List<string> HistoryKeys { get; private set; } = new();

    public bool Initiated { get; private set; }

    public async Task Init(HttpClient httpClient)
    {
        if (Initiated)
            return;

        var jsonString = await httpClient.GetStringAsync("milkyapi.json");

        var o = JObject.Parse(jsonString);

        foreach (var key in o.First.Values<JObject>())
        {
            foreach (var property in key.Properties())
            {
                HistoryKeys.Add(property.Name);
            }
        }

        Initiated = true;
        DataUpdated?.Invoke(null, string.Empty);
    }

    public void ReceiveData(string itemName, bool isAsk, DateTime date, double value)
    {
	    History.TryAdd(itemName, new ItemHistory());

	    History[itemName].history.TryAdd(date,new Item());

	    if (isAsk)
	    {
		    History[itemName].history[date].Ask = value;
	    }
	    else
	    {
			History[itemName].history[date].Bid = value;
		}

        Console.WriteLine($"Data received for {itemName} isAsk:{isAsk} {date} {value}");
    }
}
public class ItemHistory
{
	public Dictionary<DateTime, Item> history = new();
}
public class Item
{
	public double Ask { get; set; } = -1;

	public double Bid { get; set; } = -1;
}