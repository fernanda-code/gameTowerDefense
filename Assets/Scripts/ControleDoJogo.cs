using UnityEngine;
using UnityEngine.SceneManagement;

// Controla a partida na Fase1:
// - conta o tempo máximo (se acabar, é derrota);
// - mostra na tela a vida do castelo, a onda, os inimigos derrotados e o tempo;
// - quando o jogo acaba, mostra a tela de fim com os botões.
// Quem decide a vitória é o EnemySpawner e quem decide o Game Over é o BaseHealth
// (scripts da Fernanda). Este script só pergunta para eles.
public class ControleDoJogo : MonoBehaviour
{
    public static ControleDoJogo instancia;

    [Header("Regras da partida")]
    public float tempoMaximo = 180f;   // segundos (3 minutos)

    [Header("Ligações com a cena")]
    public BaseHealth castelo;
    public EnemySpawner spawner;

    private float tempoRestante;
    private bool acabou = false;
    private bool venceu = false;
    private string mensagem = "";

    void Awake()
    {
        instancia = this;
        Time.timeScale = 1f;   // garante que o jogo não começa pausado
    }

    void Start()
    {
        tempoRestante = tempoMaximo;
    }

    public bool Acabou()
    {
        return acabou;
    }

    void Update()
    {
        if (acabou)
            return;

        // Derrota: o castelo chegou a zero (o BaseHealth avisa pelo IsGameOver)
        if (castelo != null && castelo.IsGameOver)
        {
            Terminar(false, "O castelo foi destruído!");
            return;
        }

        // Vitória: o EnemySpawner diz que todas as ondas foram vencidas
        if (spawner != null && spawner.Venceu())
        {
            Terminar(true, "Você defendeu o castelo!");
            return;
        }

        // Tempo máximo da partida
        tempoRestante = tempoRestante - Time.deltaTime;

        if (tempoRestante <= 0)
        {
            tempoRestante = 0;
            Terminar(false, "O tempo acabou!");
        }
    }

    void Terminar(bool vitoria, string motivo)
    {
        acabou = true;
        venceu = vitoria;
        mensagem = motivo;
        Time.timeScale = 0f;   // pausa o jogo
    }

    void OnGUI()
    {
        // ----- Informações no topo da tela -----
        GUIStyle estilo = new GUIStyle(GUI.skin.label);
        estilo.fontSize = 20;
        estilo.fontStyle = FontStyle.Bold;
        estilo.normal.textColor = Color.white;

        int vida = 0;
        if (castelo != null)
            vida = castelo.VidaAtual();

        string onda = "-";
        int derrotados = 0;
        if (spawner != null)
        {
            onda = spawner.OndaAtual() + "/" + spawner.totalWaves;
            derrotados = spawner.InimigosDerrotados();
        }

        int segundos = Mathf.CeilToInt(tempoRestante);
        string tempo = (segundos / 60) + ":" + (segundos % 60).ToString("00");

        GUI.Box(new Rect(250, 10, 170, 50), "");
        GUI.Label(new Rect(260, 20, 165, 40), "Castelo: " + vida, estilo);

        GUI.Box(new Rect(430, 10, 120, 50), "");
        GUI.Label(new Rect(440, 20, 115, 40), "Onda: " + onda, estilo);

        GUI.Box(new Rect(560, 10, 170, 50), "");
        GUI.Label(new Rect(570, 20, 165, 40), "Derrotados: " + derrotados, estilo);

        GUI.Box(new Rect(740, 10, 150, 50), "");
        GUI.Label(new Rect(750, 20, 145, 40), "Tempo: " + tempo, estilo);

        // ----- Tela de fim de jogo -----
        if (!acabou)
            return;

        float largura = 460;
        float altura = 300;
        Rect painel = new Rect((Screen.width - largura) / 2, (Screen.height - altura) / 2, largura, altura);

        GUI.Box(painel, "");
        GUI.Box(painel, "");   // duas vezes para o fundo ficar mais escuro

        GUIStyle titulo = new GUIStyle(GUI.skin.label);
        titulo.fontSize = 44;
        titulo.fontStyle = FontStyle.Bold;
        titulo.alignment = TextAnchor.MiddleCenter;
        titulo.normal.textColor = venceu ? Color.green : Color.red;

        GUIStyle texto = new GUIStyle(GUI.skin.label);
        texto.fontSize = 22;
        texto.alignment = TextAnchor.MiddleCenter;
        texto.normal.textColor = Color.white;

        GUI.Label(new Rect(painel.x, painel.y + 25, largura, 60), venceu ? "VITÓRIA!" : "DERROTA", titulo);
        GUI.Label(new Rect(painel.x, painel.y + 95, largura, 40), mensagem, texto);

        GUIStyle botao = new GUIStyle(GUI.skin.button);
        botao.fontSize = 20;
        botao.fontStyle = FontStyle.Bold;

        if (GUI.Button(new Rect(painel.x + 40, painel.y + 170, 180, 60), "Jogar de novo", botao))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (GUI.Button(new Rect(painel.x + 240, painel.y + 170, 180, 60), "Menu", botao))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Menu");
        }
    }
}
