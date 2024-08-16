
using UnityEngine;

public class BossChaseState : BossBaseState
{
    //public float moveSpeed = 0.5f;
    private float timer;
    private float interval = 3.0f; // 1 second interval


    public BossChaseState(BossStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Boss.AnimationData.MoveParameterHash);
        stateMachine.Boss.delayTime();
    }

    public override void Exit()
    {
        stateMachine.Boss.moveSpeed = 0f;
        StopAnimation(stateMachine.Boss.AnimationData.MoveParameterHash);
        base.Exit();

    }

    public override void Update()
    {
        base.Update();
        AttackTarget();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Move();

    }

    private void Move()
    {
        if (stateMachine.Boss.searchTargetCollider != null)
        {           
            // 타겟 방향 계산
            Vector2 direction = (stateMachine.Boss.searchTargetCollider.gameObject.transform.position - stateMachine.Boss.transform.position).normalized;
            // Rigidbody2D의 velocity 설정
            stateMachine.Boss.rigidBody.velocity = direction * stateMachine.Boss.moveSpeed;
        }
    }

    private void AttackTarget()
    {
        timer += Time.deltaTime;

        if (stateMachine.Boss.searchTargetCollider == null)
        {
            stateMachine.Boss.rigidBody.velocity = Vector3.zero;
            stateMachine.ChangeState(stateMachine.IdleState);
        }

        if (timer >= interval && stateMachine.Boss.attackTargetCollider != null)
        {
            timer = 0f;

            stateMachine.Boss.rigidBody.velocity = Vector3.zero;
            stateMachine.ChangeState(stateMachine.AttackState);
        }

    }

    
}