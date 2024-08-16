using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject DialogueUI;
    [SerializeField] GameObject SelectionUI;

    [SerializeField] Image faceIcon;
    string previousSpeakedNPC = null;

    [SerializeField] TextMeshProUGUI textDialogue;
    [SerializeField] TextMeshProUGUI textName;

    [SerializeField] TextMeshProUGUI[] selectionText;
    [SerializeField] Button[] selectionButtons;

    [SerializeField] float textDelay;
    WaitForSecondsRealtime wfs_textDelay;
    Dialogue[] dialogues;
    Selections[] selections;

    public static bool isDialogue = false;
    bool isNext = false;

    // 하기의 조건이 true일 때, space 키가 눌리면 안됨. 
    bool isTyping = false; // 현재 코루틴을 통해 대사가 출력되는 중인지
    bool isSelectionActive = false; // 현재 선택지 UI가 활성화된 상태인지

    int lineCount = 0;
    int contextCount = 0;
    static public int skipDialogueNum = -1;
    InteractionEvent currentInteraction;

    private void Awake()
    {
        if (GameManager.Instance.DialogueManager != null) return;

        GameManager.Instance.DialogueManager = this;

        SelectionUI.SetActive(false);
        wfs_textDelay = new WaitForSecondsRealtime(textDelay);
    }

    private void Update()
    {
        if (isDialogue)
        {
            Player.Instance.isPlayerInteracting = true;

            if (isNext && !isTyping && !isSelectionActive)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isNext = false;
                    textDialogue.text = "";

                    if (lineCount >= dialogues.Length)
                    {
                        EndDialogue();
                        return;
                    }

                    if (contextCount < dialogues[lineCount].context.Length - 1)
                    {
                        contextCount++;
                        StartCoroutine(TypeDialogue());
                    }
                    else
                    {
                        contextCount = 0;
                        lineCount++;
                        if (lineCount < dialogues.Length)
                        {
                            StartCoroutine(TypeDialogue());
                        }
                        else // 현재 대화가 끝났을 때,
                        {
                            // 다음 대화 지점을 갱신
                            if (currentInteraction != null && dialogues[lineCount - 1].newLineX > 0 && dialogues[lineCount - 1].newLineY > 0)
                            {
                                currentInteraction.UpdateDialogueLines(dialogues[lineCount - 1].newLineX, dialogues[lineCount - 1].newLineY);
                            }

                            // 선택지 이벤트가 있다면 UI를 화면에 출력
                            if (dialogues[lineCount - 1].eventNumber > 0)
                            {
                                isSelectionActive = true;
                                ShowOptionsUI(dialogues[lineCount - 1].eventNumber);
                            }
                            else
                            {
                                EndDialogue();
                            }
                        }
                    }
                }
            }
        }
    }

    private void EndDialogue()
    {
        isDialogue = false;
        contextCount = 0;
        lineCount = 0;
        dialogues = null;
        isNext = false;
        isTyping = false;  // 대화가 끝났으므로 플래그 해제
        isSelectionActive = false; // 선택지 플래그도 초기화

        Player.Instance.isPlayerInteracting = false;
        SettingUI(false);
    }

    public void ShowDialogue()
    {
        isDialogue = true;

        textDialogue.text = "";
        textName.text = "";

        StartCoroutine(TypeDialogue());
    }

    void SettingUI(bool isActive)
    {
        DialogueUI.SetActive(isActive);
    }

    public void GetDialogues(Dialogue[] dialogues, InteractionEvent interaction = null)
    {
        this.dialogues = dialogues;
        currentInteraction = interaction; // 현재 상호작용하는 NPC를 저장
    }

    void ShowOptionsUI(int eventNum)
    {
        selections = GameManager.Instance.CsvParseManager.GetSelections(eventNum);

        if (selections.Length == 0) return;

        SelectionUI.SetActive(true);

        int selectedNumber = Mathf.Min(selectionText.Length, selections.Length);

        for (int i = 0; i < selectionText.Length; i++)
        {
            selectionText[i].transform.parent.gameObject.SetActive(false);
            selectionButtons[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < selectedNumber; i++)
        {
            selectionText[i].transform.parent.gameObject.SetActive(true);
            selectionText[i].text = selections[i].Option;
            selectionText[i].gameObject.SetActive(true);

            selectionButtons[i].onClick.RemoveAllListeners();
            selectionButtons[i].onClick.AddListener(() => OnChoiceSelected(i));
        }
    }

    public void OnChoiceSelected(int idx)
    {
        SelectionUI.SetActive(false);

        if (idx >= 0 && idx < selections.Length)
        {
            int x = selections[idx].NextLineX;
            int y = selections[idx].NextLineY;
            string methodName = selections[idx].MethodName;

            if (y == 0 || x == 0)
            {
                // 대화를 끝내고 메서드만 실행 → 이 경우에는 딜레이 없이 바로 켜지도록 
                if (!string.IsNullOrEmpty(methodName))
                {
                    EndDialogue();
                    GameManager.Instance.SelectionActions.CallMethod(methodName, 0f);
                }
            }
            else
            {
                dialogues = GameManager.Instance.CsvParseManager.GetDialogue(x, y);
                if (dialogues.Length == 0 || dialogues[0].context.Length == 0)
                {
                    ResetToIntiDialogue();
                }
                else
                {
                    lineCount = 0;
                    contextCount = 0;
                    ShowDialogue();

                    if (!string.IsNullOrEmpty(methodName))
                    {
                        GameManager.Instance.SelectionActions.CallMethod(methodName);
                    }
                }
            }
        }
        else
        {
            ResetToIntiDialogue();
        }

        isSelectionActive = false;
    }

    IEnumerator TypeDialogue()
    {
        if (!DialogueUI.activeInHierarchy)
            SettingUI(true);

        isTyping = true; 

        // 스킵할 대사 ID가 있는지 확인
        if (skipDialogueNum > 0 && skipDialogueNum == dialogues[lineCount].ID)
        {
            skipDialogueNum = -1;  // 스킵 후 초기화
            contextCount = 0; // 스킵 후 contextCount를 초기화

            if (currentInteraction != null && dialogues[lineCount - 1].newLineX > 0 && dialogues[lineCount - 1].newLineY > 0)
            {
                currentInteraction.UpdateDialogueLines(dialogues[lineCount - 1].newLineX, dialogues[lineCount - 1].newLineY);
            }

            lineCount++;

            if (lineCount >= dialogues.Length) // 다음 출력할 문장이 있으면 실행, 없으면 그대로 대사 종료 
            {
                EndDialogue();
                yield break;
            }
        }

        if (dialogues == null || dialogues.Length == 0 || dialogues[lineCount] == null ||
            dialogues[lineCount].context == null || dialogues[lineCount].context.Length == 0)
        {
            EndDialogue();
            yield break;
        }

        string replaceText = dialogues[lineCount].context[contextCount];
        replaceText = replaceText.Replace("'", ",");
        replaceText = replaceText.Replace("-", "\n");
        replaceText = replaceText.Replace("ⓖ", "<color=#919191>");
        replaceText = replaceText.Replace("ⓦ", "<color=#ffffff>");
        replaceText = replaceText.Replace("@", DataManager.Instance.currentPlayer.name);

        if (GameManager.Instance.TaxManager != null)
            replaceText = replaceText.Replace("$", GameManager.Instance.TaxManager.TaxPrice().ToString());
        string name = dialogues[lineCount].name;

        if (previousSpeakedNPC != name)
        {
            if (Player.Instance.playerDialogueIcon?.speakerSO?.speaker != null && Player.Instance.playerDialogueIcon.speakerSO.speaker.CompareTo(name) == 0)
            {
                faceIcon.sprite = Player.Instance.playerDialogueIcon.speakerSO.faceIcon;
            }
            else if (DialogueDetector.InteractiveNPC?.speakerSO?.speaker != null && DialogueDetector.InteractiveNPC.speakerSO.speaker.CompareTo(name) == 0)
            {
                faceIcon.sprite = DialogueDetector.InteractiveNPC.speakerSO.faceIcon;
            }
            previousSpeakedNPC = name;
        }

        name = name.Replace("ⓦ", "<color=#ffffff>");
        name = name.Replace("ⓑ", "<color=#6CF6FF>");
        name = name.Replace("@", DataManager.Instance.currentPlayer.name);
        name = name + "</color>";

        textName.text = name;

        int i = 0;
        while (i < replaceText.Length)
        {
            // 태그를 인식하고 한 번에 출력
            if (replaceText[i] == '<')
            {
                int tagEndIndex = replaceText.IndexOf('>', i);
                if (tagEndIndex != -1)
                {
                    // 태그를 한 번에 출력
                    textDialogue.text += replaceText.Substring(i, tagEndIndex - i + 1);
                    i = tagEndIndex + 1;
                    continue;
                }
            }

            SoundManager.Instance.PlaySoundEffect((int)SoundEffects.TypeDialogue, 0.1f);

            // 일반 문자는 한 글자씩 출력
            textDialogue.text += replaceText[i];
            i++;
            yield return wfs_textDelay;
        }

        textDialogue.text += "\n<color=#959595><size=26>▼ [Space] </color>";
        isNext = true;

        yield return new WaitForSeconds(.3f);
        isTyping = false; 
    }

    public void ResetToIntiDialogue()
    {
        lineCount = 0;
        contextCount = 0;
        isNext = false;
        StopAllCoroutines();
        ShowDialogue();
    }

    public void SetInitialDialogue(int initialID)
    {
        dialogues = GameManager.Instance.CsvParseManager.GetDialogue(initialID, initialID);
        lineCount = 0;
        contextCount = 0;
        isNext = false;
        StopAllCoroutines();
        ShowDialogue();
    }

    public bool IsDialogueActive()
    {
        return isDialogue;
    }
}
