using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public Dictionary<string, bool> ItemsCollected;
    public Dictionary<string, List<string>> RequiredItemsUsed;
    public Dictionary<string, bool> InventoryItems;

    public GameData() 
    {
        ItemsCollected = new Dictionary<string, bool>();
        RequiredItemsUsed = new Dictionary<string, List<string>>();
        InventoryItems = new Dictionary<string, bool>();
    }

}
