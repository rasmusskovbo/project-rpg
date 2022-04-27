using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationLoader : MonoBehaviour
{
    void Start()
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        GameManager gameManager = FindObjectOfType<GameManager>();
        
        player.transform.position = gameManager.PlayerData.position;
        player.SetPlayerFacing(gameManager.PlayerData.playerFacingDirection);
    }
    
}
