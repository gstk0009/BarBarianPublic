using UnityEngine;

public interface IUsableItem
{
    bool Use();
}
public interface IEnchantable
{
    int EnchantLevel { get; }
    float EnhanceRate { get;  }
    void Enchant();
}

public abstract class ItemData : ScriptableObject
{
    public int ID => id;
    public string Name => itemName;
    public string ToolTip // 읽기, 쓰기 모두 가능하도록
    {
        get => toolTip;
        set => toolTip = value; 
    }
    public Sprite IconSprite => iconSprite;
    public int SellPrice => sellPrice;
    public int BuyPrice => buyPrice;
    public EnchantMaterial enchantMaterial => EnchantMateriale; // 디폴트는 None
    public SellItem Sell => sellItem;
    public BuyItem Buy => buyItem;

    [SerializeField] private int id;
    [SerializeField] public string itemName;
    [SerializeField] private string toolTip;
    [SerializeField] private Sprite iconSprite;
    [SerializeField] private int sellPrice;
    [SerializeField] private int buyPrice;
    [SerializeField] private EnchantMaterial EnchantMateriale;
    [SerializeField] private SellItem sellItem;
    [SerializeField] private BuyItem buyItem;


    public abstract Item CreateItem();
}

// 인첸트 할 때 사용될 Enum
public enum EnchantMaterial
{
    None, // 인첸트가 불가능한 아이템 및 재료로도 사용되지 않는 아이템
    Enchantable, // 인첸트 가능한 아이템
    EnchantMaterial, // 인첸트할 때 사용될 재료 
}

public enum SellItem
{
    Impossible,
    Possible
}
public enum BuyItem
{
    Impossible,
    Possible
}
