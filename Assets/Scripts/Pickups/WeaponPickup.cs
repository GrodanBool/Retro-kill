using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    public static WeaponPickup instance;
    public List<GameObject> weaponPickUp = new List<GameObject>();

    private void Awake()
    {
        instance = this;
    }

    public void RespawnWeapon(Transform pickUpSpawn)
    {
        int randomGun = Random.Range(0, 3);
        Instantiate(weaponPickUp[randomGun], pickUpSpawn.position, pickUpSpawn.rotation);
    }

    [Server]
    public void RespawnOnlineWeapon(Transform pickUpSpawn)
    {
        int randomGun = Random.Range(0, 3);
        NetworkServer.Spawn(Instantiate(weaponPickUp[randomGun], pickUpSpawn.position, pickUpSpawn.rotation));
    }
}
