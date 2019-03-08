module Program

open Support
open Fake.Core


[<EntryPoint>]
let main args =
    use ctxt = fakeExecutionContext (Array.toList args)
    Context.setExecutionContext (Context.RuntimeContext.Fake ctxt)

    Target.create "test" <| DotNet.test "zdt-cli-test"
    Target.create "build" <| DotNet.build

    "build" |> dependsOn [ "test" ]

    Target.runOrDefault "build"
    0
