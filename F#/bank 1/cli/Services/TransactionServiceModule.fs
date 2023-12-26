module TransactionServiceModule

open TransactionModule
open System

type BuyOperationTax = 
    {
        tax: float
        average : float
        assets  : int64
        profit  : float
    }

type SellOperationTax = 
    {
        tax: float
        assets  : int64
        profit  : float
    }

let getTax operation profit : float =
    match operation with
    | Buy -> 0.00
    | Sell -> if profit < 0.00 then 0.0 else profit * 0.2

let getTaxOnBuyOperation
    (transaction: Transaction)
    weightedAverage
    currentAssets
    allOperationProfit
    : BuyOperationTax=
        let quantityAsFloat = transaction.quantity |> float
        let currentAssetsAsFloat = currentAssets |> float

        let calculateAverage assets =
            ((currentAssetsAsFloat * assets) + (quantityAsFloat * transaction.unitCost)) / (currentAssetsAsFloat + quantityAsFloat)
        {
            tax = getTax Buy 0
            average = 
                if currentAssets = 0L
                    then 0.00
                else weightedAverage
                |> calculateAverage
            assets = currentAssets + transaction.quantity
            profit = if currentAssets = 0L then 0.00 else allOperationProfit
        }

let getTaxOnSellOperation (transaction: Transaction) weightedAverage currentAssets allProfit : SellOperationTax=
    let newCurrentAssets = currentAssets - transaction.quantity
    let quantityAsFloat = transaction.quantity |> float

    match quantityAsFloat * transaction.unitCost with
    | operationCost when operationCost <= 20000.00 ->
        {
            tax = 0.00
            assets = newCurrentAssets
            profit =
                if transaction.unitCost < weightedAverage
                then allProfit - ((quantityAsFloat * weightedAverage) - operationCost)
                else allProfit
        }
    | operationCost ->
            let transctionProfit = operationCost - (quantityAsFloat * weightedAverage)
            let currentProfit = allProfit + transctionProfit;
            {
                tax = match currentProfit with
                      | value when value <= 0 -> 0.00
                      | _ ->
                        if allProfit < 0
                            then transctionProfit + allProfit
                        else
                            transctionProfit
                        |> getTax Sell
                assets = newCurrentAssets
                profit = currentProfit
            }

let calculateTaxes transaction =
    let mutable allOperationProfit = 0.00
    let mutable weightedAverage = 0.00
    let mutable currentAssets = 0L

    transaction
    |> List.map (fun op ->
        let tax : float =
            match op.operation with
            | Buy ->
                getTaxOnBuyOperation op weightedAverage currentAssets allOperationProfit
                |> (fun op -> 
                    weightedAverage <- op.average
                    currentAssets <- op.assets
                    allOperationProfit <- op.profit
                    op.tax)
            | Sell ->
                getTaxOnSellOperation op weightedAverage currentAssets allOperationProfit
                |> (fun op -> 
                    currentAssets <- op.assets
                    allOperationProfit <- op.profit
                    op.tax)
        { tax = Math.Round(tax, 2) })
    |> List.ofSeq

