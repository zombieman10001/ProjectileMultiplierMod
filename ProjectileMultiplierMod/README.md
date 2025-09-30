# Projectile Multiplier Mod (Skeleton)

This is a starter Bannerlord mod that multiplies projectiles fired per shot (player & NPC) with in-game sliders via MCM.

## Features
- Separate player & NPC multipliers.
- Normal range (1-10) plus optional extreme (up to 100).
- Reflection-based patch now (slower); can be replaced with explicit method once you provide signature.

## Requirements
Install these mods/libraries in Bannerlord `Modules/` and reference their DLLs when building:
- Harmony (0Harmony.dll) (usually included by other mods)
- Mod Configuration Menu (MCM v5 for Bannerlord 1.2.x)

## Folder Layout
```
ProjectileMultiplierMod/
  SubModule.xml
  YourModName.csproj
  src/
    SubModule.cs
    Settings.cs
    Patches.AddMissile.cs
  bin/Win64_Shipping_Client/ (build output will place DLL here)
```

## Build
Open the `.csproj` in Visual Studio (net472). Define environment variable `BANNERLORD_BIN` to point to your Bannerlord `bin/Win64_Shipping_Client/` (or edit .csproj HintPaths manually).

Example (PowerShell, temporary for session):
```
$env:BANNERLORD_BIN = "C:\Program Files (x86)\Steam\steamapps\common\Mount & Blade II Bannerlord\bin\Win64_Shipping_Client\\"
```
Then build in Release. The post-build target copies DLL to module output folder.

## Usage
Enable Harmony, MCM, then this mod in launcher. Adjust multipliers in MCM > Projectile Multiplier.

## Next Step
Provide actual `SpawnMissile` (or related) method signature(s) so we can replace the reflection patch with a faster, stable patch.

## Disclaimer
Extreme values can heavily impact performance or stability.
