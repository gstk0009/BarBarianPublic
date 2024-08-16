using UnityEngine;

public class CabinDoor : MonoBehaviour
{
    private Animator Door;
    public Collider2D ChildCollider;
    private SpriteRenderer SpriteRenderer;
    private int DoorUp = 25;
    private int DoorDown = 5;

    private void Awake()
    {
        Door = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        CabinTop.PlayerEnter += DoorLayerSetting;
    }

    private void OnDisable()
    {
        CabinTop.PlayerEnter -= DoorLayerSetting;
    }

    private void DoorLayerSetting(bool isPlayerIn)
    {
        if (isPlayerIn)
            SpriteRenderer.sortingOrder = DoorUp;
        else
            SpriteRenderer.sortingOrder = DoorDown;
    }

    public void ColliderOnOff()
    {
        ChildCollider.enabled = !ChildCollider.enabled;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Door.SetBool("Enter", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Door.SetBool("Enter", false);
        }
    }
}
