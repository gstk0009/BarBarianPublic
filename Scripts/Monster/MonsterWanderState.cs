using UnityEngine;

public class MonsterWanderState : MonsterBaseState
{
    public MonsterWanderState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.monster.agent.speed = stateMachine.monster.stats.walkSpeed.curValue;
        StartAnimation(stateMachine.monster.AnimationData.MoveParameterHash);
        stateMachine.monster.agent.isStopped = false;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.monster.AnimationData.MoveParameterHash);
    }

    public override void Update()
    {
        base.Update();

        Collider2D detectRay = Physics2D.OverlapCircle(stateMachine.monster.transform.position, stateMachine.monster.stats.detectRadius.curValue, stateMachine.monster.targetLayer);

        if (detectRay)
        {
            stateMachine.monster.target = detectRay.transform;
            stateMachine.ChangeState(stateMachine.ChaseState);
            return;
        }

        stateMachine.monster.LookAt();

        if (stateMachine.monster.agent.remainingDistance <= stateMachine.monster.agent.stoppingDistance)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

    }
}
