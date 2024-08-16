using UnityEngine;

public class BowNpcAnimationController : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EventHandler.BowNpcMovementEvent += PlayAnim;
        EventHandler.BowNpcMovementInputEvent += SetAnimationInputParameters;
    }

    private void OnDisable()
    {
        EventHandler.BowNpcMovementEvent -= PlayAnim;
        EventHandler.BowNpcMovementInputEvent -= SetAnimationInputParameters;
    }

    public void PlayAnim(string playAnim)
    {
        animator.Play(playAnim, -1, 0f);
    }

    public void SetAnimationInputParameters(float inputX, float inputY, Direction direction, float speed)
    {
        animator.SetFloat("xInput", inputX);
        animator.SetFloat("yInput", inputY);
        animator.SetInteger("direction", (int)direction);
        animator.SetFloat("AttackSpeed", speed);
    }
}