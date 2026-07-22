using System;

using OmoriMod.Content.Util.Interfaces;

using Terraria.ModLoader.IO;

namespace OmoriMod.Content.Util;

/// <summary>
/// A utility class that helps with timing. Can run for a really long time.
/// </summary>
public class TickTimer : ISaveableWithGenerate
{
    /// <summary>
    /// <see cref="string"/> for saving purposes.
    /// </summary>
    private const string SaveTotalTicks = "TickTimer:totalTicks:";

    /// <summary>
    /// <see cref="string"/> for saving purposes.
    /// </summary>
    private const string SaveOriginalTicks = "TickTimer:originalTicks:";

    /// <summary>
    /// How many total ticks there are left in this <see cref="TickTimer"/>
    /// </summary>
    private long _totalTicks;

    /// <summary>
    /// How many total ticks this <see cref="TickTimer"/> was initialized with
    /// </summary>
    private long _originalTicks;

    /// <summary>
    /// How many hours are left in the <see cref="TickTimer"/>
    /// </summary>
    public long Hours => _totalTicks / 216000;          // 60 * 60 * 60
    /// <summary>
    /// How many minutes are left in the <see cref="TickTimer"/>
    /// </summary>
    public long Minutes => _totalTicks / 3600 % 60;     // 60 * 60

    /// <summary>
    /// How many seconds are left in the <see cref="TickTimer"/>
    /// </summary>
    public long Seconds => _totalTicks / 60 % 60;

    /// <summary>
    /// How many ticks are left in the <see cref="TickTimer"/>
    /// </summary>
    public long Ticks => _totalTicks % 60;

    /// <summary>
    /// Returns the total ticks that are left in this <see cref="TickTimer"/>
    /// </summary>
    public long TotalTicks => _totalTicks;

    /// <summary>
    /// Returns <c>True</c> if this <see cref="TickTimer"/> is out of ticks. Only useful for <see cref="TickTimer"/> instances that decrement
    /// </summary>
    public bool IsDone => _totalTicks <= 0;

    // --- Constructors ---

    /// <summary>
    /// Creates a <see cref="TickTimer"/> with its total ticks starting at 0.
    /// </summary>
    public TickTimer()
    {
        _totalTicks = 0;
        _originalTicks = _totalTicks;
    }

    /// <summary>
    /// Copies the <paramref name="originalTimer"/> to make a new <see cref="TickTimer"/>
    /// </summary>
    /// <param name="originalTimer">The original <see cref="TickTimer"/> to copy.</param>
    public TickTimer(TickTimer originalTimer)
    {
        _totalTicks = originalTimer._totalTicks;
        _originalTicks = originalTimer._originalTicks;
    }

    /// <summary>
    /// Creates a <see cref="TickTimer"/> from ticks.
    /// </summary>
    /// <param name="totalTicks"></param>
    public TickTimer(long totalTicks)
    {
        _totalTicks = Math.Max(0, totalTicks);
        _originalTicks = _totalTicks;
    }

    /// <summary>
    /// Creates a <see cref="TickTimer"/> from these time chunks.
    /// </summary>
    /// <param name="seconds"></param>
    /// <param name="ticks"></param>
    public TickTimer(long seconds, long ticks)
    {
        _totalTicks =
            seconds * 60 +
            ticks;

        if (_totalTicks < 0) _totalTicks = 0; // clamp safety
        _originalTicks = _totalTicks;
    }

    /// <summary>
    /// Creates a <see cref="TickTimer"/> from these time chunks.
    /// </summary>
    /// <param name="minutes"></param>
    /// <param name="seconds"></param>
    /// <param name="ticks"></param>
    public TickTimer(long minutes, long seconds, long ticks)
    {
        _totalTicks =
            minutes * 3600 +
            seconds * 60 +
            ticks;

        if (_totalTicks < 0) _totalTicks = 0; // clamp safety
        _originalTicks = _totalTicks;
    }

    /// <summary>
    /// Creates a <see cref="TickTimer"/> from these time chunks.
    /// </summary>
    /// <param name="hours"></param>
    /// <param name="minutes"></param>
    /// <param name="seconds"></param>
    /// <param name="ticks"></param>
    public TickTimer(long hours, long minutes, long seconds, long ticks)
    {
        _totalTicks =
            hours * 216000 +
            minutes * 3600 +
            seconds * 60 +
            ticks;

        if (_totalTicks < 0) _totalTicks = 0; // clamp safety
        _originalTicks = _totalTicks;
    }

