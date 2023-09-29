using MilkyWayMarket.Code;
using System.Xml.Linq;

List<Container> Elements = new List<Container>();



Elements = await GetCommitData.Call(await GetDeployments.Call());

