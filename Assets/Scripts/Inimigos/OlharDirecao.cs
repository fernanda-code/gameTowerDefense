using UnityEngine;

// Faz o inimigo virar para o lado em que está andando.
// O desenho do soldado olha para a direita.
public class OlharDirecao : MonoBehaviour
{
    private Vector3 posicaoAnterior;

    void Start()
    {
        posicaoAnterior = transform.position;
    }

    void Update()
    {
        Vector3 movimento = transform.position - posicaoAnterior;

        if (movimento.magnitude > 0.001f)
        {
            float angulo = Mathf.Atan2(movimento.y, movimento.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angulo);
        }

        posicaoAnterior = transform.position;
    }
}
