using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject skillMenuUI;
    
    private void Awake()
    {
        skillMenuUI.SetActive(false);
    }
}
