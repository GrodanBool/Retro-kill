using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public static HealthPickup instance;

    public GameObject healthPickup;

    private void Awake()
    {
        instance = this;
    }

    public void RespawnHealth(Transform pickUpSpawn)
    {
        Instantiate(healthPickup, pickUpSpawn.position, pickUpSpawn.rotation);
    }
}
