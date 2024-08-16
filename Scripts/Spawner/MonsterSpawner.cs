using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : BaseSpawner
{
    [SerializeField] int dayTimer = 10;
    [SerializeField] int nightTimer = 5;
    [SerializeField] int daySpawnCnt = 3;
    [SerializeField] int nightSpawnCnt = 5;
    private Coroutine spawnTimer;
    WaitForSeconds delayTime = new WaitForSeconds(0.1f);
    float monsterSpawnMinDistance = 2f;
    List<Vector3> monsterSpawnPos = new List<Vector3>();

    int _maxAttempts = 300; // 최대 시도 횟수
    int _attempts = 0;

    public void SpawnMonster(int cnt, string monsterType)
    {
        // 맵 경계 설정 → 현재 스테이지의 카메라 경계 값을 가져오면 됨.
        mapMinBounds = CameraController.Instance.minCameraBoundary;
        mapMaxBounds = CameraController.Instance.maxCameraBoundary;

        for (int i = 0; i < cnt; i++)
        {
            Vector3 randomPos = GetRandomPos();

            _attempts = 0;

            while (!isPossiblePos(randomPos))
            {
                _attempts++;
                if (_attempts >= _maxAttempts) // 몬스터 스폰 유효한 위치를 못 찾음
                {   
                    break;
                }
                randomPos = GetRandomPos();
            }

            GameObject monster = ObjectPool.Instance.SpawnFromPool(monsterType);

            if (monster != null)
            {
                monster.transform.position = randomPos;
                monsterSpawnPos.Add(randomPos);

                monster.SetActive(true);
            }
            else // 몬스터 풀 모두 사용
            {
                return;
            }
       }
    }

    bool isPossiblePos(Vector3 pos)
    {
        foreach (var position in monsterSpawnPos)
        {
            if (Vector3.Distance(position, pos) < monsterSpawnMinDistance)
            {
                return false;
            }
        }
        return true;
    }

    public void DeactivateAllMonsters()
    {
        if(ObjectPool.Instance != null)
            ObjectPool.Instance.DeactivateAll();
        
        monsterSpawnPos.Clear();

    }

    // 마을 Scene 이동 시 정지시켜야 에러 안 뜸
    public IEnumerator InitSpawnMonster(Stage currentStage)
    {
        yield return new WaitUntil(() => ObjectPool.Instance != null);

        if (currentStage.monsters.Count != 0)
        {
            for (int i = 0; i < currentStage.monsters.Count; ++i)
            {
                string monsterType = currentStage.monsters[i].type;
                SpawnMonster(currentStage.monsters[i].cnt, monsterType);
            }

            StopSpawner();
            spawnTimer = StartCoroutine(SpawnMonsterTimer(currentStage));
        }
            

        yield return null;
    }

    private IEnumerator SpawnMonsterTimer(Stage currentStage)
    {
        yield return new WaitUntil(() => ObjectPool.Instance != null);

        int cur_min = ClockSystem.Minute;
        int minCnt = 0;

        while (true)
        {
            if (ObjectPool.Instance == null)
                break;

            yield return delayTime;

            bool day = ClockSystem.IsDayOrNight();

            int timer = day ? dayTimer : nightTimer;
            int maxspawnCnt = day ? daySpawnCnt : nightSpawnCnt;

            // ClockSystem 기준을 timer 만큼 대기

            if (ClockSystem.Minute != cur_min)
            {
                cur_min = ClockSystem.Minute;
                minCnt++;

                if (minCnt >= timer)
                {
                    for (int i = 0; i < currentStage.monsters.Count; ++i)
                    {
                        int cnt = Random.Range(1, maxspawnCnt);
                        string monsterType = currentStage.monsters[i].type;
                        SpawnMonster(cnt, monsterType);
                    }
                    minCnt = 0;
                }
            }
        }
    }


    public void StopSpawner()
    {
        if (spawnTimer != null)
            StopCoroutine(spawnTimer);
    }
}
