// Implementing cmd line `wc` in F#

open Argu

type CmdArgs =
    | [<MainCommand>] Files of file: string list

    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Files _ -> "Input file(s)"

let getCounts path =
    let lines = System.IO.File.ReadAllText path

    let nlines =
        lines.Split([| '\n' |]) |> Array.filter (fun x -> x <> "") |> Array.length

    let nwords = lines.Split() |> Array.filter (fun x -> x <> "") |> Array.length
    let nchar = String.length lines
    nlines, nwords, nchar, path

[<EntryPoint>]
let main argv =
    let errorHandler = ProcessExiter(None)

    let parser =
        ArgumentParser.Create<CmdArgs>(
            programName = "wc",
            helpTextMessage = "Count lines, words, and characters in one or more files",
            errorHandler = errorHandler
        )

    let result = parser.ParseCommandLine(argv)
    let paths = result.TryGetResult Files |> Option.get

    if List.length paths > 1 then
        let totals =
            paths
            |> List.map getCounts
            |> List.map (fun (x: int * int * int * string) ->
                let nl, nw, nc, p = x
                printfn "%8i%8i%8i %s" nl nw nc p
                (nl, nw, nc))
            |> List.fold (fun (x1, x2, x3) (y1, y2, y3) -> (x1 + y1, x2 + y2, x3 + y3)) (0, 0, 0)

        totals |||> printfn "%8i%8i%8i total"
    else
        getCounts paths[0]
        |> (fun x ->
            let nl, nw, nc, p = x
            printfn "%8i%8i%8i %s" nl nw nc p)

    0

(* Parallel version, not faster probably b/c paralell overhead
    |> List.toArray
    |> Array.Parallel.map getCounts
    |> Array.map (fun (x: int * int * int * string) ->
        let nl, nw, nc, p = x
        printfn "%8i%8i%8i %s" nl nw nc p
        (nl, nw, nc))
    |> Array.fold (fun (x1, x2, x3) (y1, y2, y3) -> (x1+y1, x2+y2, x3+y3)) (0, 0, 0)
*)
