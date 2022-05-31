using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (SceneManager.GetActiveScene().name != "OnlineLevel")
        {
            if (other.tag == "Player" && damagePlayer)
            {
                other.gameObject.GetComponent<PlayerHealthController>().DamagePlayer(damage);
            }
        }
        else if (SceneManager.GetActiveScene().name == "OnlineLevel")
        {
            if (other.tag == "Player" && damagePlayer)
            {
                other.gameObject.GetComponent<PlayerOnlineHealthController>().DamagePlayer(damage);
            }
        }

        if (other.tag == "Enemy" && damageEnemy)
        {
            // If it's an enemy, fetch the class and run the correct function
            other.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(damage);
        }

        if (other.tag == "Weapon" || other.tag == "Portal" || other.tag == "Health" || other.tag == "Ammo" || other.tag == "Player" && !damagePlayer)
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
    }
}
