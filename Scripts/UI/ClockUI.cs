using TMPro;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    public TextMeshProUGUI Time, Dday, LifeCnt;

    private void OnEnable()
    {
        ClockSystem.OnTimeChanged += UpdateTime;
    }

    private void OnDisable()
    {
        ClockSystem.OnTimeChanged -= UpdateTime;
    }

    private void UpdateTime()
    {
        Time.text = $"{ClockSystem.Hour:00}:{ClockSystem.Minute:00}";
        LifeCnt.text = $"인생 {DataManager.Instance.currentPlayer.lifeCnt} 회차";
        Dday.text = $"생존 {ClockSystem.Dday} 일차";
    }


}