using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossConditionUI : MonoBehaviour
{
    public Image hpSlider;
    public TextMeshProUGUI hpText;
    private Canvas hpCanvas;

    [SerializeField] private BossMonster currentBoss;
    private void Start()
    {
        hpCanvas = GetComponent<Canvas>();
        if (hpCanvas.worldCamera == null)
            hpCanvas.worldCamera = GameManager.Instance.Outro.UIcamera;
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
            hpSlider.fillAmount = currentBoss.stats.HP.curValue / currentBoss.stats.HP.maxValue;
        }
    }

    public void UpdateHPSlider()
    {
        if (hpSlider != null)
        {
            hpSlider.fillAmount = currentBoss.stats.HP.curValue / currentBoss.stats.HP.maxValue;

            hpText.text = ((int)currentBoss.stats.HP.curValue).ToString() + " / " + currentBoss.stats.HP.maxValue.ToString();
        }
    }
}