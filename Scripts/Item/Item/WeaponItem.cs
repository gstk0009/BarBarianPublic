public class WeaponItem : EquipmentItem, IEnchantable
{
    public WeaponItemsData weaponItemData;

    public float WeaponItemStr; 
   
    public WeaponItem(WeaponItemsData data) : base(data)
    {
        weaponItemData = data;
        WeaponItemStr = data.WeaponStr;
    }

    public override void Enchant()
    {
        EnchantLevel++;

        // 강화까지 적용된 str 수치를 받으려면 WeaponItemStr + EnhanceRate와 같은 작업 수행
        float str = WeaponItemStr / 2 > 0 ? WeaponItemStr / 2 : 1f;
        EnhanceRate += str;
    }

    public float EnchantedStat()
    {
        return EnhanceRate + WeaponItemStr;
    }
}
