using UnityEngine;
public class NetManager : MonoBehaviour
{
    // This manager instance
    public static NetManager Instance { get; private set; }

    // Position of each net - will be used for randomizing of net positions
    public Transform[] NetPositions;

    // Constants
    private const float xOffset = 0.5f;
    private const int AmountOfNets = 5;

    // what do we need:
    // we need a reference to each net so we can randomize their positions
    // maybe index's of each net for easy identification?]
    // idk ull figure it out after your shower

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }


    // TODO: FINISH IMPLEMENTATION OF NET RANDOMIZATION
    public void SpawnNets(Net net)
    {
        Net currentNet = null;
        Net previousNet = null;

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
        }
    }


    // this function will randomize the position of each net at runtime
    public void RandomizeNets()
    {
        for (int i = 0; i < AmountOfNets; i++)
        {
            
        }

        // this function will be called every time a row has been finished shifting
        // first we need get the current positions of all the nets
        // then we needa iterate through the array of nets and for each net, swap it with the one that is in front, 
        // if at the end of array, swap it with i - 1
    }
}