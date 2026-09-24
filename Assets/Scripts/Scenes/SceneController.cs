using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene("hola");
    }

    public void Search()
    {
        SceneManager.LoadScene("Expedicion");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}