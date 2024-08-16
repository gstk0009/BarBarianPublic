using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[Serializable]
public class Stage : MonoBehaviour
{
    [Serializable]
    public class Stage_Monster
    {
        public string type;
        public int cnt;
    }

    public int StageIDX;
    public GameObject minCameraBoundary;
    public GameObject maxCameraBoundary;
    public GameObject[] Doors;
    public bool isVisited;
    public GameObject PlayerSpawnPos;
    public List<Stage_Monster> monsters;


    private void Awake()
    {
        StartCoroutine(InitializeStage());
    }

    private void Start()
    {
        if(GameManager.Instance.MoveStageController == null)
            StartCoroutine(InitializeStage());
    }

    public void ActiveDoor()
    {
        if (Doors == null || Doors.Length == 0 || isVisited) // 문이 없거나 방문한 상태면 열지 않음
            return;

        int idx = UnityEngine.Random.Range(0, Doors.Length);

        GameObject activeDoor = Doors[idx];
        activeDoor.GetComponent<Animator>().enabled = true;
        activeDoor.GetComponent<BoxCollider2D>().enabled = true;
        activeDoor.GetComponent <Light2D>().enabled = true;
        isVisited = true;
    }
    private IEnumerator InitializeStage()
    {
        yield return new WaitUntil(() => GameManager.Instance.MoveStageController != null);
        GameManager.Instance.MoveStageController.SettingStageObjs(this);
    }


}

