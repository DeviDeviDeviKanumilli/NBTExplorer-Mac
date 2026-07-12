# NBTExplorer

NBTExplorer is an open-source NBT editor for all common sources of NBT data.  It's mainly intended for editing [Minecraft](http://www.minecraft.net) game data.

## Supported Formats

NBTExplorer supports reading and writing the following formats:

* Standard NBT files (e.g. level.dat)
* Schematic files
* Uncompressed NBT files (e.g. idcounts.dat)
* Minecraft region files (*.mcr)
* Minecraft anvil files (*.mca)
* Cubic Chunks region files (r2*.mcr, r2*.mca)

## System Requirements

### Windows

Windows XP or later, .NET Framework 2.0 or later.

### Linux

NBTExplorer is compatible with recent Mono runtimes, at least 2.6 or later.
Minimally, your system needs the `mono-core` and `mono-winforms` packages, or whatever package set is equivalent.

### Mac

A native Mac version is available. The new Mac target replaces the original MonoMac/.NET 2.0 build, which depended on
obsolete tooling and stale project paths. It uses modern .NET for macOS and AppKit and is currently configured for
Apple Silicon (`osx-arm64`).

It requires macOS 12 or later, an arm64 .NET 10 SDK, the macOS workload, and Xcode command-line tools.

From the repository root:

```sh
dotnet workload install macos
dotnet build NBTExplorer.MacArm64.csproj -c Release -p:CreateAppBundle=true
open bin/Release/net10.0-macos/osx-arm64/NBTExplorer.app
```

The project produces an `osx-arm64` app bundle and includes the managed Substrate dependency. For a local ad-hoc signed
build, clear inherited file metadata and sign the nested native libraries before opening the app:

```sh
xattr -cr bin/Release/net10.0-macos/osx-arm64/NBTExplorer.app
scripts/sign-macos-app.sh bin/Release/net10.0-macos/osx-arm64/NBTExplorer.app
open bin/Release/net10.0-macos/osx-arm64/NBTExplorer.app
```

For distribution, replace the ad-hoc signing in the script with an Apple Developer signing identity and appropriate
entitlements.

The build was verified to produce an arm64 Mach-O executable with the application resources, native nib files, icon, and
`Substrate.dll` embedded in the app bundle. The Intel Mac target (`osx-x64`) is not included yet, but the managed code
is not architecture-specific and can be added as a second runtime target.

The macOS port also updates the legacy Cocoa constructors to modern `NativeHandle` constructors and explicitly starts the
AppKit delegate/window. This is required for the nib-based UI to instantiate correctly on current .NET macOS runtimes.

The Windows version of NBTExplorer can still be used if the Mac version does not work.  You will need to install the
Mono runtime, and then run NBTExplorer with Mono from the command line.
