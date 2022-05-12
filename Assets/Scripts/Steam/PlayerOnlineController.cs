using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerOnlineController : NetworkBehaviour
{
    public float speed = 1.0f;
    public GameObject Playermodel;
    public float moveSpeed, gravityModifier, jumpPower, runSpeed = 12;
    public CharacterController charCon;

    private Vector3 moveInput;

    public Transform camTrans;

    public float mouseSensitivity;
    public bool invertX;
    public bool invertY;

    private bool canJump, canDoubleJump;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

    public Animator anim;

    public GameObject bullet;
    public Transform firePoint;
    // Start is called before the first frame update
    void Start()
    {
        Playermodel.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "OnlineLevel")
        {
            if (!Playermodel.activeSelf)
            {
                Playermodel.SetActive(true);
                SetPosition();
            }
            if (hasAuthority)
            {
                Movement();
            }
        }
    }
    public void Movement()
    {
        float yStore = moveInput.y;
        Vector3 vertMove = transform.forward * Input.GetAxisRaw("Vertical");
        Vector3 horiMove = transform.right * Input.GetAxisRaw("Horizontal");

        moveInput = horiMove + vertMove;
        moveInput.Normalize();

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveInput = moveInput * runSpeed;
        }
        else
        {
            moveInput = moveInput * moveSpeed;
        }

        moveInput.y = yStore;

        moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;


        if (charCon.isGrounded)
        {
            moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
        }

        // Check if the player is grounded. Meaning, we can jump
        canJump = Physics.OverlapSphere(groundCheckPoint.position, 1f, whatIsGround).Length > 0;

        // Handle jumping
        // If the SPACE key is pressed, and canJump is true, change the move input for the y axis to the jump power
        // public variable value
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canJump)
            {
                moveInput.y = jumpPower;
                canDoubleJump = true;
            }
            else if (canDoubleJump == true)
            {
                moveInput.y = jumpPower;
                canDoubleJump = false;
            }
        }

        // Move the character
        charCon.Move(moveInput * Time.deltaTime);

        // Control camera rotaion
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        if (invertX)
        {
            mouseInput.x = -mouseInput.x;
        }
        if (invertY)
        {
            mouseInput.y = -mouseInput.y;
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);

        camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));


        // Handle shooting
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(camTrans.position, camTrans.forward, out hit, 50f))
            {
                if (Vector3.Distance(camTrans.position, hit.point) > 2f)
                {
                    firePoint.LookAt(hit.point);
                }
            }
            else
            {
                firePoint.LookAt(camTrans.position + (camTrans.forward * 30f));
            }
            // Create a copy of something
            Instantiate(bullet, firePoint.position, firePoint.rotation);
        }
    }
    //random spawn points
    public void SetPosition()
    {
        transform.position = new Vector3(Random.Range(-5, 5), 1.0f, Random.Range(-15, 7));
    }
}
