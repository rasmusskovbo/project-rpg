using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationLoader : MonoBehaviour
{
    void Start()
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        GameManager dataManager = FindObjectOfType<GameManager>();
        
        player.transform.position = dataManager.GetPlayerPosition();
        player.SetPlayerFacing(dataManager.PlayerData.PlayerFacingDirection);
    }
    
}
