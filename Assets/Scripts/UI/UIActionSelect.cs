using UnityEngine.UI;
using UnityEngine;

public class UIActionSelect : MonoBehaviour
{
    [SerializeField] private GameObject skillMenuUI; // can be fetched with FindObject
    [SerializeField] private Button primaryButton;
    
    public void OnAttackSelect()
    {
        skillMenuUI.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SetPrimaryButton()
    {
        primaryButton.Select();
    }
}
