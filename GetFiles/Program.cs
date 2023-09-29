
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mime;
using Models;

public class Test
{
    public static void Main(string[] args)
    {
        new Test().Call();
    }

    public async void Call()
    {
        try
        {
            var marketDb = "https://github.com/holychikenz/MWIApi/raw/main/market.db";
            var marketapi = "https://raw.githubusercontent.com/holychikenz/MWIApi/main/milkyapi.json";
            var medianmarket = "https://raw.githubusercontent.com/holychikenz/MWIApi/main/medianmarket.json";

            //https://raw.githubusercontent.com/holychikenz/MWIApi/723a6551e36872347dac2927bcd21116f7f0b8a6/milkyapi.json
            using var httpClient = new HttpClient();

            //var response_marketDb = httpClient.GetAsync(marketDb).Result;
            var response_marketapi = httpClient.GetAsync(marketapi).Result;
            //var response_medianmarket = httpClient.GetAsync(medianmarket).Result;



            if (response_marketapi.IsSuccessStatusCode)
            {
                System.Net.Http.HttpContent content = response_marketapi.Content;
                var contentStream = content.ReadAsStringAsync().Result;

                var array = JsonConvert.DeserializeObject<dynamic>(contentStream);
                var jArray = JsonConvert.DeserializeObject<dynamic>(contentStream).market;
                var tempObject = jArray as dynamic[];

                var items = new List<Item>();
                foreach (var test in jArray)
                {
                    items.Add(new Item()
                    {
                        Name = test.Name,
                        Ask = Convert.ToInt32((test.Value["ask"] as JValue).Value),
                        Bid = Convert.ToInt32((test.Value["bid"] as JValue).Value),
                        Vendor = Convert.ToInt32((test.Value["vendor"] as JValue).Value),
                    });
                }

                foreach (var stat in items) Console.WriteLine($"{stat.Name} = ({stat.Ask}, {stat.Bid}, {stat.Vendor})");
           
            }
            else
            {
                throw new FileNotFoundException();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            Console.ReadKey();
        }
    }

}

namespace Models
{
    public class Container
    {
        public Item[] Items { get; set; }
    }

    public class Item
    {
        //Contains the name of the statistic
        public string Name { get; set; }
        public int Ask { get; set; }
        public int Bid { get; set; }
        public int Vendor { get; set; }
    }
}