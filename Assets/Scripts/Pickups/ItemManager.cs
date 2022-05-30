using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
        else
        {
            NormalSpawns();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "OnlineLevel")
        {
            ServerContinuousSpawn();
        }
        else
        {
            NormalContinuousSpawn();
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

    public void CalledRespawn()
    {
        Invoke("RespawnHealth", 5f);
    }

    private void CreateSpawnPoints()
    {
        foreach (var item in spawnPointLocations)
        {
            this.spawnPoints.Add(new SpawnPoints { spawnPoint = item, occupied = false });
        }
    }

    public void SpawnNewItem(Vector3 location, GameObject go)
    {
        Instantiate(go, location, Quaternion.identity);
    }
}
