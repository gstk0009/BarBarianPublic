using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStageController : MonoBehaviour
{
    LayerType[] StageLayers;
    [SerializeField]List<Stage> stageObjs = new List<Stage>();

    [SerializeField] GameObject DungeonToVillagePos;
    [SerializeField] GameObject PlayerRespawnPos;

    public static bool isBossStage = false;
    public Stage currentStage;
    
    public void MoveToNextStage(PortalType portalType)
    {
        StartCoroutine(FadeOutAndMoveStage(portalType));
    }

    IEnumerator FadeOutAndMoveStage(PortalType portalType)
    {
        if (portalType != PortalType.VillagePortal && portalType != PortalType.VillageToDungeonPortal 
            && portalType != PortalType.NextDungeonPortal)
            yield return StartCoroutine(SceneTransitionManager.Instance.FadeController.FadeOut());

        if(portalType == PortalType.VillageToDungeonPortal) // 마을에서 던전으로 씬 이동 
        {
            ClearStageObjs();

            StageLayers = new LayerType[3];
            StageLayers[0] = LayerType.Stage1;
            StageLayers[1] = LayerType.Stage2;
            StageLayers[2] = LayerType.StageBoss;

            SceneTransitionManager.Instance.LoadScene((int)SceneNumber.DugeonScene_1); // 여기에 던전 초기 세팅 있음
        }
        else if (portalType == PortalType.NextDungeonPortal)
        {
            ClearStageObjs();
            GameManager.Instance.SpawnersManager.NPCSpawner.ResetData();

            StageLayers = new LayerType[4];
            StageLayers[0] = LayerType.Stage1;
            StageLayers[1] = LayerType.Stage2;
            StageLayers[2] = LayerType.Stage3;
            StageLayers[3] = LayerType.StageBoss;

            SceneTransitionManager.Instance.LoadScene((int)SceneNumber.DugeonScene_2);
        }
        else if (portalType == PortalType.StagePortal) // 던전 씬 내부에서 stage 이동
        {
            SetDungeonStage();

        }
        else if (portalType == PortalType.VillagePortal)
        {
            MoveToVillageStage();
        }
        else if (portalType == PortalType.PreStagePortal)
        {
            // 여기서 CurrentstageIdx가 0이면 Fade 재생시켜줘야함 
            if ((GameManager.Instance.CurrentStageIdx - 1) >= 0)
            {
                GameManager.Instance.CurrentStageIdx -= 2;
                MoveToNextStage(PortalType.StagePortal);
            }
        }
        else if(portalType == PortalType.VillageUpTownPortal)
        {
            GameManager.Instance.CurrentStageIdx = 1;
            SettingCameraPos();

        }
        else if(portalType == PortalType.VillageDownTownPortal)
        {
            GameManager.Instance.CurrentStageIdx = 0;
            SettingCameraPos();
        }
        else if (portalType == PortalType.ScaffoldPortal)
        {
            GameManager.Instance.CurrentStageIdx = 2;
            SettingCameraPos();
        }
        else if(portalType == PortalType.EndingPortal)
        {
            GameManager.Instance.SpawnersManager.NPCSpawner.ResetData();
            GameManager.Instance.Outro.ShowEnding();
        }

        if (portalType != PortalType.VillagePortal && portalType != PortalType.VillageToDungeonPortal
             && portalType != PortalType.NextDungeonPortal && portalType != PortalType.EndingPortal) 
        { 
            yield return StartCoroutine(SceneTransitionManager.Instance.FadeController.FadeIn());
            Player.Instance.isPlayerInteracting = false;

        }

    }
    void MoveToVillageStage()
    {
        ClearStageObjs();
        ClearPreviousDatas();

        // 마을로 돌아가면 진행되었던 스테이지를 초기화 -> UpTown에서 스폰되기 위해 값은 1
        GameManager.Instance.CurrentStageIdx = 1;
        GameManager.Instance.SpawnersManager.NPCSpawner.ResetData();

        GameManager.Instance.SurfaceLayerChanger = null;
        SceneTransitionManager.Instance.LoadScene((int)SceneNumber.VillageScene);
        ClearStageObjs();
        GameManager.Instance.TaxManager.CheckTaxPayment();
    }

    void SetDungeonStage()
    {
        ClearPreviousDatas();
        isBossStage = false;
        SoundManager.Instance.SetCurDungeonBGM();

        LayerType layerType = StageLayers[GameManager.Instance.CurrentStageIdx];
        if (GameManager.Instance.CurrentStageIdx < StageLayers.Length - 1)
        {
            SettingCameraPos();

            StartCoroutine(GameManager.Instance.SurfaceLayerChanger.UpdateNavMeshForStage(layerType));
            StartCoroutine(GameManager.Instance.SpawnersManager.MonsterSpawner.InitSpawnMonster(currentStage));

            StartCoroutine(GameManager.Instance.SpawnersManager.NPCSpawner.UpdateFollowingNPC());

            //해당 스테이지에서 npc가 생성된 적 없을 경우에만 npc 생성
            if (GameManager.Instance.SpawnersManager.NPCSpawner.spawnCount < GameManager.Instance.CurrentStageIdx)
                StartCoroutine(GameManager.Instance.SpawnersManager.NPCSpawner.SpawnInteractiveNPC());
        }
        else // 보스 스테이지
        {
            isBossStage = true;
            SettingCameraPos();

            StartCoroutine(GameManager.Instance.SurfaceLayerChanger.UpdateNavMeshForStage(layerType));
            StartCoroutine(GameManager.Instance.SpawnersManager.NPCSpawner.UpdateFollowingNPC());

            SoundManager.Instance.SetBossBgm();
        }

    }
    public void SettingCameraPos()
    {
        if (stageObjs.Count > 0)
        {
            for (int i = 0; i < stageObjs.Count; i++)
            {
                if (stageObjs[i].StageIDX == GameManager.Instance.CurrentStageIdx)
                {
                    currentStage = stageObjs[i]; // 현재 위치한 스테이지를 지정
                    Player.Instance.transform.position = stageObjs[i].PlayerSpawnPos.transform.position;

                    CameraController.Instance.minCameraBoundary = stageObjs[i].minCameraBoundary.transform.position;
                    CameraController.Instance.maxCameraBoundary = stageObjs[i].maxCameraBoundary.transform.position;

                    stageObjs[i].ActiveDoor();

                    break;
                }
            }

            GameManager.Instance.CurrentStageIdx++;
        }
    }
    void ClearPreviousDatas()
    {
        GameManager.Instance.SpawnersManager.NPCSpawner.SetActiveFalseToNPC();
        GameManager.Instance.SpawnersManager.MonsterSpawner.DeactivateAllMonsters();
    }
    public GameObject GetDungeoToVillagePos()
    {
        return DungeonToVillagePos;
    }
    public GameObject GetPlayerRespawnPos()
    {
        return PlayerRespawnPos;
    }
    public void SettingStageObjs(Stage stage) 
    {
         stageObjs.Add(stage);
    }
    public void ClearStageObjs()
    {
        if (stageObjs.Count > 0)
            stageObjs.Clear();
    }
}
