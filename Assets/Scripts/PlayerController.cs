using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

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

    public Gun activeGun;

    // Happens straight away in Unity (before start runs)
    private void Awake()
    {

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        //moveInput.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        //moveInput.z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        // store y velocity
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


        //anim.SetFloat("moveSpeed", moveInput.magnitude);
        //anim.SetBool("onGround", canJump);
    }

    public void FireShot()
    {
        if (activeGun.currentAmmo > 0)
        {

            activeGun.currentAmmo--;

            Instantiate(activeGun.bullet, firePoint.position, firePoint.rotation);

            activeGun.fireCounter = activeGun.fireRate;

            UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;
        }
    }
}