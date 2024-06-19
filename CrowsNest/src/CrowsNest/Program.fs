// Ch. 2 of Youens-Clark's *Tiny Python Projects*

// Command line boilerplate
open Argu
type CliArguments =
    | [<MainCommand>] Word of word: string // As positional argument
    //| [<AltCommandLine("-n")>] Name of name: string

    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Word _ -> "A word"

// An active pattern to identify vowels
let (|Vowel|Consonant|) (input: char) =
    let vowels = ['a'; 'e'; 'i'; 'o'; 'u']
    if List.contains input vowels then Vowel else Consonant

[<EntryPoint>]
let main args =
    // Letting Argu handle incorrect inputs gracefully
    let errorHandler = ProcessExiter(None)

    // Set up the argument parser
    let parser =
        ArgumentParser.Create<CliArguments>(programName = "CrowsNest", errorHandler = errorHandler)
    
    // Parse the arguments
    let result = parser.ParseCommandLine(args)
    let word = result.TryGetResult Word |> Option.defaultValue "danger"

    // Getting the right determiner
    let determiner =
        match (System.Char.ToLower word[0]) with
        | Vowel -> "an"
        | Consonant -> "a"

    // Output
    printfn $"Ahoy, Captain, {determiner} {word} off the larbord bow!"
    0
