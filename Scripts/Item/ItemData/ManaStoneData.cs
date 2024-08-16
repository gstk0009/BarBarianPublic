using UnityEngine;

[CreateAssetMenu(fileName = "ManaStone", menuName = "ItemData/ManaStone", order = 5)]
public class ManaStoneData : CountableItemData
{
    public ManaStoneType StoneType => manaStoneType;
    [SerializeField] private ManaStoneType manaStoneType;
    public override Item CreateItem()
    {
        return new ManaStone(this);
    }
}

public enum ManaStoneType
{
    E,
    D,
    C,
    B,
    A
}
