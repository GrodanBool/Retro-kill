using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    public int currentHealth = 5;

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