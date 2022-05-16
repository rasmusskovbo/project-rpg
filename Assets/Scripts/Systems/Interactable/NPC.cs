using System;
using UnityEngine;

[Serializable]
public class NPC : MonoBehaviour, Interactable
{
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private Quest quest;
    [SerializeField] private bool questHasBeenCompleted;
    
    public void Interact()
    {
        // Add this to milanote board
        // Change references to singletons to use static method instead of "expensive" findobject of type
        // Esp places where we do not cache the ref.
        if (!questHasBeenCompleted)
        {
            DialogueManager.Instance.ShowDialog(dialogue);
            QuestManager.Instance.AddQuest(quest);
        }
        else
        {
            // Play outro dialogue or sth here
            questHasBeenCompleted = true;
            QuestManager.Instance.CompleteQuest(quest);
        }

    }
}
