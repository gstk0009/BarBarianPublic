using UnityEngine;

public enum SkillType
{
    Attack, 
    Buff, 
    Heal, 
}
public abstract class SkillBase : ScriptableObject
{
    public SkillType skillType; // 스킬 유형 추가
    public AudioClip SkillSound; // 스킬 사용할 때 효과음

    public string SkillName; // 스킬 이름
    public float CoolTime; // 스킬 쿨타임
    public float CastTime; // 스킬 시전 시간
    public int ManaAmout; // 마나 사용량

    public GameObject particlePrefab; // 스킬에 쓸 파티클

    public float yOffset = 0f; // y축 오프셋
    public float xOffset = 0f; // x축 오프셋
    public float forwardOffset = 0.5f; // 전방 오프셋

    protected bool canSkill = false;

    public abstract void Activate(BaseStat stat);
}