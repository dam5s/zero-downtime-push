module Program

open Cli.Push


let private cf (args: string list): Result<unit, string> =
    try
        use p = new System.Diagnostics.Process()
        p.StartInfo.FileName <- "cf"
        p.StartInfo.Arguments <- String.concat " " args
        p.StartInfo.RedirectStandardError <- false
        p.StartInfo.RedirectStandardOutput <- false
        p.Start () |> ignore
        p.WaitForExit ()

        if p.ExitCode = 0
        then Ok ()
        else Error "cf CLI exited with an error"
    with
    | ex ->
        Error $"There was an exception running cf: %s{ex.Message}"


[<EntryPoint>]
let main argv =
    let result =
        match  Array.toList argv with
        | [] ->
            Error "Expected at least one argument, the application name."
        | appName :: otherArgs ->
            push cf appName otherArgs

    match result with
    | Error message ->
        eprintfn $"\nERROR - %s{message}"
        1
    | Ok _ ->
        printfn "\nSUCCESS"
        0
