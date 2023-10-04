using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;
using MilkyWayMarket.Shared;
using Newtonsoft.Json.Linq;

namespace MilkyWayMarket.Services;

public interface IDataService
{
	Dictionary<string, ItemHistory> History { get; }
	List<string> HistoryKeys { get; }

	bool Initiated { get; }
	bool Busy { get; }

	event EventHandler<string> DataUpdated;

	Task Query(List<string> items);

	void ReceiveData(string itemName, bool isAsk, DateTime date, double value);
	Task Init(HttpClient httpClient, IJSObjectReference inMooket, IJSObjectReference inDatabase, DotNetObjectReference<MainLayout> dotNetObjectReference);
	void ReceivedDataComplete();
}

public class DataService : IDataService
{
	private IJSObjectReference dataBase;
	private IJSObjectReference mooket;
	private DotNetObjectReference<MainLayout> dotNetObjectReferenceMailLayout;
	public bool Busy { get; set; } = false;
	public event EventHandler<string> DataUpdated;

	public Dictionary<string, ItemHistory> History { get; } = new();

	public List<string> HistoryKeys { get; } = new List<string>()
	{
		"Abyssal Essence", "Advanced Task Ring", "Amber", "Amethyst", "Apple", "Apple Gummy", "Apple Yogurt",
		"Aqua Arrow", "Aqua Essence", "Arabica Coffee Bean", "Arcane Bow", "Arcane Crossbow", "Arcane Fire Staff",
		"Arcane Log", "Arcane Lumber", "Arcane Nature Staff", "Arcane Shield", "Arcane Water Staff", "Artisan Tea",
		"Attack Coffee", "Azure Boots", "Azure Brush", "Azure Buckler", "Azure Bulwark", "Azure Cheese", "Azure Chisel",
		"Azure Enhancer", "Azure Gauntlets", "Azure Hammer", "Azure Hatchet", "Azure Helmet", "Azure Mace",
		"Azure Milk", "Azure Needle", "Azure Plate Body", "Azure Plate Legs", "Azure Pot", "Azure Shears",
		"Azure Spatula", "Azure Spear", "Azure Sword", "Bag Of 10 Cowbells", "Bamboo Boots", "Bamboo Branch",
		"Bamboo Fabric", "Bamboo Gloves", "Bamboo Hat", "Bamboo Robe Bottoms", "Bamboo Robe Top", "Basic Task Ring",
		"Bear Essence", "Beast Boots", "Beast Bracers", "Beast Chaps", "Beast Hide", "Beast Hood", "Beast Leather",
		"Beast Tunic", "Berserk", "Birch Bow", "Birch Crossbow", "Birch Fire Staff", "Birch Log", "Birch Lumber",
		"Birch Nature Staff", "Birch Shield", "Birch Water Staff", "Black Bear Fluff", "Black Bear Shoes",
		"Black Tea Leaf", "Blackberry", "Blackberry Cake", "Blackberry Donut", "Blessed Tea", "Blueberry",
		"Blueberry Cake", "Blueberry Donut", "Brewing Tea", "Burble Boots", "Burble Brush", "Burble Buckler",
		"Burble Bulwark", "Burble Cheese", "Burble Chisel", "Burble Enhancer", "Burble Gauntlets", "Burble Hammer",
		"Burble Hatchet", "Burble Helmet", "Burble Mace", "Burble Milk", "Burble Needle", "Burble Plate Body",
		"Burble Plate Legs", "Burble Pot", "Burble Shears", "Burble Spatula", "Burble Spear", "Burble Sword",
		"Burble Tea Leaf", "Cedar Bow", "Cedar Crossbow", "Cedar Fire Staff", "Cedar Log", "Cedar Lumber",
		"Cedar Nature Staff", "Cedar Shield", "Cedar Water Staff", "Centaur Boots", "Centaur Hoof", "Channeling Coffee",
		"Cheese", "Cheese Boots", "Cheese Brush", "Cheese Buckler", "Cheese Bulwark", "Cheese Chisel",
		"Cheese Enhancer", "Cheese Gauntlets", "Cheese Hammer", "Cheese Hatchet", "Cheese Helmet", "Cheese Mace",
		"Cheese Needle", "Cheese Plate Body", "Cheese Plate Legs", "Cheese Pot", "Cheese Shears", "Cheese Spatula",
		"Cheese Spear", "Cheese Sword", "Cheesesmithing Tea", "Chrono Gloves", "Chrono Sphere", "Cleave", "Cocoon",
		"Coin", "Collector's Boots", "Colossus Core", "Colossus Plate Body", "Colossus Plate Legs", "Cooking Tea",
		"Cotton", "Cotton Boots", "Cotton Fabric", "Cotton Gloves", "Cotton Hat", "Cotton Robe Bottoms",
		"Cotton Robe Top", "Cowbell", "Crab Pincer", "Crafting Tea", "Crimson Boots", "Crimson Brush",
		"Crimson Buckler", "Crimson Bulwark", "Crimson Cheese", "Crimson Chisel", "Crimson Enhancer",
		"Crimson Gauntlets", "Crimson Hammer", "Crimson Hatchet", "Crimson Helmet", "Crimson Mace", "Crimson Milk",
		"Crimson Needle", "Crimson Plate Body", "Crimson Plate Legs", "Crimson Pot", "Crimson Shears",
		"Crimson Spatula", "Crimson Spear", "Crimson Sword", "Critical Coffee", "Crushed Amber", "Crushed Amethyst",
		"Crushed Garnet", "Crushed Jade", "Crushed Moonstone", "Crushed Pearl", "Cupcake", "Defense Coffee",
		"Demonic Core", "Demonic Plate Body", "Demonic Plate Legs", "Donut", "Dragon Fruit", "Dragon Fruit Gummy",
		"Dragon Fruit Yogurt", "Earrings Of Armor", "Earrings Of Gathering", "Earrings Of Rare Find",
		"Earrings Of Regeneration", "Earrings Of Resistance", "Efficiency Tea", "Egg", "Elemental Affinity",
		"Elusiveness", "Emp Tea Leaf", "Enchanted Gloves", "Enhancing Tea", "Entangle", "Excelsa Coffee Bean",
		"Expert Task Ring", "Eye Of The Watcher", "Eye Watch", "Eyessence", "Fieriosa Coffee Bean", "Fighter Necklace",
		"Fireball", "Firestorm", "Flame Arrow", "Flame Blast", "Flaming Cloth", "Flaming Robe Bottoms",
		"Flaming Robe Top", "Flax", "Fluffy Red Hat", "Foraging Tea", "Frenzy", "Frost Sphere", "Frost Staff",
		"Frost Surge", "Garnet", "Gathering Tea", "Gator Vest", "Giant Pouch", "Ginkgo Bow", "Ginkgo Crossbow",
		"Ginkgo Fire Staff", "Ginkgo Log", "Ginkgo Lumber", "Ginkgo Nature Staff", "Ginkgo Shield",
		"Ginkgo Water Staff", "Gobo Boomstick", "Gobo Boots", "Gobo Bracers", "Gobo Chaps", "Gobo Defender",
		"Gobo Essence", "Gobo Hide", "Gobo Hood", "Gobo Leather", "Gobo Rag", "Gobo Shooter", "Gobo Slasher",
		"Gobo Smasher", "Gobo Stabber", "Gobo Tunic", "Goggles", "Golem Essence", "Gourmet Tea", "Granite Bludgeon",
		"Green Tea Leaf", "Grizzly Bear Fluff", "Grizzly Bear Shoes", "Gummy", "Heal", "Holy Boots", "Holy Brush",
		"Holy Buckler", "Holy Bulwark", "Holy Cheese", "Holy Chisel", "Holy Enhancer", "Holy Gauntlets", "Holy Hammer",
		"Holy Hatchet", "Holy Helmet", "Holy Mace", "Holy Milk", "Holy Needle", "Holy Plate Body", "Holy Plate Legs",
		"Holy Pot", "Holy Shears", "Holy Spatula", "Holy Spear", "Holy Sword", "Ice Spear", "Icy Cloth",
		"Icy Robe Bottoms", "Icy Robe Top", "Infernal Battlestaff", "Infernal Ember", "Intelligence Coffee", "Jade",
		"Jungle Essence", "Large Artisan's Crate", "Large Meteorite Cache", "Large Pouch", "Large Treasure Chest",
		"Liberica Coffee Bean", "Linen Boots", "Linen Fabric", "Linen Gloves", "Linen Hat", "Linen Robe Bottoms",
		"Linen Robe Top", "Living Granite", "Log", "Lucky Coffee", "Lumber", "Luna Robe Bottoms", "Luna Robe Top",
		"Luna Wing", "Magic Coffee", "Magnet", "Magnetic Gloves", "Magnifying Glass", "Maim", "Marine Chaps",
		"Marine Scale", "Marine Tunic", "Marsberry", "Marsberry Cake", "Marsberry Donut", "Medium Artisan's Crate",
		"Medium Meteorite Cache", "Medium Pouch", "Medium Treasure Chest", "Milk", "Milking Tea", "Minor Heal",
		"Mirror Of Protection", "Mooberry", "Mooberry Cake", "Mooberry Donut", "Moolong Tea Leaf", "Moonstone",
		"Nature's Veil", "Necklace Of Efficiency", "Necklace Of Wisdom", "Orange", "Orange Gummy", "Orange Yogurt",
		"Panda Fluff", "Panda Gloves", "Peach", "Peach Gummy", "Peach Yogurt", "Pearl", "Pierce", "Pincer Gloves",
		"Plum", "Plum Gummy", "Plum Yogurt", "Poke", "Polar Bear Fluff", "Polar Bear Shoes", "Power Coffee",
		"Precision", "Processing Tea", "Puncture", "Purpleheart Bow", "Purpleheart Crossbow", "Purpleheart Fire Staff",
		"Purpleheart Log", "Purpleheart Lumber", "Purpleheart Nature Staff", "Purpleheart Shield",
		"Purpleheart Water Staff", "Purple's Gift", "Quick Shot", "Radiant Boots", "Radiant Fabric", "Radiant Fiber",
		"Radiant Gloves", "Radiant Hat", "Radiant Robe Bottoms", "Radiant Robe Top", "Rain Of Arrows", "Rainbow Boots",
		"Rainbow Brush", "Rainbow Buckler", "Rainbow Bulwark", "Rainbow Cheese", "Rainbow Chisel", "Rainbow Enhancer",
		"Rainbow Gauntlets", "Rainbow Hammer", "Rainbow Hatchet", "Rainbow Helmet", "Rainbow Mace", "Rainbow Milk",
		"Rainbow Needle", "Rainbow Plate Body", "Rainbow Plate Legs", "Rainbow Pot", "Rainbow Shears",
		"Rainbow Spatula", "Rainbow Spear", "Rainbow Sword", "Ranged Coffee", "Ranger Necklace", "Red Chef's Hat",
		"Red Panda Fluff", "Red Tea Leaf", "Redwood Bow", "Redwood Crossbow", "Redwood Fire Staff", "Redwood Log",
		"Redwood Lumber", "Redwood Nature Staff", "Redwood Shield", "Redwood Water Staff", "Reptile Boots",
		"Reptile Bracers", "Reptile Chaps", "Reptile Hide", "Reptile Hood", "Reptile Leather", "Reptile Tunic",
		"Revenant Anima", "Revenant Chaps", "Revenant Tunic", "Ring Of Armor", "Ring Of Gathering", "Ring Of Rare Find",
		"Ring Of Regeneration", "Ring Of Resistance", "Robusta Coffee Bean", "Rough Boots", "Rough Bracers",
		"Rough Chaps", "Rough Hide", "Rough Hood", "Rough Leather", "Rough Tunic", "Scratch", "Shard Of Protection",
		"Shoebill Feather", "Shoebill Shoes", "Sighted Bracers", "Silencing Shot", "Silk Boots", "Silk Fabric",
		"Silk Gloves", "Silk Hat", "Silk Robe Bottoms", "Silk Robe Top", "Smack", "Small Artisan's Crate",
		"Small Meteorite Cache", "Small Pouch", "Small Treasure Chest", "Snail Shell", "Snail Shell Helmet",
		"Snake Fang", "Snake Fang Dirk", "Sorcerer Boots", "Sorcerer Essence", "Sorcerer's Sole", "Soul Fragment",
		"Soul Hunter Crossbow", "Spaceberry", "Spaceberry Cake", "Spaceberry Donut", "Spacia Coffee Bean",
		"Spike Shell", "Spiked Bulwark", "Stalactite Shard", "Stalactite Spear", "Stamina Coffee", "Star Fragment",
		"Star Fruit", "Star Fruit Gummy", "Star Fruit Yogurt", "Steady Shot", "Strawberry", "Strawberry Cake",
		"Strawberry Donut", "Stunning Blow", "Sugar", "Super Attack Coffee", "Super Brewing Tea",
		"Super Cheesesmithing Tea", "Super Cooking Tea", "Super Crafting Tea", "Super Defense Coffee",
		"Super Enhancing Tea", "Super Foraging Tea", "Super Intelligence Coffee", "Super Magic Coffee",
		"Super Milking Tea", "Super Power Coffee", "Super Ranged Coffee", "Super Stamina Coffee", "Super Tailoring Tea",
		"Super Woodcutting Tea", "Swamp Essence", "Sweep", "Swiftness Coffee", "Tailoring Tea", "Task Crystal",
		"Task Token", "Tome Of Healing", "Tome Of The Elements", "Toughness", "Toxic Pollen", "Treant Bark",
		"Treant Shield", "Turtle Shell", "Turtle Shell Body", "Turtle Shell Legs", "Twilight Essence", "Umbral Boots",
		"Umbral Bracers", "Umbral Chaps", "Umbral Hide", "Umbral Hood", "Umbral Leather", "Umbral Tunic",
		"Vampire Fang", "Vampire Fang Dirk", "Vampiric Bow", "Vampirism", "Verdant Boots", "Verdant Brush",
		"Verdant Buckler", "Verdant Bulwark", "Verdant Cheese", "Verdant Chisel", "Verdant Enhancer",
		"Verdant Gauntlets", "Verdant Hammer", "Verdant Hatchet", "Verdant Helmet", "Verdant Mace", "Verdant Milk",
		"Verdant Needle", "Verdant Plate Body", "Verdant Plate Legs", "Verdant Pot", "Verdant Shears",
		"Verdant Spatula", "Verdant Spear", "Verdant Sword", "Vision Helmet", "Vision Shield", "Watchful Relic",
		"Water Strike", "Werewolf Claw", "Werewolf Slasher", "Wheat", "Wisdom Coffee", "Wisdom Tea", "Wizard Necklace",
		"Woodcutting Tea", "Wooden Bow", "Wooden Crossbow", "Wooden Fire Staff", "Wooden Nature Staff", "Wooden Shield",
		"Wooden Water Staff", "Yogurt"};


