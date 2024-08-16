using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    [Header("Stage & Portal")]
    public MoveStageController MoveStageController;
    public SpawnersManager SpawnersManager;
    public SurfaceLayerChanger SurfaceLayerChanger;

    [Header("Dialogues")]
    public CsvParseManager CsvParseManager;
    public DialogueManager DialogueManager;
    public DialogueController DialogueController;
    public SelectionActions SelectionActions;

    [Header("Player")]
    public PlayerMovementController playerMovementController;
    public ItemManager itemManager;
    public EnchantEffectController EnchantEffectController;
    public EnchantManager EnchantManager;

    [Header("Items")]
    public ItemSpawnManager itemSpawnManager;

    [Header("Else")]
    // 현재 플레이어가 위치한 스테이지 : (MainStageIdx - CurrentStageIdx)
    public int MainStageIdx = 0;
    public int CurrentStageIdx = 0;  // 예를 들어 2-1 스테이지라고 할때, 2가 전자고 1이 후자
    public NPCTargetSystem NPCTargetSystem;
    public Outro Outro;
    public TaxManager TaxManager;
    public BuyOrSellManager buyOrSellManager;
    public Uis Uis;

    public bool canOpenInventory;

    protected override void Awake()
    {
        if (instance == null)
        {
            UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;
            MoveStageController = GetComponent<MoveStageController>();
        }

        base.Awake();
    }

    private void Start()
    {
        canOpenInventory = true;
        InitWithSaveDatas();
    }
 
    // 데이터 로드 후, 특정 데이터를 참조식으로 할당해줘야 하는 경우
    public void InitWithSaveDatas()
    {
        if(DialogueController != null)
        {
            DialogueController.SetSavedDialogues();
        }

        if(DataManager.Instance.currentPlayer.playerPos != Vector3.zero)
        {
            Player.Instance.transform.position = DataManager.Instance.currentPlayer.playerPos;
        }

        if(DataManager.Instance.currentPlayer.clockSystem != null)
        {
            ClockSystem.Init(DataManager.Instance.currentPlayer.clockSystem);
        }
    }
}
