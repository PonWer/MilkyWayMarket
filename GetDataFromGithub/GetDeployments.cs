using Newtonsoft.Json;
using RestSharp;

namespace MilkyWayMarket.Code;

public static class GetDeployments
{
	public static async Task<List<Container>> Call(RestClient restClient, int per_page, int page)
	{
		var list = new List<Container>();


		try
		{
			var request = new RestRequest("/repos/holychikenz/MWIApi/deployments");
			request.AddParameter("per_page", per_page);
			request.AddParameter("page", page);
			request.AddHeader("User-Agent", "request");

			var response = await restClient.GetAsync(request);
			var converted = JsonConvert.DeserializeObject<List<Deployments>>(response.Content);

			list = converted.Select(x => new Container()
			{
				Commit = x.sha.Trim(),
				Date = x.created_at
			}).ToList();
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
		}



		return list;
	}
}