namespace Challenge.CLI.Tests.Base;

using Challenge.CLI.Domain;

public static class TransactionObjectMother
{
    public static Transaction MakeBuy(decimal unitCost, long quantity) => new()
    {
        Operation = OperationType.Buy,
        Quantity = quantity,
        UnitCost = unitCost
    };

    public static Transaction MakeSell(decimal unitCost, long quantity) => new()
    {
        Operation = OperationType.Sell,
        Quantity = quantity,
        UnitCost = unitCost
    };
}
