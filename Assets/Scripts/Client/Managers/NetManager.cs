using UnityEngine;

public class NetManager : MonoBehaviour
{
    // This manager instance
    public static NetManager Instance { get; private set; }

    // Position of each net - will be used for randomizing of net positions
    public Transform[] NetPositions;

    // Constants
    private const float xOffset = 1.4f;
    private const int AmountOfNets = 4;


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

        NetPositions = new Transform[AmountOfNets];

        Debug.Log("NetManager: " + this);
    }


    // This function simply spawns the nets
    public void SpawnNets(Net net)
    {
        Net currentNet = null;
        Net previousNet = null;

        Debug.Log("Inside SpawnNets: " + net);

        for (int i = 0; i < AmountOfNets; i++)
        {
            // if we're on the first net spawn
            if (i == 0)
            {
                currentNet = Instantiate(net);
                previousNet = currentNet;
                currentNet = null;
            }
            else
            {
                // spawn next net at an offset of the previous
                currentNet = Instantiate(net, new Vector3(previousNet.transform.position.x + xOffset, previousNet.transform.position.y, 0), Quaternion.identity);
                previousNet = currentNet;
                currentNet = null;
            }

            // Assign the colour to each net
            previousNet.spriteColour = ResourceManager.NetColours[i];

            // Assign this nets position to the position grid
            NetPositions[i] = previousNet.transform;
        }
    }


    // This function implements the fisher yates shuffling data structure. It simply shuffles the nets position properly each time.
    public void Shuffle()
    {
        int n = NetPositions.Length;

        while (n > 1)
        {
            n--;

            int k = Random.Range(0, AmountOfNets);

            Vector3 pos = NetPositions[k].position;

            NetPositions[k].position = NetPositions[n].position;

            NetPositions[n].position = new Vector3(pos.x, pos.y, pos.z);

        }
    }
}