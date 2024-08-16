using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public LayerMask atkTarget;

    [SerializeField] public NPC npc;
    [SerializeField] public Player player;
    [SerializeField] public Monster monster;
    [SerializeField] public BossMonster boss;
    [SerializeField] public int layerType;

    private DungeonThings dt;
    public BaseStat dtStat;
    public Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((atkTarget.value & (1 << other.gameObject.layer)) == 0) return; //공격 대상으로 설정한 오브젝트가 아니면, return
        if (other.gameObject.TryGetComponent(out IDamagable damagable))
        {
            damagable.TakeDamage(dtStat.STR.curValue);
        }
    }

    public void activeCollider()
    {
        col.enabled = !col.enabled;
    }
}
