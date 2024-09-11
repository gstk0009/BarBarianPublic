using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerBaseState
{
    private Vector2 beforeMoveInput;

    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 1f;
        isWalk = true;
        base.Enter();
        Timer = 0;
        DelayTime = 0.2f;
        if (inputController.playerMovementActions.Run.IsPressed() && !isAttackState && stateMachine.player.playerStat.Stamina.curValue > 2f)
        {
            stateMachine.ChangeState(stateMachine.RunState);
            return;
        }
        beforeMoveInput = movementInput;
        playWalkAnim();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        walkWithAttackOrDefence();
        updateWalkAnim();
        base.Update();
    }

    protected override void OnRunPerformed(InputAction.CallbackContext context)
    {
        if (!isAttackState && stateMachine.player.playerStat.Stamina.curValue >= needRunStamina)
            stateMachine.ChangeState(stateMachine.RunState);
        base.OnRunPerformed(context);
    }

    protected override void OnWalkCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.IdleState);
        base.OnWalkCanceled(context);
    }

    private void walkWithAttackOrDefence()
    {
        if (isWalk)
        {
            Timer += Time.deltaTime;
            if (Timer >= DelayTime)
            {
                isWalk = false;
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

    private void playWalkAnim()
    {
        if (isAttackState)
            stateMachine.player.Animation.PlayAnimation(stateMachine.player.AnimationData.SwordWalk);
        else
            stateMachine.player.Animation.PlayAnimation(stateMachine.player.AnimationData.Walk);
    }

    private void updateWalkAnim()
    {
        if (beforeMoveInput != movementInput)
        {
            playWalkAnim();
            beforeMoveInput = movementInput;
        }
    }
}
