
using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    [Header("Vida da Base")]
    public int maxHealth = 500;

    private int currentHealth;
    private bool gameOver = false;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log("Vida inicial da base: " + currentHealth);
    }

    public void TakeDamage(int damage)
    {
        if (gameOver)
            return;

        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        Debug.Log("Vida da base: " + currentHealth);

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        gameOver = true;
        Debug.Log("GAME OVER! A base foi destruída.");
    }
}
