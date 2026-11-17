using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Vida do Inimigo")]
    public float maxHealth = 100f;

    [Header("Recompensa")]
    public int moedasAoMorrer = 25;

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

        // Dá moedas para o jogador comprar mais torres
        Loja.GanharMoedas(moedasAoMorrer);

        if (spawner != null)
        {
            spawner.EnemyDefeated();
        }

        Destroy(gameObject);
    }
}
