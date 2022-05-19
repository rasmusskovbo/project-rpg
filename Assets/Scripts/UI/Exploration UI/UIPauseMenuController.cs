using UnityEngine;
using UnityEngine.UI;

public class UIPauseMenuController : MonoBehaviour
{
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button saveGameButton;
    [SerializeField] private Button exitGameButton;

    private void Start()
    {
        settingsButton.gameObject.SetActive(false);
        saveGameButton.onClick.AddListener(() => SaveGame());
        exitGameButton.onClick.AddListener(() => ExitGame());
    }

    private void SaveGame()
    {
        FindObjectOfType<DataPersistenceManager>().SaveGame();
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
