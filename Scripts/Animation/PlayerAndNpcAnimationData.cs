
public class PlayerAndNpcAnimationData
{
    // Base
    public string Idle { get; private set; }
    public string Walk { get; private set; }
    public string Run { get; private set; }
    public string Sleep { get; private set; }
    public string Sitting { get; private set; }
    public string SchockDown { get; private set; }

    // Sword
    public string SwordDraw { get; private set; }
    public string SwordSheath { get; private set; }
    public string SwordIdle { get; private set; }
    public string SwordWalk { get; private set; }
    // 앞구르기
    public string SwordLunge { get; private set; }
    // 방어
    public string SwordDodge { get; private set; }
    // 백스텝
    public string SwordRetreat { get; private set; }
    public string SwordSlash1 { get; private set; }
    public string SwordSlash2 { get; private set; }
    public string SwordThrust { get; private set; }
    public string SwordShieldBash { get; private set; }
    public string SwordHurt { get; private set; }
    public string SwordDead { get; private set; }

    // Spear
    public string SpearDraw { get; private set; }
    public string SpearSheath { get; private set; }
    public string SpearIdle { get; private set; }
    public string SpearWalk { get; private set; }
    // 앞구르기
    public string SpearLunge { get; private set; }
    // 백스텝
    public string SpearRetreat { get; private set; }
    public string SpearThrust { get; private set; }
    public string SpearThrust2 { get; private set; }
    public string SpearSlash { get; private set; }
    public string SpearDead { get; private set; }

    // Bow
    public string BowDraw { get; private set; }
    public string BowSheath { get; private set; }
    public string BowIdle { get; private set; }
    public string BowWalk { get; private set; }
    // 앞구르기
    public string BowLunge { get; private set; }
    // 백스텝
    public string BowRetreat { get; private set; }
    public string BowShootStraight { get; private set; }
    public string BowShootUp { get; private set; }
    public string BowDead { get; private set; }

    public string CombatIdle { get; private set; }
    public string CombatWalk { get; private set; }
    public string CombatDraw { get; private set; }
    public string CombatSheath { get; private set; }
    public string CombatAttack1 { get; private set; }
    public string CombatAttack2 { get; private set; }
    public string CombatAttack3 { get; private set; }
    public string CombatAttack4 { get; private set; }
    public string CombatDead { get; private set; }

    public void InitializePlayer()
    {
        // Base
        Idle = "IdleState";
        Walk = "WalkState";
        Run = "RunState";
        Sitting = "SittingState";
        Sleep = "SleepState";
        SchockDown = "SchockDownState";

        // Sword
        SwordDraw = "SwordDrawState";
        SwordSheath = "SwordSheathState";
        SwordIdle = "SwordIdleState";
        SwordWalk = "SwordWalkState";
        SwordLunge = "SwordLungeState";
        SwordDodge = "SwordDodgeState";
        SwordRetreat = "SwordRetreatState";
        SwordThrust = "SwordThrustState";
        SwordSlash1 = "SwordSlash1State";
        SwordSlash2 = "SwordSlash2State";
        SwordShieldBash = "SwordShieldBashState";
        SwordHurt = "SwordHurtState";
        SwordDead = "SwordDeadState";
    }

    public void InitializeNPC(NPCType npcType)
    {
        // Base
        Idle = "IdleState";
        Walk = "WalkState";
        Run = "RunState";
        Sitting = "SittingState";
        Sleep = "SleepState";
        SchockDown = "SchockDownState";

        switch (npcType)
        {
            case NPCType.Spear:
                CombatIdle = "SpearIdleState";
                CombatWalk = "SpearWalkState";
                CombatDraw = "SpearDrawState";
                CombatSheath = "SpearSheathState";
                CombatAttack1 = "SpearSlash1State";
                CombatAttack2 = "SpearThrustState";
                CombatAttack3 = "SpearThrust2State";
                CombatDead = "SpearDeadState";
                break;
            case NPCType.Bow:
                CombatIdle = "BowIdleState";
                CombatWalk = "BowWalkState";
                CombatDraw = "BowDrawState";
                CombatSheath = "BowSheathState";
                CombatAttack1 = "BowShootupState";
                CombatAttack2 = "BowShootStraightState";
                CombatDead = "BowDeadState";
                break;
            case NPCType.Wand:
                CombatIdle = "SwordIdleState";
                CombatWalk = "SwordWalkState";
                CombatDraw = "SwordDrawState";
                CombatSheath = "SwordSheathState";
                CombatAttack1 = "SwordSlash1State";
                CombatAttack2 = "SwordSlash2State";
                CombatAttack3 = "SwordThrustState";
                CombatAttack4 = "SwordShieldBashState";
                CombatDead = "SwordDeadState";
                break;
        }
    }
}
