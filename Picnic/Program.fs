open Argu
open System

type CliArguments =
    | [<MainCommand; Mandatory>] Items of items: string list
    | Sorted

    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Items _ -> "List of items to include"
            | Sorted -> "Sort the items, default is false"


let fmtList (lst: string list) =
    match (List.length lst) with
    | 0 -> ""
    | 1 -> lst[0]
    | 2 -> lst |> String.concat " and "
    | ln ->
        lst
        |> fun x ->
            let beg, last = List.splitAt (ln - 1) x
            let first_part = String.concat ", " beg
            first_part + ", and " + last[0]

[<EntryPoint>]
let main argv =
    let errorHandler =
        ProcessExiter(
            colorizer =
                function
                | ErrorCode.HelpText -> None
                | _ -> Some ConsoleColor.Red
        )

    let parser =
        ArgumentParser.Create<CliArguments>(programName = "picnic", errorHandler = errorHandler)

    let results = parser.Parse argv
    let items = results.TryGetResult Items
    let sorted = results.Contains Sorted

    if sorted then
        let sorted_items = items |> Option.map List.sort |> Option.map fmtList
        printfn $"You are bringing {Option.get sorted_items}."
    else
        printf $"You are bringing {items |> Option.map fmtList |> Option.get}."

    0
