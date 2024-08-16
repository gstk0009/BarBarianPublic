using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    public PlayerInput playerInput { get; private set; }
    public PlayerInput.PlayerMovementActions playerMovementActions { get; private set; }
    public PlayerInput.PlayerMouseAndinterfactActions playerMouseAndInteractActions { get; private set; }
    public PlayerInput.PlayerSkillsActions playerSkillActions { get; private set; }
    private void Awake()
    {
        playerInput = new PlayerInput();
        playerMovementActions = playerInput.PlayerMovement;
        playerMouseAndInteractActions = playerInput.PlayerMouseAndinterfact;
        playerSkillActions = playerInput.PlayerSkills;
    }

   
    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    
}
