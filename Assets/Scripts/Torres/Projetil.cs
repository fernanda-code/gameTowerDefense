using UnityEngine;

// Script do projétil (flecha, bala de canhão ou míssil).
// Ele vai atrás do inimigo e causa dano quando COLIDE com ele.
// Para a colisão funcionar, o projétil precisa de um Collider 2D (Is Trigger)
// e de um Rigidbody 2D, e o inimigo precisa de um Collider 2D.
public class Projetil : MonoBehaviour
{
    public float velocidade = 10f;
    public float dano = 10f;
    public float tempoMaximo = 3f;   // se não acertar nada nesse tempo, some

    [HideInInspector]
    public GameObject alvo;   // a torre preenche isso quando atira

    private bool acertou = false;

    void Awake()
    {
        // Garante que o projétil tem física para detectar a colisão
        Rigidbody2D corpo = GetComponent<Rigidbody2D>();
        if (corpo == null)
        {
            corpo = gameObject.AddComponent<Rigidbody2D>();
        }
        corpo.bodyType = RigidbodyType2D.Dynamic;
        corpo.gravityScale = 0;   // sem gravidade, ele não cai

        Collider2D colisor = GetComponent<Collider2D>();
        if (colisor == null)
        {
            CircleCollider2D circulo = gameObject.AddComponent<CircleCollider2D>();
            circulo.radius = 0.15f;
            colisor = circulo;
        }
        colisor.isTrigger = true;
    }

    void Start()
    {
        // Garante que o inimigo também tem um colisor
        if (alvo != null && alvo.GetComponent<Collider2D>() == null)
        {
            CircleCollider2D circulo = alvo.AddComponent<CircleCollider2D>();
            circulo.radius = 0.25f;
            circulo.isTrigger = true;
        }

        // Garante que o projétil não fica para sempre no jogo
        Destroy(gameObject, tempoMaximo);
    }

    void Update()
    {
        // Se o inimigo já morreu, o projétil some
        if (alvo == null)
        {
            Destroy(gameObject);
            return;
        }

        // Aponta para o inimigo (o desenho do míssil aponta para cima)
        Vector2 direcao = alvo.transform.position - transform.position;
        float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angulo - 90);

        // Anda em direção ao inimigo (translação)
        transform.position = Vector2.MoveTowards(transform.position, alvo.transform.position, velocidade * Time.deltaTime);
    }

    // O Unity chama esta função quando o colisor do projétil encosta em outro colisor
    void OnTriggerEnter2D(Collider2D outro)
    {
        if (acertou)
            return;

        if (outro.CompareTag("Enemy"))
        {
            acertou = true;
            outro.SendMessage("TakeDamage", dano);
            Destroy(gameObject);
        }
    }
}
