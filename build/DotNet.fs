module DotNet

open Fake.DotNet
open Architectures


let private dotnet (command: string) (args: string) =
    let result = DotNet.exec id command args
    Support.ensureSuccessExitCode result.ExitCode


let restore _ =
    dotnet "restore" ""

let build _ =
    dotnet "build" ""

let test project _ =
    dotnet "test" project

let run project _ =
    dotnet "run" (sprintf "-p %s" project)

let release project (arch: Architecture) _ =
    arch
    |> dotNetFlag
    |> sprintf "%s -c Release -r %s" project
    |> dotnet "publish"
