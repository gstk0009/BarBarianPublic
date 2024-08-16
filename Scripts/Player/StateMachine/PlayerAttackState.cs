using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0f;
        base.Enter();
        Timer = 0;
        DelayTime = 0.5f;
        stateMachine.player.Animation.PlayAnimation(stateMachine.player.AnimationData.SwordThrust);
        isAttack = true;
        SoundManager.Instance.PlayAttackSound();

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        attackWithWalk();

        base.Update();
    }

    private void attackWithWalk()
    {
        if (isAttack)
        {
            Timer += Time.deltaTime;
            if (Timer >= DelayTime)
            {
                isAttack = false;
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
