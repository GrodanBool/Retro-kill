using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDamage : MonoBehaviour
{
    [SerializeField] float time = 5f, damge = 5f;
    [SerializeField] bool takeDamage = true;
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(TakeDamage(time, other));
        }


    }

    IEnumerator TakeDamage(float time, Collider other)
    {
        if (takeDamage)
        {
            other.gameObject.GetComponent<PlayerHealthController>().DamagePlayer(damge);
            takeDamage = !takeDamage;
            yield return new WaitForSeconds(time);
            takeDamage = !takeDamage;
        }
    }
}
