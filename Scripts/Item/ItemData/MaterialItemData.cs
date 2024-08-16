using UnityEngine;

[CreateAssetMenu(fileName = "EnchantMaterialItem", menuName = "ItemData/EnchantMaterial")]
public class MaterialItemData : CountableItemData
{
    [SerializeField]
    private int amount = 1;

    public int Amount
    {
        get => amount;
        set => amount = Mathf.Clamp(value, 0, MaxAmount);
    }

    public override Item CreateItem()
    {
        return new MaterialItem(this);
    }
}