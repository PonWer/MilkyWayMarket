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
        public string Name { get; set; }
        public int Ask { get; set; }
        public int Bid { get; set; }
        public int Vendor { get; set; }
    }


}
