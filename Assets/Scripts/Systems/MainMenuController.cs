using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void NewGame()
    {
        // Something with data persistence here.
        SceneManager.LoadScene(1);
    }
}
