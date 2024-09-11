public enum LayerType
{
    Managers = 3,
    Enemy = 7,
    NPC = 8,
    Player = 9,
    Interaction_NPC = 11,
    Interaction_Obj,
    Boss,
    Stage1,
    Stage2,
    StageBoss,
    Stage3 =18,
}

public enum MatrixType
{
    One_Four,
    Two_Two
}

public enum AttackType
{
    Melee,
    Ranged,
    Heal
}

public enum MonsterTag
{
    Goblin,
    Skeleton,
}

enum SceneNumber
{
    StartScene,
    SelectSeene,
    VillageScene,
    DugeonScene_1,
    DugeonScene_2, 
    EndScene, 
}

public enum TutorialType
{
    TaxSystem,
    DungeonSystem,
    ShopSystem, 
    EnchantSystem,
}

public enum PortalType
{
    StagePortal, // 일반 스테이지 포탈
    SpecialPortal, // 특별 스테이지 포탈
    VillagePortal, // 마을 포탈
    PreStagePortal, // 이전 스테이지로 갈 수 있는 포탈
    VillageUpTownPortal, // 윗 마을 포탈
    VillageDownTownPortal, // 아랫 마을 포탈
    VillageToDungeonPortal, // 마을 → 던전으로 갈 수 있는 포탈
    ScaffoldPortal, // 마을 -> 처형장
    NextDungeonPortal, // 1층 던전 -> 2층 던전으로 갈 수 있는 포탈 
    EndingPortal, // 엔딩 씬으로 가는 포탈(임시)
}

public enum Direction
{
    right,
    left,
    up,
    down,
    none
}

public enum PlayerState
{
    None,
    Attack
}