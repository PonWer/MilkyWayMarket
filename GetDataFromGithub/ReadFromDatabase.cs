using MilkyWayMarket.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace GetDataFromGithub
{
	public static class ReadFromDatabase
	{
		public static async Task<Dictionary<string, ItemHistory>> Call(string path, EventHandler<string> OnDataUpdated = null)
		{
			var history = new Dictionary<string, ItemHistory>();

			//SQLitePCL.Batteries.Init();

			using (var connection = new SqliteConnection($"Data Source={path};Mode=ReadOnly"))
			{
				
				connection.Open();

				var getAllAsk = connection.CreateCommand();
				getAllAsk.CommandText =
					@"
        SELECT *
        FROM ask
    ";

				// Unix timestamp is seconds past epoch
				DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

				using (var reader = getAllAsk.ExecuteReader())
				{
					while (reader.Read())
					{
						var timeAsInt32 = reader.GetInt32(0);

						var dt = dateTime.AddSeconds(timeAsInt32).ToLocalTime();

						for (int i = 1; i < reader.FieldCount; i++)
						{
							var name = reader.GetName(i);

							if (reader.IsDBNull(i))
								continue;

							var valueAsString = reader.GetString(i);
							var value = double.Parse(valueAsString);

							history.TryAdd(name, new ItemHistory());
							history[name].history.TryAdd(dt, new Item() {Ask = value});
						}

						OnDataUpdated?.Invoke(null,$"Reading Ask prices from {dt}");
					}
				}

				var getAllBid = connection.CreateCommand();
				getAllBid.CommandText =
					@"
        SELECT *
        FROM bid
    ";

				using (var reader = getAllBid.ExecuteReader())
				{
					while (reader.Read())
					{
						var timeAsInt32 = reader.GetInt32(0);

						var dt = dateTime.AddSeconds(timeAsInt32).ToLocalTime();

						for (int i = 1; i < reader.FieldCount; i++)
						{
							var name = reader.GetName(i);

							if (reader.IsDBNull(i))
								continue;

							var valueAsString = reader.GetString(i);
							var value = double.Parse(valueAsString);

							history.TryAdd(name, new ItemHistory());
							if (!history[name].history.TryAdd(dt, new Item() {Ask = value}))
							{
								history[name].history[dt].Bid = value;
							}
						}

						OnDataUpdated?.Invoke(null, $"Reading Bid prices from {dt}");
					}
				}
			}

			return history;
		}
	}
}
