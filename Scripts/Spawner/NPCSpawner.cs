using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCSpawner : BaseSpawner
{
    [SerializeField] GameObject sampleNPC;
    [SerializeField] GameObject sampleNPC_enemy;

    [SerializeField] GameObject[] InteractiveNpcs;
    [SerializeField] public List<GameObject> FriendlyNPCs;
    [SerializeField] RectTransform NPCInfoUI;
    [SerializeField] GameObject NPCInfoPrefab;
    [SerializeField] List<GameObject> CanInteractiveNPC;
    [SerializeField] public int spawnCount = 0;
    [SerializeField] int interactionCount = 0;

    Vector3 spawnPos;

    // 대화용 NPC 스폰 
    public IEnumerator SpawnInteractiveNPC()
    {
        if (spawnCount >= InteractiveNpcs.Length) yield break;

        yield return new WaitUntil(() => ObjectPool.Instance != null);

        mapMinBounds = CameraController.Instance.minCameraBoundary;
        mapMaxBounds = CameraController.Instance.maxCameraBoundary;

        spawnDistanceFromPlayer = 3f;
        spawnPos = GetRandomPos();

        CanInteractiveNPC.Add(Instantiate(InteractiveNpcs[spawnCount], spawnPos, Quaternion.identity));


        // 게임 매니저의 자식 오브젝트가 되도록 설정(씬을 이동해도 파괴 x)
        CanInteractiveNPC[spawnCount].transform.SetParent(DataManager.Instance.gameObject.transform);
        spawnCount++;

        yield return null;
    }

    // 동료 npc로 변경
    public void SpawnFollowingNPC()
    {
        NPC npc = CanInteractiveNPC[GameManager.Instance.CurrentStageIdx - 1].GetComponent<NPC>();

        if (CanInteractiveNPC != null)
        {
            Debug.Log(GameManager.Instance.NPCTargetSystem.NPCIndex.Count);
            // 현재 동료의 수가 4보다 작다면, 
            if (GameManager.Instance.NPCTargetSystem.NPCIndex.Count < 4)
            {
                // NPC 정보 UI 생성
                GameObject npcInfo = Instantiate(NPCInfoPrefab);
                npcInfo.GetComponent<NPCInfoUI>().npc = npc;
                npcInfo.GetComponent<RectTransform>().SetParent(NPCInfoUI);
                npcInfo.GetComponent<RectTransform>().localScale = Vector3.one;
                npcInfo.gameObject.SetActive(true);

                CanInteractiveNPC[GameManager.Instance.CurrentStageIdx - 1].GetComponent<NavMeshAgent>().enabled = true;
                npc.enabled = true;
                CanInteractiveNPC[GameManager.Instance.CurrentStageIdx - 1].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                CanInteractiveNPC[GameManager.Instance.CurrentStageIdx - 1].layer = (int)LayerType.NPC;

                // 게임 매니저의 자식 오브젝트가 되도록 설정(씬을 이동해도 파괴 x)
                FriendlyNPCs.Add(CanInteractiveNPC[GameManager.Instance.CurrentStageIdx - 1]);
                GameManager.Instance.NPCTargetSystem.NPCIndex.Add(CanInteractiveNPC[GameManager.Instance.CurrentStageIdx - 1]);
                GameManager.Instance.NPCTargetSystem.TargetList.Add(CanInteractiveNPC[GameManager.Instance.CurrentStageIdx - 1]);
                npc.SetMatrix();

            }

        }

    }
    public void SpawnEnemyNPC()
    {
        if (CanInteractiveNPC != null)
        {
            SoundManager.Instance.SetBGM(SoundManager.Instance.Bgms[(int)BGM.BattleNPC]);

            NPC npc = CanInteractiveNPC[GameManager.Instance.CurrentStageIdx - 1].GetComponent<NPC>();

            CanInteractiveNPC[GameManager.Instance.CurrentStageIdx - 1].GetComponent<NavMeshAgent>().enabled = true;
            npc.enabled = true;
            CanInteractiveNPC[GameManager.Instance.CurrentStageIdx - 1].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            npc.atkTarget = (1 << 9 | 1 << 8);
            CanInteractiveNPC[GameManager.Instance.CurrentStageIdx - 1].layer = (int)LayerType.Enemy;

            npc.Target = Player.Instance.gameObject;

            if(npc.npcHpBar != null)
            {
                npc.npcHpBar.SetActive(true);
            }
        }
    }


    public void SetActiveFalseToNPC()
    {

        if (FriendlyNPCs == null) return;

        foreach (var npc in FriendlyNPCs)
        {
            npc.SetActive(false);
        }
    }
    public void ActivateFollowingNPC()
    {
        if (FriendlyNPCs == null) return;

        foreach (var npc in FriendlyNPCs)
        {
            npc.transform.position = Player.Instance.transform.position;
            npc.SetActive(true);
        }
        FriendlyNPCs[spawnCount - 1].transform.position = GetRandomPos();
    }

    public IEnumerator UpdateFollowingNPC()
    {
        yield return new WaitUntil(() => ObjectPool.Instance != null);

        if (FriendlyNPCs == null) yield break;

        foreach (var npc in FriendlyNPCs)
        {
            npc.transform.position = Player.Instance.transform.position;
            npc.SetActive(true);
        }

        yield return null;
    }

    public void ResetData()
    {
        foreach (var npc in FriendlyNPCs)
        {
            Destroy(npc);
        }

        foreach (var npc in CanInteractiveNPC)
        {
            Destroy(npc);
        }

        // 하위 오브젝트들을 제거하는 루프
        foreach (Transform child in NPCInfoUI)
        {
            // 하위 오브젝트를 제거
            Destroy(child.gameObject);
        }


        FriendlyNPCs.Clear();
        CanInteractiveNPC.Clear();
        GameManager.Instance.NPCTargetSystem.NPCIndex.Clear();
        spawnCount = 0;
    }
}