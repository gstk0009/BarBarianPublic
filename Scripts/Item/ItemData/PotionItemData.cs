using UnityEngine;

[CreateAssetMenu(fileName = "ItemPotion", menuName = "ItemData/Potion", order = 3)]
public class PotionItemData : CountableItemData
{
    // 효과량
    public float Value => value;
    [SerializeField] private float value;

    public override Item CreateItem()
    {
        return new PotionItem(this);
    }
}