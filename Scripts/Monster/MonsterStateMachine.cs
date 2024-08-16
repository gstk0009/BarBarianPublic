public class MonsterStateMachine : StateMachine
{
    public Monster monster { get; }
    public MonsterIdleState IdleState { get; private set; }
    public MonsterWanderState WanderState { get; private set; }
    public MonsterAttackState AttackState { get; private set; }
    public MonsterChaseState ChaseState{ get; private set; }

    public MonsterStateMachine(Monster monster)
    {
        this.monster = monster;
        IdleState = new MonsterIdleState(this);
        WanderState = new MonsterWanderState(this);
        AttackState = new MonsterAttackState(this);
        ChaseState = new MonsterChaseState(this);
    }
}
