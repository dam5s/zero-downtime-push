module Cli.Action

type Action =
    { Forward : unit -> Result<unit, string>
      Rollback : unit -> unit
    }


type private RunAllState =
    { Rollback : unit -> unit
      Result : Result<unit, string>
    }


let private initState : RunAllState =
    { Rollback = fun _ -> ()
      Result = Ok()
    }


let private runAction (state : RunAllState) (action: Action) : RunAllState =
    let newResult = state.Result |> Result.bind action.Forward
    let newRollback = fun _ -> action.Rollback () ; state.Rollback ()

    match newResult with
    | Ok _ ->
        { Rollback = newRollback
          Result = newResult
        }
    | Error _ ->
        newRollback ()

        { Rollback = id
          Result = newResult
        }


let runAll (actions : Action list) : Result<unit, string> =
    let finalState =
        List.fold
            (fun (state : RunAllState) (action : Action) ->
                match state.Result with
                | Ok _ -> runAction state action
                | Error _ -> state
            )
            initState
            actions

    finalState.Result
