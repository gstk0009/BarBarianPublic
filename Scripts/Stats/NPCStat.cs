using System.Collections;
using UnityEngine;

public class NPCStat : BaseStat
{
    public NPCSO npcSO;
    public SpriteRenderer[] spriteRenderer;

    private bool canDamaged = true;


    //SO데이터에서 초기값으로 설정해주는 베이스 스탯들을 Condition으로 선언합니다.
    public Condition Exp;
    public Condition Mp;
    public Condition Def;
    public Condition SkillCoolTime;
    public Condition SKillSpeed;
    public Condition AtkRange;


    private void Awake()
    {
        InitializeStats();
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();

    }

    private void InitializeStats()
    {
        if (npcSO != null)
        {
            // Exp, Hp, Mp, Atk, Def의 maxValue를 NPCSO의 데이터로 설정합니다.
            Exp = new Condition(npcSO.Exp);
            HP = new Condition(npcSO.HP + 50 * GameManager.Instance.MainStageIdx);
            Mp = new Condition(npcSO.MP * GameManager.Instance.MainStageIdx);
            STR = new Condition(npcSO.ATK + 10 * GameManager.Instance.MainStageIdx);
            Def = new Condition(npcSO.DEF + 5 * GameManager.Instance.MainStageIdx);
            SkillCoolTime = new Condition(npcSO.SkillCoolTime);
            SKillSpeed = new Condition(npcSO.SkillSpeed);
            AtkRange = new Condition(npcSO.AttackRange);
        }
    }


    public override float GetAtk()
    {
        return base.GetAtk();
    }

    //NPC 피격 이벤트
    public override void TakeDamage(float damageAmount, EnchantType type)
    {
        base.TakeDamage(damageAmount);

        if (!canDamaged) return;
        //피격받는 캐릭터의 방어력에 비례한 데미지 경감 계산
        float realDamage = Mathf.Max(damageAmount - Def.curValue, 1); //방어력이 공격력보다 높으면, 1데미지만 들어가도록 계산

        HP.SubtractCurValue(realDamage); //damaageAmount 대신 경감데미지를 넣는다;
        StartCoroutine(DamageFlash());
    }

    public IEnumerator DamageFlash()
    {
        canDamaged = false;
        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i].color = new Color(0.65f, 0.01f, 0.01f);
        }

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < spriteRenderer.Length; i++)
        {
            spriteRenderer[i].color = Color.white;
        }
        canDamaged = true;
    }

}
