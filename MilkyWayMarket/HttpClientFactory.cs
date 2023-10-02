namespace MilkyWayMarket
{
	internal interface IHttpClientFactory
	{
		void Register(string name, HttpClient client);
		HttpClient Resolve(string name);
	}

	class HttpClientFactory : IHttpClientFactory
	{
		private readonly Dictionary<string, HttpClient> _clients = new Dictionary<string, HttpClient>();

		public void Register(string name, HttpClient client)
		{
			_clients[name] = client;
		}

		public HttpClient Resolve(string name)
		{
			return _clients[name];
		}
	}
}
