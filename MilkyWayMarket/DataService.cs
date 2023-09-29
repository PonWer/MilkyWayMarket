using MilkyWayMarket.Code;
using MudBlazor;

namespace MilkyWayMarket
{
	public interface IDataService
	{
		Task Init();

		List<Container> DataPoints { get; }

		bool Loading { get; }
		MudTheme CurrentTheme { get; set; }
	}
	public class DataService : IDataService
	{
		private bool initiated = false;
		private List<Container> dataPoints = new List<Container>();
		private MudTheme _currentTheme;

		public bool Loading => !initiated;

		public MudTheme CurrentTheme
		{
			get => _currentTheme;
			set => _currentTheme = value;
		}

		public List<Container> DataPoints => dataPoints;

		public async Task Init()
		{
			dataPoints = await GetCommitData.Call(await GetDeployments.Call());
			initiated = true;
		}
	}
}
