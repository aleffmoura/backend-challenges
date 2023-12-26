namespace Challenge.CLI.Tests.Services;

using Moq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Challenge.CLI.Domain;
using Challenge.CLI.Tests.Base;
using System.Collections.Generic;
using System.Linq;
using static Challenge.CLI.ApplicationServices.ApplicationCore;
using Challenge.CLI.Infra.Cross;

[TestFixture]
public class ApplicationCoreTests
{
    static JsonSerializerSettings _jsonSerializerSettings = new()
    {
        Formatting = Formatting.None,
        FloatFormatHandling = FloatFormatHandling.DefaultValue,
        ContractResolver = new DefaultContractResolver { NamingStrategy = new KebabCaseNamingStrategy() }
    };

    [Test]
    public void ApplicationCoreServiceTests_CaseOnePlusTwo_ShouldBeOk()
    {
        //Arrange
        var createBuy = TransactionObjectMother.MakeBuy;
        var createSell = TransactionObjectMother.MakeSell;
        var costBuy = 10.00m;
        var costSell = 10.00m;
        var quantityBuy = 100;
        var quantitySell = 50;

        var transactionsOne = new List<Transaction>
        {
            createBuy(costBuy, quantityBuy),
            createSell(costSell, quantitySell),
            createSell(costSell, quantitySell)
        };

        var createBuy2 = TransactionObjectMother.MakeBuy;
        var createSell2 = TransactionObjectMother.MakeSell;

        var quantityBuy2 = 10000;
        var quantitySell2 = 5000;

        var transactionsTwo = new List<Transaction>
        {
            createBuy(costBuy, quantityBuy2),
            createSell(20.00m, quantitySell2),
            createSell(5.00m, quantitySell2)
        };

        var printfn = new Mock<Action<string>>();

        //action
        execute(new List<List<Transaction>>
        {
            { transactionsOne },
            { transactionsTwo }
        }, printfn.Object);

        //verifies
        printfn.Verify(mock => mock(It.IsAny<string>()), Times.Exactly(2));
    }
}
