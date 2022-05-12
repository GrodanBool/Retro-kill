using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDamage : MonoBehaviour
{
    [SerializeField] float time = 5f, damge = 5f;
    [SerializeField] bool takeDamage = true;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Lava")
        {
            PlayerHealthController health = other.GetComponent<PlayerHealthController>();
            TakeDamage(time,health);
        }
        Debug.Log("lava");
    }

    IEnumerator TakeDamage(float time, PlayerHealthController health)
    {
        if (takeDamage)
        {
            health.DamagePlayer(damge);
            takeDamage = !takeDamage;
            yield return new WaitForSeconds(time);
            takeDamage = !takeDamage;
        }

    }
}
