using MilkyWayMarket.Code;
using MudBlazor;

namespace MilkyWayMarket
{
	public interface IDataService
	{
		Task Init();

		List<Container> DataPoints { get; }
		int EmptyDataPoints { get; }
		int DataPointsToFetch { get; }

		bool Loading { get; }
		MudTheme CurrentTheme { get; set; }

		public Dictionary<string, ItemHistory> History { get; }

		public List<string> HistoryKeys { get; }
		event EventHandler DataUpdated;

		public bool PagesLoading { get; }

	}
	public class DataService : IDataService
	{
		private bool initiated = false;
		private bool _pagesLoading = true;
		private List<Container> dataPoints = new List<Container>();
		private List<Container> emptyDataPoints = new List<Container>();
		private MudTheme _currentTheme;
		private Dictionary<string, ItemHistory> history = new Dictionary<string, ItemHistory>();
		private List<string> historyKeys = new List<string>();

		public event EventHandler DataUpdated;

		public bool Loading => !initiated;

		public MudTheme CurrentTheme
		{
			get => _currentTheme;
			set => _currentTheme = value;
		}

		public List<Container> DataPoints => dataPoints;
		public int EmptyDataPoints => emptyDataPoints.Count;
		public int DataPointsToFetch => 20;
		public int DataPointsSkip => 5;

		public bool PagesLoading => _pagesLoading;

		public Dictionary<string, ItemHistory> History => history;

		public List<string> HistoryKeys => historyKeys;

		public async Task Init()
		{
			if(initiated)
				return;

			for (int i = 0; i < DataPointsToFetch; i++)
			{
				emptyDataPoints.AddRange(await GetDeployments.Call(2,i));
				DataUpdated.Invoke(null, EventArgs.Empty);
			}

			_pagesLoading = false;

			foreach (var emptyDataPoint in emptyDataPoints)
			{
				dataPoints.Insert(0,await GetCommitData.Call(emptyDataPoint));
				DataUpdated.Invoke(null,EventArgs.Empty);
			}
			
            foreach (var dataPoint in dataPoints)
            {
                foreach (var item in dataPoint.Items)
                {
	                history.TryAdd(item.Name, new ItemHistory());

	                if(dataPoint.Date != null && !history[item.Name].history.ContainsKey((DateTime)dataPoint.Date))
					    history[item.Name].history.Add((DateTime)dataPoint.Date,item);
                }
            }

            historyKeys = history.Keys.ToList();

            initiated = true;
            DataUpdated.Invoke(null, EventArgs.Empty);
		}
	}

    public class ItemHistory
    {
       public Dictionary<DateTime,Item> history = new Dictionary<DateTime,Item>();
    }
}
