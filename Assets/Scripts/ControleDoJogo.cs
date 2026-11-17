using UnityEngine;
using UnityEngine.SceneManagement;

// Controla a partida: tempo máximo, vitória, derrota
// e mostra na tela a vida do castelo, a onda e o tempo.
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

        // Conta o tempo para trás
        tempoRestante = tempoRestante - Time.deltaTime;

        if (tempoRestante <= 0)
        {
            tempoRestante = 0;
            Derrota("O tempo acabou!");
            return;
        }

        // Vitória: todas as ondas já saíram e não sobrou nenhum inimigo vivo
        if (spawner != null && spawner.terminou)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                Vitoria();
            }
        }
    }

    // O castelo chama isto quando a vida dele chega a zero
    public void Derrota(string motivo)
    {
        if (acabou)
            return;

        acabou = true;
        venceu = false;
        mensagem = motivo;
        Time.timeScale = 0f;   // pausa o jogo
    }

    void Vitoria()
    {
        acabou = true;
        venceu = true;
        mensagem = "Você defendeu o castelo!";
        Time.timeScale = 0f;
    }

    void OnGUI()
    {
        // ----- Informações no topo da tela -----
        GUIStyle estilo = new GUIStyle(GUI.skin.label);
        estilo.fontSize = 22;
        estilo.fontStyle = FontStyle.Bold;
        estilo.normal.textColor = Color.white;

        int vida = 0;
        if (castelo != null)
            vida = castelo.VidaAtual();

        string onda = "-";
        if (spawner != null)
            onda = spawner.OndaAtual() + " / " + spawner.totalWaves;

        int segundos = Mathf.CeilToInt(tempoRestante);
        string tempo = (segundos / 60) + ":" + (segundos % 60).ToString("00");

        GUI.Box(new Rect(250, 10, 200, 50), "");
        GUI.Label(new Rect(262, 18, 190, 40), "Castelo: " + vida, estilo);

        GUI.Box(new Rect(460, 10, 160, 50), "");
        GUI.Label(new Rect(472, 18, 150, 40), "Onda: " + onda, estilo);

        GUI.Box(new Rect(630, 10, 150, 50), "");
        GUI.Label(new Rect(642, 18, 140, 40), "Tempo: " + tempo, estilo);

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
