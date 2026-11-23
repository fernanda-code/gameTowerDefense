using UnityEngine;

// Loja de torres: mostra as moedas e, quando o jogador clica numa base de pedra,
// abre um menu do lado da base para escolher qual torre construir.
public class Loja : MonoBehaviour
{
    // Atalho para outros scripts acharem a loja (ex.: Loja.GanharMoedas(25))
    public static Loja instancia;

    public int moedas = 200;

    [Header("Torres à venda (mesma ordem nas três listas)")]
    public string[] nomes = { "Arqueiro", "Canhão", "Míssil" };
    public GameObject[] torres;
    public int[] precos = { 50, 100, 150 };

    private PontoDeTorre pontoEscolhido;   // a base que o jogador clicou
    private string aviso = "";
    private float tempoAviso = 0f;

    void Awake()
    {
        instancia = this;
    }

    // Os inimigos chamam isto quando morrem
    public static void GanharMoedas(int quantidade)
    {
        if (instancia != null)
        {
            instancia.moedas = instancia.moedas + quantidade;
        }
    }

    void Update()
    {
        if (tempoAviso > 0)
        {
            tempoAviso = tempoAviso - Time.deltaTime;
        }
    }

    // OnGUI desenha a interface da loja na tela
    void OnGUI()
    {
        // Quando a partida acaba, a loja some
        if (ControleDoJogo.instancia != null && ControleDoJogo.instancia.Acabou())
            return;

        // ----- Moedas no canto de cima -----
        GUIStyle estiloMoedas = new GUIStyle(GUI.skin.label);
        estiloMoedas.fontSize = 26;
        estiloMoedas.fontStyle = FontStyle.Bold;
        estiloMoedas.normal.textColor = Color.yellow;

        GUI.Box(new Rect(10, 10, 230, 50), "");
        GUI.Label(new Rect(22, 16, 220, 40), "Moedas: " + moedas, estiloMoedas);

        // ----- Aviso (ex.: moedas insuficientes) -----
        if (tempoAviso > 0)
        {
            GUIStyle estiloAviso = new GUIStyle(GUI.skin.label);
            estiloAviso.fontSize = 24;
            estiloAviso.fontStyle = FontStyle.Bold;
            estiloAviso.normal.textColor = Color.red;
            GUI.Label(new Rect(10, Screen.height - 90, 600, 40), aviso, estiloAviso);
        }

        // ----- Menu de escolha da torre (aparece do lado da base clicada) -----
        if (pontoEscolhido != null)
        {
            DesenharMenuDeTorres();
        }

        // ----- Clique no mapa -----
        Event evento = Event.current;
        if (evento.type == EventType.MouseDown && evento.button == 0)
        {
            // A tela do OnGUI tem o Y invertido, por isso o "Screen.height - y"
            Vector2 posicaoTela = new Vector2(evento.mousePosition.x, Screen.height - evento.mousePosition.y);
            Vector2 posicaoMundo = Camera.main.ScreenToWorldPoint(posicaoTela);

            PontoDeTorre ponto = AcharPonto(posicaoMundo);

            if (ponto == null)
            {
                pontoEscolhido = null;           // clicou fora: fecha o menu
            }
            else if (ponto.ocupado)
            {
                Avisar("Já tem uma torre aqui!");
                pontoEscolhido = null;
            }
            else
            {
                pontoEscolhido = ponto;          // abre o menu nessa base
                evento.Use();
            }
        }
    }

    void DesenharMenuDeTorres()
    {
        // Converte a posição da base (no mundo) para a posição na tela
        Vector3 posicaoNaTela = Camera.main.WorldToScreenPoint(pontoEscolhido.transform.position);

        float largura = 200;
        float altura = 45 + torres.Length * 55 + 50;
        float x = posicaoNaTela.x + 40;
        float y = (Screen.height - posicaoNaTela.y) - altura / 2;

        // Não deixa o menu sair da tela
        x = Mathf.Clamp(x, 10, Screen.width - largura - 10);
        y = Mathf.Clamp(y, 70, Screen.height - altura - 10);

        GUI.Box(new Rect(x, y, largura, altura), "");
        GUI.Box(new Rect(x, y, largura, altura), "");   // duas vezes para ficar mais escuro

        GUIStyle titulo = new GUIStyle(GUI.skin.label);
        titulo.fontSize = 20;
        titulo.fontStyle = FontStyle.Bold;
        titulo.alignment = TextAnchor.MiddleCenter;
        titulo.normal.textColor = Color.yellow;
        GUI.Label(new Rect(x, y + 5, largura, 35), "Escolha a torre", titulo);

        GUIStyle botao = new GUIStyle(GUI.skin.button);
        botao.fontSize = 17;
        botao.fontStyle = FontStyle.Bold;

        for (int i = 0; i < torres.Length; i++)
        {
            Rect lugar = new Rect(x + 10, y + 45 + i * 55, largura - 20, 50);

            // Torre cara demais aparece apagada
            GUI.enabled = moedas >= precos[i];

            if (GUI.Button(lugar, nomes[i] + "  (" + precos[i] + ")", botao))
            {
                Construir(i);
            }
        }
        GUI.enabled = true;

        if (GUI.Button(new Rect(x + 10, y + 45 + torres.Length * 55, largura - 20, 40), "Cancelar", botao))
        {
            pontoEscolhido = null;
        }
    }

    // Procura uma base de pedra perto de onde o jogador clicou
    PontoDeTorre AcharPonto(Vector2 posicao)
    {
        PontoDeTorre[] pontos = FindObjectsByType<PontoDeTorre>(FindObjectsSortMode.None);

        foreach (PontoDeTorre ponto in pontos)
        {
            if (Vector2.Distance(posicao, ponto.transform.position) < 0.6f)
                return ponto;
        }
        return null;
    }

    void Construir(int qualTorre)
    {
        if (moedas < precos[qualTorre])
        {
            Avisar("Moedas insuficientes!");
            return;
        }

        moedas = moedas - precos[qualTorre];
        pontoEscolhido.Construir(torres[qualTorre]);
        pontoEscolhido = null;   // fecha o menu
    }

    void Avisar(string mensagem)
    {
        aviso = mensagem;
        tempoAviso = 2f;
    }
}
