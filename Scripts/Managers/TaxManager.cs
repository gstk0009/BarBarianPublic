using System;
using UnityEngine;

public class TaxManager : MonoBehaviour
{
    public int lastPayment = 0;
    public int TaxDue = 3;
    public static Action OnCutSceneEvent;

    private void Awake()
    {
        if (GameManager.Instance.TaxManager != null) return;

        GameManager.Instance.TaxManager = this;
    }


    private void OnEnable()
    {
        ClockSystem.OnCheckTaxPayment += CheckTaxPayment;
    }

    private void OnDisable()
    {
        ClockSystem.OnCheckTaxPayment -= CheckTaxPayment;
    }

    public int TaxPrice()
    {
        int week = (ClockSystem.Dday - 1) / TaxDue + 1;

        int price = week * 1000;

        return price;
    }
    public void PayTax()
    {
        int price = TaxPrice();


        if (DataManager.Instance.currentPlayer.gold >= price) // 돈 있을 때
        {
            lastPayment = ClockSystem.Dday;
            DataManager.Instance.currentPlayer.gold -= price;

            DialogueManager.skipDialogueNum = 22;
        }
        else // 돈 없을 때
        {
            DialogueManager.skipDialogueNum = 21;
        }
    }

    public void CheckTaxPayment()
    {
        if(ClockSystem.Dday - lastPayment <= TaxDue) // 세금 냄
        {
            return;
        }
        else
        {
            OnCutSceneEvent?.Invoke();
        }
    }

}
