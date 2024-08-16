using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopupUI : MonoBehaviour
{
    // 아이템 버리기 확인 팝업
    [Header("Confirmation Popup")]
    [SerializeField] private GameObject confirmationPopupObject;
    [SerializeField] private TextMeshProUGUI confirmationItemNameText;
    [SerializeField] private TextMeshProUGUI confirmationText;

    // 수량 입력 팝업
    [Header("Amount Input Popup")]
    [SerializeField] private GameObject amountInputPopupObject;
    [SerializeField] private TextMeshProUGUI amountInputItemNameText;
    [SerializeField] private TMP_InputField amountInputField;

    [SerializeField] private InventoryUIDragAndDrop dragAndDrop;

    private int maxAmount;
    private int curIndex;
    private int curItemAindex;
    private int curItemBindex;

    private void Awake()
    {
        HideConfiramtionPopup();
        HideAmountInputPopup();
    }

    public void OpenConfirmationPopup(string itemName, int index)
    {
        ShowConfirmationPopup(itemName);
        curIndex = index;
    }

    public void OpenAmountInputPopup(string itemName, int currentAmount, int indexA, int indexB)
    {
        maxAmount = currentAmount - 1;
        amountInputField.text = "1";
        curItemAindex = indexA;
        curItemBindex = indexB;

        ShowAmountInputPopup(itemName);
    }

    public void AmountInputOkBtn()
    {
        HideAmountInputPopup();
        dragAndDrop.TryInventorySeparateAmount(curItemAindex, curItemBindex, int.Parse(amountInputField.text));
    }

    public void AmountInputCancelBtn()
    {
        HideAmountInputPopup();
    }

    public void ConfiramtionOkBtn()
    {
        HideConfiramtionPopup();
        dragAndDrop.TryRemoveItem(curIndex);
    }

    public void ConfirmationCancelBtn()
    {
        HideConfiramtionPopup();
    }

    // Btn에 연결
    public void AmountInputPlusBtn()
    {
        int.TryParse(amountInputField.text, out int amount);
        if (amount < maxAmount)
        {
            int nextAmount = Input.GetKey(KeyCode.LeftShift) ? amount + 10 : amount + 1;
            if (nextAmount > maxAmount)
                nextAmount = maxAmount;
            amountInputField.text = nextAmount.ToString();
        }
    }

    //Btn에 연결
    public void AmountInputMinusBtn()
    {
        int.TryParse(amountInputField.text, out int amount);
        if (amount > 1)
        {
            int nextAmount = Input.GetKey(KeyCode.LeftShift) ? amount - 10 : amount - 1;
            if (nextAmount < 1)
                nextAmount = 1;
            amountInputField.text = nextAmount.ToString();
        }
    }

    private void HideConfiramtionPopup() => confirmationPopupObject.SetActive(false);
    private void HideAmountInputPopup() => amountInputPopupObject.SetActive(false);

    private void ShowConfirmationPopup(string itemName)
    {
        confirmationItemNameText.text = itemName;
        confirmationPopupObject.SetActive(true);
    }

    private void ShowAmountInputPopup(string itemName)
    {
        amountInputItemNameText.text = itemName;
        amountInputPopupObject.SetActive(true);
    }
}
