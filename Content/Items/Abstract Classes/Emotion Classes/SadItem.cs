using OmoriMod.Systems.EmotionSystem;

namespace OmoriMod.Content.Items.Abstract_Classes;

/// <summary>
/// Automatically sets <see cref="EmotionItem.Emotion"/> to <see cref="EmotionType.SAD"/>.
/// </summary>
public abstract class SadItem : EmotionItem
{
    public SadItem()
    {
        SetEmotionType(EmotionType.SAD);
    }
}