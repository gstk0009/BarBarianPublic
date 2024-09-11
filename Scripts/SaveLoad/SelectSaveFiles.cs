using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectSaveFiles : MonoBehaviour
{
    public GameObject InputNameUI;
    public GameObject DeleteFileUI;
    public GameObject TutorialStroyUI;
    public TextMeshProUGUI WarningText;

    public TextMeshProUGUI[] slotText;
    [SerializeField] Button[] saveFileButtons;
    public TMP_InputField playerName;

    bool[] savefiles = new bool[3];
    bool isCreatingPlayer = false;
    public static bool isStartGame = false;

    private void Start()
    {
        playerName.characterLimit = 4;
        SetSlotTexts();

        EnableBtns();
        SceneTransitionManager.Instance.OntransitionComplete += EnableBtns;
    }

    void OnDestroy()
    {
        // 이벤트 구독 해제
        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.OntransitionComplete -= EnableBtns;
        }
    }

    void SetSlotTexts()
    {
        for (int i = 0; i < 3; i++)
        {
            string saveDataKey = $"SaveData_{i}";
            if (PlayerPrefs.HasKey(saveDataKey))
            {
                savefiles[i] = true;

                DataManager.Instance.currentSaveDataSlot = i;
                DataManager.Instance.LoadData();
                slotText[i].text =
                    "<color=#959595>이름:</color>\n" +
                    DataManager.Instance.currentPlayer.name
                    + "\n\n<color=#959595>게임 클리어 수:</color>\n"
                    + DataManager.Instance.currentPlayer.clearCnt + "번"
                    + "\n\n<color=#959595>던전 정복 시도:</color>\n"
                    + DataManager.Instance.currentPlayer.tryCnt + "번"
                    + "\n\n<color=#959595>마지막 플레이:</color>\n" + // 마지막 세이브 시간
                    DataManager.Instance.currentPlayer.exitTime;
            }
            else
            {
                slotText[i].text = "<color=#959595>(빈 저장 칸)</color>";
            }
            DataManager.Instance.DataClear();
        }
    }

    public void Slot(int num)
    {
        DataManager.Instance.currentSaveDataSlot = num;
        if (savefiles[num])
        {
            if (!isCreatingPlayer)
            {
                DataManager.Instance.LoadData();
                StartMainGame();
            }
        }
        else // 새로운 캐릭터
        {
            DataManager.Instance.currentPlayer.inventoryItemDatas = new List<InventoryItemData>();
            DataManager.Instance.setInventoryInit = true;
            DataManager.Instance.DataClear();
            CreatePlayer(true);
        }
    }

    public void StartMainGame()
    {
        DisableBtns();
        if (!savefiles[DataManager.Instance.currentSaveDataSlot])
        {
            // 공백을 입력했을 경우 디폴트 이름으로 생성되도록
            if (string.IsNullOrEmpty(playerName.text))
            {
                DataManager.Instance.currentPlayer.name = "플레이어";
            }
            else
            {
                DataManager.Instance.currentPlayer.name = playerName.text;
            }

            DataManager.Instance.SaveData();
        }
        isStartGame = true;
        SceneTransitionManager.Instance.LoadScene((int)SceneNumber.VillageScene);
    }

    public void CreatePlayer(bool active)
    {
        DataManager.Instance.setPlayerInit = true;

        isCreatingPlayer = active;
        InputNameUI.SetActive(active);
    }
    public void ShowTutorialStroyUI(bool active)
    {
        isCreatingPlayer = active;
        DataManager.Instance.currentPlayer.SetTutorial = active;
        InputNameUI.SetActive(false);
        TutorialStroyUI.SetActive(active);
    }

    public void DeleteSaveDataUI(int num)
    {
        string saveDataKey = $"SaveData_{num}";
        if (PlayerPrefs.HasKey(saveDataKey))
        {
            DataManager.Instance.currentSaveDataSlot = num;
            DataManager.Instance.LoadData();

            ToggleDeleteFileUI(true);
            WarningText.text = $"정말로 <color=#FFD826>{DataManager.Instance.currentPlayer.name}</color> 데이터를 \r\n삭제하시겠습니까?";
        }
        else
        {
            // 삭제할 데이터 없음
        }
    }

    public void ToggleDeleteFileUI(bool active)
    {
        DeleteFileUI.SetActive(active);
    }

    public void DeleteData() // 삭제하기 버튼을 누르면 실제로 동작하는 곳 
    {
        int num = DataManager.Instance.currentSaveDataSlot;

        DataManager.Instance.setInventoryInit = true;
        DataManager.Instance.setPlayerInit = true;
        DataManager.Instance.currentPlayer.inventoryItemDatas = new List<InventoryItemData>();

        DataManager.Instance.DeleteData();
        SetSlotTexts();

        savefiles[num] = false;
        ToggleDeleteFileUI(false);
    }

    void EnableBtns()
    {
        foreach (var btn in saveFileButtons)
        {
            btn.interactable = true;
        }
    }

    void DisableBtns()
    {
        foreach (var btn in saveFileButtons)
        {
            btn.interactable = false;
        }
    }

}
