using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunState : PlayerBaseState
{
    private Vector2 beforeMoveInput;
    public PlayerRunState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 1f;
        moveRunSpeed = 1.5f;
        isRun = true;
        base.Enter();
        Timer = 0;
        DelayTime = 0.2f;
        playRunAnim();
        beforeMoveInput = movementInput;
    }

    public override void Exit()
    {
        moveRunSpeed = 1f;
        base.Exit();
    }

    public override void Update()
    {
        if (movementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
        runWithAttackOrDefence();
        updateRunAnim();
        base.Update();
    }

    protected override void OnRunCanceled(InputAction.CallbackContext context)
    {
        base.OnRunCanceled(context);
        if (movementInput != Vector2.zero)
            stateMachine.ChangeState(stateMachine.WalkState);
    }

    private void runWithAttackOrDefence()
    {
        if (isRun)
        {
            Timer += Time.deltaTime;
            if (Timer >= DelayTime)
            {
                isRun = false;
            }
        }
        else if (isAttackState)
        {
            if (inputController.playerMovementActions.Attack.IsPressed())
            {
                stateMachine.ChangeState(stateMachine.AttackState);
            }
            else if (inputController.playerMovementActions.Defence.IsPressed())
            {
                stateMachine.ChangeState(stateMachine.DefenceState);
            }
        }
    }

    private void playRunAnim()
    {
        stateMachine.player.Animation.PlayAnimation(stateMachine.player.AnimationData.Run);
    }

    private void updateRunAnim()
    {
        if (beforeMoveInput != movementInput)
        {
            playRunAnim();
            beforeMoveInput = movementInput;
        }
    }
}
