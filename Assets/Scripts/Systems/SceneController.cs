using System;
using UnityEngine.SceneManagement;

public class SceneController : PersistentSingleton<SceneController>
{
    public void NewGame()
    {
        // Something with data persistence here.
        SceneManager.LoadScene(0);
    }
}
