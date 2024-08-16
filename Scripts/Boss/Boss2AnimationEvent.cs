using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2AnimationEvent : MonoBehaviour
{
    public GameObject[] effects;
    public Collider2D effectCollider;

    [SerializeField] private GameObject[] bulletObjects;
    [SerializeField] private List<Vector3> bulletStartPosition;
    [SerializeField] private GameObject dashEffect;
    [SerializeField] private GameObject cloudLightning;
    [SerializeField] private float weaponSpeed = 5;

    private void Awake()
    {
        effectCollider = GetComponent<Collider2D>();
        bulletStartPosition = new List<Vector3>();
        if (bulletObjects != null)
        {
            foreach(GameObject obj in bulletObjects)
            {
                bulletStartPosition.Add(obj.transform.position);
            }
        }
    }

    void StartLightning()
    {
        cloudLightning.SetActive(true);
    }
    void FinshLightning()
    {
        cloudLightning.SetActive(false);
    }

    void StartDash()
    {
        dashEffect.SetActive(true);
    }

    void FinshDash()
    {
        effectCollider.enabled = false;
        dashEffect.SetActive(false);
    }

    void StartEffect()
    {
        StartCoroutine(ActivateGameObjectsInRandomOrder());
    }

    void EndEffect()
    {
        effectCollider.enabled = false;
        gameObject.SetActive(false);
    }

    void ActiveCollider()
    {
        effectCollider.enabled = true;
    }


    IEnumerator ActivateGameObjectsInRandomOrder()
    {        

        // GameObject 배열을 리스트로 변환
        List<GameObject> gameObjectList = new List<GameObject>(effects);

        // 리스트를 랜덤하게 섞음
        ShuffleList(gameObjectList);

        // 섞인 순서대로 0.5초 간격으로 GameObject를 활성화
        foreach (GameObject obj in gameObjectList)
        {
            obj.SetActive(true);
            yield return new WaitForSeconds(0.25f); // 0.5초 대기
        }
    }

    void ShuffleList(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }


    public void Shooting()
    {
        BossMonster boss = gameObject.GetComponent<BossMonster>();
        if(boss.searchTargetCollider != null)
        {
            for (int i = 0; i < bulletObjects.Length; i++)
            {
                //bulletObjects[i].transform.position = bulletStartPosition[i];
                bulletObjects[i].transform.position = transform.position + new Vector3(-1.5f + i, 2, 0);


                //무기 생성
                bulletObjects[i].SetActive(true);

                // 방향 계산
                Vector3 direction = (boss.searchTargetCollider.transform.position - bulletObjects[i].transform.position).normalized;

                // 화살의 방향 설정
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                bulletObjects[i].transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 30));

                // 화살에 힘을 가해 이동
                Rigidbody2D rb = bulletObjects[i].GetComponent<Rigidbody2D>();
                rb.velocity = direction * weaponSpeed;
            }
        }        
     
    }
}
