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


[<Fact>]
let ``Failing during push`` () =
    let mutable out : string list = []

    let cf (args : string list) : Result<unit, string> =
        out <- List.append out [ cmd args ]

        match List.head args with
        | "push" -> Error "Push error"
        | _ -> Ok ()


    let result = push cf "my-app" [ "-f" ; "my-manifest.yml" ]


    result |> shouldEqual (Error "Push error")
    out |> shouldEqual [ "cf rename my-app my-app-venerable"
                         "cf push my-app -f my-manifest.yml"
                         "cf delete my-app"
                         "cf rename my-app-venerable my-app"
                       ]
