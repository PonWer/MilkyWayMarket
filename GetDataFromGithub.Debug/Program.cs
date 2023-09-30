using Microsoft.Data.Sqlite;
using MilkyWayMarket.Code;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Linq;
using GetDataFromGithub;

var path = $@"{System.IO.Path.GetTempPath()}MilkyWayMarket.db";


//var latest = await GetDeployments.Call(1, 0);
//await GetCommitData.Call(latest.First(),path);
var history = await ReadFromDatabase.Call(path);

Console.ReadKey();
