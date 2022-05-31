using UnityEngine;
using UnityEngine.SceneManagement;

public class Gun : MonoBehaviour
{
    public GameObject bullet;

    public bool canAutoFire;

    public float fireRate;

    [HideInInspector]
    public float fireCounter;

    public int currentAmmo, pickupAmount;

    public Transform firepoint;

    public float zoomAmount;

    public string gunName;

    public GameObject muzzelFlash;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (fireCounter > 0)
        {
            fireCounter -= Time.deltaTime;
        }
    }

    public void GetAmmo(bool updateUI)
    {
        currentAmmo += pickupAmount;

        if (updateUI && SceneManager.GetActiveScene().name != "OnlineLevel")
        { 
            UIController.instance.ammoText.text = "AMMO: " + currentAmmo;
        }
        else if (updateUI && SceneManager.GetActiveScene().name == "OnlineLevel")
        {
            OnlineUIController.instance.ammoText.text = "AMMO: " + currentAmmo;
        }
    }
}
