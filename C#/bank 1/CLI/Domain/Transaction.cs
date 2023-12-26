namespace Challenge.CLI.Domain;
using Newtonsoft.Json;

public class Transaction
{
    public OperationType Operation { get; set; }
    [JsonProperty("unit-cost")]
    public decimal UnitCost { get; set; }
    public long Quantity { get; set; }
}
