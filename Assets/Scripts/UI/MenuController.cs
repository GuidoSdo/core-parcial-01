using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] string gameplayScene = "Gameplay";
    [SerializeField] string mainMenuScene = "MainMenu";

    public void Play() =>
        SceneManager.LoadScene(gameplayScene, LoadSceneMode.Single);

    public void Quit()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene, LoadSceneMode.Single);
    }
}
