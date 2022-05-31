using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

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
        if (SceneManager.GetActiveScene().name == "CreateLobbyScreen" && set)
        {
            UIController.instance.healthSlider.maxValue = maxHealth;
            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
            set = true;
        }

        if (invincCounter > 0)
        {
            invincCounter -= Time.deltaTime;
        }
    }

    public void DamagePlayer(float damageAmount)
    {
        if (invincCounter <= 0)
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
            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
        }
    }

    public void HealPlayer(int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
    }

    IEnumerator DamageOvertime(float time)
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
