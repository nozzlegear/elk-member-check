open System
open CommandLine

[<Verb("afk", HelpText = "Checks raider rank to find members that have been offline for a certain amount of days")>]
type AfkOptions = {
    [<Option('d', "days", Default = 30, HelpText = "Number of days that must pass before a member is considered AFK")>]
    days: int    
}

[<Verb("rank", HelpText = "Checks Wowprogress and compares to guild roster to find members that have recently raided but aren't at raider rank")>]
type RankOptions = {
    [<Option('c', "csv", Default = "", HelpText = "Optional path to a CSV file containing raid roster. Will download from Wowprogress if absent")>]
    csv: string option
}

let findAfk (options: AfkOptions) = 
    // TODO: Find raiders who have been AFK for more than 30 days
    printfn "Finding raiders who have been AFK for more than %i days..." options.days
    0

let checkRanks (options: RankOptions) = 
    // TODO: Download CSV of guild members from wowprogress and check which active raiders aren't at raider rank
    printfn "Checking wowprogress to find members that have recently raided but are not at raider rank..."
    0

[<EntryPoint>]
let main argv =
    let result = CommandLine.Parser.Default.ParseArguments<AfkOptions, RankOptions>(argv)

    match result with
    | :? Parsed<obj> as command -> 
        match command.Value with 
        | :? AfkOptions as parsed -> findAfk parsed
        | :? RankOptions as parsed -> checkRanks parsed
        | _ -> 
            printfn "Bad command match"
            1
    | :? NotParsed<obj> as notParsed -> 
        notParsed.Errors
        |> Seq.iter (fun error -> 
            match error.Tag with 
            | CommandLine.ErrorType.MissingRequiredOptionError
            | CommandLine.ErrorType.HelpRequestedError  
            | CommandLine.ErrorType.HelpVerbRequestedError
            | CommandLine.ErrorType.VersionRequestedError ->
                // Parser will have already written the options to console
                ()
            | _ ->             
                printfn "Received parse error %A" error.Tag)

        1            
    | _ ->
        printfn "Bad result match" 
        1        