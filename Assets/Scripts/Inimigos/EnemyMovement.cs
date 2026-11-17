using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movimentação")]
    public Transform[] waypoints;
    public float speed = 2f;

    [Header("Ataque à Base")]
    public BaseHealth baseHealth;
    public int damageToBase = 50;

    private int currentWaypoint = 0;

    void Update()
    {
        MoveEnemy();
    }

    void MoveEnemy()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        Transform target = waypoints[currentWaypoint];

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            currentWaypoint++;

            if (currentWaypoint >= waypoints.Length)
            {
                if (baseHealth != null)
                {
                    baseHealth.TakeDamage(damageToBase);
                }

                // Avisa o spawner que este inimigo saiu do jogo (chegou na base)
                EnemyHealth health = GetComponent<EnemyHealth>();
                if (health != null && health.spawner != null)
                {
                    health.spawner.EnemyReachedBase();
                }

                Destroy(gameObject);
            }
        }
    }
}
