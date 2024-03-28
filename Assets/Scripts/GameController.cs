using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public GameObject magicianPrefab; // Prefab for the magician
    public GameObject monsterPrefabs; // Array of monster prefabs

    public Transform magicianSpawnPoint; // Transform location for magician spawn

    public GameObject Area;
    public GameObject spawnWall;

    public int maxMonsters = 10; // Maximum number of monsters allowed on stage
    public float borderOffset = 0.5f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject); 
        }
        DontDestroyOnLoad(gameObject); 
    }

    void Start()
    {
        // Spawn the magician
        Instantiate(magicianPrefab, magicianSpawnPoint.position, magicianSpawnPoint.rotation);

        // Spawn initial monsters
        for (int i = 0; i < maxMonsters; i++)
        {
            InstantiateMonster();
        }
    }

    private void InstantiateMonster()
    {

        Bounds spawnBounds = spawnWall.GetComponent<Collider>().bounds;
        float randomX = Random.Range(spawnBounds.min.x, spawnBounds.max.x);
        float randomY = Random.Range(spawnBounds.min.y, spawnBounds.max.y);
        float randomZ = spawnBounds.center.z;  // Assuming spawning on a single Z position

        Vector3 spawnPosition = new Vector3(randomX, randomY, randomZ);
        Instantiate(monsterPrefabs, spawnPosition, Quaternion.identity); // No rotation

    }
    public void SpawnMonsters()
    {
        InstantiateMonster();
    }
}
