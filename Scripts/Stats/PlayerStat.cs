using System.Collections;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerStat : BaseStat
{
    public PlayerSO playerSO;
    public SpriteRenderer[] spriteRenderer;

    private bool canDamaged = true;


    //SO데이터에서 초기값으로 설정해주는 베이스 스탯들을 Condition으로 선언합니다.
    public Condition Level;
    public Condition Exp;
    public Condition Mp;
    public Condition Def;
    public Condition Dex;
    public Condition Luk;
    public Condition Tired;
    public Condition Hungry;
    public Condition Stamina;

    private void Awake()
    {
        InitializeStats();
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();

    }

    public void InitializeStats()
    {
        if (playerSO != null)
        {
            //플레이어 스탯들의 maxValue를 NPCSO의 데이터로 설정합니다.
            Level = new Condition(playerSO.Lv);
            Exp = new Condition(playerSO.Exp);
            HP = new Condition(playerSO.HP);
            Mp = new Condition(playerSO.MP);
            STR = new Condition(playerSO.STR);
            Def = new Condition(playerSO.DEF);
            Dex = new Condition(playerSO.DEX);
            Luk = new Condition(playerSO.LCK);
            Tired = new Condition(playerSO.TiredGuage);
            Stamina = new Condition(playerSO.Stamina);
            Hungry = new Condition(playerSO.HungryGuage);

            //curValue가 0부터 시작해야 하는 스탯들의 값을 설정합니다.
            Exp.SetCurValueToZero();
        }
    }


    public override float GetAtk()
    {
        return base.GetAtk();
    }


    //Player 피격 이벤트
    public override void TakeDamage(float damageAmount, bool isSkill = false, EnchantType type = EnchantType.None)
    {
        base.TakeDamage(damageAmount);

        if (!canDamaged) return;

        float realDamage = Mathf.Max(damageAmount - Def.curValue / 2, 1); //방어력이 공격력보다 높으면, 1데미지만 들어가도록 계산
        HP.SubtractCurValue(realDamage); //damaageAmount 대신 경감데미지를 넣는다

        if (!isSkill)
        {
            GameObject createDamageText = ObjectPool.Instance.SpawnFromPool("DamageText");
            createDamageText.transform.position = gameObject.transform.position;
            DamageTextManager Damage = createDamageText.GetComponent<DamageTextManager>();

            Damage.damage = (int)realDamage;
            Damage.SetTextColor(type, true);

            createDamageText.SetActive(true);

            GameManager.Instance.cameraController.CameraShake(0.5f, 0.05f);
        }

        if ((int)HP.curValue <= 0)
        {
            if (DungeonTutorialBase.isTutorialing)
            {
                HP.curValue = 40; 
                StartCoroutine(DT_CheckSkilledMonsters.instance.ShowUnClearDialogue());
                return;
            }

            Player.Instance.isPlayerInteracting = true;
            PlayerBaseState.isAttackState = false;
            Player.Instance.playerStateMachine.ChangeState(Player.Instance.playerStateMachine.DeadState);

            // 여기서 아웃트로
            GameManager.Instance.MoveStageController.ClearStageObjs();
            GameManager.Instance.SpawnersManager.NPCSpawner.ResetData();
            GameManager.Instance.Outro.PlayingOutro();
        }
        else
        {
            StartCoroutine(DamageFlash());
        }
    }

    public IEnumerator DamageFlash()
    {
        canDamaged = false;
        for(int i=0; i<spriteRenderer.Length;i++)
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

    public void GetExp(float exp)
    {
        Exp.curValue += exp;
        if(Exp.curValue >= Exp.maxValue)
        {
            LevelUP();
            float overExp = Exp.curValue - Exp.maxValue;
            Exp.maxValue += 10;
            Exp.curValue = overExp;
        }
    }

    private void LevelUP()
    {
        Level.curValue++;
    }

    public float ReturnExp()
    {
        throw new System.NotImplementedException();
    }

    public void ApplyKnockback(Vector3 knockbackAmount)
    {
        throw new System.NotImplementedException();
    }
}
