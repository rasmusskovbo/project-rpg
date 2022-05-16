using System;
using UnityEngine;

[Serializable]
public class NPC : MonoBehaviour, Interactable
{
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private Quest quest;
    
    public void Interact()
    {
        // Add this to milanote board
        // Change references to singletons to use static method instead of "expensive" findobject of type
        // Esp places where we do not cache the ref.
        DialogueManager.Instance.ShowDialog(dialogue);
        QuestManager.Instance.AddQuest(quest);
    }
}
