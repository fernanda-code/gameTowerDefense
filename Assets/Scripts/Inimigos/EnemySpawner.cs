
using UnityEngine;
using System.Collections;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    [Header("Configuração dos Inimigos")]
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public Transform[] waypoints;
    public BaseHealth baseHealth;

    [Header("Interface")]
    public TMP_Text textoOnda;
    public TMP_Text textoInimigosDerrotados;
    public GameObject textoFimDeJogo;

    [Header("Configuração das Ondas")]
    public int enemiesPerWave = 6;
    public int totalWaves = 3;
    public float timeBetweenSpawns = 2f;
    public float timeBetweenWaves = 4f;

    [Header("Aumento de Dificuldade")]
    public int inimigosExtrasPorOnda = 4;
    public float reducaoIntervaloPorOnda = 0.3f;
    public float intervaloMinimo = 0.8f;

    private int currentWave = 0;
    private int defeatedEnemies = 0;
    private int reachedBaseEnemies = 0;
    private int spawnedEnemies = 0;

    private bool allWavesSpawned = false;
    private bool victory = false;

    // Informações usadas pelo ControleDoJogo
    public int OndaAtual()
    {
        return currentWave;
    }

    public int InimigosDerrotados()
    {
        return defeatedEnemies;
    }

    public bool Venceu()
    {
        return victory;
    }

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (currentWave < totalWaves)
        {
            currentWave++;

            // Aumenta os inimigos a cada nova onda
            int quantidadeInimigos =
                enemiesPerWave +
                (currentWave - 1) * inimigosExtrasPorOnda;

            // Diminui o intervalo entre os inimigos
            float intervaloAtual = Mathf.Max(
                intervaloMinimo,
                timeBetweenSpawns -
                (currentWave - 1) * reducaoIntervaloPorOnda
            );

            Debug.Log(
                "Iniciando onda " + currentWave +
                " com " + quantidadeInimigos + " inimigos."
            );

            if (textoOnda != null)
            {
                textoOnda.text =
                    "Onda: " + currentWave + "/" + totalWaves;
            }

            for (int i = 0; i < quantidadeInimigos; i++)
            {
                GameObject enemy = Instantiate(
                    enemyPrefab,
                    spawnPoint.position,
                    Quaternion.identity
                );

                spawnedEnemies++;

                EnemyMovement movement =
                    enemy.GetComponent<EnemyMovement>();

                if (movement != null)
                {
                    movement.waypoints = waypoints;
                    movement.baseHealth = baseHealth;
                }

                EnemyHealth health =
                    enemy.GetComponent<EnemyHealth>();

                if (health != null)
                {
                    health.spawner = this;
                }

                yield return new WaitForSeconds(intervaloAtual);
            }

            Debug.Log(
                "Onda " + currentWave + " finalizada!"
            );

            if (currentWave < totalWaves)
            {
                yield return new WaitForSeconds(
                    timeBetweenWaves
                );
            }
        }

        Debug.Log("Todas as ondas foram geradas!");

        allWavesSpawned = true;

        CheckVictory();
    }

    // Chamado quando um inimigo é derrotado
    public void EnemyDefeated()
    {
        defeatedEnemies++;

        Debug.Log(
            "Inimigos derrotados: " + defeatedEnemies
        );

        if (textoInimigosDerrotados != null)
        {
            textoInimigosDerrotados.text =
                "Inimigos derrotados: " + defeatedEnemies;
        }

        CheckVictory();
    }

    // Chamado quando um inimigo chega ao castelo
    public void EnemyReachedBase()
    {
        reachedBaseEnemies++;

        CheckVictory();
    }

    void CheckVictory()
    {
        if (victory || !allWavesSpawned)
        {
            return;
        }

        if (baseHealth == null || baseHealth.IsGameOver)
        {
            return;
        }

        // Vitória quando todos os inimigos saíram do cenário
        // e o castelo continua de pé
        if (defeatedEnemies + reachedBaseEnemies ==
            spawnedEnemies && spawnedEnemies > 0)
        {
            victory = true;

            Debug.Log(
                "VITÓRIA! O castelo sobreviveu a todas as ondas!"
            );

            if (textoFimDeJogo != null)
            {
                textoFimDeJogo.SetActive(true);
            }

            // O ControleDoJogo detecta a vitória e
            // pausa a partida, exibindo a tela final.
        }
    }
}
