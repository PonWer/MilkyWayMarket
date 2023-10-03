using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;
using MilkyWayMarket.Shared;
using Newtonsoft.Json.Linq;

namespace MilkyWayMarket.Services;

public interface IDataService
{
    Dictionary<string, ItemHistory> History { get; }
    List<string> HistoryKeys { get; }

    bool Initiated { get; }
    event EventHandler<string> DataUpdated;

    Task Query(List<string> items);

	void ReceiveData(string itemName, bool isAsk, DateTime date, double value);
    Task Init(HttpClient httpClient, IJSObjectReference inMooket, IJSObjectReference inDatabase, DotNetObjectReference<MainLayout> dotNetObjectReference);
}

public class DataService : IDataService
{
	private IJSObjectReference dataBase;
	private IJSObjectReference mooket;
	private DotNetObjectReference<MainLayout> dotNetObjectReferenceMailLayout;
	public event EventHandler<string> DataUpdated;

    public Dictionary<string, ItemHistory> History { get; } = new();

    public List<string> HistoryKeys { get; private set; } = new();

    public bool Initiated { get; private set; }

    public async Task Init(HttpClient httpClient, IJSObjectReference inMooket, IJSObjectReference inDatabase, DotNetObjectReference<MainLayout> dotNetObjectReference)
    {
        if (Initiated)
            return;

        mooket = inMooket;
        dataBase = inDatabase;
        dotNetObjectReferenceMailLayout = dotNetObjectReference;
        
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
	public async Task Query(List<string> items)
	{
		var itemsToLoad = new List<string>();
		foreach (var item in items)
		{
			if (!History.ContainsKey(item))
			{
				itemsToLoad.Add(item);
			}
		}

		if (!itemsToLoad.Any())
			return;

		var query = "SELECT DATETIME(time,\"unixepoch\") AS time, " +
		            string.Join(",",itemsToLoad.Select(x => $"\"{x}\"")) +
		            "FROM ask " +
		            "LIMIT 10";

		Console.WriteLine("12112121:::: " + query);

		await mooket.InvokeVoidAsync("query", dataBase, query, dotNetObjectReferenceMailLayout);
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