using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Pinta o mapa sozinho: grama, estrada, manchas de areia e pedra, com bordas
// arredondadas, e espalha arbustos e pedrinhas.
// Para usar: clique nos 3 pontinhos do componente -> "Montar mapa".
public class MontarMapa : MonoBehaviour
{
    public enum Terreno { Grama, Estrada, Areia, Pedra }

    // Os desenhos de um tipo de terreno: o do meio e os de borda
    // (grama com um pedaço do terreno de um lado).
    [System.Serializable]
    public class Bordas
    {
        public TileBase meio;
        public TileBase emCima;
        public TileBase emBaixo;
        public TileBase naDireita;
        public TileBase naEsquerda;
        public TileBase cimaEsquerda;
        public TileBase cimaDireita;
        public TileBase baixoEsquerda;
        public TileBase baixoDireita;
        public TileBase pontinhaCimaEsquerda;
        public TileBase pontinhaCimaDireita;
        public TileBase pontinhaBaixoEsquerda;
        public TileBase pontinhaBaixoDireita;
    }

    // Uma mancha redonda de areia ou pedra no mapa
    [System.Serializable]
    public class Mancha
    {
        public Vector2Int centro;
        public float raio = 2.5f;
        public Terreno tipo = Terreno.Areia;
    }

    [Header("Grama")]
    public TileBase grama;
    public TileBase gramaComTextura;

    [Header("Terrenos")]
    public Bordas estrada;
    public Bordas areia;
    public Bordas pedra;

    [Header("Manchas de areia e pedra")]
    public Mancha[] manchas;

    [Header("Decoração (arbustos e pedras)")]
    public Sprite[] decoracoes;
    public int quantidadeDecoracoes = 35;

    [Header("Tamanho do mapa")]
    public int minX = -13;
    public int maxX = 13;
    public int minY = -8;
    public int maxY = 8;

    // Pontos por onde a estrada passa. Ela liga um ponto ao próximo.
    public Vector2Int[] caminho =
    {
        new Vector2Int(-13, 2),
        new Vector2Int(-5, 2),
        new Vector2Int(-5, -3),
        new Vector2Int(2, -3),
        new Vector2Int(2, 3),
        new Vector2Int(10, 3)
    };

    private HashSet<Vector2Int> celulasEstrada = new HashSet<Vector2Int>();

