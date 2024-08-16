using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttackState : NPCBaseState
{
    private bool isNotReadyAttack = false;

    public NPCAttackState(NPCStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();

        isNotReadyAttack = true;
        timer = 0;
        delayTime = 0.7f;
        stateMachine.NPC.Animation.PlayAnimation(stateMachine.NPC.AnimationData.CombatDraw);

        if (stateMachine.NPC.otherObject != null)
            NpcAttackDirection();
    }

    public override void Exit()
    {
        stateMachine.NPC.isAttack = false;
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
       
        if (isNotReadyAttack)
        {
            timer += Time.deltaTime;
            if (timer > delayTime)
            {
                NPCAttack();
                isNotReadyAttack = false;
            }
        }

        //공격 후 일정 시간이 지났으면
        if (!isNotReadyAttack && Time.time - stateMachine.NPC.lastAttackTime > stateMachine.NPC.npcStat.SKillSpeed.curValue)
        {
            stateMachine.NPC.particle.SetActive(false);
            if(stateMachine.NPC.AutoMove) stateMachine.ChangeState(stateMachine.ChasingState);
            else stateMachine.ChangeState(stateMachine.FormationState);
        }
    }

    private void NPCAttack()
    {
        stateMachine.NPC.SkillActivate();
        stateMachine.NPC.lastAttackTime = Time.time;
        Attack();
        stateMachine.NPC.isAttack = true;
    }

    private void Attack()
    {
        stateMachine.NPC.Animation.PlayAnimation(stateMachine.NPC.AnimationData.CombatAttack1);
    }

    private void NpcAttackDirection()
    {
        Vector2 monsterPostion = stateMachine.NPC.otherObject.transform.position;

        Vector2 NpcPosition = stateMachine.NPC.transform.position;

        Vector2 NpcAttackDirection = (monsterPostion - NpcPosition).normalized;

        float xinput = 0f;
        float yinput = 0f;

        if (NpcAttackDirection.x > 0.5f)
            xinput = 1f;
        else
            xinput = -1f;
        if (NpcAttackDirection.y > 0.5f)
            yinput = 1f;
        else
            yinput = -1f;

        stateMachine.NPC.Animation.SetInputXY(xinput, yinput);
    }

}
