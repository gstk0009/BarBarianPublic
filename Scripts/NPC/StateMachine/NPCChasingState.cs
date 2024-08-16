
public class NPCChasingState : NPCBaseState
{
    public NPCChasingState(NPCStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.NPC.AutoMove = true;
    }

    public override void Exit()
    {
        base.Exit();
    }


    public override void Update()
    {
        base.Update();

        if (stateMachine.NPC.Agent != null)
        {
            // AutoMode: NPC 이동
            stateMachine.NPC.Agent.SetDestination(stateMachine.NPC.Target.transform.position);
        }

        if (!stateMachine.NPC.Agent.pathPending)
        {
            if (stateMachine.NPC.Agent.remainingDistance <= stateMachine.NPC.Agent.stoppingDistance)
            {
                stateMachine.ChangeState(stateMachine.IdleState);
            }
            else
            {
                playRunAnim();
            }
        }
    }
}
