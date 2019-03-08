module Architectures

type Architecture =
    | Linux
    | Windows
    | Mac

let dotNetFlag (arch : Architecture): string =
    match arch with
    | Linux -> "linux-x64"
    | Windows -> "win-x64"
    | Mac -> "osx-x64"

let warpFlag (arch : Architecture): string =
    match arch with
    | Linux -> "linux-x64"
    | Windows -> "windows-x64"
    | Mac -> "macos-x64"
