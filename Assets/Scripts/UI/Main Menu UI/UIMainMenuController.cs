using System;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuController : MonoBehaviour
{
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button newGameBtn;
    [SerializeField] private Button settingsBtn;

    private void Start()
    {
        continueBtn.gameObject.SetActive(false);
        
        /*
         * if (gameData.exists) {
         *  enable continueBtn
         * else disable
         */
        
        settingsBtn.gameObject.SetActive(false);
    }
}
