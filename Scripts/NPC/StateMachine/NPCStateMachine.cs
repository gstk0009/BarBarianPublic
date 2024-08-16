

public class NPCStateMachine : StateMachine
{
    public NPC NPC { get; }

    public NPCIdleState IdleState { get; }
    public NPCChasingState ChasingState { get; }
    public NPCAttackState AttackState { get; }
    public NPCFormationState FormationState { get; }
    public NPCDeadState DeadState { get; }

    public NPCStateMachine(NPC npc)
    {
        this.NPC = npc;

        IdleState = new NPCIdleState(this);
        ChasingState = new NPCChasingState(this);
        AttackState = new NPCAttackState(this);
        FormationState = new NPCFormationState(this);
        DeadState = new NPCDeadState(this);
    }

}
