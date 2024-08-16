using UnityEngine;

public abstract class EquipmentItemData : ItemData
{
    public int MaxDurability => maxDurability;
    public Texture2D ItemTexture => texture;
    public EquimentItemType Type => type;
    public string ItemLayer => layer;
    [SerializeField] private int maxDurability = 100;
    [SerializeField] private Texture2D texture;
    [SerializeField] private string layer;
    [SerializeField] private EquimentItemType type;
}

public enum EquimentItemType
{
    None = 0,
    Ring,
    Weapon,
    Hat,
    Armor,
    Sheld,
}
