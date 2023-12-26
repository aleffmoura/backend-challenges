// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;
using Challenge.CLI.Domain;
using static Challenge.CLI.ApplicationServices.ApplicationCore;

List<Transaction> deserializeObject(string linha)
    => JsonConvert.DeserializeObject<List<Transaction>>(linha) ?? new List<Transaction>();

List<List<Transaction>> getTransactions(Func<string?> readLine, string? linha)
{
    if (string.IsNullOrWhiteSpace(linha))
        return new List<List<Transaction>>();

   var transactions = getTransactions(readLine, readLine());

    transactions.Insert(0, deserializeObject(linha));

    return transactions;
}

execute(getTransactions(Console.ReadLine, Console.ReadLine()), Console.WriteLine);

return 1;