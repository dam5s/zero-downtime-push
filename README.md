# Zero Downtime Push

A small CLI to push an app to Cloud Foundry with zero downtime.

This is not a proper blue/green deployment. It just renames your app, pushes a new one, and deletes the old one.
There is no warranty whatsoever, use at your own risks.

## Development dependencies

 * Install [.NET 5](https://dotnet.github.io/)
 * Install [Warp Packer](https://github.com/dgiagio/warp)

## Building on Unix based systems

Run `./build-all`, this will test, build, and release binaries for Windows, Mac and Linux.

## Building on Windows

Most of the build should be similar. In `build/Program.fs` you may want to change the way
we shell out to warp-packer to invoke `warp-packer.exe` instead.

If you are using bash, you can run `./build-all`, otherwise just run

```
dotnet restore build
dotnet run -p build
```

## Usage

```
cf-zdt-push my-app [...]
```

The application name is required.
Any flags after that are directly passed to the underlying `cf push my-app` command.

See `zdt-cli/Cli/Push.fs` for more details.
