
using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Configuração dos Inimigos")]
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public Transform[] waypoints;
    public BaseHealth baseHealth;

    [Header("Configuração das Ondas")]
    public int enemiesPerWave = 5;
    public int totalWaves = 3;
    public float timeBetweenSpawns = 2f;
    public float timeBetweenWaves = 5f;

    private int currentWave = 0;

    [HideInInspector]
    public bool terminou = false;   // vira true quando a última onda termina de sair

    // Usado pelo ControleDoJogo para mostrar a onda na tela
    public int OndaAtual()
    {
        return currentWave;
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

            Debug.Log("Iniciando onda " + currentWave);

            for (int i = 0; i < enemiesPerWave; i++)
            {
                GameObject enemy = Instantiate(
                    enemyPrefab,
                    spawnPoint.position,
                    Quaternion.identity
                );

                EnemyMovement movement = enemy.GetComponent<EnemyMovement>();

                if (movement != null)
                {
                    movement.waypoints = waypoints;
                    movement.baseHealth = baseHealth;
                }

                yield return new WaitForSeconds(timeBetweenSpawns);
            }

            Debug.Log("Onda " + currentWave + " finalizada!");

            if (currentWave < totalWaves)
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            }
        }

        terminou = true;
        Debug.Log("Todas as ondas foram concluídas!");
    }
}
