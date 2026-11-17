using UnityEngine;

// Script da torre (serve para o arqueiro, o canhão e o míssil).
// A diferença entre elas são os valores que colocamos no Inspector.
public class Torre : MonoBehaviour
{
    public float alcance = 5f;           // até onde a torre enxerga
    public float tempoEntreTiros = 1f;   // segundos entre um tiro e outro
    public GameObject projetil;          // prefab da flecha, da bala ou do míssil

    private float contador = 0f;

    void Update()
    {
        contador = contador + Time.deltaTime;

        GameObject alvo = ProcurarInimigo();

        if (alvo != null)
        {
            VirarPara(alvo);
        }

        if (alvo != null && contador >= tempoEntreTiros)
        {
            Atirar(alvo);
            contador = 0f;
        }
    }

    // Procura o inimigo mais perto que esteja dentro do alcance
    GameObject ProcurarInimigo()
    {
        GameObject[] inimigos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject maisPerto = null;
        float menorDistancia = alcance;

        foreach (GameObject inimigo in inimigos)
        {
            float distancia = Vector2.Distance(transform.position, inimigo.transform.position);

            if (distancia <= menorDistancia)
            {
                menorDistancia = distancia;
                maisPerto = inimigo;
            }
        }

        return maisPerto;
    }

    // Gira a torre para apontar para o inimigo.
    // O desenho da torre aponta para cima, por isso o "- 90".
    void VirarPara(GameObject alvo)
    {
        Vector2 direcao = alvo.transform.position - transform.position;
        float angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angulo - 90);
    }

    void Atirar(GameObject alvo)
    {
        GameObject novoProjetil = Instantiate(projetil, transform.position, Quaternion.identity);
        novoProjetil.GetComponent<Projetil>().alvo = alvo;
    }

    // Mostra o alcance da torre na aba Scene (círculo azul)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, alcance);
    }
}
