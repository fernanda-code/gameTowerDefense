using UnityEngine;

// Um lugar do mapa (a base de pedra) onde o jogador pode construir uma torre.
// A Loja chama Construir() quando o jogador clica aqui.
public class PontoDeTorre : MonoBehaviour
{
    [HideInInspector]
    public bool ocupado = false;

    public void Construir(GameObject prefabTorre)
    {
        // Cria a torre em cima da base de pedra
        Instantiate(prefabTorre, transform.position, Quaternion.identity);
        ocupado = true;
    }
}
