using UnityEngine;

public class DialogueDetector : MonoBehaviour
{
    public LayerMask detectLayer;
    [SerializeField] float detectRange = 1f;
    private Vector2 overlapCirclePosition;

    // 범위 내에 감지된 interactive object
    public static InteractionEvent detectedResult;
    public static DialogueIcon InteractiveNPC;
    private void Update()
    {
        overlapCirclePosition = new Vector2(transform.position.x, transform.position.y + 0.15f);
        var detectedObj = Physics2D.OverlapCircle(overlapCirclePosition, detectRange, detectLayer);

        if (detectedObj != null )
        {
            int obejctLayer = detectedObj.gameObject.layer;

            detectedResult = detectedObj.GetComponent<InteractionEvent>();
            InteractiveNPC = detectedObj.GetComponent<DialogueIcon>();

            if (obejctLayer == (int)LayerType.Interaction_NPC)
            {
                GameManager.Instance.DialogueController.ShowDialogueWithNPC(detectedResult);
                GameManager.Instance.DialogueController.UpdateInteractionUI(true, InteractiveNPC.speakerSO.speaker, true);
            }
            else if (obejctLayer == (int)LayerType.Interaction_Obj)
            {
                GameManager.Instance.DialogueController.ShowDialogueWithObj(detectedResult);
                GameManager.Instance.DialogueController.UpdateInteractionUI(true, InteractiveNPC.speakerSO.speaker);
            }
        }
        else
        {
            if (GameManager.Instance.DialogueController != null)
            {
                if (GameManager.Instance.DialogueController.interactionUI.gameObject.activeSelf
                    && !DialogueController.isDialogueInteracting)
                {
                    GameManager.Instance.DialogueController.UpdateInteractionUI(false);
                }
            }
        }

        // NPC가 범위를 벗어나면 상호작용 상태 해제
        if (detectedObj == null && DialogueController.isDialogueInteracting)
        {
            DialogueController.isDialogueInteracting = false;
        }
    }

}
