using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuController : MonoBehaviour
{
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button settingsBtn;
    [SerializeField] private string fileName = "data.game";
    public bool loadGameFound;
    private IFileHandler fileHandler;
    

    private void Start()
    {
        fileHandler = new FileHandler(Application.persistentDataPath, fileName);
        GameData gd = fileHandler.Load();
        loadGameFound = gd != null;
        
        continueBtn.gameObject.SetActive(loadGameFound);
        settingsBtn.gameObject.SetActive(false);
    }
}
