using UnityEngine;

public class ConsumableManager : PersistentSingleton<ConsumableManager>
{
    /*
     * Currently items are only enabled in exploring mode
     * When enabling items in combat, make sure to change controller calls
     *  (e.g. GameManager for expl.) based on current scene
     */
    public void UseItem(ConsumableItem item)
    {
        switch (item.Type)
        {
            case ConsumableType.HpRecovery:
                GameManager gameManager = FindObjectOfType<GameManager>();
                var currentHp = gameManager.PlayerData.currentHp;
                var maxHp = gameManager.PlayerData.unitBase.MaxHp;

                if ((currentHp + item.Value) > maxHp)
                {
                    gameManager.PlayerData.currentHp = maxHp;
                }
                else
                {
                    currentHp += item.Value;
                    gameManager.PlayerData.currentHp = Mathf.Round(currentHp);
                }

                break;
        }
    }
}
