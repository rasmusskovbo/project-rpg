using UnityEngine;

public class MainMenuController : PersistentSingleton<MainMenuController>
{
    private bool isNewGame;
    
    public void NewGame()
    {
        isNewGame = true;
        StartGame();
    }
    
    public void ContinueGame()
    {
        isNewGame = false;
        StartGame();
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

    public void StartGame()
    {
        FindObjectOfType<SceneTransition>().LoadScene(SceneIndexType.Exploration);
    }
}
