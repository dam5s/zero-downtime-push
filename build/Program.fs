module Program

open Support
open Fake.Core
open Architectures


let private warpPack cmdArgs =
    { Program = "warp-packer"
      WorkingDir = "."
      CommandLine = cmdArgs
      Args = []
    }
    |> Process.shellExec
    |> Support.ensureSuccessExitCode


let private pack (arch : Architecture) =
    let dotNetFlag = dotNetFlag arch
    let warpFlag = warpFlag arch

    let cliInput =
        match arch with
        | Windows -> "zdt-cli.exe"
        | _ -> "zdt-cli"

    let cliOutput =
        match arch with
        | Windows -> "cf-zdt-push-windows.exe"
        | Linux -> "cf-zdt-push-linux"
        | Mac -> "cf-zdt-push-macosx"

    let cmdArgs =
        sprintf
            "--arch %s --input_dir zdt-cli/bin/Release/netcoreapp2.2/%s/publish --exec %s --output zdt-cli/bin/Release/%s"
            warpFlag
            dotNetFlag
            cliInput
            cliOutput

    warpPack cmdArgs


let private release arch =
    DotNet.release "zdt-cli" arch ()
    pack arch



[<EntryPoint>]
let main args =
    use ctxt = fakeExecutionContext (Array.toList args)
    Context.setExecutionContext (Context.RuntimeContext.Fake ctxt)

    Target.create "test" <| DotNet.test "zdt-cli-test"
    Target.create "build" <| DotNet.build
    Target.create "release" (fun _ ->
        release Linux
        release Windows
        release Mac
    )

    "build" |> dependsOn [ "test" ]
    "release" |> dependsOn [ "build" ]

    Target.runOrDefault "release"
    0
