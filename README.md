# NBTExplorer for Mac

This repository is an independent, open-source fork of
[the original NBTExplorer](https://github.com/jaquadro/NBTExplorer), created by Justin Aquadro. It preserves the original
project's NBT editing capabilities while providing a maintained native build for Apple Silicon Macs. This fork is not an
official release of the upstream project.

NBTExplorer reads and edits common Named Binary Tag (NBT) data formats, primarily for
[Minecraft](https://www.minecraft.net/) worlds and related files.

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

A native Apple Silicon version is available. `NBTExplorer.MacArm64.csproj` replaces the original MonoMac/.NET 2.0 Mac
build with modern .NET for macOS and AppKit. The active Mac target is ARM64-only (`osx-arm64`); it does not produce an
Intel Mac binary. The legacy Windows/x86 and MonoMac projects remain in the repository for reference, but none of their
WinForms, Windows interop, or old MonoMac files are compiled into the Apple Silicon app.

It requires macOS 12 or later, an arm64 .NET 10 SDK, the macOS workload, and Xcode command-line tools.

From the repository root:

```sh
dotnet workload install macos
dotnet build NBTExplorer.MacArm64.csproj -c Release -p:CreateAppBundle=true
open bin/Release/net10.0-macos/osx-arm64/NBTExplorer.app
```

The project produces an `osx-arm64` app bundle and includes the managed, architecture-neutral Substrate dependency. For
a local ad-hoc signed build, clear inherited file metadata and sign the nested native libraries before opening the app:

```sh
xattr -cr bin/Release/net10.0-macos/osx-arm64/NBTExplorer.app
scripts/sign-macos-app.sh bin/Release/net10.0-macos/osx-arm64/NBTExplorer.app
open bin/Release/net10.0-macos/osx-arm64/NBTExplorer.app
```

For distribution, replace the ad-hoc signing in the script with an Apple Developer signing identity and appropriate
entitlements.

The Release build has been verified as an ARM64 Mach-O executable with the application resources, native nib files, icon,
and `Substrate.dll` embedded in the app bundle. Ad-hoc signature verification also passes. The Intel Mac target
(`osx-x64`) is not included; adding it would require a separate runtime build and testing on Intel hardware.

#### Apple Silicon migration notes

The macOS audit and migration addressed the following issues:

* duplicate AppKit delegate initialization no longer opens two windows;
* the outline uses native, clickable disclosure chevrons and readable system text colors;
* tree delegates are retained correctly, rows have native spacing and alternating backgrounds, and every supported array
  tag has a visible icon;
* direct file arguments work, including paths containing spaces, and unsupported paths now report an error instead of
  leaving a blank window;
* modern `NSAlert` and `NSOpenPanel` APIs replace obsolete MonoMac calls, including correct unsaved-change confirmation;
* list edits correctly mark files as modified, clipboard corruption is handled safely, and inaccessible macOS folders no
  longer crash browsing;
* Find respects the Name and Value controls, safely completes background searches, and clears stale Find Next state;
* reflection-based tag construction was replaced with direct construction so Release trimming is predictable on ARM64.

The bundled `Substrate.dll` is an older managed .NET Framework assembly. Some file tools label managed assemblies as
PE32/x86 because of their container format, but it does not contain native x86 machine code and runs inside the ARM64 .NET
process. Release builds may report linker warning `IL2104` for its legacy metadata; the signed Release app was runtime-tested
without the Debug resolver messages. Replacing it with the full upstream source would add a large legacy world-editing and
Windows drawing surface that NBTExplorer does not use, so this fork keeps the small managed dependency.

Current Mac limitation: byte/short/int/long array values can be browsed, but the legacy hex-array editor has not been
ported to AppKit.

## License and attribution

This fork remains available under the original project's [MIT License](LICENSE.txt). The original copyright and license
notices are retained, and credit for the original NBTExplorer project belongs to Justin Aquadro and its contributors.
