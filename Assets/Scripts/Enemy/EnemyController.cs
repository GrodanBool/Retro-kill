using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // The point that the enemy should move towards
    private Vector3 targetPoint;

    public NavMeshAgent agent;

    public GameObject bullet;
    public Transform firePoint;

    // rate of fire
    public float fireRate;

    // How much time to wait before firing again
    // This value is set to equal firerate in game
    // Meaning first shot will be fired after 2 secs
    // Second and on will be depending on the fire rate
    private float fireCount = 2f;

    public Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Target the player instance position
        targetPoint = PlayerController.instance.transform.position;

        // Enemy will now never look up or down, only side to side
        //targetPoint.y = transform.position.y;

        //Vector3 corrector = new Vector3(PlayerController.instance.transform.position.x, transform.position.y, PlayerController.instance.transform.position.z);

        // Always face the player
        transform.LookAt(PlayerController.instance.transform);

        agent.SetDestination(targetPoint);

        if (agent.velocity.x <= 0 && agent.velocity.z <= 0)
        {
            anim.SetBool("isMoving", false);
        }
        else
        {
            anim.SetBool("isMoving", true);
        }

        fireCount -= Time.deltaTime;

        
        if (fireCount <= 0)
        {
            fireCount = fireRate;

            // Keep agent facing player
            firePoint.LookAt(PlayerController.instance.transform.position);

            // Check the angle towards the player
            Vector3 targetDir = PlayerController.instance.transform.position - transform.position;
            float angle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);

            if (Mathf.Abs(angle) < 30f)
            {
                Instantiate(bullet, firePoint.position, firePoint.rotation);
                anim.SetTrigger("fireShot");
            }

            anim.SetBool("isMoving", false);
        }
    }
}
