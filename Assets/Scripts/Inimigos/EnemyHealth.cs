
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Vida do Inimigo")]
    public float maxHealth = 100f;

    private float currentHealth;
    private bool isDead = false;

    public EnemySpawner spawner;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float dano)
    {
        if (isDead)
            return;

        currentHealth -= dano;

        Debug.Log("Vida do inimigo: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        Debug.Log("Inimigo derrotado!");

        if (spawner != null)
        {
            spawner.EnemyDefeated();
        }

        Destroy(gameObject);
    }
}
