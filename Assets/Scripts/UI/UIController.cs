using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject skillMenuUI;
    private CombatAction activeAction;
    
    private void Awake()
    {
        skillMenuUI.SetActive(false);
    }

    public void SetActiveAction(CombatAction action)
    {
        this.activeAction = action;
    }

    public CombatAction GetActiveAction()
    {
        return activeAction;
    }
}
