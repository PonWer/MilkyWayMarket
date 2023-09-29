using MilkyWayMarket.Code;

namespace MilkyWayMarket
{
	public interface IDataService
	{
		Task Init();

		List<Container> DataPoints { get; }

		bool Loading { get; }
	}
	public class DataService : IDataService
	{
		private bool initiated = false;
		private List<Container> dataPoints = new List<Container>();

		public bool Loading => !initiated;
		public List<Container> DataPoints => dataPoints;

		public async Task Init()
		{
			dataPoints = await GetCommitData.Call(await GetDeployments.Call());
			initiated = true;
		}
	}
}
