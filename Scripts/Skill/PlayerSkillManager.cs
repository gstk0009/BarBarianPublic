using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillManager : Singleton<PlayerSkillManager>
{
    public List<SkillBase> skills = new List<SkillBase>(); // 현재 장착된 플레이어의 스킬 
    public Image[] CoolTimeImages; // 장착된 스킬들의 쿨타임 이미지 
    public bool[] isSkillCooltime = new bool[3];
    private float dashCoolTime = 1f;
    public SkillBase GetSkill(int index)
    {
        if (index >= 0 && index < skills.Count)
        {
            return skills[index];
        }
        return null;
    }

    public void StartSKillCoolDown(int idx, SkillBase skill = null)
    {
        float coolTime;
        isSkillCooltime[idx] = true;

        if (skill != null)
            coolTime = skill.CoolTime;
        else
            coolTime = dashCoolTime;

        StartCoroutine(Cooldown(coolTime, idx));
    }

    private IEnumerator Cooldown(float coolitme, int idx)
    {
        float time = 0f;

        while (time < coolitme)
        {
            time += Time.deltaTime;
            CoolTimeImages[idx].fillAmount = time / coolitme;
            yield return null;
        }

        CoolTimeImages[idx].fillAmount = 1;
        isSkillCooltime[idx] = false;

    }
}
