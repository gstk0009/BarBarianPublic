using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class NPCFormationState : NPCBaseState
{
    public NPCFormationState(NPCStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.NPC.AutoMove = false;
        GameManager.Instance.NPCTargetSystem.TargetList.Remove(stateMachine.NPC.gameObject);
        stateMachine.NPC.SetMatrix();
    }

    public override void Exit()
    {
        GameManager.Instance.NPCTargetSystem.TargetList.Add(stateMachine.NPC.gameObject);
        stateMachine.NPC.SetMatrix();
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.NPC.AutoMove)
        {
            stateMachine.ChangeState(stateMachine.ChasingState);
        }

        if (!stateMachine.NPC.Agent.pathPending)
        {
            if (stateMachine.NPC.Agent.remainingDistance <= stateMachine.NPC.Agent.stoppingDistance)
            {
                stateMachine.ChangeState(stateMachine.IdleState);
                return;
            }
            else
            {
                playRunAnim();
                return;
            }
        }
        CastRadialRays();
    }
}