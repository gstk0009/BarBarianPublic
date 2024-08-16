using System.Collections.Generic;
using UnityEngine;

public class CsvParseManager : MonoBehaviour
{
    [SerializeField] string csv_FileName; // 대화 파일
    [SerializeField] string csv_SelectionFileName; // 선택지 파일

    Dictionary<int, Dialogue> dialogueDic = new Dictionary<int, Dialogue>();
    Dictionary<int, List<Selections>> eventDic = new Dictionary<int, List<Selections>>();

    public static bool isFinish = false;

    private void Awake()
    {
        if (GameManager.Instance.CsvParseManager != null) return;

        GameManager.Instance.CsvParseManager = this;
        SetDialogueDictionary();
        SetEventDictionary();
    }

    void SetDialogueDictionary()
    {
        DialogueParser parser = GetComponent<DialogueParser>();
        if (parser == null)
        {
            return;
        }
        Dialogue[] dialogues = parser.Parse(csv_FileName);

        for (int i = 0; i < dialogues.Length; i++)
        {
            dialogueDic.Add(i + 1, dialogues[i]);
        }

        isFinish = true;
    }

   

    void SetEventDictionary()
    {
        EventParser parser = GetComponent<EventParser>();
        
        if (parser == null)
        {
            return;
        }

        Selections[] selectOptions = parser.Parse(csv_SelectionFileName);

        foreach (var selection in selectOptions)
        {
            if (!eventDic.ContainsKey(selection.ID))
            {
                eventDic[selection.ID] = new List<Selections>();
            }

            eventDic[selection.ID].Add(selection);
        }
    }

    public Dialogue[] GetDialogue(int startNum, int endNum)
    {
        List<Dialogue> dialogueList = new List<Dialogue>();

        for (int i = startNum; i <= endNum; i++)
        {
            if (dialogueDic.ContainsKey(i))
            {
                dialogueList.Add(dialogueDic[i]);
            }
        }
        return dialogueList.ToArray();
    }

   
    public Selections[] GetSelections(int eventNum)
    {
        if (eventDic.ContainsKey(eventNum))
        {
            List<Selections> selections = eventDic[eventNum];
            for (int i = 0; i < selections.Count; i++)
            {
                string replaceText = selections[i].Option;
                replaceText = replaceText.Replace("'", ",");
                replaceText = replaceText.Replace("-", "\n");
                replaceText = replaceText.Replace("ⓖ", "<color=#919191>");
                replaceText = replaceText.Replace("ⓦ", "<color=#ffffff>");

                selections[i].Option = replaceText;
            }
            return selections.ToArray();
        }
        return new Selections[0];
    }
}
