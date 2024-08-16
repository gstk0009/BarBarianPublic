using UnityEngine;

public class BossAttackState : BossBaseState
{
    private float timer;
    private float interval = 2.5f; // 1 second interval
    int attack = Animator.StringToHash("Attack");
    int attack2 = Animator.StringToHash("Attack2");
    int dash = Animator.StringToHash("Dash");
    int threeCombo = Animator.StringToHash("ThreeComboAtk");
    int combo = Animator.StringToHash("Combo");
    int type;

    public BossAttackState(BossStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.Boss.rigidBody.velocity = Vector3.zero;
        Attack();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(type);
    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (timer >= interval)
        {
            timer = 0f;
            stateMachine.ChangeState(stateMachine.ChaseState);
        }
    }

    public void Attack()
    {
        type = AttackType();

        StartAnimation(type);
        if (stateMachine.Boss.stats.HP.curValue / stateMachine.Boss.stats.HP.maxValue < 0.5) //남은 체력이 50% 이하이면 콤보어택
        {
            StartAnimation(combo);
        }
    }

    private int AttackType()
    {
        int attacktype = Random.Range(0, 100);

        if (attacktype < 40)
        {
            return attack;
        }
        else if (attacktype >= 40 && attacktype < 60 )
        {
            return attack2;
        }
        else if (attacktype >= 60 && attacktype < 80)
        {
            return threeCombo;
        }
        else
        {
            return dash;
        }
    }


}