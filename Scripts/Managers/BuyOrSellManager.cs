using System.Collections;
using UnityEngine;

public class BuyOrSellManager : MonoBehaviour
{
    [SerializeField] private ShopInventoryUI shopInventoryUI;
    private Coroutine SubtractGold;
    private Coroutine plusGold;

    private void Start()
    {
        if (GameManager.Instance.buyOrSellManager != null) return;

        GameManager.Instance.buyOrSellManager = this;
    }

    public void Buy(int initGold, int afterGold, ShopInventoryMouseEvent BuyEvent)
    {
        SubtractGold = StartCoroutine(SubtractPlayerGold(initGold, afterGold, BuyEvent));
    }

    public void Sell(int initGold, int afterGold, SellInventoryMouseEvent SellEvent)
    {
        plusGold = StartCoroutine(PlusPlayerGold(initGold, afterGold, SellEvent));
    }

    IEnumerator SubtractPlayerGold(int initGold, int afterGold, ShopInventoryMouseEvent BuyEvent)
    {
        int before = initGold;
        int after = afterGold;
        while (BuyEvent.isBuying)
        {
            if (before - after > 0)
            {
                before -= 10;
                shopInventoryUI.SetPlayerGold(before);
            }
            else
            {
                DataManager.Instance.currentPlayer.gold = after;
                BuyEvent.isBuying = false;
            }
            yield return null;
        }
        StopCoroutine(SubtractGold);
    }

    IEnumerator PlusPlayerGold(int initGold, int afterGold, SellInventoryMouseEvent SellEvent)
    {
        int before = initGold;
        int after = afterGold;
        while (SellEvent.isSelling)
        {
            if (before < after)
            {
                before += 10;
                shopInventoryUI.SetPlayerGold(before);
            }
            else
            {
                DataManager.Instance.currentPlayer.gold = after;
                SellEvent.isSelling = false;
            }
            yield return null;
        }
        StopCoroutine(plusGold);
    }
}