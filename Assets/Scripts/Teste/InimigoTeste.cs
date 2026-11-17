using UnityEngine;

// Inimigo FALSO só para testar as torres.
// Pode apagar quando o inimigo da Fernanda estiver pronto.
// Não esquecer de colocar a tag "Enemy" no objeto!
public class InimigoTeste : MonoBehaviour
{
    public float vida = 50f;
    public float velocidade = 1f;

    void Update()
    {
        // Anda para a direita
        transform.position = transform.position + Vector3.right * velocidade * Time.deltaTime;
    }

    // As torres chamam esta função quando acertam o inimigo
    public void TakeDamage(float dano)
    {
        vida = vida - dano;
        Debug.Log(gameObject.name + " levou " + dano + " de dano. Vida: " + vida);

        if (vida <= 0)
        {
            Destroy(gameObject);
        }
    }
}
