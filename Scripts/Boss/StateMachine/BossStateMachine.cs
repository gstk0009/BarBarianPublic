
public class BossStateMachine : StateMachine
{
    public BossMonster Boss { get; }

    public BossIdleState IdleState { get; }
    public BossChaseState ChaseState { get; }
    public BossAttackState AttackState { get; }

    public BossStateMachine(BossMonster boss)
    {
        this.Boss = boss;

        IdleState = new BossIdleState(this);
        ChaseState = new BossChaseState(this);
        AttackState = new BossAttackState(this);

    }

}
