public class PotionItem : CountableItem, IUsableItem
{
    private PotionItemData potionItemData;
    public PotionItem(PotionItemData data, int amount = 1) : base(data, amount)
    {
        potionItemData = data;
    }

    public bool Use()
    {
        if(Data.ID == 1001)
        {
            Amount -= Player.Instance.playerStat.HP.AddCurValue(potionItemData.Value);
        }
        else if(Data.ID == 1002)
        {
            Amount -= Player.Instance.playerStat.Mp.AddCurValue(potionItemData.Value);
        }

        return true;
    }

    protected override CountableItem Clone(int amount)
    {
        return new PotionItem(CountableData as PotionItemData, amount);
    }
}