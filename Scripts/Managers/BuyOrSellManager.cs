using Cysharp.Threading.Tasks;
using UnityEngine;

public class BuyOrSellManager : MonoBehaviour
{
    [SerializeField] private ShopInventoryUI shopInventoryUI;
    private int maxCount = 10;

    private void Start()
    {
        if (GameManager.Instance.buyOrSellManager != null) return;

        GameManager.Instance.buyOrSellManager = this;
    }

    public void Buy(int initGold, int afterGold, ShopInventoryMouseEvent BuyEvent)
    {
        subtractPlayerGold(initGold, afterGold, BuyEvent).Forget();
    }

    public void Sell(int initGold, int afterGold, SellInventoryMouseEvent SellEvent)
    {
        plusPlayerGold(initGold, afterGold, SellEvent).Forget();
    }

    private async UniTask subtractPlayerGold(int initGold, int afterGold, ShopInventoryMouseEvent BuyEvent)
    {
        int beforeGold = initGold;
        int subRange = (initGold - afterGold) / maxCount;

        for (int count = 0; count < maxCount; count++)
        {
            beforeGold -= subRange;
            shopInventoryUI.SetPlayerGold(beforeGold);
            await UniTask.Yield();
        }

        DataManager.Instance.currentPlayer.gold = beforeGold;
        BuyEvent.isBuying = false;
    }

    private async UniTask plusPlayerGold(int initGold, int afterGold, SellInventoryMouseEvent SellEvent)
    {
        int beforeGold = initGold;
        int addRange = (afterGold - initGold) / maxCount;

        for (int count = 0; count < maxCount; count++)
        {
            beforeGold += addRange;
            shopInventoryUI.SetPlayerGold(beforeGold);
            await UniTask.Yield();
        }

        DataManager.Instance.currentPlayer.gold = beforeGold;
        SellEvent.isSelling = false;
    }
}