using Mirror;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OnlineEnemyManager : NetworkBehaviour
{
    public static OnlineEnemyManager instance;
    public GameObject onlineEnemyPrefab;
    public Transform[] spawnPoints;

    [Header("Spawn after x seconds")]
    public int spawnRate;
    private float spawnCounter;
    private int nrOfSpawnPoints;

    private void Awake()
    {
        nrOfSpawnPoints = spawnPoints.Length;
        instance = this;
    }

    void Update()
    {
        if (spawnCounter > 0)
        {
            spawnCounter -= Time.deltaTime;
        }
        else if (spawnCounter <= 0)
        {
            SpawnOnlineNewEnemyFromSpawnPoint();
            spawnCounter = spawnRate;
        }
    }

    [Server]
    public void SpawnOnlineNewEnemyFromSpawnPoint()
    {
        List<PlayerOnlineController> onlineControllers = GameObject.FindGameObjectsWithTag("Player")
                                                                   .Where(a => a.GetComponent<PlayerOnlineController>() != null)
                                                                   .Select(a => a.GetComponent<PlayerOnlineController>())
                                                                   .ToList();

        PlayerOnlineController newLocalPlayerOnlineController = onlineControllers[Random.Range(0, onlineControllers.Count)];

        GameObject newEnemy = Instantiate(onlineEnemyPrefab, spawnPoints[Random.Range(0, nrOfSpawnPoints)].transform.position, Quaternion.identity);
        newEnemy.GetComponent<EnemyOnlineController>().localPlayerOnlineController = newLocalPlayerOnlineController;

        NetworkServer.Spawn(newEnemy);
    }
}