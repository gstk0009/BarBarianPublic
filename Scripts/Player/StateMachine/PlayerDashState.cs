using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = 0f;
        base.Enter();
        isDash = true;
        Timer = 0;
        DelayTime = 0.6f;
        stateMachine.player.Animation.PlayAnimation(stateMachine.player.AnimationData.SwordLunge);
        GameManager.Instance.playerMovementController.PlayerDash(stateMachine.player.Animation.DirectionWay);
        PlayerSkillManager.Instance.StartSKillCoolDown((int)SkillIndex.Dash);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (isDash)
        {
            Timer += Time.deltaTime;
            if (Timer >= DelayTime)
            {
                isDash = false;
                GameManager.Instance.playerMovementController.SetIsDash(isDash);
            }
        }
        else
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}
