

public class BossIdleState : BossBaseState
{
    public BossIdleState(BossStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Boss.AnimationData.IdleParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Boss.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        base.Update();
        SearchTarget();
    }

    private void SearchTarget()
    {
        if (stateMachine.Boss.searchTargetCollider != null)
        {
            stateMachine.ChangeState(stateMachine.ChaseState);
        }
    }
}