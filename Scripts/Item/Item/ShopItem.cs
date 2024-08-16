public class ShopItem : Item, IUsableItem
{
    private ShopItemData shopItemData;
    public ShopItem(ShopItemData data) : base(data)
    {
        shopItemData = data;
    }

    public bool Use()
    {
        return Player.Instance.inventory.AddCapacity(shopItemData.BagSize);
    }
}