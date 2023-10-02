namespace MilkyWayMarket.Code
{
    public class Container
    {
        public DateTime? Date { get; set; }

        public string Commit { get; set; }
        public List<Item> Items { get; set; }
    }

    public class Item
    {
        //Contains the name of the statistic
        //public string Name { get; set; }
        public double Ask { get; set; }
        public double Bid { get; set; }
		//public double Vendor { get; set; }
    }


    public class ItemHistory
    {
	    public Dictionary<DateTime, Item> history = new Dictionary<DateTime, Item>();
    }

}
