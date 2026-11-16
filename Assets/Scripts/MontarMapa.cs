using UnityEngine;
using UnityEngine.Tilemaps;

// Pinta o mapa sozinho: grama no fundo e uma estrada por cima.
public class MontarMapa : MonoBehaviour
{
    public TileBase grama;
    public TileBase terra;

    // Pontos por onde a estrada passa. Ela liga um ponto ao próximo.
    public Vector2Int[] caminho =
    {
        new Vector2Int(-10, 2),
        new Vector2Int(-5, 2),
        new Vector2Int(-5, -3),
        new Vector2Int(2, -3),
        new Vector2Int(2, 3),
        new Vector2Int(10, 3)
    };

    [ContextMenu("Montar mapa")]
    void Montar()
    {
        Tilemap mapa = GetComponent<Tilemap>();
        mapa.ClearAllTiles();

        // Grama em tudo
        for (int x = -10; x < 10; x++)
        {
            for (int y = -6; y < 6; y++)
            {
                mapa.SetTile(new Vector3Int(x, y, 0), grama);
            }
        }

        // Estrada ligando os pontos
        for (int i = 0; i < caminho.Length - 1; i++)
        {
            Vector2Int atual = caminho[i];
            Vector2Int destino = caminho[i + 1];

            while (atual != destino)
            {
                mapa.SetTile(new Vector3Int(atual.x, atual.y, 0), terra);

                if (atual.x < destino.x) atual.x++;
                else if (atual.x > destino.x) atual.x--;
                else if (atual.y < destino.y) atual.y++;
                else if (atual.y > destino.y) atual.y--;
            }
            mapa.SetTile(new Vector3Int(destino.x, destino.y, 0), terra);
        }

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(mapa);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
#endif
    }
}