	public bool Initiated { get; private set; }

	public async Task Init(HttpClient httpClient, IJSObjectReference inMooket, IJSObjectReference inDatabase, DotNetObjectReference<MainLayout> dotNetObjectReference)
	{
		if (Initiated)
			return;

		mooket = inMooket;
		dataBase = inDatabase;
		dotNetObjectReferenceMailLayout = dotNetObjectReference;

		Initiated = true;
		DataUpdated?.Invoke(null, string.Empty);
	}

	public void ReceivedDataComplete()
	{
		Busy = false;
		DataUpdated?.Invoke(null, "Data received complete");
	}

	public async Task Query(List<string> items)
	{
		var itemsToLoad = new List<string>();
		foreach (var item in items)
		{
			if (!History.ContainsKey(item))
			{
				itemsToLoad.Add($"\"{item}\"");
			}
		}

		if (!itemsToLoad.Any())
			return;

		Busy = true;

		var listAsString = string.Join(",", itemsToLoad);

		DataUpdated.Invoke(null, $"Fetching data about {listAsString}");
		var query = "SELECT DATETIME(time,\"unixepoch\") AS time, " +
		            listAsString +
					"FROM ask";

		await mooket.InvokeVoidAsync("query", dataBase, query, dotNetObjectReferenceMailLayout);
	}

	public void ReceiveData(string itemName, bool isAsk, DateTime date, double value)
	{
		History.TryAdd(itemName, new ItemHistory());

		History[itemName].history.TryAdd(date, new Item());

		if (isAsk)
		{
			History[itemName].history[date].Ask = value;
		}
		else
		{
			History[itemName].history[date].Bid = value;
		}
	}
}
public class ItemHistory
{
	public Dictionary<DateTime, Item> history = new();
}
public class Item
{
	public double Ask { get; set; } = -1;

	public double Bid { get; set; } = -1;
}