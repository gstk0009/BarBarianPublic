using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WeaponShooter : MonoBehaviour
{

    [SerializeField] private GameObject bulletObject;
    [SerializeField] private float weaponSpeed;

    [SerializeField] private NPC npc;
    [SerializeField] private Player player;
    [SerializeField] private Monster monster;
    private void Awake()
    {
        switch (gameObject.layer)
        {
            case (int)LayerType.Enemy:
                monster = GetComponentInParent<Monster>();
                break;
            case (int)LayerType.NPC:
                npc = GetComponentInParent<NPC>();
                break;
            case (int)LayerType.Player:
                player = GetComponentInParent<Player>();
                break;
        }
    }

    public void Shooting(Vector3 Destination)
    {
        // 마우스 클릭 위치 가져오기
        Destination.z = 0;

        //무기 생성
        GameObject weapon = Instantiate(bulletObject, transform.position, Quaternion.identity);
        Bullet bulletData = weapon.GetComponent<Bullet>();
        bulletData.layerType = gameObject.layer;
        if (npc != null) bulletData.npc = npc;
        if (player != null) bulletData.player = player;
        if (monster != null) bulletData.monster = monster;

        // 방향 계산
        Vector3 direction = (Destination - transform.position).normalized;

        // 화살의 방향 설정
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weapon.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-30));

        // 화살에 힘을 가해 이동
        Rigidbody2D rb = weapon.GetComponent<Rigidbody2D>();
        rb.velocity = direction * weaponSpeed;
    }
}
