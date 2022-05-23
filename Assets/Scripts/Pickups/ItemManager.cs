using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    public SpawnPoints[] spawnPoints;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        spawnPoints[0].occupied = true;
        spawnPoints[1].occupied = true;
        spawnPoints[2].occupied = true;
        AmmoPickup.instance.RespawnAmmo(spawnPoints[0].spawnPoint);
        HealthPickup.instance.RespawnHealth(spawnPoints[1].spawnPoint);
        WeaponPickup.instance.RespawnWeapon(spawnPoints[2].spawnPoint);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CalledRespawn()
    {
        Invoke("RespawnHealth", 5f);
    }
}

public class SpawnPoints 
{
    public Transform spawnPoint;

    public bool occupied = false;
}
