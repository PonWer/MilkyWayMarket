using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;
using SimpleJSON;

namespace MilkyWayMarket.Code;

public static class GetCommitData
{
	public static async Task<List<Container>> Call(List<Container> containers)
	{
		foreach (var container in containers)
		{
			Console.WriteLine($"Commit <{container.Commit}> from {container.Date}");
		}

		foreach (var container in containers)
		{
			for (var i = 0; i < 3; i++)
			{
				try
				{
					//var marketDb = $"https://github.com/holychikenz/MWIApi/raw/{commit}/market.db";
					var marketapi = $"https://raw.githubusercontent.com/holychikenz/MWIApi/{container.Commit}/milkyapi.json";
					var medianmarket = $"https://raw.githubusercontent.com/holychikenz/MWIApi/{container.Commit}/medianmarket.json";

					using var httpClient = new HttpClient();

					var response_marketapi = await httpClient.GetAsync(marketapi);
					//var response_medianmarket = httpClient.GetAsync(medianmarket).Result;

					if (!response_marketapi.IsSuccessStatusCode) 
						throw new FileNotFoundException();

					var content = response_marketapi.Content;
					var contentStream = await content.ReadAsStringAsync();
					ConvertContent(contentStream, container);

					break;

				}
				catch (Exception e)
				{
					Console.WriteLine($"Error {i} on {container.Commit} from {container.Date}: {e.Message}");
					Thread.Sleep(5);
				}
			}

		}

		return containers;
	}

	private static void ConvertContent(string contentStream, Container container)
	{
		JSONNode data = JSON.Parse(contentStream);

		container.Items = new List<Item>();

		foreach (var marketItem in data["market"])
		{
			var item = new Item
			{
				Name = marketItem.Key
			};

			if (marketItem.Value == null)
			{
				Console.WriteLine($"{marketItem.Key} has no marketItem.Value");
				continue;
			}

			if (int.TryParse(marketItem.Value["vendor"].Value, out int result))
				item.Vendor = result;
			else
				item.Vendor = -1;

			if (int.TryParse(marketItem.Value["bid"].Value, out result))
				item.Bid = result;
			else
				item.Bid = -1;

			if (int.TryParse(marketItem.Value["ask"].Value, out result))
				item.Ask = result;
			else
				item.Ask = -1;

			container.Items.Add(item);
		}
	}
}