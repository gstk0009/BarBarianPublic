using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class Monster : DungeonThings
{

    private MonsterStateMachine stateMachine;
    public Transform target;

    [field: SerializeField] public MonsterSO MonsterData { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public MonsterAnimationData AnimationData { get; private set; }


    [Header("Attack")] 

    public LayerMask targetLayer;

    public NavMeshAgent agent;

    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public MonsterStat stats;

    public bool isDead = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        AnimationData.Initialize();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        agent.updateRotation = false; // Agent 가 경로를 따라 이동하며 회전할 것인가
        agent.updateUpAxis = false; // 경로를 따라 이동할 때, Agent 가 항상 지형의 위쪽을 향하도록 할 것인가

        stats = GetComponent<MonsterStat>();

        stateMachine = new MonsterStateMachine(this);
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.IdleState);
        SetAnimationSpeed();
        GetRandomNavMeshPosition();
    }

    private void OnEnable() // 반환되었다가, 다시 생성했을 때
    {
        stateMachine.ChangeState(stateMachine.IdleState);
        spriteRenderer.color = Color.white;
        isDead = false;
        stats.InitializeStats();
        GetRandomNavMeshPosition();
    }

    private void Update()
    {
        stateMachine.Update();

        if (stats.isFrozenState)
        {
            SetAnimationSpeed();
            agent.speed = stats.runSpeed.curValue;
        }
    }

    public void LookAt() // 타겟 방향으로 
    {
        if (agent.destination.x < transform.position.x) // 목적지가 타겟보다 왼쪽에 있다면, 왼쪽을 바라보게
            spriteRenderer.flipX = true;

        else // 그렇지 않다면 오른쪽을 바라보게
            spriteRenderer.flipX = false;
    }

    public void Targeting() // 타겟 방향으로 
    {
        if (target.transform.position.x < transform.position.x) // 목적지가 타겟보다 왼쪽에 있다면, 왼쪽을 바라보게
            spriteRenderer.flipX = true;

        else // 그렇지 않다면 오른쪽을 바라보게
            spriteRenderer.flipX = false;
    }
    public void GetRandomNavMeshPosition()
    {
        if (stateMachine.currentState != stateMachine.IdleState)
            return;

        if (agent.enabled != true)
            return;
        
        Vector3 randomDirection = Random.insideUnitSphere * stateMachine.monster.stats.wanderRadius.curValue;
        randomDirection += transform.position;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, stateMachine.monster.stats.wanderRadius.curValue, NavMesh.AllAreas);
        stateMachine.monster.agent.SetDestination(hit.position);

        stateMachine.ChangeState(stateMachine.WanderState);
    }

    public void SetAnimationSpeed()
    {
        animator.SetFloat("AttackSpeed", 1 / stats.attackRate.curValue);
    }
}