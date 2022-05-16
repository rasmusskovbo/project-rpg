using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Transform questGoalContainer;
    [SerializeField] private GameObject questCompletedIndicator;
    [SerializeField] private Button abandonQuestButton;
    [SerializeField] private GameObject questGoalPrefab;

    public void Init(Quest quest)
    {
        questCompletedIndicator.SetActive(quest.IsQuestComplete());
        title.text = quest.info.Name;
        quest.Goals.ForEach(goal => InitQuestGoal(goal));
    }

    public void AssignRemoveCallback(Action onClickCallback)
    {
       abandonQuestButton.onClick.AddListener(() => onClickCallback()); 
    }

    private void InitQuestGoal(QuestGoal goal)
    {
        var questGoal = Instantiate(questGoalPrefab, questGoalContainer);
        var texts = questGoal.GetComponentsInChildren<TextMeshProUGUI>();
        texts[1].text = goal.IsComplete() ? "<s>goal.Description</s>" : goal.Description;
        texts[2].text = $"{goal.CurrentAmount}/{goal.RequiredAmount}";
    }

    public void UpdateProgressOnUI(Quest quest)
    {
        questCompletedIndicator.SetActive(quest.IsQuestComplete());
        for (int i = 0; i < questGoalContainer.childCount; i++)
        {
            var texts = questGoalContainer.transform.GetChild(i)
                .GetComponentsInChildren<TextMeshProUGUI>();
            
            texts[2].text = $"{quest.Goals[i].CurrentAmount}/{quest.Goals[i].RequiredAmount}";
        }
    }
    
}
