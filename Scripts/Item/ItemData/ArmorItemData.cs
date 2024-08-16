using UnityEngine;

[CreateAssetMenu(fileName = "ItemArmor", menuName = "ItemData/Armor", order = 2)]
public class ArmorItemsData : EquipmentItemData
{
    public float ArmorItemDef => def;
    [SerializeField] private float def = 1;
    public override Item CreateItem()
    {
        return new ArmorItem(this);
    }
}
