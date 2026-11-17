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
    public int enemiesPerWave = 5;
    public int totalWaves = 3;
    public float timeBetweenSpawns = 2f;
    public float timeBetweenWaves = 5f;

    private int currentWave = 0;
    private int defeatedEnemies = 0;
    private int reachedBaseEnemies = 0;   // inimigos que chegaram no castelo
    private int spawnedEnemies = 0;
    private bool allWavesSpawned = false;
    private bool victory = false;

    // Usados pelo ControleDoJogo para mostrar as informações na tela da Fase1
    public int OndaAtual() { return currentWave; }
    public int InimigosDerrotados() { return defeatedEnemies; }
    public bool Venceu() { return victory; }

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (currentWave < totalWaves)
        {
            currentWave++;

            Debug.Log("Iniciando onda " + currentWave);
            if (textoOnda != null)
            {
                textoOnda.text = "Onda: " + currentWave + "/" + totalWaves;
            }

            for (int i = 0; i < enemiesPerWave; i++)
            {
                GameObject enemy = Instantiate(
                    enemyPrefab,
                    spawnPoint.position,
                    Quaternion.identity
                );

                spawnedEnemies++;

                Debug.Log("Inimigos criados: " + spawnedEnemies);

                EnemyMovement movement = enemy.GetComponent<EnemyMovement>();

                if (movement != null)
                {
                    movement.waypoints = waypoints;
                    movement.baseHealth = baseHealth;
                }

                EnemyHealth health = enemy.GetComponent<EnemyHealth>();

                if (health != null)
                {
                    health.spawner = this;
                }

                yield return new WaitForSeconds(timeBetweenSpawns);
            }

            Debug.Log("Onda " + currentWave + " finalizada!");

            if (currentWave < totalWaves)
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }

        Debug.Log("Todas as ondas foram concluídas!");

        allWavesSpawned = true;

        CheckVictory();
    }
    public void EnemyDefeated()
    {
        defeatedEnemies++;

        Debug.Log("Inimigos derrotados: " + defeatedEnemies);

        if (textoInimigosDerrotados != null)
        {
            textoInimigosDerrotados.text = "Inimigos derrotados: " + defeatedEnemies;
        }

        CheckVictory();
    }

    // Chamado pelo EnemyMovement quando um inimigo chega no castelo
    public void EnemyReachedBase()
    {
        reachedBaseEnemies++;
        CheckVictory();
    }

    void CheckVictory()
    {
        if (victory || !allWavesSpawned)
            return;

        if (baseHealth == null || baseHealth.IsGameOver)
            return;

        // Vitória: todos os inimigos criados já saíram do jogo
        // (derrotados ou chegaram no castelo) e o castelo continua de pé
        if (defeatedEnemies + reachedBaseEnemies == spawnedEnemies && spawnedEnemies > 0)
        {
            victory = true;

            Debug.Log("VITÓRIA! Todos os inimigos foram derrotados!");

            if (textoFimDeJogo != null)
            {
                textoFimDeJogo.SetActive(true);
            }

            Time.timeScale = 0f;
        }
    }

}
