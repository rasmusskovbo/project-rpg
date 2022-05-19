using System;

public class GameEvents : PersistentSingleton<GameEvents>
{
    /*
     * Dialogue
     */
    public event Action onShowDialog;
    public void OnShowDialogInvoke()
    {
        onShowDialog?.Invoke();
    }
    
    public event Action onCloseDialog;
    public void OnCloseDialogInvoke()
    {
        onCloseDialog?.Invoke();
    }
    
    /*
     * Questing
     */
    public event Action onCombatVictory;
    public void CombatVictoryInvoke()
    {
        onCombatVictory?.Invoke();
    }

    // Change to enum for type safety
    public event Action<string> onGatheredTrigger;
    public void GatheredInvoke(string name)
    {
        if (onGatheredTrigger != null)
        {
            onGatheredTrigger(name);
        }
    }
    
    // Change to enum for type safety
    public event Action<string> onInteractedWithTrigger;
    public void InteractedWithTrigger(string name)
    {
        if (onInteractedWithTrigger != null)
        {
            onInteractedWithTrigger(name);
        }
    }
}
