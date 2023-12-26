namespace Challenge.CLI.Tests.Services;

using FluentAssertions;
using Challenge.CLI.ApplicationServices;
using Challenge.CLI.Domain;
using Challenge.CLI.Tests.Base;
using System.Collections.Generic;

[TestFixture]
public class TransactionTaxDomainServiceTests
{
    private TransactionService transactionService = new();

    [SetUp]
    public void Setup()
    {
        transactionService = new TransactionService();
    }

    [Test]
    public void TransactionTaxDomainServiceTests_CaseOne_ShouldBeOk()
    {
        //Arrange
        var createBuy = TransactionObjectMother.MakeBuy;
        var createSell = TransactionObjectMother.MakeSell;

        var costBuy = 10.00m;
        var costSell = 10.00m;
        var quantityBuy = 100;
        var quantitySell = 50;

        var transactions = new List<Transaction>
        {
            createBuy(costBuy, quantityBuy),
            createSell(costSell, quantitySell),
            createSell(costSell, quantitySell)
        };

        //action
        var outputs = transactionService.CalculateTaxes(transactions);

        //verifies
        outputs.Should().NotBeEmpty();
        outputs.Count.Should().Be(expected: transactions.Count);
        outputs[0].Tax.Should().Be(expected: 0);
        outputs[1].Tax.Should().Be(expected: 0);
        outputs[2].Tax.Should().Be(expected: 0);
    }

    [Test]
    public void TransactionTaxDomainServiceTests_CaseTwo_ShouldBeOk()
    {
        //Arrange
        var createBuy = TransactionObjectMother.MakeBuy;
        var createSell = TransactionObjectMother.MakeSell;

        var costBuy = 10.00m;
        var quantityBuy = 10000;

        var quantitySell = 5000;

        var transactions = new List<Transaction>
        {
            createBuy(costBuy, quantityBuy),
            createSell(20.00m, quantitySell),
            createSell(5.00m, quantitySell)
        };

        //action
        var outputs = transactionService.CalculateTaxes(transactions);

        //verifies
        outputs.Should().NotBeEmpty();
        outputs.Count.Should().Be(expected: transactions.Count);
        outputs[0].Tax.Should().Be(expected: 0.00m);
        outputs[1].Tax.Should().Be(expected: 10000.00m);
        outputs[2].Tax.Should().Be(expected: 0.00m);
    }

    [Test]
    public void TransactionTaxDomainServiceTests_CaseOneAndTwo_ShouldBeOk()
    {
        //Arrange
        var createBuy = TransactionObjectMother.MakeBuy;
        var createSell = TransactionObjectMother.MakeSell;

        var transactions1 = new List<Transaction>
        {
            createBuy(10.00m, 100),
            createSell(15.00m, 50),
            createSell(15.00m, 50)
        };

        var transactions2 = new List<Transaction>
        {
            createBuy(10.00m, 10000),
            createSell(20.00m, 5000),
            createSell(5.00m, 5000)
        };

        //action
        var outputs1 = transactionService.CalculateTaxes(transactions1);
        var outputs2 = transactionService.CalculateTaxes(transactions2);

        //verifies
        outputs1.Should().NotBeEmpty();
        outputs1.Count.Should().Be(expected: transactions1.Count);
        outputs1[0].Tax.Should().Be(expected: 0);
        outputs1[1].Tax.Should().Be(expected: 0);
        outputs1[2].Tax.Should().Be(expected: 0);

        outputs2.Should().NotBeEmpty();
        outputs2.Count.Should().Be(expected: transactions1.Count);
        outputs2[0].Tax.Should().Be(expected: 0.00m);
        outputs2[1].Tax.Should().Be(expected: 10000.00m);
        outputs2[2].Tax.Should().Be(expected: 0.00m);
    }

    [Test]
    public void TransactionTaxDomainServiceTests_CaseThree_ShouldBeOk()
    {
        //Arrange
        var createBuy = TransactionObjectMother.MakeBuy;
        var createSell = TransactionObjectMother.MakeSell;

        var transactions = new List<Transaction>
        {
            createBuy(10.00m, 10000),
            createSell(5.00m, 5000),
            createSell(20.00m, 3000),
        };

        //action
        var outputs = transactionService.CalculateTaxes(transactions);

        //verifies
        outputs.Should().NotBeEmpty();
        outputs.Count.Should().Be(expected: transactions.Count);
        outputs[0].Tax.Should().Be(expected: 0);
        outputs[1].Tax.Should().Be(expected: 0);
        outputs[2].Tax.Should().Be(expected: 1000.00m);
    }
    [Test]
    public void TransactionTaxDomainServiceTests_CaseFour_ShouldBeOk()
    {
        //Arrange
        var createBuy = TransactionObjectMother.MakeBuy;
        var createSell = TransactionObjectMother.MakeSell;

        var transactions = new List<Transaction>
        {
            createBuy(10.00m, 10000),
            createBuy(25.00m, 5000),
            createSell(15.00m, 10000),
        };

        //action
        var outputs = transactionService.CalculateTaxes(transactions);

        //verifies
        outputs.Should().NotBeEmpty();
        outputs.Count.Should().Be(expected: transactions.Count);
        outputs[0].Tax.Should().Be(expected: 0);
        outputs[1].Tax.Should().Be(expected: 0);
        outputs[2].Tax.Should().Be(expected: 0);
    }

