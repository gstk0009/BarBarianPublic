using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerConditionUI : Singleton<PlayerConditionUI>
{
    public Image hpSlider;
    public Image staminaSlider;
    public GameObject Atkstate;

    public Slider expSlider;
    public Player player;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI nickNameText;

    private void Start()
    {
        SetMaxHP();
        SetNickName();
        //SetMaxExp();
    }

    private void SetNickName()
    {
        nickNameText.text = DataManager.Instance.currentPlayer.name;
    }

    private void Update()
    {
        UpdateHPSlider();
        UpdateStaminaSlider();
        //UpdateExpSlider();
    }

    public void UpdateStaminaSlider()
    {
        if (staminaSlider != null)
        {
            staminaSlider.fillAmount = player.playerStat.Stamina.curValue/ player.playerStat.Stamina.maxValue;
        }
    }
    public void SetMaxHP()
    {
        if (hpSlider != null)
        {
            hpSlider.fillAmount = player.playerStat.HP.curValue / player.playerStat.HP.maxValue;
        }
    }

    public void UpdateHPSlider()
    {
        if (hpSlider != null)
        {
            hpSlider.fillAmount = player.playerStat.HP.curValue / player.playerStat.HP.maxValue;

            hpText.text = ((int)player.playerStat.HP.curValue).ToString() + " / " + player.playerStat.HP.maxValue.ToString();
        }
    }

    public void SetMaxExp()
    {
        if(expSlider != null)
        {
            hpSlider.fillAmount = player.playerStat.HP.maxValue;
        }        
    }

    public void UpdateExpSlider()
    {
        //if (expSlider != null)
        //    hpSlider.fillAmount = player.stats.HP.curValue;
    }

    public void SetAtkState()
    {
        Atkstate.SetActive(true);
    }
    public void SetIdleState()
    {
        Atkstate.SetActive(false);
    }

}
