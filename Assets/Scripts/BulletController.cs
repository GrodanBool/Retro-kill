using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletController : MonoBehaviour
{
    public float moveSpeed, lifeTime;

    public Rigidbody theRB;

    public GameObject impactEffect;

    public int damage = 1;

    public bool damagePlayer, damageEnemy;

    public Action onHitEnemy;

    public bool isRocketLauncherBullet;

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

        if (other.tag == "Health")
        {
        }
        if (other.tag == "Ammo")
        {
            
        }
        if (other.tag == "Weapon")
        {
            
        }
        else
        {
            Destroy(gameObject);
            Instantiate(impactEffect, transform.position + (transform.forward * (-moveSpeed * Time.deltaTime)), transform.rotation);
        }

        if (isRocketLauncherBullet)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);

            foreach (Collider col in colliders)
            {
                var enemy = col.GetComponent<EnemyHealthController>();

                if (enemy != null)
                {
                    enemy.DamageEnemy(damage);
                }
            }
        }

        Destroy(gameObject);

        Instantiate(impactEffect, transform.position + (transform.forward * (-moveSpeed * Time.deltaTime)), transform.rotation);
    }
}
