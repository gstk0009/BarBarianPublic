using System;
using UnityEngine;

public class BossBaseState : IState
{
    protected BossStateMachine stateMachine;
    public static event Action OnBossDie;


    public BossBaseState(BossStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    //State가 시작될 때 실행
    public virtual void Enter()
    {

    }

    //State가 종료될 때실행
    public virtual void Exit()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }
    public void HandleInput()
    {

    }

    //State가 실행중일 때 반복
    public virtual void Update()
    {
        TargetSetting();
        OnDie();
    }

    private void OnDie()
    {
        if ((int)stateMachine.Boss.stats.HP.curValue <= 0 && !stateMachine.Boss.IsDead)
        {
            stateMachine.Boss.animator.Play(stateMachine.Boss.AnimationData.DeadAnim);
            stateMachine.Boss.IsDead = true;

            GameManager.Instance.MoveStageController.ClearStageObjs();

            if (GameManager.Instance.MainStageIdx == 2)
            {
                DataManager.Instance.currentPlayer.clearCnt++;
            }

            OnBossDie?.Invoke();

        }
    }

    protected void TargetSetting()
    {
        if(stateMachine.Boss.bossCollider.enabled)
        {
            stateMachine.Boss.searchTargetCollider = Physics2D.OverlapCircle(stateMachine.Boss.transform.position, 5, stateMachine.Boss.targetLayer); //(오브젝트 현재위치, 탐색범위(반지름), 타겟 레이어)
            stateMachine.Boss.attackTargetCollider = Physics2D.OverlapCircle(stateMachine.Boss.transform.position, 3, stateMachine.Boss.targetLayer); //(오브젝트 현재위치, 탐색범위(반지름), 타겟 레이어)
        }
    }

    protected void StartAnimation(int animatorHash)
    {
        stateMachine.Boss.animator.SetBool(animatorHash, true);
    }
    protected void StartAnimationTrigger(int animatorHash)
    {
        stateMachine.Boss.animator.SetTrigger(animatorHash);
    }
    protected void StopAnimation(int animatorHash)
    {
        stateMachine.Boss.animator.SetBool(animatorHash, false);
    }


}
