using UnityEngine;

// Script do projétil (flecha ou bala de canhão).
// Ele vai atrás do inimigo e causa dano quando encosta.
public class Projetil : MonoBehaviour
{
    public float velocidade = 10f;
    public float dano = 10f;

    [HideInInspector]
    public GameObject alvo;   // a torre preenche isso quando atira

    void Update()
    {
        // Se o inimigo já morreu, o projétil some
        if (alvo == null)
        {
            Destroy(gameObject);
            return;
        }

        // Anda em direção ao inimigo
        transform.position = Vector2.MoveTowards(transform.position, alvo.transform.position, velocidade * Time.deltaTime);

        // Chegou no inimigo: causa dano e some
        if (Vector2.Distance(transform.position, alvo.transform.position) < 0.1f)
        {
            alvo.SendMessage("TakeDamage", dano);
            Destroy(gameObject);
        }
    }
}
