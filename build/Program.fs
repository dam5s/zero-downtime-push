module Program

open Support
open Fake.Core


[<EntryPoint>]
let main args =
    use ctxt = fakeExecutionContext (Array.toList args)
    Context.setExecutionContext (Context.RuntimeContext.Fake ctxt)

    Target.create "build" <| DotNet.build

    Target.runOrDefault "build"
    0
