using System.Collections;
using UnityEngine;

public class VillageSystemTutorial : MonoBehaviour
{
    [SerializeField] GameObject Image;
    [SerializeField] TutorialType tutorialType;

    InteractionEvent interaction;
    DialogueIcon icon;

    private void Start()
    {

        interaction = GetComponent<InteractionEvent>();
        icon = GetComponent<DialogueIcon>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Player.Instance.isPlayerInteracting = true;

            StartCoroutine(TaxTutorial());
        }
    }

    private IEnumerator TaxTutorial()
    {
        DialogueDetector.InteractiveNPC = icon;

        TutorialManager.Instance.SetUI(false);

        yield return StartCoroutine(SceneTransitionManager.Instance.FadeController.FadeOut());
        Image.SetActive(true);
        yield return StartCoroutine(SceneTransitionManager.Instance.FadeController.FadeIn());


        GameManager.Instance.DialogueManager.GetDialogues(interaction.GetDialogue());
        GameManager.Instance.DialogueManager.ShowDialogue();

        while (GameManager.Instance.DialogueManager.IsDialogueActive()) // 대화가 끝날 때까지 대기.
        {
            yield return null;
        }

        yield return StartCoroutine(SceneTransitionManager.Instance.FadeController.FadeOut());
        EndTutorial();
        yield return StartCoroutine(SceneTransitionManager.Instance.FadeController.FadeIn());
        this.gameObject.SetActive(false);

    }

    void EndTutorial()
    {
        TutorialManager.Instance.SetUI(true);
        Image.SetActive(false);
        DataManager.Instance.currentPlayer.tutorialClearInfo[(int)tutorialType] = true;
        Player.Instance.isPlayerInteracting = false;
    }
}