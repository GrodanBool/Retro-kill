using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public static WeaponPickup instance;

    public GameObject weaponPickUp;

    private void Awake()
    {
        instance = this;
    }

    public void RespawnWeapon(Transform pickUpSpawn)
    {
        Instantiate(weaponPickUp, pickUpSpawn.position, pickUpSpawn.rotation);
    }
}
