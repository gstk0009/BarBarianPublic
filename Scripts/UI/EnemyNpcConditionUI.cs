using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyNpcConditionUI : MonoBehaviour
{
    public Image hpSlider;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI nameText;
    private Canvas hpCanvas;

    [SerializeField] private DialogueIcon npcName;
    [SerializeField] private NPC currentNPC;
    private void Start()
    {
        hpCanvas = GetComponent<Canvas>();
        if (hpCanvas.worldCamera == null)
            hpCanvas.worldCamera = GameManager.Instance.Outro.UIcamera;
        nameText.text = npcName.speakerSO.speaker;
        SetMaxHP();
    }

    private void Update()
    {
        UpdateHPSlider();
    }

    private void SetMaxHP()
    {
        if (hpSlider != null)
        {
            hpSlider.fillAmount = currentNPC.npcStat.HP.curValue / currentNPC.npcStat.HP.maxValue;
        }
    }

    public void UpdateHPSlider()
    {
        if (hpSlider != null)
        {
            hpSlider.fillAmount = currentNPC.npcStat.HP.curValue / currentNPC.npcStat.HP.maxValue;

            hpText.text = ((int)currentNPC.npcStat.HP.curValue).ToString() + " / " + currentNPC.npcStat.HP.maxValue.ToString();
        }
    }
}