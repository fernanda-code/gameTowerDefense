using UnityEngine;
using TMPro;

public class BaseHealth : MonoBehaviour
{
    [Header("Vida da Base")]
    public int maxHealth = 500;

    [Header("Interface")]
    public TMP_Text textoVidaBase;
    public TMP_Text textoFimDeJogo;

    private int currentHealth;
    private bool gameOver = false;

    public bool IsGameOver
    {
        get { return gameOver; }
    }

    // Usado pelo ControleDoJogo para mostrar a vida na tela da Fase1
    public int VidaAtual()
    {
        return currentHealth;
    }

    void Start()
    {
        Time.timeScale = 1f;
        currentHealth = maxHealth;
        gameOver = false;

        Debug.Log("Vida inicial da base: " + currentHealth);
        AtualizarTextoVida();
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
        AtualizarTextoVida();

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        gameOver = true;

        Debug.Log("GAME OVER! A base foi destruída.");

        if (textoFimDeJogo != null)
        {
            textoFimDeJogo.text = "GAME OVER!";
            textoFimDeJogo.gameObject.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    void AtualizarTextoVida()
    {
        if (textoVidaBase != null)
        {
            textoVidaBase.text = "Vida da Base: " + currentHealth + "/" + maxHealth;
        }
    }
}
