using System.Collections;


//DungeonTutorial_CheckAttackState
public class DT_CheckAttackState : DungeonTutorialBase
{
    public static bool isTutorialing01 = false;

    public static int checkAttackState = 0;
    string defalut = "공격 상태로 전환하라! ";
    public override void Enter()
    {
        isTutorialing = true;

        TutorialGudieIMG.SetActive(true);
        tutorialText.text = defalut + $"({checkAttackState}/3)";

        DialogueDetector.InteractiveNPC = DungeonTutorialIntro.icon;
        GameManager.Instance.DialogueManager
            .GetDialogues(DungeonTutorialIntro.interaction.GetDialogue(), DungeonTutorialIntro.interaction);
        GameManager.Instance.DialogueManager.ShowDialogue();
        isTutorialing01 = true;
    }

    public override void Execute(DungeonTutorialController dtc)
    {
        tutorialText.text = defalut + $"({checkAttackState}/3)";

        // 대화가 진행 중이 아니고, 조건을 만족했을 때만 대화를 시작하고 다음 튜토리얼로 넘어가도록 함
        if (checkAttackState >= 3 && !GameManager.Instance.DialogueManager.IsDialogueActive())
        {
            StartCoroutine(Dialogues(dtc));
        }
    }

    private IEnumerator Dialogues(DungeonTutorialController dtc)
    {
        DialogueDetector.InteractiveNPC = DungeonTutorialIntro.icon;

        GameManager.Instance.DialogueManager.GetDialogues(DungeonTutorialIntro.interaction.GetDialogue(),
            DungeonTutorialIntro.interaction);
        GameManager.Instance.DialogueManager.ShowDialogue();

        // 대화가 끝날 때까지 대기
        while (GameManager.Instance.DialogueManager.IsDialogueActive())
        {
            yield return null;
        }

        // 대화가 끝난 후에 다음 튜토리얼로 넘어감
        dtc.SetNextTutorial();
    }


    public override void Exit()
    {
        isTutorialing01 = false;
    }
}