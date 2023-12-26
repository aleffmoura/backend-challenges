open System
open TransactionModule
open Newtonsoft.Json
open TransactionServiceModule
open Newtonsoft.Json.Linq
open JsonParser

let printMethod (value: string) = 
    printfn "%s" value
    |> ignore

let execute = ApplicationCoreModule.execute calculateTaxes printMethod

let getTransactions () : Map<int, Transaction list> =
    let rec redLine iterator (transact: Map<int, Transaction list>) =
        match Console.ReadLine() with
        | value when value |> String.IsNullOrWhiteSpace -> transact 
        | value ->
            value
            |> JsonConvert.DeserializeObject<JArray>
            |> Seq.map toTransaction
            |> Seq.toList
            |> fun parsedList ->  transact.Add(iterator, parsedList)
            |> redLine (iterator + 1)
    
    redLine 0 Map.empty<int, Transaction list> 


getTransactions () |> execute