using UnityEngine;

[CreateAssetMenu(fileName = "Monster",menuName = "Monster/Stats")]
public class MonsterSO : ScriptableObject
{
    [field:Header("Monster Stats")]

    [field: SerializeField] public float HP { get; private set; }
    [field: SerializeField] public float STR { get; private set; }  // 공격력
    [field: SerializeField] public float DEF { get; private set; } // 방어력
    [field: SerializeField] public float LCK { get; private set; } // 크리티컬 관련 스탯

    [field: Header("Monster AI")]
    [field: SerializeField] public float WalkSpeed { get; private set; } // 순찰 시 이동 속도
    [field: SerializeField] public float WanderRadius { get; private set; } // 순찰 반경
    [field: SerializeField] public float WanderWaitTime { get; private set; } // 순찰 대기 시간

    

    [field: Header("Monster Attack State")]
    [field: SerializeField] public float RunSpeed { get; private set; } // 공격 시 이동 속도
    [field: SerializeField] public float DetectRadius { get; private set; } // 타겟 감지 반경
    [field:SerializeField]  public float AttackRate { get; private set; } // 공격 속도
    [field: SerializeField] public float AttackDistance { get; private set; }
    
    [field: SerializeField] public float Exp { get; private set; } // 처치 시 경험치
}
