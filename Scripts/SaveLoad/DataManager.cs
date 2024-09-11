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
    public int lastPayment = 0; // 마지막 세금 납부일
    public int gold = 1000;
    public string exitTime;
    public int[] clockSystem = new int[] { 7, 0, 1 }; // hour, minute, dDay

    // Dialogue 관련 정보 저장
    public List<string> keys = new List<string>();
    public List<Vector2> lines = new List<Vector2>();

    // 인벤토리 아이템 정보
    public List<InventoryItemData> inventoryItemDatas = new List<InventoryItemData>();
    public int inventoryCapacity = 11; // 가방 초기 세팅

    public bool SetTutorial = false;

    // 순서대로 세금, 던전, 강화, 상점 튜토리얼이 들어감 
    public List<bool> tutorialClearInfo = new List<bool>(new bool[] { false, false, false, false });
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

        // SetTutorial 저장
        PlayerPrefs.SetInt("SetTutorial_" + currentSaveDataSlot, currentPlayer.SetTutorial ? 1 : 0);

        // tutorialClearInfo를 문자열로 변환해서 저장
        PlayerPrefs.SetString("TutorialClearInfo_" + currentSaveDataSlot, BoolListToString(currentPlayer.tutorialClearInfo));

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

            currentPlayer.SetTutorial = PlayerPrefs.GetInt("SetTutorial_" + currentSaveDataSlot, 0) == 1;

            // tutorialClearInfo를 불러와서 List<bool>로 변환
            string tutorialInfoString = PlayerPrefs.GetString("TutorialClearInfo_" + currentSaveDataSlot, "");
            currentPlayer.tutorialClearInfo = StringToBoolList(tutorialInfoString);
        }
    }

    private string BoolListToString(List<bool> boolList)
    {
        string result = "";
        foreach (bool b in boolList)
        {
            result += b ? "1" : "0";
        }
        return result;
    }

    private List<bool> StringToBoolList(string str)
    {
        List<bool> boolList = new List<bool>();
        foreach (char c in str)
        {
            if (c == '1')
            {
                boolList.Add(true);
            }
            else if (c == '0')
            {
                boolList.Add(false);
            }
        }
        return boolList;
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

        PlayerPrefs.DeleteKey("SetTutorial_" + currentSaveDataSlot);
        PlayerPrefs.DeleteKey("TutorialClearInfo_" + currentSaveDataSlot);

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

        bool lastSetTutorial = currentPlayer.SetTutorial;
        List<bool> lastTutorialClearInfo = currentPlayer.tutorialClearInfo;

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

        currentPlayer.SetTutorial = lastSetTutorial;
        currentPlayer.tutorialClearInfo = lastTutorialClearInfo;

        Player.Instance.playerStat.InitializeStats();

        Player.Instance.SetPlayerStatUIDef();
        Player.Instance.SetPlayerStatUIStr();

        Player.Instance.inventory.ClearInventory(); 
        currentPlayer.inventoryItemDatas = new List<InventoryItemData>();
        currentPlayer.inventoryItemDatas = InventorySaver.Instance.GetInventoryData();

        SaveGame();
    }

}
