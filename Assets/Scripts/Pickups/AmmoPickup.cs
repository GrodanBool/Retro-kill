using Mirror;
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

    [Server]
    public void RespawnOnlineAmmo(Transform pickUpSpawn)
    {
        NetworkServer.Spawn(Instantiate(ammoPickup, pickUpSpawn.position, pickUpSpawn.rotation));
    }
}
