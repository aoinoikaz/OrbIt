using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static Sprite[] OrbColours;
    public static Orb OrbInstance;

    public static void LoadGameResources()
    {
        // Load colour graphics
        if(OrbColours == null)
        OrbColours = Resources.LoadAll<Sprite>("Graphics/Orbs");

        // Load orb prefab
        if (OrbInstance == null)
            OrbInstance = Resources.Load<Orb>("Prefabs/Orb");
    }
}
