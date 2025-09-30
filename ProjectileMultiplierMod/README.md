# Projectile Multiplier Mod

A Mount & Blade II: Bannerlord mod that multiplies projectiles fired per shot for both players and NPCs with configurable settings via MCM.

## Features
- Separate multipliers for player and NPCs
- Normal range (1-10) and extreme mode (up to 100)
- Optional random spread for projectiles
- Hard safety cap for NPCs to prevent performance issues
- One-time warning when extreme mode is active
- Built with Harmony for compatibility

## Requirements
Install these dependencies in Bannerlord's `Modules/` folder:
- **Harmony** (0Harmony.dll) - Usually included with other mods
- **Mod Configuration Menu (MCM v5)** - For in-game configuration

## Installation
1. Download the latest release
2. Extract to `Mount & Blade II Bannerlord/Modules/`
3. Enable the mod in the Bannerlord launcher
4. Configure settings in-game via MCM Options

## Configuration
Access settings through **Mod Options > Projectile Multiplier** in-game:

### Player Settings
- **Use Extreme Multiplier**: Enable extreme mode (up to 100 projectiles)
- **Multiplier (Normal)**: 1-10 projectiles per shot
- **Multiplier (Extreme)**: 1-100 projectiles per shot

### NPC Settings
- **Use Extreme Multiplier**: Enable extreme mode for NPCs
- **Multiplier (Normal)**: 1-10 projectiles
- **Multiplier (Extreme)**: 1-100 projectiles
- **Hard Safety Cap**: Maximum projectiles regardless of settings

### Other Options
- **Slight Random Spread**: Add variation to projectile direction
- **Show Extreme Warning**: Display warning when extreme mode is active
- **Multiply Thrown Weapons**: Apply multiplier to thrown weapons (experimental)

## Building from Source
1. Open `ProjectileMultiplierMod.csproj` in Visual Studio
2. Update DLL reference paths in the .csproj to match your Bannerlord installation
3. Build in Release configuration
4. The DLL will be copied to the appropriate module folder

## Performance Notes
- Default multiplier values are set to 3 for balanced gameplay
- Extreme values (50-100) can significantly impact performance
- NPC hard cap prevents AI from overwhelming the game
- Use extreme mode with caution in large battles

## Compatibility
- Built for Mount & Blade II: Bannerlord e1.2.x+
- Uses Harmony patching for broad compatibility
- Compatible with most other mods

## Credits
Built with reference to best practices from the [NobleKillerExtended](https://github.com/Danqna/NobleKillerExtended) mod.

## Disclaimer
Extreme multiplier values can heavily impact game performance and stability. Use at your own risk.
