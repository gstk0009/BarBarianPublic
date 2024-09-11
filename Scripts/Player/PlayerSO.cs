using UnityEngine;

[CreateAssetMenu(fileName = "PlayerInfo", menuName = "Player/Stats")]
public class PlayerSO : ScriptableObject
{
    // Stat 값 변경할 수 없게 property private set으로 설정
    [field: SerializeField] public int Lv { get; private set; } = 1;
    [field: SerializeField] public float Exp { get; private set; } = 0;
    [field: SerializeField] public int STR { get; private set; } = 1;
    [field: SerializeField] public int DEF { get; private set; } = 1;
    [field: SerializeField] public int DEX { get; private set; } = 1;
    [field: SerializeField] public int LCK { get; private set; } = 1;
    [field: SerializeField] public int HP { get; private set; } = 100;
    [field: SerializeField] public int MP { get; private set; } = 50;
    [field: SerializeField] public float TiredGuage { get; private set; } = 0;
    [field:SerializeField] public float Stamina { get; private set; } = 100;
    [field: SerializeField] public float HungryGuage { get; private set; } = 80f;
}
