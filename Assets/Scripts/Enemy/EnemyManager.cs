using Mirror;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    public GameObject enemyPrefab;
    public GameObject onlineEnemyPrefab;

    public Transform[] spawnPoints;

    [Header("Spawn after x seconds")]
    public int spawnRate;

    private float spawnCounter;

    private int nrOfSpawnPoints;

    private bool spawnComplete = false;
    private bool useSpawnPoints = false;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        nrOfSpawnPoints = spawnPoints.Length;

        if (SceneManager.GetActiveScene().name != "OnlineLevel")
        {
            useSpawnPoints = true;
            SpawnNewEnemy();
        }
        else if (SceneManager.GetActiveScene().name == "OnlineLevel")
        {
            useSpawnPoints = true;
            SpawnNewOnlineEnemy();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnCounter > 0)
        {
            spawnCounter -= Time.deltaTime;
        }
        else if (spawnCounter <= 0 && SceneManager.GetActiveScene().name != "OnlineLevel")
        {
            SpawnNewEnemy();
            spawnCounter = spawnRate;
        }
        else if (spawnCounter <= 0 && SceneManager.GetActiveScene().name == "OnlineLevel")
        {
            SpawnNewOnlineEnemy();
            spawnCounter = spawnRate;
        }
    }

    public void SpawnNewEnemyFromSpawnPoint()
    {
        Instantiate(enemyPrefab, spawnPoints[Random.Range(0, nrOfSpawnPoints)].transform.position, Quaternion.identity);
    }

    [Command]
    public void SpawnNewOnlineEnemy()
    {
        SpawnOnlineNewEnemyFromSpawnPoint();
    }

    [ClientRpc]
    public void SpawnOnlineNewEnemyFromSpawnPoint()
    {
        Instantiate(onlineEnemyPrefab, spawnPoints[Random.Range(0, nrOfSpawnPoints)].transform.position, Quaternion.identity);
    }

    public async void SpawnNewEnemy()
    {
        if (!useSpawnPoints)
        {
            if (!spawnComplete)
            {
                await InstantiateNewEnemy();
            }
        }
        else
        {
            SpawnNewEnemyFromSpawnPoint();
        }
    }

    public Task<bool> InstantiateNewEnemy()
    {
        Instantiate(enemyPrefab, RandomNavmeshSpawnLocation(PlayerController.instance.transform.position, 25), Quaternion.identity);
        return Task.FromResult(true);
    }

    public Vector3 RandomNavmeshSpawnLocation(Vector3 playerPostition, int maxRadius)
    {
        List<ColliderPositions> colliderPositions = new List<ColliderPositions>();
        Vector3 finalPosition = Vector3.zero;
        bool positionFound = false;
        NavMeshHit hit;

        var test = Physics.OverlapSphere(playerPostition, maxRadius);

        Physics.OverlapSphere(playerPostition, maxRadius).ToList()
                                                         .ForEach(a => colliderPositions
                                                         .Add(new ColliderPositions() { Position = a, Tried = false }));


        while (!positionFound && colliderPositions.Any(a => a.Tried == false))
        {
            Vector3 randomNavmeshSpawn = colliderPositions.Where(a => a.Tried == false).ToList()
                                         [Random.Range(0, colliderPositions.Count)].Position.transform.position;

            colliderPositions.Where(a => a.Position.transform.position == randomNavmeshSpawn)
                             .Select(a => { a.Tried = true; return a; })
                             .ToList();

            if (NavMesh.SamplePosition(randomNavmeshSpawn, out hit, maxRadius, 1))
            {
                finalPosition = hit.position;
                positionFound = true;
            }
        }
        return finalPosition;
    }
}

public class ColliderPositions
{
    public Collider Position;
    public bool Tried = false;
}
