
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Vida do Inimigo")]
    public float maxHealth = 100f;

    [Header("Recompensa")]
    public int moedasAoMorrer = 25;

    private float currentHealth;
    private bool morto = false;

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
        if (morto) return;   // evita morrer duas vezes no mesmo frame
        morto = true;

        Debug.Log("Inimigo derrotado!");
        Loja.GanharMoedas(moedasAoMorrer);
        Destroy(gameObject);
    }
}
