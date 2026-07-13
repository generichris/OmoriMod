using Terraria.ModLoader.IO;

namespace OmoriMod.Util.Interfaces;

/// <summary>
/// A contract in for objects that need to be saved. 
/// Promises that they are <see cref="TagCompound"/> serializable.
/// Also promises a method to create a <see cref="TagCompound"/> in case of use in collections
/// </summary>
public interface ISaveableWithGenerate : ISaveable
{
    public TagCompound GenerateTagCompound(string identifier);
}