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
    public GameObject head;
    private Vector3 randomLocation = new Vector3(0, 0, 0);
    private float angle;
    private float searchTimeout = 5f;
    private float search = 5f;
    private float attackDistance = 50f;
    private bool canSeePlayer;
    private bool tryNewLocation;
    private bool tryPlayerTracing = true;
    private bool canShoot;

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
        targetPoint.y = transform.position.y;
        transform.LookAt(targetPoint);

        //can enemy see player
        RaycastHit hit;
        var rayDirection = PlayerController.instance.transform.position - transform.position;
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

        //animation
        if (agent.velocity.x <= 0 && agent.velocity.z <= 0)
        {
            anim.SetBool("isMoving", false);
        }
        else
        {
            anim.SetBool("isMoving", true);
        }

        //change position countdown
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
        //Debug.Log(agent.path);

        //follow player or try new location
        if (tryPlayerTracing && !tryNewLocation/*&& !canShoot*/)
        {
            firePoint.LookAt(PlayerController.instance.transform.position);
            agent.SetDestination(targetPoint);
        }
        else if (!tryNewLocation && !tryPlayerTracing && !canShoot && agent.velocity.x == 0 && agent.velocity.z == 0)
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
            firePoint.LookAt(PlayerController.instance.transform.position);

            // Check the angle towards the player
            Vector3 targetDir = PlayerController.instance.transform.position - transform.position;
            angle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);

            if (Mathf.Abs(angle) < 60f)
            {
                if (Vector3.Distance(transform.position, agent.destination) > agent.stoppingDistance)
                {
                    agent.speed = 5;
                }
                else
                {
                    agent.speed = 0;
                }
                canShoot = true;
                tryPlayerTracing = true;
                if (canSeePlayer && Vector3.Distance(transform.position, targetPoint) < attackDistance)
                {
                    Instantiate(bullet, firePoint.position, firePoint.rotation);
                    anim.SetTrigger("fireShot");

                    if (agent.destination == randomLocation)
                    {
                        tryNewLocation = true;
                    }
                }
                else
                {
                    canShoot = false;
                    agent.speed = 5;
                    if (agent.destination == randomLocation)
                    {
                        tryNewLocation = false;
                    }
                }
            }
            else
            {
                agent.speed = 5;
                canShoot = false;
            }

            anim.SetBool("isMoving", false);
        }
    }

    void LateUpdate()
    {
        head.gameObject.transform.LookAt(PlayerController.instance.transform.position);
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
