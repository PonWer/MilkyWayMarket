using Microsoft.Data.Sqlite;
using MilkyWayMarket.Code;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Linq;
using GetDataFromGithub;
using RestSharp;

var path = $@"{System.IO.Path.GetTempPath()}MilkyWayMarket.db";

using var httpClient = new HttpClient();

var options = new RestClientOptions("https://api.github.com")
{
	ThrowOnAnyError = true,
	MaxTimeout = 60_000
};

var client = new RestClient(options);


var latest = await GetDeployments.Call(client, 1, 0);
await GetCommitData.Call(httpClient,latest.First(),path);
var history = await ReadFromDatabase.Call(path);

Console.ReadKey();
