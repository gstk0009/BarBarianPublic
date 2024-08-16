using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveData
{
    public string name = "플레이어";

    public Vector3 playerPos = Vector3.zero;
    public int tryCnt = 0; // 던전 시도 횟수
    public int lifeCnt = 1; // 게임 시도 횟수(플레이어 사망 횟수)
    public int clearCnt = 0; // 보스 클리어 횟수 

    public int gold = 1000;
    public string exitTime;
    public int[] clockSystem = new int[] { 7, 0, 1 }; // hour, minute, dDay

    // Dialogue 관련 정보 저장
    public List<string> keys = new List<string>();
    public List<Vector2> lines = new List<Vector2>();

    // 인벤토리 아이템 정보
    public List<InventoryItemData> inventoryItemDatas = new List<InventoryItemData>();
    public int inventoryCapacity = 11; // 가방 초기 세팅
}

public class DataManager : Singleton<DataManager>
{
    public int currentSaveDataSlot;

    public SaveData currentPlayer = new SaveData();
    public bool setInventoryInit = false;
    public bool setPlayerInit = false; 

    protected override void Awake()
    {
        base.Awake();
    }

    public void SaveData()
    {
        string jsonData = JsonUtility.ToJson(currentPlayer);
        PlayerPrefs.SetString("SaveData_" + currentSaveDataSlot, jsonData);

        PlayerPrefs.SetFloat("PlayerPosX_" + currentSaveDataSlot, currentPlayer.playerPos.x);
        PlayerPrefs.SetFloat("PlayerPosY_" + currentSaveDataSlot, currentPlayer.playerPos.y);
        PlayerPrefs.SetFloat("PlayerPosZ_" + currentSaveDataSlot, currentPlayer.playerPos.z);

        PlayerPrefs.Save();
    }

    public void LoadData()
    {
        string jsonData = PlayerPrefs.GetString("SaveData_" + currentSaveDataSlot, "");

        if (!string.IsNullOrEmpty(jsonData))
        {
            currentPlayer = JsonUtility.FromJson<SaveData>(jsonData);

            float x = PlayerPrefs.GetFloat("PlayerPosX_" + currentSaveDataSlot, 0f);
            float y = PlayerPrefs.GetFloat("PlayerPosY_" + currentSaveDataSlot, 0f);
            float z = PlayerPrefs.GetFloat("PlayerPosZ_" + currentSaveDataSlot, 0f);
            currentPlayer.playerPos = new Vector3(x, y, z);
        }
    }

    public void DataClear()
    {
        currentPlayer = new SaveData();
    }

    public void DeleteData()
    {
        PlayerPrefs.DeleteKey("SaveData_" + currentSaveDataSlot);
        PlayerPrefs.DeleteKey("PlayerPosX_" + currentSaveDataSlot);
        PlayerPrefs.DeleteKey("PlayerPosY_" + currentSaveDataSlot);
        PlayerPrefs.DeleteKey("PlayerPosZ_" + currentSaveDataSlot);
        DataClear();
    }

    public void SaveGame()
    {
        if (Player.Instance != null)
        {
            InventorySaver.Instance.SaveInventory(currentSaveDataSlot);
            // 회복하면 체력 max
            Player.Instance.playerStat.HP.curValue = Player.Instance.playerStat.HP.maxValue;
        }

        currentPlayer.exitTime = DateTime.Now.ToString("yyyy-MM-dd-hh-mm");
        currentPlayer.clockSystem[0] = ClockSystem.Hour;
        currentPlayer.clockSystem[1] = ClockSystem.Minute;
        currentPlayer.clockSystem[2] = ClockSystem.Dday;

        foreach (var dict in GameManager.Instance.DialogueController.dialogueLines)
        {
            currentPlayer.keys.Add(dict.Key);
            currentPlayer.lines.Add(dict.Value);
        }

        currentPlayer.playerPos = Player.Instance.gameObject.transform.position;
        currentPlayer.inventoryItemDatas = InventorySaver.Instance.GetInventoryData();
        currentPlayer.inventoryCapacity = Player.Instance.inventory.Capacity;

        SaveData();
    }

    public void SetGameOverData()
    {
        // 기존 데이터를 백업
        string playerName = currentPlayer.name;
        int playerTryCnt = currentPlayer.tryCnt;
        int playerLifeCnt = currentPlayer.lifeCnt;
        int playerGold = currentPlayer.gold;

        string lastExitTime = currentPlayer.exitTime;
        int[] lastClockSystem = currentPlayer.clockSystem;

        Player.Instance.Animation.SetEmptyTextureAll();

        // 새로운 SaveData로 초기화
        currentPlayer = new SaveData();

        // 백업된 데이터를 다시 할당
        currentPlayer.name = playerName;
        currentPlayer.tryCnt = playerTryCnt;
        currentPlayer.lifeCnt = playerLifeCnt;

        currentPlayer.exitTime = lastExitTime;
        currentPlayer.clockSystem = lastClockSystem;

        currentPlayer.gold = playerGold / 2;
        Player.Instance.playerStat.InitializeStats();

        Player.Instance.SetPlayerStatUIDef();
        Player.Instance.SetPlayerStatUIStr();

        Player.Instance.inventory.ClearInventory();
        currentPlayer.inventoryItemDatas = new List<InventoryItemData>();
        currentPlayer.inventoryItemDatas = InventorySaver.Instance.GetInventoryData();

        SaveGame();
    }
}
