using System;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float moveSpeed, lifeTime;

    public Rigidbody theRB;

    public GameObject impactEffect;

    public int damage = 1;

    public bool damagePlayer, damageEnemy;

    public Action onHitEnemy;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Time.deltaTime not needed here
        theRB.velocity = transform.forward * moveSpeed;

        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Checks if this script is connected to an object and does something
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy" && damageEnemy)
        {
            // If it's an enemy, fetch the class and run the correct function
            other.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(damage);
        }

        if (other.tag == "Player" && damagePlayer)
        {
            other.gameObject.GetComponent<PlayerHealthController>().DamagePlayer(damage);
        }

        if (other.tag == "Portal")
        {
            
        }
        else
        {
            Destroy(gameObject);
            Instantiate(impactEffect, transform.position + (transform.forward * (-moveSpeed * Time.deltaTime)), transform.rotation);
        }
    }
}
