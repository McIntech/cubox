using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    public GameObject objectPrefab; // Reference to the prefab of the object to spawn
    public float spawnInterval = 5f; // Time interval in seconds

    private void Start()
    {
        InvokeRepeating("Spawn", spawnInterval, spawnInterval);
    }

    private void Spawn()
    {
        Instantiate(objectPrefab, transform.position, transform.rotation);
    }
}
