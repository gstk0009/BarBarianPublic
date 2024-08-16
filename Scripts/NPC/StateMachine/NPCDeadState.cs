using UnityEngine;

public class NPCDeadState : NPCBaseState
{
    private bool isDead = true;
    public NPCDeadState(NPCStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.NPC.isDeadState = true;
        timer = 0;
        delayTime = 1f;
        stateMachine.NPC.Animation.PlayAnimation(stateMachine.NPC.AnimationData.CombatDead);

        SoundManager.Instance.SetCurDungeonBGM();
    }

    public override void Exit()
    {
        base.Exit();
    }


    public override void Update()
    {
        base.Update();
        if (isDead)
        {
            timer += Time.deltaTime;
            if (timer > delayTime)
            {
                isDead = false;
                Exit();
                stateMachine.NPC.DieAnimationEvent();
            }
        }
    }
}
