module Cli.Push

open Cli
open Cli.Action


let push (cf : string list -> Result<unit, string>) (appName : string) (args : string list) : Result<unit, string> =
    let venerableAppName = sprintf "%s-venerable" appName

    let actions : Action list = [
        { Forward = fun _ -> cf [ "rename" ; appName ; venerableAppName ]
          Rollback = id
        }
        { Forward = fun _ -> cf <| List.append ["push" ; appName ] args
          Rollback = fun _ ->
            cf [ "logs" ; appName ; "--recent" ] |> ignore
            cf [ "delete" ; "-f" ; appName ] |> ignore
            cf [ "rename" ; venerableAppName ; appName ] |> ignore
        }
        { Forward = fun _ -> cf [ "delete" ; "-f" ; venerableAppName ]
          Rollback = id
        }
    ]

    Action.runAll actions
