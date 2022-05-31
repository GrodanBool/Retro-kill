using Mirror;
using UnityEngine;

public class EnemyOnlineHealthController : NetworkBehaviour
{
    public int currentHealth = 5;

    [Command]
    public void DamageEnemy(int damageAmount)
    {
        RpcDamageEnemy(damageAmount);
    }

    [ClientRpc]
    public void RpcDamageEnemy(int damageAmount)
    {
        // When called, damage enemy by one
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            if (PlayerPrefs.GetString("activemod").Contains("Lose Health"))
            {
                if (currentHealth < PlayerOnlineHealthController.instance.maxHealth)
                {
                    PlayerOnlineHealthController.instance.HealPlayer(1);
                }
            }

            ScoreController.instance.OnEnemyKilled();
            Destroy(gameObject);
        }
    }
}