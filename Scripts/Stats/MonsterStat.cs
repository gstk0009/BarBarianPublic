using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class MonsterStat : BaseStat
{
    public MonsterSO monsterSO;

    public GameObject DamageText;

    public SpriteRenderer spriteRenderer;

    public Condition def;
    public Condition lck;
    public Condition walkSpeed;
    public Condition wanderRadius;
    public Condition wanderWaitTime;
    public Condition detectRadius;
    public Condition attackDistance;
    public Condition exp;

    // 넉백 힘
    public float knockbackForce = 10f;
    // 넉백 지속 시간
    public float knockbackDuration = 0.2f;

    private Rigidbody2D rb;
    private bool isKnockedback;
    private float knockbackEndTime;


    private void Awake()
    {
        InitializeStats();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isKnockedback && Time.time > knockbackEndTime)
        {
            isKnockedback = false;
            rb.velocity = Vector3.zero; // 넉백이 끝나면 속도를 0으로 설정
        }
    }

    public void InitializeStats()
    {
        if (monsterSO != null)
        {
            // Stat들의 maxValue를 NPCSO의 데이터로 설정합니다.
            HP = new Condition(monsterSO.HP);
            STR = new Condition(monsterSO.STR);
            def = new Condition(monsterSO.DEF);
            lck = new Condition(monsterSO.LCK);
            walkSpeed = new Condition(monsterSO.WalkSpeed);
            wanderRadius = new Condition(monsterSO.WanderRadius);
            wanderWaitTime = new Condition(monsterSO.WanderWaitTime);
            runSpeed = new Condition(monsterSO.RunSpeed);
            detectRadius = new Condition(monsterSO.DetectRadius);
            attackRate = new Condition(monsterSO.AttackRate);
            attackDistance = new Condition(monsterSO.AttackDistance);
            exp = new Condition(monsterSO.Exp);
        }
    }


    public override float GetAtk()
    {
        float damageRange = Random.Range(-0.3f, 0.3f);
        double randomValue = Random.Range(0f, 100f);

        if (randomValue < 10f)
        {
            //20% 확률로 크리티컬 데미지(150%) 반환
            return STR.curValue * (1 + damageRange) * 1.5f;
        }
        else
        {
            return STR.curValue * (1 + damageRange);
        }
    }


    //Monster 피격 이벤트
    public override void TakeDamage(float damageAmount, bool isSkill = false, EnchantType type = EnchantType.None)
    {
        // 1번 튜토리얼이 진행 중일 때는 몬스터에게 데미지가 안 들어가도록. 
        if (!DT_CheckAttackState.isTutorialing01)
        {
            base.TakeDamage(damageAmount);

            if (HP.curValue <= 0) return;

            StartCoroutine(DamageFlash());

            //TODO: 피격받는 캐릭터의 방어력에 비례한 데미지 경감 계산
            float realDamage = Mathf.Max(damageAmount - def.curValue, 1); //방어력이 공격력보다 높으면, 1데미지만 들어가도록 계산

            HP.SubtractCurValue(realDamage); //damaageAmount 대신 경감데미지를 넣는다

            //UI에 데미지 표시;
            GameObject createDamageText = ObjectPool.Instance.SpawnFromPool("DamageText");
            createDamageText.transform.position = gameObject.transform.position;
            DamageTextManager Damage = createDamageText.GetComponent<DamageTextManager>();

            Damage.damage = (int)realDamage;
            Damage.SetTextColor(type);

            createDamageText.SetActive(true);
        }
    }

    public IEnumerator DamageFlash()
    {
        spriteRenderer.color = new Color(0.65f, 0.01f, 0.01f);

        yield return new WaitForSeconds(0.5f);

        spriteRenderer.color = Color.white;
    }

    // 넉백을 적용하는 메서드
    public void ApplyKnockback(Vector3 direction)
    {
        if (rb != null)
        {
            isKnockedback = true;
            knockbackEndTime = Time.time + knockbackDuration;
            rb.velocity = direction.normalized * knockbackForce;
        }
    }

    //Die 애니메이션 이벤트로 연결
    public void Despawn()
    {
        if (DT_CheckSkilledMonsters.isTutorialing02 && DT_CheckSkilledMonsters.checkMonsterCnt < 5)
        {
            DT_CheckSkilledMonsters.checkMonsterCnt++;
        }

        int monsterLayer = 7;
        int bossLayer = 13;
        if (gameObject.layer == monsterLayer)
        {
            GameManager.Instance.itemSpawnManager.DropMonsterItem(gameObject);
            gameObject.SetActive(false);
        }
        else if (gameObject.layer == bossLayer)
        {
            GameManager.Instance.itemSpawnManager.DropBossItem(gameObject);
            Destroy(gameObject);
        }
    }

    //몬스터가 처치되면, 설정한 값만큼 경험치 반환(드랍)
    public float ReturnExp()
    {
        if(HP.curValue <= 0)
        {
            return 10;
        }
        else
        {
            return 0;
        }
    }
}
