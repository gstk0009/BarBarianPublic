using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerConditionUI : MonoBehaviour
{
    public Image hpSlider;
    public Slider expSlider;
    public Player player;
    public TextMeshProUGUI hpText;

    private static PlayerConditionUI instance;

    void Awake()
    {
        // 싱글톤 패턴 적용
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 로드될 때 파괴되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 중복된 인스턴스는 파괴
        }
    }

    private void Start()
    {
        SetMaxHP();
        SetMaxExp();
    }

    private void Update()
    {
        UpdateHPSlider();
        //UpdateExpSlider();
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


}
