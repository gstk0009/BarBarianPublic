using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    //use this for initialisation
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        EventHandler.PlayerMovementEvent += PlayAnim;
        EventHandler.PlayerMovementInputEvent += SetAnimationInputParameters;
    }

    private void OnDisable()
    {
        EventHandler.PlayerMovementEvent -= PlayAnim;
        EventHandler.PlayerMovementInputEvent -= SetAnimationInputParameters;
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
