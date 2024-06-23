// Implementing cmd line `wc` in F#

open Argu

type CmdArgs = 
    //| File of file: string list
    | [<MainCommand>] Files of file: string list
with
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Files _ -> "Input file(s)"

let getCounts path =
    let lines = System.IO.File.ReadAllText path
    let nlines = lines.Split([|'\n'|]) |> Seq.filter (fun x -> x <> "") |> Seq.length
    let nwords = lines.Split() |> Seq.filter (fun x -> x <> "") |> Seq.length
    let nchar = String.length lines
    printfn "%8i%8i%8i %s" nlines nwords nchar path
    nlines, nwords, nchar

[<EntryPoint>]
let main argv =
    let errorHandler = ProcessExiter(None)
    let parser = ArgumentParser.Create<CmdArgs>(programName = "wc",
        helpTextMessage = "Count lines, words, and characters in one or more files",
        errorHandler = errorHandler)
    let result = parser.ParseCommandLine(argv)
    let paths = result.TryGetResult Files |> Option.get

    if List.length paths > 1 then
        let totals =
            paths
            |> List.map getCounts
            |> List.fold (fun (x1, x2, x3) (y1, y2, y3) -> (x1+y1, x2+y2, x3+y3)) (0, 0, 0)
        totals |||> printfn "%8i%8i%8i total"
        else getCounts paths[0] |> ignore
    0