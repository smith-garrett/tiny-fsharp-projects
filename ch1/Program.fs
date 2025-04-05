open Argu

type Arguments =
    | [<AltCommandLine("-n")>] Name of name: string

    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Name _ -> "Name to greet"

[<EntryPoint>]
let main argv =
    let errorHandler = ProcessExiter(None)
    let parser = ArgumentParser.Create<Arguments>(programName = "helloWorld", errorHandler=errorHandler)

    let result = parser.ParseCommandLine argv
    let name = result.GetResult(Name, defaultValue = "world")

    printfn $"Hello, {name}!"
    0
