using UnityEngine;

public class MonsterIdleState : MonsterBaseState
{
    private float timer;

    public MonsterIdleState(MonsterStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timer = 0f;
        StartAnimation(stateMachine.monster.AnimationData.IdleParameterHash);

        stateMachine.monster.agent.velocity = Vector3.zero;
        stateMachine.monster.agent.ResetPath();
        stateMachine.monster.agent.isStopped = true;
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.monster.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if(Player.Instance.isPlayerInteracting)
        {
            return;
        }


        Collider2D detectRay = Physics2D.OverlapCircle(stateMachine.monster.transform.position, stateMachine.monster.stats.detectRadius.curValue, stateMachine.monster.targetLayer);

        if (detectRay)
        {
            stateMachine.monster.target = detectRay.transform;
            
            stateMachine.ChangeState(stateMachine.ChaseState);
            return;
        }

        timer += Time.deltaTime;

        if (timer >= stateMachine.monster.stats.wanderWaitTime.curValue)
        {
            timer = 0f;
            stateMachine.monster.GetRandomNavMeshPosition();
        }
    }
}
