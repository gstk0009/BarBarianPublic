using UnityEngine;

public class PortalsController : MonoBehaviour
{
    [SerializeField] PortalType portalType;
    [SerializeField] bool isFromScaffold;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !DungeonTutorialBase.isTutorialing)
        {
            Player.Instance.isPlayerInteracting = true;

            GameManager.Instance.MoveStageController.MoveToNextStage(portalType);
        }
    }
}
