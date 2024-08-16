using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] DialogueEvent dialogue;

    public Dialogue[] GetDialogue()
    {
        // 딕셔너리에 현재 eventName에 대하여 새로 갱신된 대화 지점의 정보가 있다면, 
        if(GameManager.Instance.DialogueController.dialogueLines.TryGetValue(dialogue.eventName, out Vector2 line))
        {
            // 해당 정보로 dialogue의 line 값을 초기화한다. 
            dialogue.SetNewLine((int)line.x, (int)line.y);
            dialogue.dialogues = GameManager.Instance.CsvParseManager.GetDialogue((int)dialogue.line.x, (int)dialogue.line.y);
        }
        else
        {
            dialogue.dialogues = GameManager.Instance.CsvParseManager.GetDialogue((int)dialogue.line.x, (int)dialogue.line.y);
        }

        return dialogue.dialogues;
    }

    public void UpdateDialogueLines(int x, int y)
    {
        // 새로 갱신되어야 할 대화 지점을 dialogueLines 딕셔너리에 eventName의 키 값으로 저장
        GameManager.Instance.DialogueController.dialogueLines[dialogue.eventName] = new Vector2(x, y);
        dialogue.SetNewLine(x, y);
    }
}
