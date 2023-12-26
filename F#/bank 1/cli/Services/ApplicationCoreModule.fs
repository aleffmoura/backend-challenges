module ApplicationCoreModule
open TransactionServiceModule
open TransactionModule
open Newtonsoft.Json


let execute
    calculate
    printAct 
    (transactions: Map<int, Transaction list>) =
        transactions
        |> Map.iter (fun _ trncs -> 
            calculate trncs
            |> JsonConvert.SerializeObject
            |> printAct
        )