    /// <summary>
    /// Creates a <see cref="TickTimer"/> from the saved <paramref name="tag"/> data. Needs an <paramref name="identifier"/> to figure out which timer it is.
    /// </summary>
    /// <param name="tag">The <see cref="TagCompound"/> that stores this <see cref="TickTimer"/> data.</param>
    /// <param name="identifier">The <see cref="string"/> name of this <see cref="TickTimer"/>.</param>
    public TickTimer(TagCompound tag, string identifier)
    {
        LoadData(tag, identifier);
    }

    /// <summary>
    /// Creates a <see cref="TickTimer"/> from the saved <paramref name="tag"/> data. Needs an <paramref name="identifier"/> to figure out which timer it is.
    /// If no <see cref="TickTimer"/> is found, this instance is set to <paramref name="fallbackTimer"/>.
    /// </summary>
    /// <param name="tag">The <see cref="TagCompound"/> that stores this <see cref="TickTimer"/> data.</param>
    /// <param name="identifier">The <see cref="string"/> name of this <see cref="TickTimer"/>.</param>
    /// <param name="fallbackTimer">A backup <see cref="TickTimer"/> if this instance is not found in the <paramref name="tag"/>.</param>
    public TickTimer(TagCompound tag, string identifier, TickTimer fallbackTimer)
    {
        if (tag.ContainsKey(identifier.OmoriModString() + SaveTotalTicks))
        {
            LoadData(tag, identifier);
        }
        else
        {
            _totalTicks = fallbackTimer._totalTicks;
            _originalTicks = fallbackTimer._originalTicks;
        }
    }


    /// <summary>
    /// Increments <see cref="_totalTicks"/> by 1.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static TickTimer operator ++(TickTimer t)
    {
        t._totalTicks++;
        return t;
    }

    /// <summary>
    /// Decrements <see cref="_totalTicks"/> by 1.
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public static TickTimer operator --(TickTimer t)
    {
        if (t._totalTicks > 0)
            t._totalTicks--;
        return t;
    }



    /// <summary>
    /// Resets the <see cref="_totalTicks"/> to <see cref="_originalTicks"/>.
    /// </summary>
    public void Reset()
    {
        _totalTicks = _originalTicks;
    }


    /// <summary>
    /// Creates a savable <see cref="TagCompound"/> out of this <see cref="TickTimer"/>.
    /// </summary>
    /// <param name="identifier">The <see cref="string"/> name of this <see cref="TickTimer"/>.</param>
    /// <returns>A <see cref="TagCompound"/> with the serialized <see cref="TickTimer"/></returns>
    public TagCompound GenerateTagCompound(string identifier)
    {
        TagCompound tag = new()
        {
            [identifier.OmoriModString() + SaveTotalTicks] = _totalTicks,
            [identifier.OmoriModString() + SaveOriginalTicks] = _originalTicks
        };
        return tag;
    }

    /// <summary>
    /// Saves this <see cref="TickTimer"/> on the provided <paramref name="tag"/>.
    /// </summary>
    /// <param name="tag">The <see cref="TagCompound"/> this <see cref="TickTimer"/> should be saved to.</param>
    /// <param name="identifier">The <see cref="string"/> name of this <see cref="TickTimer"/>.</param>
    public void SaveData(TagCompound tag, string identifier)
    {
        tag[identifier.OmoriModString() + SaveTotalTicks] = _totalTicks;
        tag[identifier.OmoriModString() + SaveOriginalTicks] = _originalTicks;
    }

    /// <summary>
    /// Loads this <see cref="TickTimer"/> from the provided <paramref name="tag"/>
    /// </summary>
    /// <param name="tag">The <see cref="TagCompound"/> this <see cref="TickTimer"/> should be loaded from.</param>
    /// <param name="identifier">The <see cref="string"/> name of this <see cref="TickTimer"/>.</param>
    public void LoadData(TagCompound tag, string identifier)
    {
        _totalTicks = tag.GetLong(identifier.OmoriModString() + SaveTotalTicks);
        _originalTicks = tag.GetLong(identifier.OmoriModString() + SaveOriginalTicks);
    }

    public override bool Equals(object obj)
    {
        return obj is TickTimer other && _totalTicks == other._totalTicks;
    }

    public override int GetHashCode()
    {
        return _totalTicks.GetHashCode();
    }
}