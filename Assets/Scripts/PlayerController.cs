using System.Collections.Generic;
using System.Collections;
using System.Linq;
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
    public List<Gun> allGuns = new List<Gun>();
    public List<Gun> unlockableGuns = new List<Gun>();
    public int currentGun;

    public float maxViewAngle = 60f;

    public float bounceAmount;
    private bool bounce;

    public float pickupTimeout = 5f;
    private float pickupCounter;



    // Happens straight away in Unity (before start runs)
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        pickupCounter = pickupTimeout;

        if (PlayerPrefs.GetString("activemod").Contains("No Guns"))
        {
            foreach (var item in allGuns)
            {
                item.gameObject.SetActive(false);
            }
            allGuns.Clear();
            unlockableGuns.Clear();
        }
        string activeMod1 = PlayerPrefs.GetString("activemod");

        UIController.instance.activeModifiers.text = "ACTIVE MODIFIERS: " + activeMod1;

        currentGun--;
        SwitchGun();
        // UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (!UIController.instance.pauseScreen.activeInHierarchy)
        {
            pickupCounter -= Time.deltaTime;

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

            if (bounce)
            {
                bounce = false;
                moveInput.y = bounceAmount;
                canDoubleJump = true;
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

            if (camTrans.rotation.eulerAngles.x > maxViewAngle && camTrans.rotation.eulerAngles.x < 180f)
            {
                camTrans.rotation = Quaternion.Euler(maxViewAngle, camTrans.rotation.eulerAngles.y, camTrans.rotation.eulerAngles.z);
            }
            else if (camTrans.rotation.eulerAngles.x > 180f && camTrans.rotation.eulerAngles.x < 360f - maxViewAngle)
            {
                camTrans.rotation = Quaternion.Euler(-maxViewAngle, camTrans.rotation.eulerAngles.y, camTrans.rotation.eulerAngles.z);
            }

            // Handle switching gun
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SwitchGun();
            }

            // Handle shooting
            // Single shots
            if (Input.GetMouseButtonDown(0) && activeGun.fireCounter <= 0 && allGuns.Count > 0)
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
                //Instantiate(bullet, firePoint.position, firePoint.rotation);
                FireShot();
            }

            // Repeating shots
            if (Input.GetMouseButton(0) && activeGun.canAutoFire && allGuns.Count > 0)
            {
                if (activeGun.fireCounter <= 0)
                {
                    FireShot();
                }
            }
        }
    }

    public void FireShot()
    {
        if (activeGun.currentAmmo > 0)
        {
            if (activeGun.gunName == "shotgun")
            {
                Vector3 shotgunPosition1 = new Vector3(0.1f, 0.2f, 0.05f);
                Vector3 shotgunPosition2 = new Vector3(0.13f, 0.07f, 0.2f);
                Vector3 shotgunPosition3 = new Vector3(0.11f, 0.03f, 0.15f);
                Vector3 shotgunPosition4 = new Vector3(0.14f, 0.014f, 0.19f);
                Vector3 shotgunPosition5 = new Vector3(0.04f, 0.015f, 0.16f);

                Instantiate(activeGun.bullet, firePoint.position + shotgunPosition1, firePoint.rotation);
                Instantiate(activeGun.bullet, firePoint.position + shotgunPosition2, firePoint.rotation);
                Instantiate(activeGun.bullet, firePoint.position + shotgunPosition3, firePoint.rotation);
                Instantiate(activeGun.bullet, firePoint.position + shotgunPosition4, firePoint.rotation);
                Instantiate(activeGun.bullet, firePoint.position + shotgunPosition5, firePoint.rotation);
                Instantiate(activeGun.bullet, firePoint.position, firePoint.rotation);

                activeGun.currentAmmo -= 6;

                activeGun.fireCounter = activeGun.fireRate;
                UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;
                StartCoroutine(MuzzleFlash());
            }

            else
            {
                activeGun.currentAmmo--;
                Instantiate(activeGun.bullet, firePoint.position, firePoint.rotation);
                activeGun.fireCounter = activeGun.fireRate;
                UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;
                StartCoroutine(MuzzleFlash());
            }
        }
    }

    public IEnumerator MuzzleFlash()
    {
        activeGun.muzzelFlash.SetActive(true);
        yield return new WaitForSeconds(0.09f);
        activeGun.muzzelFlash.SetActive(false);
    }

    public void SwitchGun()
    {
        activeGun.gameObject.SetActive(false);

        currentGun++;

        if (currentGun >= allGuns.Count)
        {
            currentGun = 0;
        }

        activeGun = allGuns[currentGun];
        activeGun.gameObject.SetActive(true);

        UIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;

        firePoint.position = activeGun.firepoint.position;
    }

    public void AddGun(string gunToAdd)
    {
        bool gunUnlocked = false;

        if (unlockableGuns.Count > 0)
        {
            for (int i = 0; i < unlockableGuns.Count; i++)
            {
                if (unlockableGuns[i].gunName == gunToAdd)
                {
                    gunUnlocked = true;

                    allGuns.Add(unlockableGuns[i]);

                    unlockableGuns.RemoveAt(i);

                    i = unlockableGuns.Count;
                }
            }
        }

        if (gunUnlocked)
        {
            currentGun = allGuns.Count - 2;
            SwitchGun();
        }
    }

    public void Bounce(float bounceforce)
    {
        bounceAmount = bounceforce;
        bounce = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (pickupCounter <= 0)
        {
            if (other.tag == "Health")
            {
                PlayerHealthController.instance.HealPlayer(5);

                ItemManager.instance.spawnPoints.Where(s => s.spawnPoint.transform.position == other.GetComponentInChildren<Transform>().transform.position)
                                                .Select(s => { s.occupied = false; return s; })
                                                .ToList();
                Destroy(other.gameObject);
                AudioManagerMusicSFX.instance.PlaySFX(2);
                pickupCounter = pickupTimeout;
            }

            if (other.tag == "Ammo")
            {
                this.activeGun.GetAmmo();

                ItemManager.instance.spawnPoints.Where(s => s.spawnPoint.transform.position == other.GetComponentInChildren<Transform>().transform.position)
                                                .Select(s => { s.occupied = false; return s; })
                                                .ToList();
                Destroy(other.gameObject);
                AudioManagerMusicSFX.instance.PlaySFX(7);
                pickupCounter = pickupTimeout;
            }

            if (other.tag == "Weapon")
            {
                AddGun(other.gameObject.GetComponent<Gun>().gunName);
                
                ItemManager.instance.spawnPoints.Where(s => s.spawnPoint.transform.position == other.GetComponentInChildren<Transform>().transform.position)
                                                .Select(s => { s.occupied = false; return s; })
                                                .ToList();

                Destroy(other.gameObject);
                AudioManagerMusicSFX.instance.PlaySFX(0);
                pickupCounter = pickupTimeout;
            }
        }
    }
}