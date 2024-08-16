using UnityEngine;

public class PlayerDefenceState : PlayerBaseState
{
    public PlayerDefenceState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0f;
        isDefence = true;
        base.Enter();
        Timer = 0;
        DelayTime = 0.3f;
        stateMachine.player.Animation.PlayAnimation(stateMachine.player.AnimationData.SwordDodge);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        defenceWithWalk();
        base.Update();
    }

    private void defenceWithWalk()
    {
        if (isDefence)
        {
            Timer += Time.deltaTime;
            if (Timer >= DelayTime)
            {
                isDefence = false;
            }
        }
        else
        {
            if (inputController.playerMovementActions.Move.IsPressed())
            {
                stateMachine.ChangeState(stateMachine.WalkState);
            }
            else
            {
                stateMachine.ChangeState(stateMachine.IdleState);
            }
        }
    }
}
