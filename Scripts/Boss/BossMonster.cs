using System.Collections;
using UnityEngine;

public class BossMonster : DungeonThings
{
    private BossStateMachine stateMachine;
    public Rigidbody2D rigidBody;
    public SpriteRenderer spriteRenderer;
    public bool isFacingRight = true; // 오브젝트가 오른쪽을 바라보고 있는지 여부
    public bool IsDead = false;

    public LayerMask targetLayer;
    public Collider2D bossCollider {  get; private set; }
    public Collider2D searchTargetCollider;
    public Collider2D attackTargetCollider;

    //Monster 스탯
    public MonsterStat stats;
    public float moveSpeed = 0f;

    [field: Header("Animations")]
    [field: SerializeField] public MonsterAnimationData AnimationData { get; private set; }
    public Animator animator;

    private void Awake()
    {
        AnimationData.Initialize();
        animator = GetComponent<Animator>();
        stats = GetComponent<MonsterStat>();
        stateMachine = new BossStateMachine(this);
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bossCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }


    public void FlipObject()
    {
        if(searchTargetCollider != null)
        {
            // 타겟 오브젝트가 오브젝트보다 왼쪽에 있는지 확인
            if (searchTargetCollider.transform.position.x < stateMachine.Boss.transform.position.x && stateMachine.Boss.isFacingRight)
            {
                Flip();
            }
            else if (searchTargetCollider.transform.position.x > stateMachine.Boss.transform.position.x && !stateMachine.Boss.isFacingRight)
            {
                Flip();
            }
        }
    }


    protected void Flip()
    {
        // 오브젝트 반전
        stateMachine.Boss.isFacingRight = !stateMachine.Boss.isFacingRight;
        Vector3 theScale = stateMachine.Boss.transform.localScale;
        theScale.x *= -1;
        stateMachine.Boss.transform.localScale = theScale;
    }

    public void SummonFinished()
    {
        bossCollider.enabled = true;
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    public void delayTime()
    {
        StartCoroutine(delay());
    }

    private IEnumerator delay()
    {
        yield return new WaitForSeconds(2.0f);
        moveSpeed = 0.5f;
    }

    private void Dash()
    {
        if (searchTargetCollider != null)
        {
            // 타겟 방향 계산
            Vector2 direction = (searchTargetCollider.gameObject.transform.position - this.transform.position).normalized;
            // Rigidbody2D의 velocity 설정
            rigidBody.velocity = direction * 5f;
        }
    }

    private void DashStop()
    {
        rigidBody.velocity = Vector3.zero;
    }
}
