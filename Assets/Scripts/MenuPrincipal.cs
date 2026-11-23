
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Cena do jogo")]
    public string cenaDoJogo = "Fase1_Medieval";

    [Header("Imagens do menu")]
    public Texture2D fundoMenu;
    public Sprite placaFrase;
    public Sprite madeiraBotao;

    private bool mostrandoComoJogar = false;
    private Vector2 rolagem = Vector2.zero;

    void OnGUI()
    {
        // Fundo medieval do menu
        if (fundoMenu != null)
        {
            GUI.DrawTexture(
                new Rect(0, 0, Screen.width, Screen.height),
                fundoMenu,
                ScaleMode.ScaleAndCrop
            );
        }

        float centroX = Screen.width / 2f;

        // Estilo do subtítulo
        GUIStyle subtitulo = new GUIStyle(GUI.skin.label);
        subtitulo.fontSize = 22;
        subtitulo.fontStyle = FontStyle.Normal;
        subtitulo.alignment = TextAnchor.MiddleCenter;
        subtitulo.normal.textColor = Color.white;

        // Estilo dos botões de madeira
        GUIStyle botao = new GUIStyle(GUIStyle.none);
        botao.fontSize = 24;
        botao.fontStyle = FontStyle.Bold;
        botao.alignment = TextAnchor.MiddleCenter;
        botao.normal.textColor = Color.white;
        botao.hover.textColor = Color.yellow;
        botao.active.textColor = Color.yellow;
        botao.focused.textColor = Color.white;

        // Tela de instruções
        if (mostrandoComoJogar)
        {
            MostrarComoJogar(centroX, botao);
            return;
        }

        // Posicionamento proporcional à altura da tela
        float inicioMenu = Screen.height * 0.48f;

        // Placa com a frase do menu
        DesenharSprite(
            new Rect(centroX - 310, inicioMenu, 620, 65),
            placaFrase
        );

        GUI.Label(
            new Rect(centroX - 300, inicioMenu + 10, 600, 40),
            "Defenda o castelo das ondas de inimigos!",
            subtitulo
        );

        // Posições dos botões
        Rect jogarRect = new Rect(
            centroX - 130, inicioMenu + 75, 260, 65
        );

        Rect comoJogarRect = new Rect(
            centroX - 130, inicioMenu + 155, 260, 65
        );

        Rect sairRect = new Rect(
            centroX - 130, inicioMenu + 235, 260, 65
        );

        // Imagens de madeira
        DesenharSprite(jogarRect, madeiraBotao);
        DesenharSprite(comoJogarRect, madeiraBotao);
        DesenharSprite(sairRect, madeiraBotao);

        // Botões interativos
        if (GUI.Button(jogarRect, "Jogar", botao))
        {
            SceneManager.LoadScene(cenaDoJogo);
        }

        if (GUI.Button(comoJogarRect, "Como jogar", botao))
        {
            mostrandoComoJogar = true;
        }

        if (GUI.Button(sairRect, "Sair", botao))
        {
            Application.Quit();
        }
    }

    // Painel medieval com as instruções
    void MostrarComoJogar(float centroX, GUIStyle botao)
    {
        float largura = Mathf.Min(680f, Screen.width - 30f);
        float altura = Mathf.Min(510f, Screen.height - 30f);

        Rect painel = new Rect(
            centroX - largura / 2f,
            (Screen.height - altura) / 2f,
            largura,
            altura
        );

        // Fundo escuro e opaco
        DesenharRetangulo(
            painel,
            new Color(0.10f, 0.07f, 0.04f, 1f)
        );

        // Moldura dourada
        DesenharBorda(
            painel,
            5f,
            new Color(0.72f, 0.52f, 0.25f, 1f)
        );

        // Moldura interna
        Rect painelInterno = new Rect(
            painel.x + 10,
            painel.y + 10,
            painel.width - 20,
            painel.height - 20
        );

        DesenharBorda(
            painelInterno,
            2f,
            new Color(0.40f, 0.29f, 0.14f, 1f)
        );

        // Título
        GUIStyle titulo = new GUIStyle(GUI.skin.label);
        titulo.fontSize = 30;
        titulo.fontStyle = FontStyle.Bold;
        titulo.alignment = TextAnchor.MiddleCenter;
        titulo.normal.textColor = new Color(
            1f, 0.83f, 0.48f
        );

        GUI.Label(
            new Rect(
                painel.x + 20,
                painel.y + 25,
                painel.width - 40,
                45
            ),
            "COMO JOGAR",
            titulo
        );

        // Linha dourada abaixo do título
        DesenharRetangulo(
            new Rect(
                painel.x + 50,
                painel.y + 80,
                painel.width - 100,
                2
            ),
            new Color(0.65f, 0.47f, 0.22f, 1f)
        );

        // Instruções
        GUIStyle texto = new GUIStyle(GUI.skin.label);
        texto.fontSize = 19;
        texto.fontStyle = FontStyle.Normal;
        texto.wordWrap = true;
        texto.alignment = TextAnchor.UpperLeft;
        texto.normal.textColor = new Color(
            0.97f, 0.92f, 0.82f
        );

        string instrucoes =
            "OBJETIVO\n" +
            "Defenda o castelo das ondas de inimigos.\n\n" +

            "CONSTRUIR TORRES\n" +
            "Clique em uma das bases de pedra disponíveis no cenário. " +
            "Depois, escolha o tipo de torre que deseja construir: " +
            "Arqueiro, Canhão ou Míssil. " +
            "Cada torre possui um custo em moedas.\n\n" +

            "TIPOS DE TORRE\n" +
            "Arqueiro: rápido e barato.\n" +
            "Canhão: disparos mais fortes.\n" +
            "Míssil: maior poder e alcance.\n\n" +

            "MOEDAS\n" +
            "Cada inimigo derrotado rende moedas.\n\n" +

            "VITÓRIA E DERROTA\n" +
            "Resista a todas as ondas para vencer.\n" +
            "Se o castelo for destruído ou o tempo acabar, você perde.";

        // Área rolável para funcionar também em telas menores
        Rect areaTexto = new Rect(
            painel.x + 35,
            painel.y + 100,
            painel.width - 70,
            painel.height - 190
        );

        float larguraTexto = Mathf.Max(
            100f,
            areaTexto.width - 20f
        );

        float alturaTexto = texto.CalcHeight(
            new GUIContent(instrucoes),
            larguraTexto
        );

        rolagem = GUI.BeginScrollView(
            areaTexto,
            rolagem,
            new Rect(
                0,
                0,
                larguraTexto,
                Mathf.Max(alturaTexto + 10f, areaTexto.height)
            )
        );

        GUI.Label(
            new Rect(0, 0, larguraTexto, alturaTexto),
            instrucoes,
            texto
        );

        GUI.EndScrollView();

        // Botão Voltar com a imagem de madeira
        Rect voltarRect = new Rect(
            centroX - 110,
            painel.yMax - 75,
            220,
            55
        );

        DesenharSprite(voltarRect, madeiraBotao);

        if (GUI.Button(voltarRect, "Voltar", botao))
        {
            mostrandoComoJogar = false;
        }
    }

    // Desenha sprites recortados do Sprite Editor
    void DesenharSprite(Rect destino, Sprite sprite)
    {
        if (sprite == null)
            return;

        Texture2D textura = sprite.texture;
        Rect recorte = sprite.textureRect;

        Rect uv = new Rect(
            recorte.x / textura.width,
            recorte.y / textura.height,
            recorte.width / textura.width,
            recorte.height / textura.height
        );

        GUI.DrawTextureWithTexCoords(
            destino,
            textura,
            uv,
            true
        );
    }

    // Desenha um retângulo de cor sólida
    void DesenharRetangulo(Rect area, Color cor)
    {
        Color corAnterior = GUI.color;

        GUI.color = cor;
        GUI.DrawTexture(area, Texture2D.whiteTexture);

        GUI.color = corAnterior;
    }

    // Desenha as quatro bordas de um painel
    void DesenharBorda(Rect area, float espessura, Color cor)
    {
        DesenharRetangulo(
            new Rect(area.x, area.y, area.width, espessura),
            cor
        );

        DesenharRetangulo(
            new Rect(
                area.x,
                area.yMax - espessura,
                area.width,
                espessura
            ),
            cor
        );

        DesenharRetangulo(
            new Rect(area.x, area.y, espessura, area.height),
            cor
        );

        DesenharRetangulo(
            new Rect(
                area.xMax - espessura,
                area.y,
                espessura,
                area.height
            ),
            cor
        );
    }
}
