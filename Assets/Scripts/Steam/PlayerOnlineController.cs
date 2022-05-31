using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerOnlineController : NetworkBehaviour
{
    public static PlayerOnlineController instance;

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


    //online variables
    public GameObject cameraMountPoint;
    public GameObject PlayerModel;


    // Happens straight away in Unity (before start runs)
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerModel.SetActive(false);
        pickupCounter = pickupTimeout;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "OnlineLevel")
        {
            if (!OnlineUIController.instance.pauseScreen.activeInHierarchy)
            {

                if (!PlayerModel.activeSelf)
                {
                    PlayerModel.SetActive(true);
                }

                if (isLocalPlayer)
                {
                    Transform cameraTransform = Camera.main.gameObject.transform;  //Find main camera which is part of the scene instead of the prefab
                    cameraTransform.SetParent(cameraMountPoint.transform);  //Make the camera a child of the mount point
                    cameraTransform.position = cameraMountPoint.transform.position;  //Set position/rotation same as the mount point
                    cameraTransform.rotation = cameraMountPoint.transform.rotation;
                }

                if (hasAuthority)
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
                        CmdSwitchGun();
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
        }
    }

    public void FireShot()
    {
        if (activeGun.currentAmmo > 0 && hasAuthority)
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
                OnlineUIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;
                StartCoroutine(MuzzleFlash());
            }

            else
            {
                activeGun.currentAmmo--;
                //Instantiate(activeGun.bullet, firePoint.position, firePoint.rotation);
                cmdInstantiateBullet();
                activeGun.fireCounter = activeGun.fireRate;
                OnlineUIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;
                StartCoroutine(MuzzleFlash());
            }
        }
    }

    [Command]
    public void cmdInstantiateBullet()
    {
        InstantiateBullet();
    }

    [ClientRpc]
    public void InstantiateBullet()
    {
        Instantiate(activeGun.bullet, firePoint.position, firePoint.rotation);
    }

    public IEnumerator MuzzleFlash()
    {
        activeGun.muzzelFlash.SetActive(true);
        CmdToggleMuzzleFlash(true);
        yield return new WaitForSeconds(0.09f);
        activeGun.muzzelFlash.SetActive(false);
        CmdToggleMuzzleFlash(false);
    }

    [Command]
    public void CmdToggleMuzzleFlash(bool toggle)
    {
        ToggleMuzzleFlash(toggle);
    }

    [ClientRpc]
    public void ToggleMuzzleFlash(bool toggle)
    {
        activeGun.muzzelFlash.SetActive(toggle);
    }
    [Command]
    public void CmdSwitchGun()
    {
        SwitchGun();
    }

    [ClientRpc]
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

        if (hasAuthority)
        {
            OnlineUIController.instance.ammoText.text = "AMMO: " + activeGun.currentAmmo;
            firePoint.position = activeGun.firepoint.position;
        }
    }

    [Command]
    public void CmdAddGun(string gunToAdd)
    {
        AddGun(gunToAdd);
    }

    [ClientRpc]
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
            CmdSwitchGun();
        }
    }

    public void Bounce(float bounceforce)
    {
        bounceAmount = bounceforce;
        bounce = true;
    }

    [Command]
    void CmdHealth(GameObject gameObject, Transform transform)
    {
        Health(gameObject, transform);
    }

    [ClientRpc]
    void Health(GameObject gameObject, Transform transform)
    {
        if (hasAuthority)
        {
            PlayerHealthController.instance.HealPlayer(5);
        }

        ItemManager.instance.spawnPoints.Where(s => s.spawnPoint.transform.position == transform.position)
                                        .Select(s => { s.occupied = false; return s; })
                                        .ToList();
        Destroy(gameObject);
        AudioManagerMusicSFX.instance.PlaySFX(2);
        pickupCounter = pickupTimeout;
    }

    [Command]
    void CmdAmmo(GameObject gameObject, Transform transform)
    {
        Ammo(gameObject, transform);
    }

    [ClientRpc]
    void Ammo(GameObject gameObject, Transform transform)
    {
        if (hasAuthority)
        {
            this.activeGun.GetAmmo(true);
        }

        ItemManager.instance.spawnPoints.Where(s => s.spawnPoint.transform.position == transform.position)
                                        .Select(s => { s.occupied = false; return s; })
                                        .ToList();
        Destroy(gameObject);
        AudioManagerMusicSFX.instance.PlaySFX(7);
        pickupCounter = pickupTimeout;
    }

    [Command]
    void CmdWeapon(GameObject gameObject, Transform transform)
    {
        Weapon(gameObject, transform);
    }

    [ClientRpc]
    void Weapon(GameObject gameObject, Transform transform)
    {
        if (hasAuthority)
        {
            if (allGuns.Any(g => g.gunName == gameObject.GetComponent<Gun>().gunName))
            {
                foreach (var gun in allGuns)
                {
                    if (gun.gunName == gameObject.GetComponent<Gun>().gunName)
                    {
                        gun.GetAmmo(false);
                    }
                }
            }
            else
            {
                CmdAddGun(gameObject.GetComponent<Gun>().gunName);
            }
        }

        ItemManager.instance.spawnPoints.Where(s => s.spawnPoint.transform.position == transform.position)
                                        .Select(s => { s.occupied = false; return s; })
                                        .ToList();

        Destroy(gameObject);
        AudioManagerMusicSFX.instance.PlaySFX(0);
        pickupCounter = pickupTimeout;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (pickupCounter <= 0)
        {
            if (other.tag == "Health")
            {
                CmdHealth(other.gameObject, other.GetComponentInChildren<Transform>().transform);
            }

            if (other.tag == "Ammo")
            {
                CmdAmmo(other.gameObject, other.GetComponentInChildren<Transform>().transform);
            }

            if (other.tag == "Weapon")
            {
                CmdWeapon(other.gameObject, other.GetComponentInChildren<Transform>().transform);
            }
        }
    }
}