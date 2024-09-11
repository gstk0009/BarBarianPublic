using UnityEngine;

public enum EnchantType // 스킬 속성
{
    None,
    Poison,
    Frozen,
}

public class AttackSkill : SkillBase
{
    public float Damage; // 데미지
    public float Range; // 스킬 범위
    public EnchantType enchantType = EnchantType.None; // 스킬 속성 타입. 디폴트는 None
    public Vector2 hitBoxSize; // 적 피격 범위(overlapBoxAll)

    [Header("Poison Set")]
    public float PoisionDamageRate; // 초당 독 피해
    public float PoisonDuration; // 독 지속 시간

    [Header("Frozen Set")]
    public float FrozenSlowRate; 
    public float FrozenDuration;

    public float ManaCost;

    public override void Activate(BaseStat stat)
    {
        // 플레이어의 방어력에 감산되어 실제로 들어가는 데미지 
        ManaCost = Player.Instance.playerStat.HP.maxValue / 20;

        if (Player.Instance.playerStat.HP.curValue > ManaCost)
        {
            Player.Instance.playerStat.TakeDamage(ManaCost + Player.Instance.playerStat.Def.curValue/2, true);
            canSkill = true;
        }
        else canSkill = false;
    }

    protected void GetDebuff(EnchantType et, BaseStat enemy)
    {
        switch (et)
        {
            case EnchantType.Poison:
                DebuffController.Instance.GetPoison(enemy, PoisionDamageRate, PoisonDuration); break;
            case EnchantType.Frozen:
                DebuffController.Instance.GetFrozen(enemy, FrozenSlowRate, FrozenDuration); break;
            default: break;
        }
    }
}
