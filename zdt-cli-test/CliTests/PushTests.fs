module CliTests.PushTests

open Xunit
open Cli.Push
open FsUnitTyped


let private cmd args =
    args
    |> String.concat " "
    |> sprintf "cf %s"


[<Fact>]
let ``Running successful push`` () =
    let mutable out : string list = []

    let cf (args : string list) : Result<unit, string> =
        out <- List.append out [ cmd args ]
        Ok ()

    let result = push cf "my-app" [ "-f" ; "my-manifest.yml" ]

    result |> shouldEqual (Ok ())
    out |> shouldEqual [ "cf rename my-app my-app-venerable"
                         "cf push my-app -f my-manifest.yml"
                         "cf delete my-app-venerable"
                       ]
