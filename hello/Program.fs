// For more information see https://aka.ms/fsharp-console-apps

open Argu

type CliArguments =
    //| [<MainCommand>] Name of name: string // As positional argument
    | [<AltCommandLine("-n")>] Name of name: string

    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Name _ -> "Name to greet"
    

[<EntryPoint>]
let main args =
    // Letting Argu handle incorrect inputs gracefully
    let errorHandler = ProcessExiter(None)

    // Set up the argument parser
    let parser =
        ArgumentParser.Create<CliArguments>(programName = "hello", errorHandler = errorHandler)
    
    // Parse the arguments
    let result = parser.ParseCommandLine(args)

    // Getting the name, if wasn't entered, then use "World"
    let name = result.TryGetResult Name |> Option.defaultValue "World"
    
    printfn $"Hello, {name}!"
    0
