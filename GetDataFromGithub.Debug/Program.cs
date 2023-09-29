using MilkyWayMarket.Code;
using System.Xml.Linq;

List<Container> Elements = new List<Container>();


foreach (var container in await GetDeployments.Call(10, 0))
{
	Elements.Add(await GetCommitData.Call(container));
}


