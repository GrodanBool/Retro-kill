using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    private bool collected;

    private void onTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !collected)
        {
            PlayerController.instance.activeGun.GetAmmo();

            Destroy(gameObject);

            AudioManager.instance.PlaySFX(2);
        }
    }
}
