using UnityEngine;
using UnityEngine.Playables;

public class DirectorController : MonoBehaviour
{
    private PlayableDirector director;
    public static bool isPlayingCutScene;
    private void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    private void OnEnable()
    {
        TaxManager.OnCutSceneEvent += PlayCutScene;
    }

    private void OnDisable()
    {
        TaxManager.OnCutSceneEvent -= PlayCutScene;
    }

    private void PlayCutScene()
    {
        Player.Instance.isPlayerInteracting = true;
        isPlayingCutScene = true;
        director.Play();
        director.stopped += OnCutSceneStopped;
    }

    private void OnCutSceneStopped(PlayableDirector director)
    {
        isPlayingCutScene = false;
        Player.Instance.isPlayerInteracting = false;
        Player.Instance.playerInputController.playerMouseAndInteractActions.Enable();
        GameManager.Instance.CurrentStageIdx = 0;
        GameManager.Instance.MoveStageController.SettingCameraPos();
        director.stopped -= OnCutSceneStopped;
        Player.Instance.transform.position = GameManager.Instance.Outro.playerRespawnPosition.transform.position;
    }
}
