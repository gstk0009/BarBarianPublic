using UnityEngine;

public class SpearNpcAnimationController : MonoBehaviour
{
    private Animator animator;

    //use this for initialisation
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EventHandler.SpearNpcMovementEvent += PlayAnim;
        EventHandler.SpearNpcMovementInputEvent += SetAnimationInputParameters;
    }

    private void OnDisable()
    {
        EventHandler.SpearNpcMovementEvent -= PlayAnim;
        EventHandler.SpearNpcMovementInputEvent -= SetAnimationInputParameters;
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