using System.Collections;
using UnityEngine;

public class DebuffController : Singleton<DebuffController>
{
    WaitForSeconds wfs_1 = new WaitForSeconds(1);

    public void GetDebuff(EnchantType et, BaseStat target, float damageRate, float duration)
    {
        switch (et)
        {
            case EnchantType.Poison:
                GetPoison(target, damageRate, duration); break;
            case EnchantType.Frozen:
                GetFrozen(target, damageRate, duration); break;
            default: break;
        }
    }

    public void GetPoison(BaseStat target, float damageRate, float duration)
    {
        StartCoroutine(PoisonState(target, damageRate, duration));
    }

    IEnumerator PoisonState(BaseStat target, float damageRate, float duration)
    {
        float time = 0;
        GameObject targetGameObject = target.gameObject;

        while (time < duration)
        {
            // 여기서 targetGameObject가 null이거나 비활성화되었는지 확인
            if (targetGameObject == null || !targetGameObject.activeSelf)
                yield break;

            target.TakeDamage(target.HP.maxValue / damageRate, false, EnchantType.Poison);
            time += 1f;

            yield return wfs_1;
        }
    }

    public void GetFrozen(BaseStat target, float slowRate, float duration)
    {
        if (!target.isFrozenState)
        {
            target.isFrozenState = true;
            target.isDebuffed = true;
            StartCoroutine(FrozenState(target, slowRate, duration));
        }
    }

    IEnumerator FrozenState(BaseStat target, float slowRate, float duration)
    {
        if(target != null && target.runSpeed!= null && target.attackRate !=null)
        {
            float time = 0f;

            GameObject targetGameObject = target.gameObject;

            // 현재 이동 속도와 공격 속도를 저장
            float originalTargetSpeed = target.runSpeed.curValue;
            float originalTargetAttackRate = target.attackRate.curValue;

            if(target.runSpeed != null && target.attackRate != null)
            {
                // 이동 속도와 공격 속도를 1/slowRate로 줄임
                target.runSpeed.curValue /= slowRate;
                target.attackRate.curValue *= slowRate;

                while (time < duration)
                {
                    // targetGameObject가 null이거나 비활성화되었는지 확인
                    if (targetGameObject == null || !targetGameObject.activeSelf)
                        yield break;

                    time += 1f;

                    yield return wfs_1;
                }

                // 원래 속도와 공격 속도로 복구
                target.runSpeed.curValue = originalTargetSpeed;
                target.attackRate.curValue = originalTargetAttackRate;

                target.isFrozenState = false;
                target.isDebuffed = false;
            }
        }
       
    }
}
