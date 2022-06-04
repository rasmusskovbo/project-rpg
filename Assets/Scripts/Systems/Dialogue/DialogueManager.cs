using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject nextLineIndicator;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private int lettersPrSecond = 50;

    private int currentLine = 0;
    private Dialogue currentDialogue;
    private bool isPrintingText;

    private void Start()
    {
        dialogueBox.SetActive(false);
    }

    public void ShowDialog(Dialogue dialogue)
    {
        FindObjectOfType<GameEvents>().OnShowDialogInvoke();
        
        dialogueBox.SetActive(true);
        currentDialogue = dialogue;
        StartCoroutine(AnimateDialogue(dialogue.Lines[0]));
    }

    public IEnumerator AnimateDialogue(string line)
    {
        isPrintingText = true;
        nextLineIndicator.SetActive(false);

        dialogueText.text = "";
        foreach (var c in line.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(1f / lettersPrSecond);
        }

        nextLineIndicator.SetActive(true);
        isPrintingText = false;
    }

    public void NextLine()
    {
        if (isPrintingText) return; //Alternatively print all text at once to make it skippable
        
        currentLine++;
        if (currentLine < currentDialogue.Lines.Count)
        {
            StartCoroutine(AnimateDialogue(currentDialogue.Lines[currentLine]));
        }
        else
        {
            currentLine = 0;
            dialogueBox.SetActive(false);
            FindObjectOfType<GameEvents>().OnCloseDialogInvoke();
        }
    }
}
