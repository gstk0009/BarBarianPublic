using System.Collections;

//DungeonTutorial_CheckSkilledMonsters 
public class DT_CheckSkilledMonsters : DungeonTutorialBase
{
    public static DT_CheckSkilledMonsters instance;

    public static bool isTutorialing02 = false;
    private bool isDialogueCompleted = false;

    public static int checkMonsterCnt = 0;
    string defalut = "몬스터를 처치하라! ";

    private void Awake()
    {
        instance = this;
    }

    public override void Enter()
    {
        TutorialGudieIMG.SetActive(true);
        tutorialText.text = defalut + $"({checkMonsterCnt}/5)";

        DialogueDetector.InteractiveNPC = DungeonTutorialIntro.icon;
        GameManager.Instance.DialogueManager.GetDialogues(DungeonTutorialIntro.interaction.GetDialogue(),
           DungeonTutorialIntro.interaction);
        GameManager.Instance.DialogueManager.ShowDialogue();
        isTutorialing02 = true;
    }

    public override void Execute(DungeonTutorialController dtc)
    {
        tutorialText.text = defalut + $"({checkMonsterCnt}/5)";

        if (checkMonsterCnt >= 5 && !GameManager.Instance.DialogueManager.IsDialogueActive())
        {
            StartCoroutine(Dialogues(dtc));
        }
    }

    public override void Exit()
    {
    }

    private IEnumerator Dialogues(DungeonTutorialController dtc)
    {
        if (isDialogueCompleted)
            yield break;

        GameManager.Instance.DialogueManager.GetDialogues(DungeonTutorialIntro.interaction.GetDialogue());
        GameManager.Instance.DialogueManager.ShowDialogue();

        while (GameManager.Instance.DialogueManager.IsDialogueActive()) // 대화가 끝날 때까지 대기
        {
            yield return null;
        }

        isDialogueCompleted = true; // 대화 완료 처리
        yield return StartCoroutine(SceneTransitionManager.Instance.FadeController.FadeOut());
        EndTutorial();
        dtc.SetNextTutorial();
    }


    void EndTutorial()
    {
        isTutorialing02 = false;
        isTutorialing = false;
        DataManager.Instance.currentPlayer.tutorialClearInfo[(int)TutorialType.DungeonSystem] = true;
        TutorialGudieIMG.SetActive(false);
    }

    public IEnumerator ShowUnClearDialogue()
    {
        DialogueDetector.InteractiveNPC = DungeonTutorialIntro.icon;
        Dialogue[] dialogues = GameManager.Instance.CsvParseManager.GetDialogue(52, 52);

        GameManager.Instance.DialogueManager.GetDialogues(dialogues, DungeonTutorialIntro.interaction);
        GameManager.Instance.DialogueManager.ShowDialogue();

        while (GameManager.Instance.DialogueManager.IsDialogueActive()) // 대화가 끝날 때까지 대기
        {
            yield return null;
        }
    }
}
