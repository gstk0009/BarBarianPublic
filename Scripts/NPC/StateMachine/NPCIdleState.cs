using UnityEngine;

public class NPCIdleState : NPCBaseState
{
    public NPCIdleState(NPCStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.NPC.Agent.ResetPath();
        playIdleAnim();
    }

    public override void Exit() 
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (!Player.Instance.isPlayerInteracting)
        {
            //AutoMode ON: PC가 플레이어와 너무 멀어지면, ChasingState로 전환
            if (IsFar() && stateMachine.NPC.AutoMove)
            {
                stateMachine.ChangeState(stateMachine.ChasingState);
            }
            else
            {
                CastRadialRays();
            }
        }
    }

    private void playIdleAnim()
    {
        stateMachine.NPC.Animation.PlayAnimation(stateMachine.NPC.AnimationData.Idle);
    }

    private bool IsFar()
    {
        if (stateMachine.NPC.Target != null)
        {
            Vector3 targetPosition = stateMachine.NPC.Target.transform.position;
            Vector3 npcPosition = stateMachine.NPC.transform.position;

            float distance = Vector2.Distance(targetPosition, npcPosition);

            return distance > stateMachine.NPC.Agent.stoppingDistance;
        }
        return false;
    }
}
