using System.Net.Http.Json;
using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;

namespace MilkyWayMarket.Services;

public interface IDataService
{
    Dictionary<string, ItemHistory> History { get; }
    List<string> HistoryKeys { get; }

    bool Initiated { get; }
    event EventHandler<string> DataUpdated;

    Task Init(HttpClient httpClient);
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
}