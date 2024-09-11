using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : DungeonThings
{
    public static Player Instance;
    [field: SerializeField] public PlayerSO PlayerData { get; private set; }

    [field: Header("Animations")]
    public PlayerAndNpcAnimationData AnimationData { get; private set; }

    public PlayerInputController playerInputController { get; private set; }
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject ItemSlotsUI;
    private InventoryUI inventoryUIComponent;
    public bool isOpenInventory = false;

    public PlayerStateMachine playerStateMachine;
    public Inventory inventory;
    private float itemMovespeed = 10f;
    private Vector2 overlapCirclePosition;

    //Player 스탯
    public PlayerSO playerSO;
    public PlayerStat playerStat;
    public LayerMask ItemLayer;
    public bool isPlayerAtkState;
    public bool IsPlayerAtk;

    public bool isPlayerInteracting = false;
    public DialogueIcon playerDialogueIcon;

    [SerializeField] private TextMeshProUGUI playerStrTxt;
    [SerializeField] private TextMeshProUGUI playerDefTxt;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Instance.gameObject.transform.position = transform.position;
            Instance.gameObject.SetActive(true);
            Destroy(gameObject);
        }

        playerInputController = GetComponent<PlayerInputController>();

        inventory = GetComponent<Inventory>();
        if(inventoryUI!= null)
            inventoryUIComponent = inventoryUI.GetComponentInParent<InventoryUI>();

        playerStat = GetComponent<PlayerStat>();
        playerStateMachine = new PlayerStateMachine(this);
        AnimationData = new PlayerAndNpcAnimationData();
        AnimationData.InitializePlayer();
        playerDialogueIcon = GetComponent<DialogueIcon>();
    }

    private void OnEnable()
    {
        playerInputController.playerMouseAndInteractActions.InventoryOpen.started += OnInventoryOpen;
        playerInputController.playerMouseAndInteractActions.AllClose.started += OnAllCloseUI;
    }

    private void OnDisable()
    {
        playerInputController.playerMouseAndInteractActions.InventoryOpen.started -= OnInventoryOpen;
        playerInputController.playerMouseAndInteractActions.AllClose.started -= OnAllCloseUI;
    }

    private void Start()
    {
        playerStateMachine.ChangeState(playerStateMachine.IdleState);
        if(playerStrTxt != null)
        {
            playerStrTxt.text = playerStat.STR.curValue.ToString();
            playerDefTxt.text = playerStat.Def.curValue.ToString();
        }
        

        if(DataManager.Instance.setPlayerInit)
        {
            playerStat.InitializeStats();
            SetPlayerStatUIDef();
            SetPlayerStatUIStr();
            Animation.SetEmptyTextureAll();

            DataManager.Instance.setPlayerInit = false;
        }
    }

    private void Update()
    {
        playerStateMachine.HandleInput();
        playerStateMachine.Update();
        Animation.PlayerAnimUpdate();
        GetItemAuto();
    }

    private void FixedUpdate()
    {
        playerStateMachine.PhysicsUpdate();
    }

    protected virtual void OnInventoryOpen(InputAction.CallbackContext context)
    {
        if(GameManager.Instance.canOpenInventory && !DialogueManager.isDialogue)
        {
            InventoryClose();
        }
    }

    private void OnAllCloseUI(InputAction.CallbackContext context)
    {
        InventoryClose(true);
        GameManager.Instance.SelectionActions.DeactiveStoreUI();
        GameManager.Instance.SelectionActions.DeactiveEnchantUI();
    }

    public void InventoryCloseBtn()
    {
        InventoryClose();
    }

    public void InventoryClose(bool isNotEscapeBtn = false)
    {
        if (!isOpenInventory && !isNotEscapeBtn)
        {
            inventoryUIComponent.SetInventoryPlayerGold();
            isPlayerInteracting = true;
            isOpenInventory = true;
        }
        else
        {
            isPlayerInteracting = false;
            isOpenInventory = false;
        }
        inventoryUIComponent.SetInventoryBackground(isOpenInventory);
        inventoryUI.SetActive(isOpenInventory);
        ItemSlotsUI.SetActive(isOpenInventory);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 30) return;

        int amount = 0;
        amount = inventory.Add(collision.GetComponent<CreateItemData>().itemData);

        if (amount == 0)
            collision.gameObject.SetActive(false);
    }

    private void GetItemAuto()
    {
        overlapCirclePosition = new Vector2(transform.position.x, transform.position.y + 0.15f);
        float radius = 0.5f;
        Collider2D hit = Physics2D.OverlapCircle(overlapCirclePosition, radius, ItemLayer);
        if (hit && (inventory.FindEmptySlotIndex() != -1)) // 비어있는 슬롯이 있으면, 
        {
            float distance = Vector2.Distance(hit.transform.position, transform.position);
            if (distance >= 0f)
            {
                hit.transform.position = Vector2.MoveTowards(hit.transform.position, transform.position, itemMovespeed * Time.deltaTime);
            }
        }
    }

    void OnDrawGizmos() // 범위 그리기
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(overlapCirclePosition, 0.5f);
    }

    public void SetPlayerStatUIStr()
    {
        playerStrTxt.text = playerStat.STR.curValue.ToString();
    }
    public void SetPlayerStatUIDef()
    {
        playerDefTxt.text = playerStat.Def.curValue.ToString();
    }

    public Vector2 PlayerPositionVector2()
    {
        return transform.position;
    }
}
