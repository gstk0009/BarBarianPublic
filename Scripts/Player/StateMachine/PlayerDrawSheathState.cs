using UnityEngine;

public class PlayerDrawSheathState : PlayerBaseState
{
    private bool hasIncreasedCheckState = false; // 플래그 추가

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
            PlayerConditionUI.Instance.SetAtkState();
            stateMachine.player.Animation.PlayAnimation(stateMachine.player.AnimationData.SwordDraw);
            // 조건을 만족하고, 플래그가 false일 때만 checkAttackState 증가
            if (DT_CheckAttackState.isTutorialing01 && DT_CheckAttackState.checkAttackState < 3 && !hasIncreasedCheckState)
            {
                DT_CheckAttackState.checkAttackState++;
                hasIncreasedCheckState = true; // 플래그 설정
            }
        }
        else
        {
            PlayerConditionUI.Instance.SetIdleState();
            stateMachine.player.Animation.PlayAnimation(stateMachine.player.AnimationData.SwordSheath);
        }
        isAttackState = !isAttackState;

        
    }

    public override void Exit()
    {
        base.Exit();
        hasIncreasedCheckState = false; // 상태에서 나갈 때 플래그 초기화
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
