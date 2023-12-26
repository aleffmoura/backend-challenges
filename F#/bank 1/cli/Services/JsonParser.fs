module JsonParser

open Newtonsoft.Json.Linq
open TransactionModule

let rec toTransaction (jToken: JToken) : Transaction =
    {
        operation = if jToken.["operation"] |> string = "buy" then Operation.Buy else Operation.Sell
        unitCost = jToken.["unit-cost"] |> float
        quantity =  jToken.["quantity"] |> int64
    }