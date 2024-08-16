using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCInfo", menuName = "NPC/Stats")]
public class NPCSO : ScriptableObject
{
    // Stat 값 변경할 수 없게 property private set으로 설정
    [field: SerializeField] public float Exp { get; private set; } = 0;
    [field: SerializeField] public int HP { get; private set; } = 100;
    [field: SerializeField] public int MP { get; private set; } = 50;
    [field: SerializeField] public int ATK { get; private set; } = 50;
    [field: SerializeField] public int DEF { get; private set; } = 50;
    [field: SerializeField] public float SkillCoolTime { get; private set; } = 2;
    [field: SerializeField] public float SkillSpeed { get; private set; } = 2;
    [field: SerializeField] public float AttackRange { get; private set; } = 2;


}