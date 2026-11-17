using UnityEditor;
using UnityEditor.SceneManagement;

// Só funciona dentro do editor do Unity:
// faz o botão Play sempre começar pela cena do Menu,
// não importa qual cena esteja aberta.
[InitializeOnLoad]
public static class ComecarPeloMenu
{
    static ComecarPeloMenu()
    {
        EditorApplication.delayCall += () =>
        {
            SceneAsset menu = AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets/Scenes/Menu.unity");
            EditorSceneManager.playModeStartScene = menu;
        };
    }
}
