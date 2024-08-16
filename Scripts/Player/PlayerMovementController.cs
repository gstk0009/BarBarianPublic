using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Vector2 dashDistance;
    public bool IsAttack = false;
    private bool isDash = false;

    private void Awake()
    {
        if (GameManager.Instance.playerMovementController == null)
        {
            rb2D = GetComponent<Rigidbody2D>();
            GameManager.Instance.playerMovementController = this;
        }
    }

    public void PlayerMove(Vector2 movement)
    {
        rb2D.velocity = movement;
    }

    public void PlayerDash(Direction DirectionWay)
    {
        dashDistance = Vector2.zero ;
        switch (DirectionWay)
        {
            case Direction.up:
                dashDistance = Vector2.up ;
                break;
            case Direction.down:
                dashDistance = Vector2.down ;
                break;
            case Direction.left:
                dashDistance = Vector2.left ;
                break;
            case Direction.right:
                dashDistance = Vector2.right;
                break;
        }
        isDash = true;
    }

    public void SetIsDash(bool Dash)
    {
        isDash = Dash;
    }

    private void FixedUpdate()
    {
        if (isDash)
        {
            Player.Instance.Animation.SetInputXY(dashDistance.x, dashDistance.y);
            rb2D.AddForce(dashDistance * Time.deltaTime * 100);
        }
    }
}
