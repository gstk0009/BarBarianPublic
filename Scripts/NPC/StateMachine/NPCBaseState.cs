using UnityEngine;
using UnityEngine.InputSystem;

public class NPCBaseState : IState
{
    protected NPCStateMachine stateMachine;
    protected NpcInput NpcInputs;
    protected float delayTime = 0;
    protected float timer = 0;

    public int numberOfRays = 36;  // 발사할 광선의 개수
    public float radius = 1f;      // 광선의 길이(= NPC 공격 사거리?)


    public NPCBaseState(NPCStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    //State가 시작될 때 실행
    public virtual void Enter()
    {
        NpcInputs = new NpcInput();
        NpcInputs.npcInput.AutoMode.performed += AutoMode_performed;
        NpcInputs.npcInput.ChangeType1.performed += ChangeType1_performed;
        NpcInputs.npcInput.ChangeType2.performed += ChangeType2_performed;
        NpcInputs.Enable();

        stateMachine.NPC.rigid.velocity = Vector3.zero;
    }

    //State가 종료될 때실행
    public virtual void Exit()
    {
        NpcInputs.Disable();
    }
    
    //State가 실행중일 때 반복
    public virtual void Update()
    {
        if (!stateMachine.NPC.isDeadState)
        {
            stateMachine.NPC.otherObject = Physics2D.OverlapCircle(stateMachine.NPC.transform.position, stateMachine.NPC.npcStat.AtkRange.curValue, stateMachine.NPC.atkTarget);
            if ((int)stateMachine.NPC.npcStat.HP.curValue <= 0)
            {
                stateMachine.ChangeState(stateMachine.DeadState);
            }
        }
    }

    //모든 NPC 자동이동 모드로 불러들이기
    protected void AutoMode_performed(InputAction.CallbackContext obj)
    {
        if (!stateMachine.NPC.AutoMove)
        {
            stateMachine.NPC.AutoMove = true;
            stateMachine.ChangeState(stateMachine.ChasingState);
        }
    }

    protected void MoveToMousePosition()
    {
        // Get the mouse position in world coordinates
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        stateMachine.NPC.Agent.SetDestination(mousePosition);
    }


    //AutoMode ON: 단축키 입력(Alt+1, Alt+2)에 따른 포지션 변경
    protected void ChangeType2_performed(InputAction.CallbackContext obj)
    {
        stateMachine.NPC.matrixType = MatrixType.One_Four;
        stateMachine.NPC.SetMatrix();
    }

    protected void ChangeType1_performed(InputAction.CallbackContext obj)
    {
        stateMachine.NPC.matrixType = MatrixType.Two_Two;
        stateMachine.NPC.SetMatrix();
    }

    protected void playRunAnim()
    {
        stateMachine.NPC.Animation.PlayAnimation(stateMachine.NPC.AnimationData.Run);
    }

    protected void CastRadialRays()
    {
        if (stateMachine.NPC.otherObject && Time.time - stateMachine.NPC.lastAttackTime > stateMachine.NPC.npcStat.SkillCoolTime.curValue)
        {
            stateMachine.ChangeState(stateMachine.AttackState);
        }
    }

    public void HandleInput()
    {
        
    }

    public void PhysicsUpdate()
    {
        
    }
}
