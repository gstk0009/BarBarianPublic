using System.Collections;
using UnityEngine;

public class Outro : MonoBehaviour
{
    InteractionEvent interaction;
    [SerializeField] public GameObject playerRespawnPosition;
    [SerializeField] public Camera UIcamera;
    private bool isPlayingOutro = false;

    private void Start()
    {
        if (GameManager.Instance.Outro != null) return;

        GameManager.Instance.Outro = this;
        interaction = GetComponent<InteractionEvent>();
        if (SceneTransitionManager.Instance.sceneManagerCanvas.worldCamera == null)
            SceneTransitionManager.Instance.sceneManagerCanvas.worldCamera = UIcamera;
    }

    public void PlayingOutro()
    {

        if (!isPlayingOutro)
        {
            Player.Instance.isPlayerInteracting = true;

            GameManager.Instance.CurrentStageIdx = 0;
            SelectSaveFiles.isStartGame = true;

            isPlayingOutro = true;
            StartCoroutine(FadeOutAndPlayingOutro());
        }
    }

    public void PlayingOutro2()
    {
        if (!isPlayingOutro) // 씬 이동 없는 아웃트로 실행 / 마을 씬에서 마을 씬으로 재배치 시 활용
        {
            Player.Instance.isPlayerInteracting = true;
            Player.Instance.playerInputController.playerMouseAndInteractActions.Disable();
            isPlayingOutro = true;
            StartCoroutine(FadeOutAndPlayingOutro2());
        }
    }

    private IEnumerator FadeOutAndPlayingOutro2()
    {
        GameManager.Instance.DialogueManager.GetDialogues(interaction.GetDialogue());
        GameManager.Instance.DialogueManager.ShowDialogue();
        StartCoroutine(SceneTransitionManager.Instance.FadeController.FadeOut());

        while (GameManager.Instance.DialogueManager.IsDialogueActive()) // 대화가 끝날 때까지 대기.
        {
            yield return null;
        }

        Player.Instance.playerStateMachine.ChangeState(Player.Instance.playerStateMachine.IdleState);

        DataManager.Instance.currentPlayer.lifeCnt++;
        ClockSystem.NewLife();

        
        DataManager.Instance.SetGameOverData(); // 골드 감소 및 인벤토리 초기화
        Player.Instance.playerStat.InitializeStats();

        isPlayingOutro = false;

        yield return new WaitUntil(() => SoundManager.Instance != null);
        SoundManager.Instance.SetBGM(SoundManager.Instance.Bgms[(int)BGM.Village]);

        yield return StartCoroutine(SceneTransitionManager.Instance.FadeController.FadeIn());
        Player.Instance.isPlayerInteracting = false;

    }

    private IEnumerator FadeOutAndPlayingOutro()
    {
        GameManager.Instance.DialogueManager.GetDialogues(interaction.GetDialogue()); // 죽었을 때의 대사를 활성화
        GameManager.Instance.DialogueManager.ShowDialogue();

        while (GameManager.Instance.DialogueManager.IsDialogueActive()) // 대화가 끝날 때까지 대기.
        {
            yield return null;
        }

        yield return StartCoroutine(SceneTransitionManager.Instance.FadeController.FadeOut());

        GameManager.Instance.SpawnersManager.MonsterSpawner.DeactivateAllMonsters();

        DataManager.Instance.currentPlayer.lifeCnt++;

        ClockSystem.NewLife(); // 다음 날짜로 변경

        DataManager.Instance.SetGameOverData();
        Player.Instance.playerStat.InitializeStats();

        Player.Instance.isPlayerInteracting = false;
        isPlayingOutro = false;

        SceneTransitionManager.Instance.LoadScene((int)SceneNumber.VillageScene, playerRespawnPosition);  // 씬 전환 수행

        Player.Instance.playerStateMachine.ChangeState(Player.Instance.playerStateMachine.IdleState);

    }

    public void ShowEnding()
    {
        Player.Instance.isPlayerInteracting = true;

        StartCoroutine(ShowEndingCoroutine());
    }

    private IEnumerator ShowEndingCoroutine()
    {
        Player.Instance.transform.position = playerRespawnPosition.transform.position;
        DataManager.Instance.SaveGame();

        Dialogue[] dialogues = GameManager.Instance.CsvParseManager.GetDialogue(34, 34);
        GameManager.Instance.DialogueManager.GetDialogues(dialogues);
        GameManager.Instance.DialogueManager.ShowDialogue();
        
        while (GameManager.Instance.DialogueManager.IsDialogueActive()) // 대화가 끝날 때까지 대기
        {
            yield return null;
        }

        yield return StartCoroutine(SceneTransitionManager.Instance.FadeController.FadeOut());

        SceneTransitionManager.Instance.LoadScene((int)SceneNumber.EndScene); // 대화 종료 후 씬 전환 수행
        Player.Instance.isPlayerInteracting = false;

    }

}
