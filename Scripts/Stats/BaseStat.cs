using UnityEngine;

public class BaseStat : MonoBehaviour, IDamagable
{
    public Condition HP;
    public Condition STR;

    public Condition runSpeed;
    public Condition attackRate;

    public bool isDebuffed { get; set; } = false;
    public bool isFrozenState { get; set; } = false;

    public virtual void CalDamage(BaseStat stat)
    {
        TakeDamage(stat.GetAtk());
    }

    public virtual float GetAtk()
    {
        float damageRange = Random.Range(-0.3f, 0.3f);
        double randomValue = Random.Range(0f, 100f);

        if (randomValue < 20f)
        {
            //20% 확률로 크리티컬 데미지(150%) 반환
            return STR.curValue * (1 + damageRange) * 1.5f;
        }
        else
        {
            return STR.curValue * (1 + damageRange);
        }
    }

    public virtual void TakeDamage(float damageAmount, EnchantType type = EnchantType.None)
    {

    }

    
}
