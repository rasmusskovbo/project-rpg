using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationLoader : MonoBehaviour
{
    void Start()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        GameManager gameManager = FindObjectOfType<GameManager>();

        player.transform.position = gameManager.PlayerData.position;
        player.SetPlayerFacing(gameManager.PlayerData.playerFacingDirection);
    }
    
}
