using UnityEngine;

public enum WeaponType
{
    lowShield,
    lowWeaponStart = 1,
    lowWeaponEnd = 3,
    highShield,
    highWeponStart,
    highWeaponEnd = 7,
}

public enum EnchantMaterialType
{
    low,
    middle,
    high,
    superhigh
}

public class ItemSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject dropItemPrefab;
    [SerializeField] private ItemData potion;
    [SerializeField] private ItemData[] manaStones;
    [SerializeField] private ItemData[] enchantMaterials;
    [SerializeField] private ItemData[] weapons;

    private void Start()
    {
        if (GameManager.Instance.itemSpawnManager != null) return;

        GameManager.Instance.itemSpawnManager = this;
    }

    public void DropMonsterItem(GameObject monsterObj)
    {

        double randomValue = Random.Range(0, 101);

        if (randomValue < 10)
        {
            SetItem(setEnchantMaterialType(), monsterObj);
        }
        else if (randomValue < 30)
        {
            //마석
            SetItem(setManaStoneType(), monsterObj);
        }
        else if (randomValue < 70)
        {
            // 포션
            SetItem(potion, monsterObj);
        }
        else { }
    }

    public void DropBossItem(GameObject bossObj)
    {
        double randomValue = Random.Range(0, 101);

        if (GameManager.Instance.MainStageIdx == 1)
        {
            if (randomValue < 10)
            {
                // 방패
                SetItem(weapons[(int)WeaponType.lowShield], bossObj);
            }
            else if (randomValue < 20)
            {
                // 무기
                int randomWeaponValue = Random.Range((int)WeaponType.lowWeaponStart, (int)WeaponType.lowWeaponEnd+1);
                SetItem(weapons[randomWeaponValue], bossObj);
            }
            else if (randomValue < 40)
            {
                // 중급 강화재료
                SetItem(enchantMaterials[(int)EnchantMaterialType.middle], bossObj);
            }
            else
            {
                // 하급 강화재료
                SetItem(enchantMaterials[(int)EnchantMaterialType.low], bossObj);
            }
            SetItem(manaStones[(int)ManaStoneType.C], bossObj);
        }
        else if (GameManager.Instance.MainStageIdx == 2)
        {
            if (randomValue < 10)
            {
                // 방패
                SetItem(weapons[(int)WeaponType.highShield], bossObj);
            }
            else if (randomValue < 20)
            {
                // 무기
                int randomWeaponValue = Random.Range((int)WeaponType.highWeponStart, (int)WeaponType.highWeaponEnd+1);
                SetItem(weapons[randomWeaponValue], bossObj);
            }
            else if (randomValue < 40)
            {
                // 상급 강화재료
                SetItem(enchantMaterials[(int)EnchantMaterialType.high], bossObj);
            }
            else
            {
                // 중급 강화재료
                SetItem(enchantMaterials[(int)EnchantMaterialType.middle], bossObj);
            }
            SetItem(manaStones[(int)ManaStoneType.B], bossObj);
        }
    }

    private void SetItem(ItemData data, GameObject monsterObj)
    {
        GameObject createItem = ObjectPool.Instance.SpawnFromPool("Item");

        createItem.GetComponent<SpriteRenderer>().sprite = data.IconSprite;

        createItem.GetComponent<CreateItemData>().SetItemData(data);

        createItem.transform.position = monsterObj.transform.position;

        createItem.SetActive(true);
    }

    private ItemData setManaStoneType()
    {
        ItemData returnManaStoneData = null;
        if (GameManager.Instance.MainStageIdx == 1)
        {
            if (GameManager.Instance.CurrentStageIdx == 1)
            {
                returnManaStoneData = manaStones[(int)ManaStoneType.E];
            }
            else
            {
                returnManaStoneData = manaStones[(int)ManaStoneType.D];
            }
        }
        else if (GameManager.Instance.MainStageIdx == 2)
        {
            if (GameManager.Instance.CurrentStageIdx == 1)
            {
                returnManaStoneData = manaStones[(int)ManaStoneType.D];
            }
            else
            {
                returnManaStoneData = manaStones[(int)ManaStoneType.C];
            }
        }
        return returnManaStoneData;
    }

    private ItemData setEnchantMaterialType()
    {
        ItemData returnEnchantMaterialData = null;
        if (GameManager.Instance.MainStageIdx == 1)
        {
            returnEnchantMaterialData = enchantMaterials[(int)EnchantMaterialType.low];
        }
        else if (GameManager.Instance.MainStageIdx == 2)
        {
            returnEnchantMaterialData = enchantMaterials[(int)EnchantMaterialType.middle];
        }
        return returnEnchantMaterialData;
    }
}