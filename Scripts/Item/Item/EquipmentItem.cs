using UnityEngine;

public abstract class EquipmentItem : Item
{
    public EquipmentItemData EquipmentData { get; private set; }

    private int durability;
    public int ItemId;
    public string ItemName;
    public string ItemToolTip;
    public Sprite sprite;

    public int EnchantLevel { get; set; }
    public float EnhanceRate { get; set; }

    public int Durability
    {
        get => durability;
        set
        {
            if (value < 0) value = 0;
            if (value > EquipmentData.MaxDurability)
                value = EquipmentData.MaxDurability;
            durability = value;
        }
    }

    protected EquipmentItem(EquipmentItemData data) : base(data)
    {
        EquipmentData = data;
        durability = data.MaxDurability;
        ItemId = data.ID;
        ItemName = data.Name;
        ItemToolTip = data.ToolTip;
        sprite = data.IconSprite;
    }

    public string Layer()
    {
        return EquipmentData.ItemLayer;
    }

    public abstract void Enchant();
    
}
