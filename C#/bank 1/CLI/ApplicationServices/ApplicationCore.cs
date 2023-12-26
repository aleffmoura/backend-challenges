namespace Challenge.CLI.ApplicationServices;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Challenge.CLI.Domain;
using System;
using System.Collections.Generic;

public static class ApplicationCore
{
    static JsonSerializerSettings _jsonSerializerSettings = new()
    {
        Formatting = Formatting.None,
        FloatFormatHandling = FloatFormatHandling.DefaultValue,
        ContractResolver = new DefaultContractResolver { NamingStrategy = new KebabCaseNamingStrategy() }
    };

    public static void execute(List<List<Transaction>> transactions, Action<string> print)
    {
        foreach (var transaction in transactions)
        {
            var transactionService = new TransactionService();
            var taxes = transactionService.CalculateTaxes(transaction);
            print(JsonConvert.SerializeObject(taxes, _jsonSerializerSettings));
        }
    }
}
