namespace Challenge.CLI.ApplicationServices;

using Challenge.CLI.Domain;
using Challenge.CLI.Infra.Cross;

public class TransactionService
{
    private decimal _weightedAverage, _allProfit;
    private long _quantityAssets;

    public List<TaxResult> CalculateTaxes(List<Transaction> transactions)
        => transactions.Select(op =>
             new TaxResult(decimal.Round(op.Operation switch
             {
                 OperationType.Buy => CalculateBuyOperation(op),
                 OperationType.Sell => CalculateSellOperation(op),
                 _ => throw new NotImplementedException("Incorrect OperationType"),
             }, decimals: 2)))
        .ToList();

    private decimal CalculateBuyOperation(Transaction transaction)
    {
        _allProfit = _quantityAssets == 0 ? 0 : _allProfit;
        _weightedAverage = _quantityAssets == 0 ? 0 : _weightedAverage;
        _weightedAverage = ( ( _quantityAssets * _weightedAverage ) + ( transaction.Quantity * transaction.UnitCost ) ) / ( _quantityAssets + transaction.Quantity );
        _quantityAssets += transaction.Quantity;
        return GetTax(OperationType.Buy, 0);
    }

    private decimal CalculateSellOperation(Transaction transaction)
    {
        _quantityAssets -= transaction.Quantity;

        Func<decimal> sellLogic(Transaction transaction) => ( transaction.Quantity * transaction.UnitCost ) switch
        {
            var operationCost when operationCost <= 20000 => () =>
            {
                if (transaction.UnitCost < _weightedAverage)
                    _allProfit -= ( transaction.Quantity * _weightedAverage ) - operationCost;
                return 0.00m;
            }
            ,
            var operationCost => () =>
            {
                decimal transctionProfit = operationCost - ( transaction.Quantity * _weightedAverage );
                var oldAllProfit = _allProfit;
                _allProfit += transctionProfit;
                transctionProfit = oldAllProfit < 0 ? transctionProfit + oldAllProfit : transctionProfit;
                return transctionProfit > 0 ? GetTax(OperationType.Sell, transctionProfit) : 0.00m;
            }
        };

        return sellLogic(transaction).Invoke();
    }

    private decimal GetTax(OperationType operation, decimal transactionProfit)
    {
        Func<decimal> taxCalculate = operation switch
        {
            OperationType.Buy => () => 0.00m,
            OperationType.Sell => () => _allProfit < 0.00m ? 0.00m : transactionProfit * 0.2m,
            _ => throw new NotImplementedException("Incorrect OperationType")
        };
        return taxCalculate();
    }
}
