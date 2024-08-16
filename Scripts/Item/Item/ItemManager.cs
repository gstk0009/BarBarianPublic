using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private ItemData[] Items; // 만들어둔 ItemSO들 다 여기 넣어줌

    private Dictionary<int, ItemData> itemDictionary;

     void Start()
    {
        if (GameManager.Instance.itemManager != null) return;
        InitItemDictionary();
        GameManager.Instance.itemManager = this;
    }

    private void InitItemDictionary()
    {
        itemDictionary = new Dictionary<int, ItemData>();
        foreach (var item in Items)
        {
            itemDictionary[item.ID] = item;
        }
    }

    public ItemData GetItemDataById(int id)
    {
        itemDictionary.TryGetValue(id, out ItemData itemData);
        return itemData;
    }
}
