using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerOnlineHealthController : NetworkBehaviour
{
    public static PlayerOnlineHealthController instance;

    public float maxHealth, currentHealth;
    [SerializeField] bool takeDamage = false;
    [SerializeField] float time = 5f;

    public float invincibleLength = 1f;
    private float invincCounter;
    private bool set = false;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("activemod").Contains("Lose Health"))
        {
            takeDamage = true;
            StartCoroutine(DamageOvertime(time));
        }

        currentHealth = maxHealth;
        if (PlayerPrefs.GetString("activemod").Contains("Lower Health"))
        {
            currentHealth = maxHealth / 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hasAuthority)
        {
            if (SceneManager.GetActiveScene().name == "CreateLobbyScreen" && set)
            {
                OnlineUIController.instance.healthSlider.maxValue = maxHealth;
                OnlineUIController.instance.healthSlider.value = currentHealth;
                OnlineUIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
                set = true;
            }

            if (invincCounter > 0)
            {
                invincCounter -= Time.deltaTime;
            }
        }
    }

    public void DamagePlayer(float damageAmount)
    {
        if (invincCounter <= 0 && hasAuthority)
        {
            // AudioManagerMusicSFX.instance.PlaySFX(6);

            currentHealth -= damageAmount;

            // UIController.instance.ShowDamage();

            if (currentHealth <= 0)
            {
                gameObject.SetActive(false);

                currentHealth = 0;


                GameManager.instance.PlayerDied();

                AudioManagerMusicSFX.instance.StopBGM();
                AudioManagerMusicSFX.instance.PlaySFX(1);
                // AudioManagerMusicSFX.instance.StopSFX(6);
            }

            invincCounter = invincibleLength;

            // Not set to instance of object right now
            OnlineUIController.instance.healthSlider.value = currentHealth;
            OnlineUIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
        }
    }

    public void HealPlayer(int healAmount)
    {
        if (hasAuthority)
        {
            currentHealth += healAmount;

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            OnlineUIController.instance.healthSlider.value = currentHealth;
            OnlineUIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
        }
    }

    IEnumerator DamageOvertime(float time)
    {
        if (hasAuthority)
        {
            while (takeDamage)
            {
                DamagePlayer(1);
                takeDamage = !takeDamage;
                yield return new WaitForSeconds(time);
                takeDamage = !takeDamage;
            }
        }
    }
}
