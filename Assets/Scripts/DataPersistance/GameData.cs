using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public Dictionary<string, bool> itemsCollected;

    public GameData() 
    {
        itemsCollected = new Dictionary<string, bool>();
    }

}
