using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount;

    private bool isCollected;

     private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isCollected)
        {
            PlayerHealthController.instance.HealPlayer(healAmount);

            Destroy(gameObject);

            AudioManagerMusicSFX.instance.PlaySFX(2);
        }
    }
}