    [ContextMenu("Montar mapa")]
    void Montar()
    {
        Tilemap mapa = GetComponent<Tilemap>();
        mapa.ClearAllTiles();

        CalcularEstrada();

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                Terreno tipo = TipoDe(x, y);
                TileBase desenho;

                if (tipo == Terreno.Grama)
                    desenho = EscolherGrama(x, y);
                else
                    desenho = BordasDe(tipo).meio;

                mapa.SetTile(new Vector3Int(x, y, 0), desenho);
            }
        }

        ColocarDecoracao();

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(mapa);
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
#endif
    }

    // Guarda todos os quadradinhos que são estrada
    void CalcularEstrada()
    {
        celulasEstrada.Clear();

        for (int i = 0; i < caminho.Length - 1; i++)
        {
            Vector2Int atual = caminho[i];
            Vector2Int destino = caminho[i + 1];

            while (atual != destino)
            {
                celulasEstrada.Add(atual);

                if (atual.x < destino.x) atual.x++;
                else if (atual.x > destino.x) atual.x--;
                else if (atual.y < destino.y) atual.y++;
                else if (atual.y > destino.y) atual.y--;
            }
            celulasEstrada.Add(destino);
        }
    }

    // Diz qual terreno fica em cada quadradinho
    Terreno TipoDe(int x, int y)
    {
        if (celulasEstrada.Contains(new Vector2Int(x, y)))
            return Terreno.Estrada;

        // As manchas ficam longe da estrada, para as bordas não se misturarem
        if (PertoDaEstrada(x, y, 2))
            return Terreno.Grama;

        if (manchas != null)
        {
            foreach (Mancha mancha in manchas)
            {
                float dx = x - mancha.centro.x;
                float dy = y - mancha.centro.y;

                if (dx * dx + dy * dy <= mancha.raio * mancha.raio)
                    return mancha.tipo;
            }
        }

        return Terreno.Grama;
    }

    Bordas BordasDe(Terreno tipo)
    {
        if (tipo == Terreno.Estrada) return estrada;
        if (tipo == Terreno.Areia) return areia;
        return pedra;
    }

    // Escolhe a grama certa olhando os vizinhos:
    // se tiver estrada, areia ou pedra do lado, usa a grama com a borda daquele lado.
    TileBase EscolherGrama(int x, int y)
    {
        Terreno[] ordem = { Terreno.Estrada, Terreno.Areia, Terreno.Pedra };

        foreach (Terreno tipo in ordem)
        {
            TileBase borda = EscolherBorda(x, y, tipo, BordasDe(tipo));
            if (borda != null)
                return borda;
        }

        // Grama normal, com umas manchinhas de grama com textura
        if (Mathf.Sin(x * 0.7f) + Mathf.Cos(y * 0.9f + x * 0.3f) > 0.6f)
            return gramaComTextura;

        return grama;
    }

    TileBase EscolherBorda(int x, int y, Terreno tipo, Bordas b)
    {
        bool cima = TipoDe(x, y + 1) == tipo;
        bool baixo = TipoDe(x, y - 1) == tipo;
        bool direita = TipoDe(x + 1, y) == tipo;
        bool esquerda = TipoDe(x - 1, y) == tipo;

        if (cima && esquerda) return b.cimaEsquerda;
        if (cima && direita) return b.cimaDireita;
        if (baixo && esquerda) return b.baixoEsquerda;
        if (baixo && direita) return b.baixoDireita;

        if (cima) return b.emCima;
        if (baixo) return b.emBaixo;
        if (direita) return b.naDireita;
        if (esquerda) return b.naEsquerda;

        if (TipoDe(x - 1, y + 1) == tipo) return b.pontinhaCimaEsquerda;
        if (TipoDe(x + 1, y + 1) == tipo) return b.pontinhaCimaDireita;
        if (TipoDe(x - 1, y - 1) == tipo) return b.pontinhaBaixoEsquerda;
        if (TipoDe(x + 1, y - 1) == tipo) return b.pontinhaBaixoDireita;

        return null;
    }

    bool PertoDaEstrada(int x, int y, int distancia)
    {
        for (int dx = -distancia; dx <= distancia; dx++)
        {
            for (int dy = -distancia; dy <= distancia; dy++)
            {
                if (celulasEstrada.Contains(new Vector2Int(x + dx, y + dy)))
                    return true;
            }
        }
        return false;
    }

    // Espalha arbustos e pedras pela grama, longe da estrada, das manchas e dos pontos de torre
    void ColocarDecoracao()
    {
        // Apaga a decoração antiga, se já existir
        Transform antiga = transform.Find("Decoracao");
        if (antiga != null)
        {
            DestroyImmediate(antiga.gameObject);
        }

        if (decoracoes == null || decoracoes.Length == 0)
            return;

        GameObject pasta = new GameObject("Decoracao");
        pasta.transform.SetParent(transform, false);

        PontoDeTorre[] pontos = FindObjectsByType<PontoDeTorre>(FindObjectsSortMode.None);
        HashSet<Vector2Int> usadas = new HashSet<Vector2Int>();

        // Sempre sorteia igual, para o mapa não mudar toda vez
        Random.InitState(7);

        int colocadas = 0;
        int tentativas = 0;

        while (colocadas < quantidadeDecoracoes && tentativas < 3000)
        {
            tentativas++;

            int x = Random.Range(minX, maxX + 1);
            int y = Random.Range(minY, maxY + 1);
            Vector2Int celula = new Vector2Int(x, y);

            if (usadas.Contains(celula)) continue;
            if (!SoGramaEmVolta(x, y)) continue;

            Vector3 posicao = new Vector3(x + 0.5f, y + 0.5f, 0);
            if (PertoDeTorre(posicao, pontos)) continue;

            GameObject enfeite = new GameObject("Enfeite");
            enfeite.transform.SetParent(pasta.transform, false);
            enfeite.transform.position = posicao;

            SpriteRenderer desenho = enfeite.AddComponent<SpriteRenderer>();
            desenho.sprite = decoracoes[Random.Range(0, decoracoes.Length)];
            desenho.sortingOrder = -5;

            usadas.Add(celula);
            colocadas++;
        }
    }

    bool SoGramaEmVolta(int x, int y)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (TipoDe(x + dx, y + dy) != Terreno.Grama) return false;
            }
        }
        return true;
    }

    bool PertoDeTorre(Vector3 posicao, PontoDeTorre[] pontos)
    {
        foreach (PontoDeTorre ponto in pontos)
        {
            if (Vector2.Distance(posicao, ponto.transform.position) < 1.5f) return true;
        }
        return false;
    }
}
