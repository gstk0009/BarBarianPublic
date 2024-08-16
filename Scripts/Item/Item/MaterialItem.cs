public class MaterialItem : CountableItem
{
    MaterialItemData materialItemData;
    public MaterialItem(MaterialItemData data, int amount = 1) : base(data, amount)
    {
        materialItemData = data;
    }

    protected override CountableItem Clone(int amount)
    {
        return new MaterialItem(materialItemData, amount);
    }


}