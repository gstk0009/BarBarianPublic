using System.Collections;
using UnityEngine;

public class DungeonTutorialIntro : DungeonTutorialBase
{
    [SerializeField] GameObject DungeonImg;

    public static InteractionEvent interaction;
    public static DialogueIcon icon;
    BoxCollider2D boxCollider;
    private bool isComplete = false;

    private void Start()
    {
        interaction = GetComponent<InteractionEvent>();
        icon = GetComponent<DialogueIcon>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Player.Instance.isPlayerInteracting = true;
            StartCoroutine(DungeonIntro());
        }
    }

    private IEnumerator DungeonIntro()
    {
        DialogueDetector.InteractiveNPC = icon;
        TutorialManager.Instance.SetUI(false);

        yield return StartCoroutine(SceneTransitionManager.Instance.FadeController.FadeOut());
        DungeonImg.SetActive(true);
        yield return StartCoroutine(SceneTransitionManager.Instance.FadeController.FadeIn());

        GameManager.Instance.DialogueManager.GetDialogues(interaction.GetDialogue(), interaction);
        GameManager.Instance.DialogueManager.ShowDialogue();

        while (GameManager.Instance.DialogueManager.IsDialogueActive()) // 대화가 끝날 때까지 대기.
        {
            yield return null;
        }

        yield return StartCoroutine(SceneTransitionManager.Instance.FadeController.FadeOut());
        EndTutorialIntro();
        yield return StartCoroutine(SceneTransitionManager.Instance.FadeController.FadeIn());
        isComplete = true;
    }

    void EndTutorialIntro()
    {
        TutorialManager.Instance.SetUI(true);
        DungeonImg.SetActive(false);

        Player.Instance.isPlayerInteracting = false;
    }

    public override void Execute(DungeonTutorialController dtc)
    {
        if (isComplete)
            dtc.SetNextTutorial();
    }
    public override void Enter()
    {
    }
    public override void Exit()
    {
        boxCollider.enabled = false;
    }
}