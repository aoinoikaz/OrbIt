using UnityEngine;

public class ResourceManager
{
    public static Sprite[] OrbColours;
    public static Sprite[] NetColours;
    public static Orb OrbInstance;
    public static Net NetInstance;

    public static void LoadGameResources()
    {
        // Load colour graphics
        if(OrbColours == null)
        OrbColours = Resources.LoadAll<Sprite>("Graphics/Orbs");

        // Load orb prefab
        if (OrbInstance == null)
            OrbInstance = Resources.Load<Orb>("Prefabs/Orb");

        if (NetInstance == null)
            NetInstance = Resources.Load<Net>("Prefabs/Net");

        if (NetColours == null)
            NetColours = Resources.LoadAll<Sprite>("Graphics/Nets");
    }
}
