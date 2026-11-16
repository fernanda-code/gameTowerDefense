using UnityEngine;

// Script da torre (serve para o arqueiro e para o canhão).
// A diferença entre elas são os valores que colocamos no Inspector.
public class Torre : MonoBehaviour
{
    public float alcance = 5f;           // até onde a torre enxerga
    public float tempoEntreTiros = 1f;   // segundos entre um tiro e outro
    public GameObject projetil;          // prefab da flecha ou da bala

    private float contador = 0f;

    void Update()
    {
        contador = contador + Time.deltaTime;

        GameObject alvo = ProcurarInimigo();

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
