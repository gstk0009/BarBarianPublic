using UnityEngine;

public class MonsterAttackState : MonsterBaseState
{
    private float timer;
    public MonsterAttackState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.monster.agent.speed = stateMachine.monster.stats.runSpeed.curValue;
        StartAnimation(stateMachine.monster.AnimationData.AttackParameterHash);
        stateMachine.monster.agent.isStopped = false;
        timer = 0f;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.monster.AnimationData.AttackParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.monster.target == null || stateMachine.monster.agent.enabled == false )
            return;

        if (Player.Instance.isPlayerInteracting)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }

        Collider2D detectRay = Physics2D.OverlapCircle(stateMachine.monster.transform.position, stateMachine.monster.stats.detectRadius.curValue, stateMachine.monster.targetLayer);
        Collider2D attackRay = Physics2D.OverlapCircle(stateMachine.monster.transform.position, stateMachine.monster.stats.attackDistance.curValue, stateMachine.monster.targetLayer);

        timer += Time.deltaTime;

        if (attackRay && timer >= stateMachine.monster.stats.attackRate.curValue)
        {
            if (attackRay.gameObject.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(stateMachine.monster.stats.GetAtk());
            }
            timer = 0f;
        }


        else // 타겟과 너무 멀리 떨어지면 -> 다시 Idle State 로 
        {
            if (detectRay)
            {
                stateMachine.monster.agent.SetDestination(stateMachine.monster.target.transform.position);
                stateMachine.monster.Targeting();

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