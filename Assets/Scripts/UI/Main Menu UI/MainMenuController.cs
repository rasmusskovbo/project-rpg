using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : PersistentSingleton<MainMenuController>
{
    private bool isNewGame;
    
    public void NewGame()
    {
        isNewGame = true;
        SceneManager.LoadScene(1);
    }
    
    public void ContinueGame()
    {
        isNewGame = false;
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public bool IsNewGame()
    {
        Destroy(this.gameObject);
        return isNewGame;
    }
}
