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


    //testing shit
    private float angle;
    private bool tryPlayerTracing = true;
    private bool canShoot;
    private Vector3 randomLocation = new Vector3(0, 0, 0);
    private float searchTimeout = 5f;
    private float search = 5f;
    private bool canSeePlayer;

    //add distanceToPlayer for making enemy approach player if far apart (maybe)
    //improve ai start shooting, sometimes moves to new position even though should be able to move
    //make ai rotate when stopped, might be tied to above

    // Start is called before the first frame update
    void Start()
    {
        anim.fireEvents = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Target the player instance position
        targetPoint = PlayerController.instance.transform.position;

        RaycastHit hit;
        var rayDirection = targetPoint - transform.position;
        if (Physics.Raycast(transform.position, rayDirection, out hit))
        {
            if (hit.transform == PlayerController.instance.gameObject.GetComponent<CapsuleCollider>() || hit.transform == PlayerController.instance.gameObject.transform)
            {
                canSeePlayer = true;
            }
            else
            {
                canSeePlayer = false;
            }
        }

        if (agent.velocity.x <= 0 && agent.velocity.z <= 0)
        {
            anim.SetBool("isMoving", false);
        }
        else
        {
            anim.SetBool("isMoving", true);
        }

        if (agent.velocity.x <= 0 && agent.velocity.z <= 0 && !canShoot)
        {
            search -= Time.deltaTime;

            if (search <= 0)
            {
                tryPlayerTracing = false;
            }
        }
        else
        {
            search = searchTimeout;
        }

        if (tryPlayerTracing && !canShoot)
        {
            firePoint.LookAt(PlayerController.instance.transform.position);
            agent.SetDestination(targetPoint);
        }
        else if (!tryPlayerTracing && !canShoot && agent.velocity.x == 0 && agent.velocity.z == 0)
        {
            firePoint.LookAt(PlayerController.instance.transform.position);
            randomLocation = RandomNavmeshLocation(50);
            agent.SetDestination(randomLocation);
        }

        fireCount -= Time.deltaTime;

        if (fireCount <= 0)
        {
            fireCount = fireRate;

            // Keep agent facing player
            firePoint.LookAt(targetPoint);

            // Check the angle towards the player
            Vector3 targetDir = PlayerController.instance.transform.position - transform.position;
            angle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);

            if (Mathf.Abs(angle) < 30f)
            {
                agent.isStopped = true;
                canShoot = true;
                tryPlayerTracing = true;
                if (canSeePlayer)
                {
                    Instantiate(bullet, firePoint.position, firePoint.rotation);
                    anim.SetTrigger("fireShot");
                }
                else
                {
                    canShoot = false;
                    agent.isStopped = false;
                }
            }
            else
            {
                agent.isStopped = false;
                canShoot = false;
            }

            anim.SetBool("isMoving", false);
        }
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}
