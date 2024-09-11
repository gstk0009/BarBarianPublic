using System.Collections;
using UnityEngine;

public class BuffSkill : SkillBase
{
    public int BuffAtk;
    public int BuffDef;
    public int BuffHp;

    public float BuffTime; // 버프 지속 시간
    bool isSkillUsed = false;
    protected WaitForSeconds wfs;

    private float originalHpPercentage; // 버프 적용 전의 체력 비율을 저장

    protected void SetBuffStat()
    {
        isSkillUsed = true;

        // 현재 체력 비율을 저장 (현재 체력 / 현재 최대 체력)
        originalHpPercentage = Player.Instance.playerStat.HP.curValue / Player.Instance.playerStat.HP.maxValue;

        // 버프 적용: 최대 체력, 공격력, 방어력 증가
        Player.Instance.playerStat.Def.AddMaxValue(BuffDef);
        Player.Instance.playerStat.Def.BuffCurValue(BuffDef);

        Player.Instance.playerStat.STR.AddMaxValue(BuffAtk);
        Player.Instance.playerStat.STR.BuffCurValue(BuffAtk);

        Player.Instance.playerStat.HP.AddMaxValue(BuffHp);
        // 새로운 최대 체력에 비례하여 현재 체력을 재조정
        Player.Instance.playerStat.HP.curValue = (Player.Instance.playerStat.HP.maxValue * originalHpPercentage);

        Player.Instance.StartCoroutine(ReturnOriginStat());
    }

    IEnumerator ReturnOriginStat()
    {
        yield return wfs;

        float defaultHP = Player.Instance.playerStat.HP.maxValue - BuffHp;
        float newCurValue = Player.Instance.playerStat.HP.curValue * (defaultHP / Player.Instance.playerStat.HP.maxValue);

        if (!Player.Instance.isPlayerInteracting)
        {
            Player.Instance.playerStat.HP.SubtractMaxValue(BuffHp);
            Player.Instance.playerStat.HP.curValue = newCurValue;

            Player.Instance.playerStat.Def.SubtractMaxValue(BuffDef);
            Player.Instance.playerStat.Def.SubtractCurValue(BuffDef);
            Player.Instance.playerStat.STR.SubtractMaxValue(BuffAtk);
            Player.Instance.playerStat.STR.SubtractCurValue(BuffAtk);
        }

        if (Player.Instance.playerStat.HP.curValue <= 0)
        {
            Player.Instance.playerStat.HP.curValue = 5;
        }

        isSkillUsed = false;
    }

    public override void Activate(BaseStat stat)
    {
        // 스킬 발동 시 필요한 마나 소모량
        float damage = ManaAmout - Player.Instance.playerStat.Def.curValue;

        if (Player.Instance.playerStat.HP.curValue > damage)
        {
            Player.Instance.playerStat.TakeDamage(ManaAmout, true);
            canSkill = true;
            if (!isSkillUsed)
            {
                wfs = new WaitForSeconds(BuffTime);
                SetBuffStat();
            }
        }
        else canSkill = false;
    }
}
