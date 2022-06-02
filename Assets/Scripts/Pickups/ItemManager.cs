using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    public List<SpawnPoints> spawnPoints = new List<SpawnPoints>();

    //public GameObject health, ammo;   

    public Transform[] spawnPointLocations;

    public float spawnTimeout = 15f;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "OnlineLevel")
        {
            ServerSpawns();
        }
        else if (SceneManager.GetActiveScene().name == "Level1")
        {
            NormalSpawns();
        }
        else
        {
            //RandomSpawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "OnlineLevel")
        {
            ServerContinuousSpawn();
        }
        else if (SceneManager.GetActiveScene().name == "Level1")
        {
            NormalContinuousSpawn();
        }
        else
        {
            RandomContinuousSpawn();
        }
    }

    void RandomSpawn()
    {
        //Maybe
    }
    void RandomContinuousSpawn()
    {
        spawnTimeout -= Time.deltaTime;

        if (spawnTimeout <= 0)
        {
            int randomItem = Random.Range(1, 4);
            Vector3 spawnPoint = RandomNavmeshSpawnLocation(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().transform.position, 50);
            spawnPoint.y += 1;
            Transform transform = gameObject.transform;
            transform.position = spawnPoint;
            transform.rotation = Quaternion.identity;
            switch (randomItem)
            {
                case 1:
                    HealthPickup.instance.RespawnHealth(transform);
                    spawnTimeout = 15f;
                    break;
                case 2:
                    AmmoPickup.instance.RespawnAmmo(transform);
                    spawnTimeout = 15f;
                    break;
                case 3:
                    WeaponPickup.instance.RespawnWeapon(transform);
                    spawnTimeout = 15f;
                    break;
                default:
                    break;
            }
        }
    }

    void NormalContinuousSpawn()
    {
        spawnTimeout -= Time.deltaTime;

        if (spawnTimeout <= 0)
        {
            bool checkOccupiedSpawnPoint = false;
            int randomSpawnPoints = Random.Range(0, spawnPoints.Count);

            while (!checkOccupiedSpawnPoint)
            {
                if (!spawnPoints[randomSpawnPoints].occupied)
                {
                    spawnTimeout = 5f;
                    checkOccupiedSpawnPoint = true;
                }
                else
                {
                    randomSpawnPoints = Random.Range(0, spawnPoints.Count);
                    spawnPoints[randomSpawnPoints].tried = true;
                    if (!spawnPoints.Any(a => a.tried == false))
                    {
                        spawnTimeout = 5f;
                        checkOccupiedSpawnPoint = true;
                    }
                }
            }

            int randomItem = Random.Range(1, 4);
            if (!spawnPoints[randomSpawnPoints].occupied)
            {
                switch (randomItem)
                {
                    case 1:
                        HealthPickup.instance.RespawnHealth(spawnPoints[randomSpawnPoints].spawnPoint);
                        spawnPoints[randomSpawnPoints].occupied = true;
                        break;
                    case 2:
                        AmmoPickup.instance.RespawnAmmo(spawnPoints[randomSpawnPoints].spawnPoint);
                        spawnPoints[randomSpawnPoints].occupied = true;
                        break;
                    case 3:
                        WeaponPickup.instance.RespawnWeapon(spawnPoints[randomSpawnPoints].spawnPoint);
                        spawnPoints[randomSpawnPoints].occupied = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    [Server]
    void ServerContinuousSpawn()
    {
        spawnTimeout -= Time.deltaTime;

        if (spawnTimeout <= 0)
        {
            bool checkOccupiedSpawnPoint = false;
            int randomSpawnPoints = Random.Range(0, spawnPoints.Count);

            while (!checkOccupiedSpawnPoint)
            {
                if (!spawnPoints[randomSpawnPoints].occupied)
                {
                    spawnTimeout = 5f;
                    checkOccupiedSpawnPoint = true;
                }
                else
                {
                    randomSpawnPoints = Random.Range(0, spawnPoints.Count);
                    spawnPoints[randomSpawnPoints].tried = true;
                    if (!spawnPoints.Any(a => a.tried == false))
                    {
                        spawnTimeout = 5f;
                        checkOccupiedSpawnPoint = true;
                    }
                }
            }

            int randomItem = Random.Range(1, 4);
            if (!spawnPoints[randomSpawnPoints].occupied)
            {
                switch (randomItem)
                {
                    case 1:
                        HealthPickup.instance.RespawnOnlineHealth(spawnPoints[randomSpawnPoints].spawnPoint);
                        spawnPoints[randomSpawnPoints].occupied = true;
                        break;
                    case 2:
                        AmmoPickup.instance.RespawnOnlineAmmo(spawnPoints[randomSpawnPoints].spawnPoint);
                        spawnPoints[randomSpawnPoints].occupied = true;
                        break;
                    case 3:
                        WeaponPickup.instance.RespawnOnlineWeapon(spawnPoints[randomSpawnPoints].spawnPoint);
                        spawnPoints[randomSpawnPoints].occupied = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }

    [Server]
    void ServerSpawns()
    {
        CreateSpawnPoints();
        spawnPoints[0].occupied = true;
        spawnPoints[1].occupied = true;
        spawnPoints[2].occupied = true;
        HealthPickup.instance.RespawnOnlineHealth(spawnPoints[0].spawnPoint);
        AmmoPickup.instance.RespawnOnlineAmmo(spawnPoints[1].spawnPoint);
        WeaponPickup.instance.RespawnOnlineWeapon(spawnPoints[2].spawnPoint);
    }

    void NormalSpawns()
    {
        CreateSpawnPoints();
        spawnPoints[0].occupied = true;
        spawnPoints[1].occupied = true;
        spawnPoints[2].occupied = true;
        HealthPickup.instance.RespawnHealth(spawnPoints[0].spawnPoint);
        AmmoPickup.instance.RespawnAmmo(spawnPoints[1].spawnPoint);
        WeaponPickup.instance.RespawnWeapon(spawnPoints[2].spawnPoint);
    }

    private void CreateSpawnPoints()
    {
        foreach (var item in spawnPointLocations)
        {
            this.spawnPoints.Add(new SpawnPoints { spawnPoint = item, occupied = false });
        }
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
