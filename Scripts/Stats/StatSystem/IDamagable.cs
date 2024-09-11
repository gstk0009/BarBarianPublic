
public interface IDamagable
{
    //피격 이벤트 함수
    void TakeDamage(float damageAmount, bool isSkill = false, EnchantType type = EnchantType.None);

    void CalDamage(BaseStat stat);

    bool isDebuffed { get; set; } // 디버프에 대한 효과를 한 번만 적용시키기 위한 bool 변수
    bool isFrozenState { get; set; } // 빙결 상태에 대한 true false

}