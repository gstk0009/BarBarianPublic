using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0f;
        isIdle = true;
        base.Enter();
        Timer = 0;
        DelayTime = 0.3f;
        if (inputController.playerMovementActions.Move.IsPressed())
        {
            stateMachine.ChangeState(stateMachine.WalkState);
            return;
        }
        playIdleAnim();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        idleWithAttackOrDefence();
    }

    protected override void OnWalkPerformed(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.WalkState);
        base.OnWalkPerformed(context);
    }

    private void idleWithAttackOrDefence()
    {
        if (isIdle)
        {
            Timer += Time.deltaTime;
            if (Timer >= DelayTime)
            {
                isIdle = false;
            }
        }
        else if (isAttackState)
        {
            if (inputController.playerMovementActions.Attack.IsPressed())
            {
                stateMachine.ChangeState(stateMachine.AttackState);
            }
            else if(inputController.playerMovementActions.Defence.IsPressed())
            {
                stateMachine.ChangeState(stateMachine.DefenceState);
            }
        }
    }

    private void playIdleAnim()
    {
        if (isAttackState)
            stateMachine.player.Animation.PlayAnimation(stateMachine.player.AnimationData.SwordIdle);
        else
        {
            stateMachine.player.Animation.PlayAnimation(stateMachine.player.AnimationData.Idle);
        }
    }
}
