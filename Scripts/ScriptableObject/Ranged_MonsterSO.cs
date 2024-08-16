using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged_MonsterSO : MonsterSO
{
    [field: Header("Monster Ranged_Attack State")]
    [field: SerializeField] public GameObject Bullet { get; private set; } // 투사체
    [field: SerializeField] public float BulletSpeed { get; private set; } // 투사체 속도

}
