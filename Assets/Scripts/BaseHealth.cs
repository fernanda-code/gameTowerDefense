
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

    // Usado pelo ControleDoJogo para mostrar a vida na tela
    public int VidaAtual()
    {
        return currentHealth;
    }

    void GameOver()
    {
        gameOver = true;

        Debug.Log("GAME OVER! A base foi destruída.");

<<<<<<< HEAD
        if (ControleDoJogo.instancia != null)
        {
            ControleDoJogo.instancia.Derrota("O castelo foi destruído!");
=======
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
>>>>>>> origin/main
        }
    }
}
