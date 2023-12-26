module TransactionModule

open Newtonsoft.Json

type Operation =
    | Buy
    | Sell

type Transaction =
    {
        operation: Operation
        unitCost: float
        quantity: int64
    }

type TaxResult = 
    {
        tax: float
    }