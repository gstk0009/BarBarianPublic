using UnityEngine;

public class PlayerSkillState : PlayerBaseState
{
    private SkillBase skill;
    private int idx;

    public PlayerSkillState(PlayerStateMachine stateMachine, SkillBase skill, int idx) : base(stateMachine)
    {
        this.skill = skill;
        this.idx = idx;
    }

    public override void Enter()
    {
        if (skill.skillType == SkillType.Attack)
        {
            stateMachine.MovementSpeedModifier = 0f;
        }
        else
        {
            stateMachine.MovementSpeedModifier = 1f;
        }

        base.Enter();

        skill.Activate(stateMachine.player.playerStat);
        isSkill = true;

        if (isSkill)
        {
            PlayerSkillManager.Instance.StartSKillCoolDown(idx, skill);
        }

    }
    public override void Exit()
    {
        base.Exit();
        isSkill = false;

    }


    public override void Update()
    {
        base.Update();

        if (isSkill)
        {
            if (skill.CastTime > 0)
            {
                Timer += Time.deltaTime;
                if (Timer >= skill.CastTime)
                {
                    isSkill = false;
                }
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