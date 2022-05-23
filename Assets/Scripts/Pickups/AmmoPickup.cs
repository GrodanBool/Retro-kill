using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public static AmmoPickup instance;

    public GameObject ammoPickup;

    private void Awake()
    {
        instance = this;
    }

    public void RespawnAmmo(Transform pickUpSpawn)
    {
        Instantiate(ammoPickup, pickUpSpawn.position, pickUpSpawn.rotation);
    }
}
