using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;

namespace MilkyWayMarket.Code;

public static class GetCommitData
{
	public static async Task<bool> Call(Container container, string path)
	{
		for (var i = 0; i < 3; i++)
		{
			try
			{
				var marketDb = $"https://github.com/holychikenz/MWIApi/raw/{container.Commit}/market.db";
				//var marketapi = $"https://raw.githubusercontent.com/holychikenz/MWIApi/{container.Commit}/milkyapi.json";
				//var medianmarket = $"https://raw.githubusercontent.com/holychikenz/MWIApi/{container.Commit}/medianmarket.json";

				using var httpClient = new HttpClient();

				var response_marketapi = await httpClient.GetAsync(marketDb);
				//var response_medianmarket = httpClient.GetAsync(medianmarket).Result;

				if (!response_marketapi.IsSuccessStatusCode)
					throw new FileNotFoundException();


				var contentStream = await response_marketapi.Content.ReadAsStreamAsync();


				using (var fileStream = File.Create(path))
				{
					contentStream.Seek(0, SeekOrigin.Begin);
					await contentStream.CopyToAsync(fileStream);
				}

				return true;

			}
			catch (Exception e)
			{
				Console.WriteLine($"Error {i} on {container.Commit} from {container.Date}: {e.Message}");
				Thread.Sleep(5);
			}
		}

		return false;
	}
}