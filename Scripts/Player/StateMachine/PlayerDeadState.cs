
public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.player.Animation.PlayAnimation(stateMachine.player.AnimationData.SwordDead);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
