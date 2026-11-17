
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Vida do Inimigo")]
    public float maxHealth = 100f;

    [Header("Recompensa")]
    public int moedasAoMorrer = 25;

    private float currentHealth;
<<<<<<< HEAD
    private bool morto = false;
=======
    private bool isDead = false;

    public EnemySpawner spawner;
>>>>>>> origin/main

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
<<<<<<< HEAD
        if (morto) return;   // evita morrer duas vezes no mesmo frame
        morto = true;

        Debug.Log("Inimigo derrotado!");
        Loja.GanharMoedas(moedasAoMorrer);
=======
        isDead = true;

        Debug.Log("Inimigo derrotado!");

        if (spawner != null)
        {
            spawner.EnemyDefeated();
        }

>>>>>>> origin/main
        Destroy(gameObject);
    }
}
