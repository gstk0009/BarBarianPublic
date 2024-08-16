using UnityEngine;
using UnityEngine.AI;

public enum NPCType
{
    Spear,
    Bow,
    Wand
}
public class NPC : DungeonThings
{
    [field: Header("NPC Icon")]
    public Sprite npcIcon;

    [field: Header("Animations")]
    [field: SerializeField] public PlayerAndNpcAnimationData AnimationData { get; private set; }
    //[field: SerializeField] public PlayerAndNpcAnimation Animation { get; private set; }

    [field: Header("SkillSO")]
    //NPC 스킬
    //public SkillBase npcSkill;
    public GameObject[] SkillPrefab;
    public GameObject particle {  get; private set; }
    public NavMeshAgent Agent { get; private set; }
    public MatrixType matrixType;
    private NPCStateMachine stateMachine;
    public GameObject Target { get; set; }
    public bool AutoMove = true;
    public bool isAttack = false;
    public bool isDeadState = false;
    public float lastAttackTime;

    [SerializeField] public NPCType npcType;
    [SerializeField]public Collider2D otherObject;
    [SerializeField]public AttackType attackType;

    public LayerMask atkTarget;     // Lay가 충돌할 레이어(공격할 대상)

    //NPC 스탯
    public NPCStat npcStat;

    public Rigidbody2D rigid;



    private void Awake()
    {
        AnimationData = new PlayerAndNpcAnimationData();
        AnimationData.InitializeNPC(npcType);
        Agent = GetComponent<NavMeshAgent>();
        stateMachine = new NPCStateMachine(this);
        npcStat = GetComponent<NPCStat>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if(npcType == NPCType.Wand && gameObject.layer == (int)LayerType.NPC)
        {
            particle = Instantiate(SkillPrefab[1]);
        }
        else
        {
            particle = Instantiate(SkillPrefab[0]);
        }
        particle.transform.SetParent(DataManager.Instance.gameObject.transform);
        particle.SetActive(false);
        matrixType = MatrixType.Two_Two;
        AutoMove = true;
        Agent.updateRotation = false; // Agent 가 경로를 따라 이동하며 회전할 것인가
        Agent.updateUpAxis = false; // 경로를 따라 이동할 때, Agent 가 항상 지형의 위쪽을 향하도록 할 것인가
        isDeadState = false;
        stateMachine.ChangeState(stateMachine.IdleState);
    }
  

    private void Update()
    {
        stateMachine.Update();
        Animation.NpcAnimUpdate(npcType);
        if (npcStat.HP.curValue > 0 && Agent.remainingDistance > Agent.stoppingDistance)
        {
            Vector2 direction = Agent.velocity.normalized;
            Animation.SetInputXY(direction.x, direction.y);
        }
    }

    private void OnDestroy()
    {
        Destroy(particle);
        if (GameManager.Instance.NPCTargetSystem.TargetList.Contains(gameObject))
        {
            GameManager.Instance.NPCTargetSystem.TargetList.Remove(gameObject);
        }

        if (GameManager.Instance.SpawnersManager.NPCSpawner.FriendlyNPCs.Contains(gameObject))
        {
            GameManager.Instance.SpawnersManager.NPCSpawner.FriendlyNPCs.Remove(gameObject);
        }

        if (GameManager.Instance.NPCTargetSystem.NPCIndex.Contains(gameObject))
        {
            GameManager.Instance.NPCTargetSystem.NPCIndex.Remove(gameObject);
            SetMatrix();
        }
    }

    public void DieAnimationEvent()
    {
        OnDestroy();
        Destroy(gameObject);
    }

    public void SetMatrix()
    {
        switch (matrixType)
        {
            case MatrixType.One_Four:
                for (int i = 1; i < GameManager.Instance.NPCTargetSystem.TargetList.Count; i++)
                {
                    GameManager.Instance.NPCTargetSystem.TargetList[i].GetComponent<NPC>().Target = GameManager.Instance.NPCTargetSystem.TargetList[i - 1];
                }
                break;
            case MatrixType.Two_Two:
                for (int i = 1; i < GameManager.Instance.NPCTargetSystem.TargetList.Count; i++)
                {
                    GameManager.Instance.NPCTargetSystem.TargetList[i].GetComponent<NPC>().Target = GameManager.Instance.NPCTargetSystem.TargetList[i / 2];
                }
                break;
        }
    }

    public void SkillActivate()
    {
        particle.SetActive(true);
        if( otherObject != null) particle.transform.position = otherObject.transform.position;
        Bullet particleData = particle.GetComponentInChildren<Bullet>();
        if (particleData != null) 
        { 
            particleData.dtStat = npcStat;
            particleData.atkTarget = this.atkTarget;
        }
        if(npcType == NPCType.Spear)
        {
            SoundManager.Instance.PlaySoundEffect((int)SoundEffects.SpearSkill, 1, 1, false);
        }
    }

    public void SkillFinish()
    {
        particle.SetActive(false);
    }
}
