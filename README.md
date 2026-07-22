# Omori Mod
**Mod Author:** K3LV0N

This is a content expansion mod for Terraria based on Omori built using the tModLoader API. This README serves as a guide to understanding the mod's file structure and naming conventions.

## Documentation

See the [documentation index](docs/README.md) for all project guides, including:

- the [emotion system architecture and extension guide](docs/EmotionSystem.md); and
- the [contribution guide](docs/CONTRIBUTING.md).

## 📁 File Structure

The project is organized hierarchically for clarity and scalability, and is typically defined as such:

```
Items
├── Ammo
|	├── Arrows
|	|	├── Regular
|	|	└── Unlimited
|	└── Bullets
|	|	├── Regular
|	|	└── Unlimited
├── OtherItemType1
└── OtherItemType2
```

For a more general example:
```
InGameThing
├── ThingType1
|	├── Type1Specialization1
|	|	└── cs and png files
|	└── Type1Specialization2
└── ThingType2
```

## 🧾 Naming Conventions

To maintain clarity and consistency, the following naming conventions are used:

### Item Classes

- The class name typically matches the **in-game item name**.
  - Example: `HappyArrow.cs` defines the item named *Happy Arrow*.

### Projectiles

- Projectiles tied to a specific item use the format: ```[ItemName]Projectile[OptionalSpecialization]```

- For example: `HappyArrowProjectileNoDust.cs`
  - **'HappyArrow'**: Refers to the item it’s based on.
  - **'Projectile'**: Indicates it is a projectile.
  - **'NoDust'** *(optional)*: Extra details or behavior modifiers.

- This naming helps easily associate projectiles with their corresponding items.

### Buffs, NPCs, etc.

- Similar to items, class names generally reflect their in-game name for ease of reference.


## Dev Setup (VS Code)

This project includes a VS Code build/launch config so you can build the mod and launch tModLoader directly with breakpoint debugging, similar to the experience in Visual Studio.

Since everyone's tModLoader install lives in a different location, the config relies on an environment variable (`TMLSTEAMPATH`) rather than a hardcoded path. You only need to set this once per machine.

### 1. Run the setup script

From the root of the repo, in a **PowerShell** terminal:

```powershell
.\PowerShell\setup.ps1
```

This will prompt you for your tModLoader Steam install path (or press Enter to use the default Steam location) and save it as a permanent user environment variable.

> If you get an error about script execution being disabled, run this once first:
> ```powershell
> Set-ExecutionPolicy -Scope CurrentUser RemoteSigned
> ```
> Then re-run the setup script and confirm with `Y` when prompted.

### 2. Restart VS Code

Environment variables are only picked up by **new** processes. Fully close and reopen VS Code (not just "Reload Window") after running the script.

### 3. Launch the mod

- Open the **Run and Debug** panel (`Ctrl+Shift+D`)
- Select the profile matching your C# debugger:
  - **Launch tModLoader — Microsoft C#** when using the Microsoft C# debugger without ReSharper.
  - **Launch tModLoader — ReSharper** when ReSharper 2026.2 or newer is enabled.
- Click the green play button (or press `F5`)

This builds the mod and launches tModLoader. Breakpoints placed in your mod's code will bind once tModLoader loads the mod assembly.

ReSharper 2026.2 introduced its own `coreclr` debugger, which requires the managed `tModLoader.dll` program and its .NET runtime executable to be configured separately. The Microsoft profile intentionally preserves the original `dotnet tModLoader.dll` workflow. VS Code remembers the last selected launch profile, so selecting the appropriate profile is normally a one-time step.

### Troubleshooting

- **Nothing launches:** Make sure the launch profile matching your enabled debugger is selected before pressing play.
- **ReSharper reports that `dotnet` does not exist:** Select **Launch tModLoader — ReSharper**. The Microsoft profile uses a relative executable name that ReSharper's debugger does not accept.
- **`dotnet-tModLoader.dll` / command not found errors:** Your `TMLSTEAMPATH` variable likely isn't set or VS Code hasn't been restarted since running the setup script. Open a new terminal and run `echo $env:TMLSTEAMPATH` to confirm it's set correctly, and `Test-Path "$env:TMLSTEAMPATH\tModLoader.dll"` to confirm the path is valid.
