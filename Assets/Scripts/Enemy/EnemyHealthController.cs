using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    public int currentHealth = 5;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageEnemy(int damageAmount)
    {
        // When called, damage enemy by one
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            if (PlayerPrefs.GetString("activemod").Contains("Lose Health"))
            { 
                if (currentHealth < PlayerHealthController.instance.maxHealth)
                {
                    PlayerHealthController.instance.HealPlayer(1);
                }
            }
            ScoreController.instance.OnEnemyKilled();
            Destroy(gameObject);
        }
    }
}
