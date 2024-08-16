using UnityEngine;

public class BossDeathHandler : MonoBehaviour
{
    [SerializeField] private GameObject portalPrefab;

    private void OnEnable()
    {
        BossBaseState.OnBossDie += HandleBossDeath;
    }

    private void OnDisable()
    {
        BossBaseState.OnBossDie -= HandleBossDeath;
    }

    private void HandleBossDeath()
    {
        if (portalPrefab != null)
        {
            Dialogue[] dialogues = GameManager.Instance.CsvParseManager.GetDialogue(35, 35);
            GameManager.Instance.DialogueManager.GetDialogues(dialogues);
            GameManager.Instance.DialogueManager.ShowDialogue();

            portalPrefab.SetActive(true);
        }
    }
}
