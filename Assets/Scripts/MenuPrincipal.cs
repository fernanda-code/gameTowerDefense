using UnityEngine;
using UnityEngine.SceneManagement;

// Menu inicial: botão de jogar, informações sobre o jogo e sair.
public class MenuPrincipal : MonoBehaviour
{
    public string cenaDoJogo = "Fase1";

    private bool mostrandoComoJogar = false;

    void OnGUI()
    {
        float centroX = Screen.width / 2f;

        GUIStyle titulo = new GUIStyle(GUI.skin.label);
        titulo.fontSize = 60;
        titulo.fontStyle = FontStyle.Bold;
        titulo.alignment = TextAnchor.MiddleCenter;
        titulo.normal.textColor = Color.yellow;

        GUIStyle subtitulo = new GUIStyle(titulo);
        subtitulo.fontSize = 22;
        subtitulo.fontStyle = FontStyle.Normal;
        subtitulo.normal.textColor = Color.white;

        GUI.Label(new Rect(0, 50, Screen.width, 80), "KINGDOM DEFENSE", titulo);
        GUI.Label(new Rect(0, 125, Screen.width, 40), "Defenda o castelo das ondas de inimigos!", subtitulo);

        GUIStyle botao = new GUIStyle(GUI.skin.button);
        botao.fontSize = 24;
        botao.fontStyle = FontStyle.Bold;

        if (mostrandoComoJogar)
        {
            GUIStyle texto = new GUIStyle(GUI.skin.label);
            texto.fontSize = 20;
            texto.wordWrap = true;
            texto.normal.textColor = Color.white;

            Rect caixa = new Rect(centroX - 300, 190, 600, 300);
            GUI.Box(caixa, "");
            GUI.Box(caixa, "");
            GUI.Label(new Rect(caixa.x + 20, caixa.y + 15, 560, 280),
                "COMO JOGAR\n\n" +
                "- Os inimigos seguem a estrada até o castelo.\n" +
                "- Escolha uma torre nos botões de baixo e clique numa base de pedra para construir.\n" +
                "- Arqueiro: rápido e barato. Canhão: mais forte. Míssil: muito forte e longe.\n" +
                "- Cada inimigo derrotado dá moedas.\n" +
                "- Vitória: resistir a todas as ondas.\n" +
                "- Derrota: o castelo chegar a 0 ou o tempo acabar.", texto);

            if (GUI.Button(new Rect(centroX - 100, 510, 200, 55), "Voltar", botao))
            {
                mostrandoComoJogar = false;
            }
            return;
        }

        if (GUI.Button(new Rect(centroX - 130, 210, 260, 65), "Jogar", botao))
        {
            SceneManager.LoadScene(cenaDoJogo);
        }

        if (GUI.Button(new Rect(centroX - 130, 290, 260, 65), "Como jogar", botao))
        {
            mostrandoComoJogar = true;
        }

        if (GUI.Button(new Rect(centroX - 130, 370, 260, 65), "Sair", botao))
        {
            Application.Quit();
        }
    }
}
