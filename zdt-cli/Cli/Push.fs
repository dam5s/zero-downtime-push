module Cli.Push

open Cli
open Cli.Action

let push (cf: string list -> Result<unit, string>) (appName: string) (args: string list): Result<unit, string> =
    let venerableAppName = $"%s{appName}-venerable"

    let actions: Action list = [
        { Forward = fun _ -> cf [ "rename" ; appName ; venerableAppName ]
          Rollback = id
        }
        { Forward = fun _ -> cf (["push" ; appName ] @ args)
          Rollback = fun _ ->
            cf [ "delete" ; appName ] |> ignore
            cf [ "rename" ; venerableAppName ; appName ] |> ignore
        }
        { Forward = fun _ -> cf [ "delete" ; venerableAppName ]
          Rollback = id
        }
    ]

    Action.runAll actions