    [Test]
    public void TransactionTaxDomainServiceTests_CaseFive_ShouldBeOk()
    {
        //Arrange
        var createBuy = TransactionObjectMother.MakeBuy;
        var createSell = TransactionObjectMother.MakeSell;

        var transactions = new List<Transaction>
        {
            createBuy(10.00m, 10000),
            createBuy(25.00m, 5000),
            createSell(15.00m, 10000),
            createSell(25.00m, 5000),
        };

        //action
        var outputs = transactionService.CalculateTaxes(transactions);

        //verifies
        outputs.Should().NotBeEmpty();
        outputs.Count.Should().Be(expected: transactions.Count);
        outputs[0].Tax.Should().Be(expected: 0);
        outputs[1].Tax.Should().Be(expected: 0);
        outputs[2].Tax.Should().Be(expected: 0);
        outputs[3].Tax.Should().Be(expected: 10000.00m);
    }

    [Test]
    public void TransactionTaxDomainServiceTests_CaseSix_ShouldBeOk()
    {
        //Arrange
        var createBuy = TransactionObjectMother.MakeBuy;
        var createSell = TransactionObjectMother.MakeSell;

        var transactions = new List<Transaction>
        {
            createBuy(10.00m, 10000),
            createSell(2.00m, 5000),
            createSell(20.00m, 2000),
            createSell(20.00m, 2000),
            createSell(25.00m, 1000),
        };

        //action
        var outputs = transactionService.CalculateTaxes(transactions);

        //verifies
        outputs.Should().NotBeEmpty();
        outputs.Count.Should().Be(expected: transactions.Count);
        outputs[0].Tax.Should().Be(expected: 0);
        outputs[1].Tax.Should().Be(expected: 0);
        outputs[2].Tax.Should().Be(expected: 0);
        outputs[3].Tax.Should().Be(expected: 0);
        outputs[4].Tax.Should().Be(expected: 3000.00m);
    }

    [Test]
    public void TransactionTaxDomainServiceTests_CaseSeven_ShouldBeOk()
    {
        //Arrange
        var createBuy = TransactionObjectMother.MakeBuy;
        var createSell = TransactionObjectMother.MakeSell;

        var transactions = new List<Transaction>
        {
            createBuy(10.00m, 10000),
            createSell(2.00m, 5000),
            createSell(20.00m, 2000),
            createSell(20.00m, 2000),
            createSell(25.00m, 1000),
            createBuy(20.00m, 10000),
            createSell(15.00m, 5000),
            createSell(30.00m, 4350),
            createSell(30.00m, 650),
        };

        //action
        var outputs = transactionService.CalculateTaxes(transactions);

        //verifies
        outputs.Should().NotBeEmpty();
        outputs.Count.Should().Be(expected: transactions.Count);
        outputs[0].Tax.Should().Be(expected: 0);
        outputs[1].Tax.Should().Be(expected: 0);
        outputs[2].Tax.Should().Be(expected: 0);
        outputs[3].Tax.Should().Be(expected: 0);
        outputs[4].Tax.Should().Be(expected: 3000.00m);
        outputs[5].Tax.Should().Be(expected: 0);
        outputs[6].Tax.Should().Be(expected: 0);
        outputs[7].Tax.Should().Be(expected: 3700.00m);
        outputs[8].Tax.Should().Be(expected: 0);
    }

    [Test]
    public void TransactionTaxDomainServiceTests_CaseEight_ShouldBeOk()
    {
        //Arrange
        var createBuy = TransactionObjectMother.MakeBuy;
        var createSell = TransactionObjectMother.MakeSell;

        var transactions = new List<Transaction>
        {
            createBuy(10.00m, 10000),
            createSell(50.00m, 10000),
            createBuy(20.00m, 10000),
            createSell(50.00m, 10000),
        };

        //action
        var outputs = transactionService.CalculateTaxes(transactions);

        //verifies
        outputs.Should().NotBeEmpty();
        outputs.Count.Should().Be(expected: transactions.Count);
        outputs[0].Tax.Should().Be(expected: 0);
        outputs[1].Tax.Should().Be(expected: 80000.00m);
        outputs[2].Tax.Should().Be(expected: 0);
        outputs[3].Tax.Should().Be(expected: 60000.00m);
    }
}
