using System.Collections;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

public class DiamondSpawner : MonoBehaviour
{
    public GameObject[] diamondPrefabs; // Array of prefabs for different diamonds
    public Transform pathCreator;      // Reference to the PathCreator object
    public Transform spawnPoint;       // Starting point for the diamonds
    public float spawnInterval = 0.1f; // Time interval between each spawn
    public float speed = 1.88f;        // Speed of the diamond
    public EndOfPathInstruction endOfPathInstruction = EndOfPathInstruction.Reverse;

    private void Start()
    {
        StartCoroutine(SpawnDiamonds());
    }

    private IEnumerator SpawnDiamonds()
    {
        int diamondCount = diamondPrefabs.Length;

        for (int i = 0; i < diamondCount; i++)
        {
            // Instantiate the diamond at the spawnPoint
            GameObject diamond = Instantiate(diamondPrefabs[i], spawnPoint.position, Quaternion.identity);

            // Configure the PathFollower component
            PathFollower pathFollower = diamond.GetComponent<PathFollower>();
            if (pathFollower != null)
            {
                pathFollower.pathCreator = pathCreator.GetComponent<PathCreator>();
                pathFollower.speed = speed;
                pathFollower.endOfPathInstruction = endOfPathInstruction;
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
