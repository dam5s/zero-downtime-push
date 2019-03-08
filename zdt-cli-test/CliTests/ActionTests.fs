module CliTests.ActionTests

open Xunit
open Cli
open Cli.Action
open FsUnitTyped


[<Fact>]
let ``Running successful actions`` () =
    let mutable out: string list = []
    let mutable rollback: string list = []

    let actions = [
        { Forward = fun _ -> out <- List.append out ["forward 1"] ; Ok ()
          Rollback = fun _ -> rollback <- List.append rollback ["rollback 1"]
        }
        { Forward = fun _ -> out <- List.append out ["forward 2"] ; Ok ()
          Rollback = fun _ -> rollback <- List.append rollback ["rollback 2"]
        }
    ]

    let result = Action.runAll actions

    result |> shouldEqual (Ok ())
    out |> shouldEqual ["forward 1" ; "forward 2"]
    rollback |> shouldBeEmpty


[<Fact>]
let ``Rollback on first action`` () =
    let mutable out: string list = []
    let mutable rollback: string list = []

    let actions = [
        { Forward = fun _ -> out <- List.append out ["forward 1"] ; Error "Error during action 1"
          Rollback = fun _ -> rollback <- List.append rollback ["rollback 1"]
        }
        { Forward = fun _ -> out <- List.append out ["forward 2"] ; Ok ()
          Rollback = fun _ -> rollback <- List.append rollback ["rollback 2"]
        }
    ]

    let result = Action.runAll actions

    result |> shouldEqual (Error "Error during action 1")
    out |> shouldEqual ["forward 1"]
    rollback |> shouldEqual ["rollback 1"]


[<Fact>]
let ``Rollback on subsequent action`` () =
    let mutable out: string list = []
    let mutable rollback: string list = []

    let actions = [
        { Forward = fun _ -> out <- List.append out ["forward 1"] ; Ok ()
          Rollback = fun _ -> rollback <- List.append rollback ["rollback 1"]
        }
        { Forward = fun _ -> out <- List.append out ["forward 2"] ; Error "Error during action 2"
          Rollback = fun _ -> rollback <- List.append rollback ["rollback 2"]
        }
    ]

    let result = Action.runAll actions

    result |> shouldEqual (Error "Error during action 2")
    out |> shouldEqual ["forward 1" ; "forward 2"]
    rollback |> shouldEqual ["rollback 2" ; "rollback 1"]
