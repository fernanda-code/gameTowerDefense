
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Vida do Inimigo")]
    public float maxHealth = 100f;

    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float dano)
    {
        currentHealth -= dano;

        Debug.Log("Vida do inimigo: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Inimigo derrotado!");
        Destroy(gameObject);
    }
}
