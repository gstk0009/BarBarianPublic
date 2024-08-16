
public class ArmorItem : EquipmentItem, IEnchantable
{
    private ArmorItemsData armorItemData;

    public float ArmorItemDef;


    public ArmorItem(ArmorItemsData data) : base(data)
    {
        armorItemData = data;
        ArmorItemDef = data.ArmorItemDef;
    }

    public override void Enchant()
    {
        EnchantLevel++;
        // 강화까지 적용된 str 수치를 받으려면 WeaponItemStr + EnhanceRate와 같은 작업 수행
        float str = ArmorItemDef / 2 > 0 ? ArmorItemDef / 2 : 1f;
        EnhanceRate += str;
    }

    public float EnchantedStat()
    {
        return EnhanceRate + ArmorItemDef;
    }
}

