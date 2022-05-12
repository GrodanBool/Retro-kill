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

    // 
    private float fireCount = 2f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        targetPoint = PlayerController.instance.transform.position;
        
        // Enemy will now never look up or down, only side to side
        targetPoint.y = transform.position.y;

        agent.SetDestination(targetPoint);

        fireCount -= Time.deltaTime;

        if (fireCount <= 0)
        {
            fireCount = fireRate;

            Instantiate(bullet, firePoint.position, firePoint.rotation);
        }
    }
}
