public class ManaStone : CountableItem
{
    private ManaStoneData mansonteData;

    public ManaStone(ManaStoneData data, int amount = 1) : base(data, amount)
    {
        mansonteData = data;
    }

    protected override CountableItem Clone(int amount)
    {
        return new ManaStone(CountableData as ManaStoneData, amount);
    }
}
