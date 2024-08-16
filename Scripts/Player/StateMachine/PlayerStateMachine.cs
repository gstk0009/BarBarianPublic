using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Vector2 MovementInput;
    public float MovementSpeedModifier = 1f;
    public Player player { get; }

    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }
    public PlayerDefenceState DefenceState { get; private set; }
    public PlayerDrawSheathState DrawSheathState {get; private set;}
    public PlayerDeadState DeadState {get; private set;}
    public PlayerSkillState SkillState { get; private set; }
    public PlayerDashState DashState { get; private set; }

    public PlayerStateMachine(Player player)
    {
        this.player = player;

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        RunState = new PlayerRunState(this);
        AttackState = new PlayerAttackState(this);
        DefenceState = new PlayerDefenceState(this);
        DrawSheathState = new PlayerDrawSheathState(this);
        DeadState = new PlayerDeadState(this);
        DashState = new PlayerDashState(this);
    }

    public void ChangeStateToSkill(SkillBase skill, int idx)
    {
        SkillState = new PlayerSkillState(this, skill, idx);
        ChangeState(SkillState);
    }
}
