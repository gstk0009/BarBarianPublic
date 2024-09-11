using System;
using System.Collections;
using UnityEngine;

// 씬이 전환될 때 기본적인 값을 세팅해줄 클래스 
public class SceneTransitionManager : Singleton<SceneTransitionManager>
{
    public FadeController FadeController;
    public event Action OntransitionComplete;
    LoadSceneManager LoadSceneManager;

    public Canvas sceneManagerCanvas;

    private void Start()
    {
        LoadSceneManager = GetComponent<LoadSceneManager>();
        sceneManagerCanvas = GetComponentInChildren<Canvas>();
    }
    public void LoadScene(int sceneNum, GameObject spawnPos = null)
    {
        StartCoroutine(LoadSceneCoroutine(sceneNum, spawnPos));
    }
    
    IEnumerator LoadSceneCoroutine(int sceneNum, GameObject spawnPos)
    {
        yield return StartCoroutine(FadeController.FadeOut());
        yield return LoadSceneManager.LoadSceneCoroutine(sceneNum);

        if (Player.Instance != null)
            Player.Instance.isPlayerInteracting = true;

        // 씬을 이동할 때 npc에 대한 데이터 초기화 
        if (GameManager.instance.SpawnersManager != null)
        {
            GameManager.instance.SpawnersManager.NPCSpawner.ResetData();
        }

        yield return new WaitUntil(() => SoundManager.Instance != null);
        SoundManager.Instance.SetSceneBgm(sceneNum);

        // 각 씬과 관련된 초기 세팅 작업 수행
        if (sceneNum == (int)SceneNumber.DugeonScene_1 || sceneNum == (int)SceneNumber.DugeonScene_2) // 던전 씬 
        {
            GameManager.Instance.CurrentStageIdx = 0;

            DataManager.Instance.currentPlayer.tryCnt++;

            GameManager.Instance.MoveStageController.SettingCameraPos();
            // TaxManager - 세금 냈는지 확인
            spawnPos = Player.Instance.gameObject;

            yield return new WaitUntil(() => GameManager.Instance.SurfaceLayerChanger != null);

            StartCoroutine(GameManager.Instance.SurfaceLayerChanger.UpdateNavMeshForStage(LayerType.Stage1));
            StartCoroutine(GameManager.Instance.SpawnersManager.MonsterSpawner.InitSpawnMonster(GameManager.Instance.MoveStageController.currentStage));
            if (GameManager.Instance.SpawnersManager.NPCSpawner.spawnCount == 0)
            {
                StartCoroutine(GameManager.Instance.SpawnersManager.NPCSpawner.SpawnInteractiveNPC());
            }
            else
            {
                StartCoroutine(GameManager.Instance.SpawnersManager.NPCSpawner.UpdateFollowingNPC());
            }
         
        }
        else if (sceneNum == (int)SceneNumber.VillageScene)
        {
            GameManager.Instance.MoveStageController.SettingCameraPos();

            spawnPos = GameManager.Instance.MoveStageController.GetDungeoToVillagePos();
            
            if (SelectSaveFiles.isStartGame) // 캐릭터 선택창 -> 마을 첫 입성
            {
                spawnPos = GameManager.Instance.MoveStageController.GetPlayerRespawnPos(); 
                SelectSaveFiles.isStartGame = false;
            }
        }
        

        if (Player.Instance != null && spawnPos != null)
        {
            Player.Instance.transform.position = spawnPos.transform.position;
            CameraController.Instance.cameraSmoothMove = false;
        }

        yield return StartCoroutine(FadeController.FadeIn());

        OntransitionComplete?.Invoke();

        if (Player.Instance != null && !DirectorController.isPlayingCutScene)
            Player.Instance.isPlayerInteracting = false;
    }
}
