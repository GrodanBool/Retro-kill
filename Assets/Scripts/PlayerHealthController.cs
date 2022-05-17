using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    public float maxHealth, currentHealth;

    public float invincibleLength = 1f;
    private float invincCounter;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (invincCounter > 0)
        {
            invincCounter -= Time.deltaTime;
        }
    }

    public void DamagePlayer(float damageAmount)
    {
        if (invincCounter <= 0)
        {
            AudioManagerMusicSFX.instance.PlaySFX(6);

            currentHealth -= damageAmount;

            // UIController.instance.ShowDamage();

            if (currentHealth <= 0)
            {
                gameObject.SetActive(false);

                currentHealth = 0;


                GameManager.instance.PlayerDied();

                AudioManagerMusicSFX.instance.StopBGM();
                AudioManagerMusicSFX.instance.PlaySFX(5);
                AudioManagerMusicSFX.instance.StopSFX(6);
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
}
