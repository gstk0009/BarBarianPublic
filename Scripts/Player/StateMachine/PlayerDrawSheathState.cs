using UnityEngine;

public class PlayerDrawSheathState : PlayerBaseState
{
    public PlayerDrawSheathState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0f;
        isDrawSheath = true;
        base.Enter();
        Timer = 0;
        DelayTime = 0.7f;
        if (!isAttackState)
        {
            stateMachine.player.Animation.PlayAnimation(stateMachine.player.AnimationData.SwordDraw);
        }
        else
        {
            stateMachine.player.Animation.PlayAnimation(stateMachine.player.AnimationData.SwordSheath);
        }
        isAttackState = !isAttackState;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        DrawSheathDelay();

        base.Update();
    }

    private void DrawSheathDelay()
    {
        if (isDrawSheath)
        {
            Timer += Time.deltaTime;
            if (Timer >= DelayTime)
            {
                isDrawSheath = false;
            }
        }
        else
        {
            if (inputController.playerMovementActions.Move.IsPressed())
                stateMachine.ChangeState(stateMachine.WalkState);
            else
                stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}
