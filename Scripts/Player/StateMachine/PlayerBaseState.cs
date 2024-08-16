using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected PlayerInputController inputController;
    protected float moveRunSpeed = 1f;
    protected float moveWalkSpeed = 2f;
    protected bool isAttack = false;
    protected bool isDefence = false;
    protected bool isSkill = false;
    protected bool isTest = false;
    protected bool isWalk = false;
    protected bool isRun = false;
    protected bool isIdle = false;
    protected bool isDrawSheath = false;
    protected bool isDead = false;
    protected bool isDash = false;
    public static bool isAttackState = false;
    protected float Timer;
    protected float DelayTime;

    protected Vector2 movementInput;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        inputController = stateMachine.player.playerInputController;
        AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();
    }

    public virtual void HandleInput()
    {
        if (!isAttack && !isDefence && !isDash)
            ReadMovementInput();
    }

    public virtual void PhysicsUpdate()
    {
        if(!Player.Instance.isPlayerInteracting)
        {
            inputController.playerMovementActions.Enable();
            GameManager.Instance.playerMovementController.PlayerMove(Move());
            
        }
        else
        {
            GameManager.Instance.playerMovementController.PlayerMove(Vector2.zero);
            inputController.playerMovementActions.Disable();
        }
    }

    public virtual void Update()
    {

    }

    protected virtual void AddInputActionsCallbacks()
    {
        inputController.playerMovementActions.Move.performed += OnWalkPerformed;
        inputController.playerMovementActions.Move.canceled += OnWalkCanceled;
        inputController.playerMovementActions.Run.performed += OnRunPerformed;
        inputController.playerMovementActions.Run.canceled += OnRunCanceled;
        inputController.playerMovementActions.Attack.started += OnAttackStarted;
        inputController.playerMovementActions.Defence.started += OnDefenceStarted;
        inputController.playerMovementActions.DrawSheath.started += OnSwardDraw;

        inputController.playerSkillActions.Skill1.started += OnSkill1;
        inputController.playerSkillActions.Skill2.started += OnSkill2;
    }

    protected virtual void RemoveInputActionsCallbacks()
    {
        inputController.playerMovementActions.Move.performed -= OnWalkPerformed;
        inputController.playerMovementActions.Move.canceled -= OnWalkCanceled;
        inputController.playerMovementActions.Run.performed -= OnRunPerformed;
        inputController.playerMovementActions.Run.canceled -= OnRunCanceled;
        inputController.playerMovementActions.Attack.started -= OnAttackStarted;
        inputController.playerMovementActions.Defence.started -= OnDefenceStarted;
        inputController.playerMovementActions.DrawSheath.started -= OnSwardDraw;

        inputController.playerSkillActions.Skill1.started -= OnSkill1;
        inputController.playerSkillActions.Skill2.started -= OnSkill2;
    }

    private void OnSwardDraw(InputAction.CallbackContext context)
    {
        if (!isDash)
        {
            SoundManager.Instance.PlaySoundEffect((int)SoundEffects.DrawSword);
            stateMachine.ChangeState(stateMachine.DrawSheathState);
        }
    }

    protected virtual void OnDefenceStarted(InputAction.CallbackContext context)
    {
        if (!isDefence && !isAttack && !stateMachine.player.isOpenInventory && isAttackState && !isDash)
            stateMachine.ChangeState(stateMachine.DefenceState);
    }

    protected virtual void OnAttackStarted(InputAction.CallbackContext context)
    {
        if (!isAttack && !isDefence && !isSkill && !stateMachine.player.isOpenInventory && isAttackState && !isDash)
            stateMachine.ChangeState(stateMachine.AttackState);
        
    }

    protected virtual void OnRunCanceled(InputAction.CallbackContext context)
    {
        
    }

    protected virtual void OnRunPerformed(InputAction.CallbackContext context)
    {
        if (!isAttack && !isDefence && !isSkill && !stateMachine.player.isOpenInventory && isAttackState && !isDash
            && !PlayerSkillManager.Instance.isSkillCooltime[(int)SkillIndex.Dash])
            stateMachine.ChangeState(stateMachine.DashState);
    }

    protected virtual void OnWalkCanceled(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnWalkPerformed(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnSkill1(InputAction.CallbackContext context)
    {
        if(!isSkill && !isDash)
            UseSkill((int)SkillIndex.Boob);
    }

    protected virtual void OnSkill2(InputAction.CallbackContext context)
    {
        if (!isSkill && !isDash)
            UseSkill((int)SkillIndex.Hooray);
    }

    private void UseSkill(int skillIndex)
    {
        if (PlayerSkillManager.Instance.isSkillCooltime[skillIndex] || isSkill)
            return;

        if (!isAttack && !isDefence && !stateMachine.player.isOpenInventory && isAttackState)
        {
            SkillBase skill = PlayerSkillManager.Instance.GetSkill(skillIndex);
            if (skill != null)
            {
                stateMachine.ChangeStateToSkill(skill, skillIndex);
            }
        }
    }

    private void ReadMovementInput()
    {
        // InputAction이 실행될 때 Vector2값 읽어오기
        movementInput = stateMachine.player.playerInputController.playerMovementActions.Move.ReadValue<Vector2>();
        stateMachine.MovementInput = movementInput;

        if (movementInput != Vector2.zero)
            stateMachine.player.Animation.SetInputXY(movementInput.x, movementInput.y);
    }

    private Vector2 Move()
    {
        float moveSpeed = GetMovementSpeed();
        Vector2 movementDirection = movementInput.normalized * moveSpeed;
        return movementDirection;
    }

    private float GetMovementSpeed()
    {
        float moveSpeed = stateMachine.player.playerStat.Dex.curValue * moveWalkSpeed * stateMachine.MovementSpeedModifier * moveRunSpeed;

        return moveSpeed;
    }
}

public enum SkillIndex
{
    Boob,
    Hooray,
    Dash
}
