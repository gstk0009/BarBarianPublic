using UnityEngine;

[CreateAssetMenu(fileName = "ShopItems", menuName = "ItemData/Shop", order = 4)]
public class ShopItemData : ItemData
{
    public int BagSize => bagSize;
    public Buyingitemtype BuyType => buyingItemType;

    [SerializeField] private int bagSize;
    [SerializeField] private Buyingitemtype buyingItemType;
    public override Item CreateItem()
    {
        return new ShopItem(this);
    }
}

public enum Buyingitemtype
{
    None,
    SingleUse
}
