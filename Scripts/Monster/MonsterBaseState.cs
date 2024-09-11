public class MonsterBaseState :  IState
{
    protected MonsterStateMachine stateMachine;

    public MonsterBaseState(MonsterStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }
    public void HandleInput()
    {

    }

    public virtual void Update()
    {
        if (Player.Instance.isPlayerInteracting || DT_CheckAttackState.isTutorialing01)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }

        Ondie();
    }

    private void Ondie()
    {
        if (stateMachine.monster.stats.HP.curValue <= 0 && !stateMachine.monster.isDead)
        {
            stateMachine.monster.isDead = true;
            stateMachine.monster.agent.speed = 0;
            stateMachine.monster.stats.attackRate.curValue = 1000;
            SoundManager.Instance.PlaySoundEffect((int)SoundEffects.GoblinDie);
            stateMachine.monster.animator.SetTrigger("Die");
            stateMachine.currentState.Exit();
        }
    }

    protected void StartAnimation(int animatorHash)
    {
        stateMachine.monster.animator.SetBool(animatorHash, true);
    }
    protected void StartAnimationTrigger(int animatorHash)
    {
        stateMachine.monster.animator.SetTrigger(animatorHash);
    }
    protected void StopAnimation(int animatorHash)
    {
        stateMachine.monster.animator.SetBool(animatorHash, false);
    }
}
