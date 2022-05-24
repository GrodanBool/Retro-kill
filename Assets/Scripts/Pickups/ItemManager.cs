using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    public List<SpawnPoints> spawnPoints = new List<SpawnPoints>();

    public Transform[] spawnPointLocations;

    public float spawnTimeout = 15f;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        CreateSpawnPoints();
        spawnPoints[0].occupied = true;
        spawnPoints[1].occupied = true;
        // spawnPoints[2].occupied = true;
        HealthPickup.instance.RespawnHealth(spawnPoints[0].spawnPoint);
        AmmoPickup.instance.RespawnAmmo(spawnPoints[1].spawnPoint);
        // WeaponPickup.instance.RespawnWeapon(spawnPoints[2].spawnPoint);
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimeout -= Time.deltaTime;

        if (spawnTimeout <= 0)
        {
            bool checkOccupiedSpawnPoint = false;
            int randomSpawnPoints = Random.Range(0, spawnPoints.Count);
            Debug.Log("hej");
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

            int randomItem = Random.Range(1, 3);
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
}
