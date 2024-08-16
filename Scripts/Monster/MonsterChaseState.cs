using UnityEngine;

public class MonsterChaseState : MonsterBaseState
{
    public MonsterChaseState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.monster.agent.speed = stateMachine.monster.stats.runSpeed.curValue;
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

        if (stateMachine.monster.target == null || stateMachine.monster.agent.enabled == false)
            return;

        stateMachine.monster.agent.SetDestination(stateMachine.monster.target.position);
        stateMachine.monster.Targeting();

        Collider2D detectRay = Physics2D.OverlapCircle(stateMachine.monster.transform.position, stateMachine.monster.stats.detectRadius.curValue, stateMachine.monster.targetLayer);
        Collider2D attackRay = Physics2D.OverlapCircle(stateMachine.monster.transform.position, stateMachine.monster.stats.attackDistance.curValue, stateMachine.monster.targetLayer);

        if (attackRay)
        {
            stateMachine.monster.target = attackRay.transform;
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }

        else
        {
            if (detectRay)
            {
                return;
            }
            else
            {
                stateMachine.monster.target = null;
                stateMachine.ChangeState(stateMachine.IdleState);
                return;
            }
        }
    }
}