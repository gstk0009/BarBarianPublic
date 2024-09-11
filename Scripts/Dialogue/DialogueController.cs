using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI interactionUI;
    public Dictionary<string, Vector2> dialogueLines = new Dictionary<string, Vector2>();

    InteractionEvent detectedObj;
    public static bool isDialogueInteracting = false;

    private void Start()
    {
        if (GameManager.Instance.DialogueController != null) return;

        GameManager.Instance.DialogueController = this;
    }

    // 탐지된 npc와의 대화를 출력해주는 메서드
    public void ShowDialogueWithDetectedObj(InteractionEvent interactionEvent)
    {
        detectedObj = DialogueDetector.detectedResult;

        if(!DialogueManager.isDialogue && !Player.Instance.isPlayerInteracting)
        {
            if (Input.GetKeyDown(KeyCode.F)) // F를 눌러야 실행
            {
                interactionUI.gameObject.SetActive(false);
                isDialogueInteracting = true;
                GameManager.Instance.DialogueManager.GetDialogues(detectedObj.GetDialogue(), interactionEvent);
                GameManager.Instance.DialogueManager.ShowDialogue();
            }
        }
    }

    public void SetSavedDialogues()
    {
        for (int i = 0; i < DataManager.Instance.currentPlayer.keys.Count; i++)
        {
            dialogueLines[DataManager.Instance.currentPlayer.keys[i]]
                = DataManager.Instance.currentPlayer.lines[i];
        }
    }
    public void UpdateInteractionUI(bool show, string speakerName = "",bool isNpc = false)
    {
        if(Player.Instance.isPlayerInteracting || DialogueManager.isDialogue)
        {
            interactionUI.gameObject.SetActive(false); return; 
        }

        if (show)
        {
            if(isNpc)
            {
                interactionUI.text = $"[F] 키를 눌러 <color=#0011FF>{speakerName}</color>와(과) 대화하기";
            }
            else 
            { 
                interactionUI.text = $"[F] 키를 눌러 <color=#0011FF>{speakerName}</color>을(를) 조사하기";
            }

            interactionUI.gameObject.SetActive(true);
        }
        else
        {
            interactionUI.gameObject.SetActive(false);
        }
    }
}
