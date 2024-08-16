using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    [SerializeField] public LayerMask atkTarget;
    [SerializeField] private float healAmount = 10;

    public Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((atkTarget.value & (1 << other.gameObject.layer)) == 0) return; //공격 대상으로 설정한 오브젝트가 아니면, return;
        if (other.gameObject.TryGetComponent(out BaseStat otherStat))
        {
            otherStat.HP.AddCurValue(healAmount);
        }
    }

    public void activeCollider()
    {
        col.enabled = !col.enabled;
    }
}
